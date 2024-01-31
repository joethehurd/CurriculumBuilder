using MobileApplication.Models;
using MobileApplication.Services;

namespace MobileApplication;

[QueryProperty("ReceivedId", "id")]

public partial class ViewAssessmentPage : ContentPage
{    
    public static Assessment selectedAssessment;

    public string ReceivedId
    {
        set
        {
            // retrieve selected Assessment
            Guid assessmentId = new Guid(value);            
            selectedAssessment = DataAccessService.GetAssessment(assessmentId);
        }
    }

    public ViewAssessmentPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        LoadDetails(selectedAssessment.Id);
    }

    public void LoadDetails(Guid courseId)
    {
        // format assesment dates
        var startDate = selectedAssessment.Start.ToLocalTime().ToString("MMMM d,yyyy");
        var endDate = selectedAssessment.End.ToLocalTime().ToString("MMMM d,yyyy");

        // set assessment details in xaml        
        AssessmentName.Text = selectedAssessment.Name;
        AssessmentDates.Text = $"{startDate} to {endDate}";
        AssessmentTypeLabel.Text = selectedAssessment.Type;
        AssessmentAlertsLabel.Text = selectedAssessment.Alerts;
        AssessmentDescriptionLabel.Text = selectedAssessment.Description;
    }

    private async void EditAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // go to edit assessment page
        await Shell.Current.GoToAsync(nameof(EditAssessmentPage));
    }

    private async void DeleteAssessmentButton_Clicked(object sender, EventArgs e)
    {
        // confirm delete
        bool result = await DisplayAlert("Confirm", "Are you sure you want to delete this assessment?", "YES", "NO");
        if (result == false)
        {
            return;
        }
        else
        {
            // delete assessment
            DataAccessService.DeleteAssessment(selectedAssessment.Id);

            // return to course page
            await Shell.Current.GoToAsync("..");
        }
    }
}