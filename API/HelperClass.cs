using DataModels.Models;
using Microsoft.AspNetCore.Http;
using SQLORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    internal class HelperClass
    {
        public static UserModel ValidateToken(HttpRequest req)
        {
            string header = Convert.ToString(req.Headers["Authorization"]);
            //Checking the header
            if (!string.IsNullOrEmpty(header) && header.StartsWith("Basic"))
            {
                //Extracting credentials
                // Removing "Basic " Substring
                string encodedUsernamePassword = header.Substring("Basic ".Length).Trim();
                //Decoding Base64
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                //Splitting Username:Password
                int seperatorIndex = usernamePassword.IndexOf(':');
                // Extracting the individual username and password
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);
                //Validating the credentials
                return IsValidUser(username, password);
            }
            else
            {
                return new UserModel();
            }
        }

        private static UserModel IsValidUser(string username, string password)
        {
            UserModel user = new UserModel { Email = username, Password = password };
            var sql = "SELECT TOP 1 * FROM Users WHERE Email=@Email and Password=@Password";
            user = SQLData.QuerySingleOrDefault<UserModel>(sql, user) ?? new UserModel();
            return user;
        }
    }
}
