using MobileApplication.Models;
using MobileApplication.Services;
using Microsoft.Maui.Controls;

namespace MobileApplication;

public partial class EditAssessmentPage : ContentPage
{
    public static List<string> typeOptions = new List<string>()
    {
        "Objective",
        "Performance"
    };

    public static List<string> alertOptions = new List<string>()
    {
        "On",
        "Off"
    };

    public EditAssessmentPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        // only 1 of each assessment type allowed
        // check for pre-existing assessments and restrict options
        if (ViewCoursePage.assessmentList.Count == 2)
        {
            typeOptions = new List<string>()
            {         
                // both assessment types already exists, restrict to its current type
                ViewAssessmentPage.selectedAssessment.Type
            };
        }
        else
        {
            // no pre-existing assessments, allow both options
            typeOptions = new List<string>()
            {
                "Objective",
                "Performance"
            };
        }

        // populate xaml status picker with type options
        AssessmentTypePicker.ItemsSource = typeOptions;

        // populate xaml alerts picker with alert options
        AssessmentAlertsPicker.ItemsSource = alertOptions;

        PopulateFields();
    }

    public void PopulateFields()
    {
        AssessmentNameEntry.Text = ViewAssessmentPage.selectedAssessment.Name;
        AssessmentStartDatePicker.Date = ViewAssessmentPage.selectedAssessment.Start.ToLocalTime();
        AssessmentEndDatePicker.Date = ViewAssessmentPage.selectedAssessment.End.ToLocalTime();
        AssessmentTypePicker.SelectedItem = ViewAssessmentPage.selectedAssessment.Type;
        AssessmentAlertsPicker.SelectedItem = ViewAssessmentPage.selectedAssessment.Alerts;
        AssessmentDescriptionEntry.Text = ViewAssessmentPage.selectedAssessment.Description;
    }

    private async void SaveAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // validate save
        if (string.IsNullOrWhiteSpace(AssessmentNameEntry.Text))
        {
            // Assessment Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(AssessmentStartDatePicker.Date.ToString()))
        {
            // Assessment Start Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(AssessmentEndDatePicker.Date.ToString()))
        {
            // Assessment End Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (AssessmentTypePicker.SelectedItem is null)
        {
            // Assessment Type = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (AssessmentAlertsPicker.SelectedItem is null)
        {
            //Assessment Alerts = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (AssessmentStartDatePicker.Date >= AssessmentEndDatePicker.Date)
        {
            //Assessment Start Date is after or on End Date
            await DisplayAlert("Alert", "Start Date must be before End Date.", "OK");
            return;
        }
        if (AssessmentStartDatePicker.Date >= ViewCoursePage.selectedCourse.End.ToLocalTime())
        {
            //Assessment Start Date is after or on Course End Date
            await DisplayAlert("Alert", "Assessment Dates must be within Course Dates.", "OK");
            return;
        }
        if (AssessmentStartDatePicker.Date < ViewCoursePage.selectedCourse.Start.ToLocalTime())
        {
            //Assessment Start Date is before Course Start Date
            await DisplayAlert("Alert", "Assessment Dates must be within Course Dates.", "OK");
            return;
        }
        if (AssessmentEndDatePicker.Date > ViewCoursePage.selectedCourse.End.ToLocalTime())
        {
            //Assessment End Date is after Course End Date
            await DisplayAlert("Alert", "Assessment Dates must be within Course Dates.", "OK");
            return;
        }
        if (AssessmentEndDatePicker.Date <= ViewCoursePage.selectedCourse.Start.ToLocalTime())
        {
            //Assessment End Date is before or on Course Start Date
            await DisplayAlert("Alert", "Assessment Dates must be within Course Dates.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(AssessmentDescriptionEntry.Text))
        {
            // format empty description
            AssessmentDescriptionEntry.Text = " ";
        }

        // update Assessment
        DataAccessService.UpdateAssessment(ViewAssessmentPage.selectedAssessment.Id, AssessmentNameEntry.Text, AssessmentStartDatePicker.Date, AssessmentEndDatePicker.Date, AssessmentTypePicker.SelectedItem.ToString(), AssessmentDescriptionEntry.Text, AssessmentAlertsPicker.SelectedItem.ToString(), ViewCoursePage.selectedCourse.Id);

        // return to view course page
        await Shell.Current.GoToAsync("..");
    }

    private async void CancelAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // return to view course page
        await Shell.Current.GoToAsync("..");
    }
}