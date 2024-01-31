using MobileApplication.Models;
using MobileApplication.Services;
using Microsoft.Maui.Controls;

namespace MobileApplication;

public partial class AddCoursePage : ContentPage
{
    public static List<string> statusOptions = new List<string>()
    { 
        "Plan to take",
        "In progress",
        "Completed",
        "Dropped"
    };

    public static List<string> alertOptions = new List<string>()
    {
        "On",
        "Off"
    };

    public AddCoursePage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        // populate xaml status picker with status options
        CourseStatusPicker.ItemsSource = statusOptions;

        // populate xaml alerts picker with alert options
        CourseAlertsPicker.ItemsSource = alertOptions;
    }

    private async void SaveCourseButton_Clicked(object sender, EventArgs e)
    {            
        // validation
        if (string.IsNullOrWhiteSpace(CourseNameEntry.Text))
        {
            // Course Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseStartDatePicker.Date.ToString()))
        {
            // Course Start Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseEndDatePicker.Date.ToString()))
        {
            // Course End Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseInstructorNameEntry.Text))
        {
            // Course Instructor Name = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseInstructorPhoneEntry.Text))
        {
            // Course Instructor Phone = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseInstructorEmailEntry.Text))
        {
            // Course Instructor Email = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (CourseStatusPicker.SelectedItem is null)
        {
            // Course Status = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (CourseAlertsPicker.SelectedItem is null)
        {
            //Course Alerts = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (CourseStartDatePicker.Date >= CourseEndDatePicker.Date)
        {
            //Course Start Date is after or on End Date
            await DisplayAlert("Alert", "Start Date must be before End Date.", "OK");
            return;
        }                      
        if (CourseStartDatePicker.Date >= ViewTermPage.selectedTerm.End.ToLocalTime())
        {
            //Course Start Date is after or on Term End Date
            await DisplayAlert("Alert", "Course Dates must be within Term Dates.", "OK");
            return;
        }       
        if (CourseStartDatePicker.Date < ViewTermPage.selectedTerm.Start.ToLocalTime())
        {
            //Course Start Date is before Term Start Date
            await DisplayAlert("Alert", "Course Dates must be within Term Dates.", "OK");
            return;
        }       
        if (CourseEndDatePicker.Date > ViewTermPage.selectedTerm.End.ToLocalTime())
        {
            //Course End Date is after Term End Date
            await DisplayAlert("Alert", "Course Dates must be within Term Dates.", "OK");
            return;
        }        
        if (CourseEndDatePicker.Date <= ViewTermPage.selectedTerm.Start.ToLocalTime())
        {
            //Course End Date is before or on Term Start Date
            await DisplayAlert("Alert", "Course Dates must be within Term Dates.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(CourseNotesEntry.Text))
        {
            // format empty notes
            CourseNotesEntry.Text = " ";
        }

        // add Course
        DataAccessService.AddCourse(new Guid(), CourseNameEntry.Text, CourseStartDatePicker.Date, CourseEndDatePicker.Date, CourseInstructorNameEntry.Text, CourseInstructorPhoneEntry.Text, CourseInstructorEmailEntry.Text, CourseStatusPicker.SelectedItem.ToString(), CourseNotesEntry.Text, CourseAlertsPicker.SelectedItem.ToString(), ViewTermPage.selectedTerm.Id);

        // return to view term page
        await Shell.Current.GoToAsync("..");
    }

    private async void CancelCourseButton_Clicked(object sender, EventArgs e)
    {
        // return to view term page
        await Shell.Current.GoToAsync("..");
    }
}