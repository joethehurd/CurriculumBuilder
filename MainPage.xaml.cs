using CommunityToolkit.Maui.Markup;
using MobileApplication.Models;
using MobileApplication.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MobileApplication
{
    public partial class MainPage : ContentPage
    {
        public static IList<Term> termList;                
        public static bool firstLoad = true;

        public MainPage()
        {
            InitializeComponent();
            InitializeTermList();              
        }

        protected override void OnAppearing()
        {
            if (firstLoad == true)
            {
                InitializeTermList();                
            }
            else
            {
                RefreshTermList();
            }

            firstLoad = false;
        }

        public void InitializeTermList()
        {
            RefreshTermList();

            if (!termList.Any())
            {
                // database is empty, populate with default data                
                DataAccessService.PopulateTables();
                RefreshTermList();
            }                  
        }

        public void RefreshTermList()
        {
            // populate term list
            termList = DataAccessService.GetAllTerms();

            // populate xaml collection view with list items
            TermCollectionView.ItemsSource = termList;
        }              

        private async void AddTermButton_Clicked(object sender, EventArgs e)
        {                      
            // check term limit
            if (termList.Count == 12)
            {                
                await DisplayAlert("Alert", "Term limit reached.", "OK");
                return;
            }
            else
            {
                // go to add term page
                await Shell.Current.GoToAsync(nameof(AddTermPage));             
            }                      
        }

        private async void ViewTermButton_Clicked(object sender, EventArgs e)
        {
            // gets the id of the selected term            
            var tempId = (Button)sender;           
          
            // passes the selected Term's ID to the View Term Page
            await Shell.Current.GoToAsync($"{nameof(ViewTermPage)}?id={tempId.CommandParameter}");
        }
    }
}