using MobileApplication.Models;
using MobileApplication.Services;
using Microsoft.Maui.Controls;

namespace MobileApplication;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();
	}

    private async void SaveTermButton_Clicked(object sender, EventArgs e)
    {
        // validation
        if (string.IsNullOrWhiteSpace(TermNameEntry.Text))
        {
            // Term Name Entry = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(TermStartDatePicker.Date.ToString()))
        {
            // Term Start Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(TermEndDatePicker.Date.ToString()))
        {
            // Term End Date = Empty
            await DisplayAlert("Alert", "All fields with * must be completed.", "OK");
            return;
        }
        if (TermStartDatePicker.Date >= TermEndDatePicker.Date)
        {
            // Term Start Date is after or on End Date
            await DisplayAlert("Alert", "Start Date must be before End Date.", "OK");
            return;
        }

        // add term
        DataAccessService.AddTerm(new Guid(), TermNameEntry.Text, TermStartDatePicker.Date, TermEndDatePicker.Date);

        // return to home page
        await Shell.Current.GoToAsync("..");
    }

    private async void CancelTermButton_Clicked(object sender, EventArgs e)
    {
        // return to home page
        await Shell.Current.GoToAsync("..");
    }  
}