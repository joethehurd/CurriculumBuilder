using MobileApplication.Models;
using MobileApplication.Services;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using static System.Net.Mime.MediaTypeNames;
using Plugin.LocalNotification;

namespace MobileApplication;

[QueryProperty("ReceivedId", "id")]

public partial class ViewCoursePage : ContentPage
{
    public static IList<Assessment> assessmentList;
    public static Course selectedCourse;

    public string ReceivedId
    {
        set
        {
            // retrieve selected Course
            Guid courseId = new Guid(value);            
            selectedCourse = DataAccessService.GetCourse(courseId);
        }
    }

    public ViewCoursePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        LoadDetails(selectedCourse.Id);
    }

    public void LoadDetails(Guid courseId)
    {
        // format course dates
        var startDate = selectedCourse.Start.ToLocalTime().ToString("MMMM d,yyyy");
        var endDate = selectedCourse.End.ToLocalTime().ToString("MMMM d,yyyy");

        // set course details in xaml
        CourseName.Text = selectedCourse.Name;
        CourseDates.Text = $"{startDate} to {endDate}";
        CourseInstructorNameLabel.Text = selectedCourse.InstructorName;
        CourseInstructorPhoneLabel.Text = selectedCourse.InstructorPhone;
        CourseInstructorEmailLabel.Text = selectedCourse.InstructorEmail;
        CourseStatusLabel.Text = selectedCourse.Status;
        CourseAlertsLabel.Text = selectedCourse.Alerts;
        CourseNotesLabel.Text = selectedCourse.Notes;

        // populate assessment list
        RefreshAssessmentList(selectedCourse.Id);

        // check for alerts
        if (assessmentList.Count > 0)
        {
            CheckForAlerts();
        }
    }

    public void CheckForAlerts()
    {
        foreach (Assessment assessment in assessmentList)
        {
            if (assessment.Alerts == "On")
            {
                if (assessment.Start.ToLocalTime() == DateTime.Today)
                {
                    var notification = new NotificationRequest
                    {
                        Title = "Assessment Notification",
                        Subtitle = "Start Date",
                        Description = $"The following assessment starts today: {assessment.Name}."
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
                if (assessment.End.ToLocalTime() == DateTime.Today)
                {
                    var notification = new NotificationRequest
                    {
                        Title = "Assessment Notification",
                        Subtitle = "End Date",
                        Description = $"The following assessment ends today: {assessment.Name}."
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
            }
        }
    }

    public void RefreshAssessmentList(Guid courseId)
    {
        // populate assessment list
        assessmentList = DataAccessService.GetAssessmentsByCourse(courseId);

        // populate xaml collection view with list items
        AssessmentCollectionView.ItemsSource = assessmentList;
    }

    private async void ViewAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // gets the id of the selected assessment          
        var tempId = (Button)sender;

        // passes the selected Assessment's ID to the View Assessment Page
        await Shell.Current.GoToAsync($"{nameof(ViewAssessmentPage)}?id={tempId.CommandParameter}");
    }

    private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // check assessment limit      
        if (assessmentList.Count == 2)
        {            
            await DisplayAlert("Alert", "Assessment limit reached.", "OK");
            return;
        }
        else
        {
            // go to add assessment page
            await Shell.Current.GoToAsync(nameof(AddAssessmentPage));
        }      
    }

    private async void EditCourseButton_Clicked(object sender, EventArgs e)
    {
        // go to edit course page
        await Shell.Current.GoToAsync(nameof(EditCoursePage));
    }

    private async void DeleteCourseButton_Clicked(object sender, EventArgs e)
    {
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this course?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // delete course
            DataAccessService.DeleteCourse(selectedCourse.Id);

            // return to term page
            await Shell.Current.GoToAsync("..");
        }
    }

    private async void ShareNotesButton_Clicked(object sender, EventArgs e)
    {        
        // check if notes are blank
        if (string.IsNullOrWhiteSpace(selectedCourse.Notes))
        {
            await DisplayAlert("Alert", "Course notes are empty.", "OK");
            return;
        }

        // share notes
        await Share.Default.RequestAsync(new ShareTextRequest
        {                   
            Subject = $"{selectedCourse.Name} - Notes",
            Text = selectedCourse.Notes
        });
    }
}