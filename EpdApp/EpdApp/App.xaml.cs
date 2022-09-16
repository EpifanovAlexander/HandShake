using EpdApp.Services.UsersService;
using EpdApp.Services.XmlsService;
using EpdApp.Views;
using Xamarin.Forms;

namespace EpdApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XmlService.Instance.ConfigureXmlFolders();

            // Определяем начальную страницу в зависимости от сохранённой в параметрах роли пользователя
            if (UserService.IsUserExist())
            {
                var role = UserService.GetUserRole();
                MainPage = new NavigationPage(new UserPage((role == "POLICE") ? UserRoles.Police : UserRoles.Driver));
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
