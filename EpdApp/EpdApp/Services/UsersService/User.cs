namespace EpdApp.Services.UsersService
{
    /// <summary>
    /// Роли пользователей: водитель или инспектор
    /// </summary>
    public enum UserRoles 
    {
        Driver = 0,
        Police = 1
    }

    /// <summary>
    /// Класс пользователя
    /// </summary>
    public class User
    {
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
        /// Роль пользователя
        /// </summary>
        public UserRoles Role { get; set; }

        public User(string name, string login, string password, UserRoles role)
        {
            Name = name;
            Login = login;
            Password = password;
            Role = role;
        }
    }
}
