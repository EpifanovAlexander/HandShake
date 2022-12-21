using EpdApp.Services.UsersService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;

namespace EpdApp.Services.DocumentsService
{
    public class DocumentsService
    {
        public static Document Document = null;

        internal static IEnumerable<string> GetDocuments()
        {
            var result = new List<string>();
            result.Add("Passport");
            return result;
        }

        /*public static void DownloadDocuments()
        {
            User user = null;

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://178.172.227.162:8080/user/auth"); //тут URL
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
        }*/
    }
}