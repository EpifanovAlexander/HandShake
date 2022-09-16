using EpdApp.Services.UsersService;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EpdApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки входа на страницу пользователя. 
        /// Запрашивает у сервера пользователя по введённому логину и паролю
        /// </summary>
        private async void LoginActionHandler(object sender, EventArgs e)
        {
            var login = LoginEntry.Text;
            var pass = PassEntry.Text;

            try
            {
                User user = await UserService.Instance.GetUser(login, pass);
                if (user != null)
                {
                    UserService.CreateUser((user.Role == UserRoles.Police) ? "POLICE" : "DRIVER");
                    await Navigation.PushAsync(new UserPage(user.Role));
                    Navigation.RemovePage(this);
                }
            }
            catch (Exception) 
            {
                await DisplayAlert("Ошибка", "Связь с сервером оборвалась...", "ОK");
            }
        }
    }
}