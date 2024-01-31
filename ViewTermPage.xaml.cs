using MobileApplication.Models;
using MobileApplication.Services;
using Plugin.LocalNotification;

namespace MobileApplication;

[QueryProperty("ReceivedId", "id")]

public partial class ViewTermPage : ContentPage
{
    public static IList<Course> courseList;
    public static Term selectedTerm;
    
    public string ReceivedId
    {
        set
        {
            // retrieve selected Term
            Guid termId = new Guid(value);           
            selectedTerm = DataAccessService.GetTerm(termId);            
        }
    }

    public ViewTermPage()
	{
		InitializeComponent();       
	}

    protected override void OnAppearing()
    {
        LoadDetails(selectedTerm.Id);
    }

    public void LoadDetails(Guid termId)
    {               
        // format term dates
        var startDate = selectedTerm.Start.ToLocalTime().ToString("MMMM d,yyyy");
        var endDate = selectedTerm.End.ToLocalTime().ToString("MMMM d,yyyy");

        // set term details in xaml
        TermName.Text = selectedTerm.Name;
        TermDates.Text = $"{startDate} to {endDate}";

        // populate course list
        RefreshCourseList(selectedTerm.Id);

        // check for alerts
        if (courseList.Count > 0)
        {
            CheckForAlerts();
        }
    }   

    public void CheckForAlerts()
    {
        foreach (Course course in courseList)
        {           
            if (course.Alerts == "On")
            {
                if (course.Start.ToLocalTime() == DateTime.Today)
                {
                    var notification = new NotificationRequest
                    {
                        Title = "Course Notification",
                        Subtitle = "Start Date",
                        Description = $"The following course starts today: {course.Name}."
                    };

                    LocalNotificationCenter.Current.Show(notification);                                    
                }
                if (course.End.ToLocalTime() == DateTime.Today)
                {
                    var notification = new NotificationRequest
                    {
                        Title = "Course Notification",
                        Subtitle = "End Date",
                        Description = $"The following course ends today: {course.Name}."
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
            }
        }                        
    }

    public void RefreshCourseList(Guid termId)
    {
        // populate course list
        courseList = DataAccessService.GetCoursesByTerm(termId);

        // populate xaml collection view with list items
        CourseCollectionView.ItemsSource = courseList;
    }   

    private async void ViewCourseButton_Clicked(object sender, EventArgs e)
    {     
        // gets the id of the selected course           
        var tempId = (Button)sender;

        // passes the selected Course's ID to the View Course Page
        await Shell.Current.GoToAsync($"{nameof(ViewCoursePage)}?id={tempId.CommandParameter}");       
    }

    private async void AddCourseButton_Clicked(object sender, EventArgs e)
    {
        // check course limit
        if (courseList.Count == 6)
        {            
            await DisplayAlert("Alert", "Course limit reached.", "OK");
            return;
        }
        else
        {
            // go to add course page
            await Shell.Current.GoToAsync(nameof(AddCoursePage));
        }
    }

    private async void EditTermButton_Clicked(object sender, EventArgs e)
    {
        // go to edit term page
        await Shell.Current.GoToAsync(nameof(EditTermPage));
    }

    private async void DeleteTermButton_Clicked(object sender, EventArgs e)
    {
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this term?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // delete term
            DataAccessService.DeleteTerm(selectedTerm.Id);

            // return to home page
            await Shell.Current.GoToAsync("..");
        }      
    }    
}