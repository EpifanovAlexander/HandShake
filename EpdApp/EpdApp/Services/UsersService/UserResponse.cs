namespace EpdApp.Services.UsersService
{
    /// <summary>
    /// Данные о пользователе, полученные с сервера
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Строковое представление роли пользователя
        /// </summary>
        public string Role { get; set; }

        public UserResponse(long id, string name, string login, string password, string role)
        {
            Id = id;
            Name = name;
            Login = login;
            Password = password;
            Role = role;
        }
    }
}
