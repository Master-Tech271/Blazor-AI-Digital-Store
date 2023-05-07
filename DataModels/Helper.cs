using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class Helper
    {
        public static UserModel User = new UserModel();
        public static void InitializeHeader(HttpClient httpClient)
        {
            if (!string.IsNullOrEmpty(User.Email) && !string.IsNullOrEmpty(User.Password))
            {
                var authBytes = System.Text.Encoding.UTF8.GetBytes($"{User.Email}:{User.Password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authBytes));
            }
        }
    }

    public enum NotificationType
    {
        Success,
        Error
    }
}
