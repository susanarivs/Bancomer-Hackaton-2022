using Xamarin.Forms;

namespace WhiteLabel
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = GetMainPage();
        }

        public static Page GetMainPage()
        {
            return new NavigationPage(new ChatMainPage());
        }
    }
}
