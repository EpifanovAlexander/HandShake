using System;
using System.Net;
using System.Net.Http;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace EpdApp.Services.UsersService
{
    /// <summary>
    /// Сервис взаимодействия с пользователем
    /// </summary>
    public class UserService
    {
        private static UserService instance;
        public static UserService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserService();
                }
                return instance;
            }
        }

        private UserService() 
        { 
        }

        /// <summary>
        /// Сохраняет роль пользователя в параметрах приложения
        /// </summary>
        /// <param name="role"> Роль пользователя </param>
        public static void CreateUser(string role)
        {
            Preferences.Set("userrole", $"{role}");
        }

        /// <summary>
        /// Вовращает роль пользователя из параметров приложения
        /// </summary>
        /// <returns> Возвращает строковое представление роли или пустую строку, если пользователь ещё не авторизован </returns>
        public static string GetUserRole()
        {
            return Preferences.ContainsKey("userrole") ? Preferences.Get("userrole", "") : "";
        }

        /// <summary>
        /// Проверяет существует ли пользователь в параметрах приложения
        /// </summary>
        public static bool IsUserExist()
        {
            return Preferences.ContainsKey("userrole");
        }

        /// <summary>
        /// Удаляет пользователя из параметров приложения
        /// </summary>
        public static void LogOut()
        {
            Preferences.Remove("userrole");
        }

        /// <summary>
        /// Возвращает пользователя с сервера
        /// </summary>
        /// <param name="login"> Введённый логин </param>
        /// <param name="password"> Введённый пароль </param>
        /// <returns></returns>
        public async Task<User> GetUser(string login, string password)
        {
            User user = null;

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://178.172.227.162:8080/user/auth");
            UserRequest userReq = new UserRequest(login, password);
            string userReqJson = JsonConvert.SerializeObject(userReq);
            request.Content = new StringContent(userReqJson, Encoding.UTF8, "application/json");
            request.Content.Headers.Add("user", userReqJson);
            request.Method = HttpMethod.Post;
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var userResponseJson = await responseContent.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserResponse>(userResponseJson);
                user = new User(userResponse.Name, userResponse.Login, userResponse.Password, (userResponse.Role == "INSPECTOR") ? UserRoles.Police : UserRoles.Driver);
            }
            return user;
        }
    }
}
