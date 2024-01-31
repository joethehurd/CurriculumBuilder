namespace MobileApplication
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
                 
            Routing.RegisterRoute(nameof(ViewTermPage), typeof (ViewTermPage));
            Routing.RegisterRoute(nameof(ViewCoursePage), typeof (ViewCoursePage));
            Routing.RegisterRoute(nameof(ViewAssessmentPage), typeof (ViewAssessmentPage));
            Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
            Routing.RegisterRoute(nameof(AddCoursePage), typeof(AddCoursePage));
            Routing.RegisterRoute(nameof(AddAssessmentPage), typeof(AddAssessmentPage));
            Routing.RegisterRoute(nameof(EditTermPage), typeof(EditTermPage));
            Routing.RegisterRoute(nameof(EditCoursePage), typeof(EditCoursePage));
            Routing.RegisterRoute(nameof(EditAssessmentPage), typeof(EditAssessmentPage));           
        }
    }
}