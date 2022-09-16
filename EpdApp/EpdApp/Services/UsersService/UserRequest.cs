namespace EpdApp.Services.UsersService
{
    /// <summary>
    /// Данные о пользователе для отправки на сервер
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// Введённый логин пользователя
        /// </summary>
        public string login { get; set; }

        /// <summary>
        /// Введённый пароль пользователя
        /// </summary>
        public string password { get; set; }

        public UserRequest(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
