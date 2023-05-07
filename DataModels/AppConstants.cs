using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    public class AppConstants
    {
        public const string AppName = "AI Digital Store";
        public const string AppDevelopedBy = "&copy; Developed By <a href='https://github.com/Master-Tech271' target='_blank'>Mohd Umar</a>";
        public class Authentication
        {
            public const string LoginKey = "user";
        }
        public class Product
        {
            public const string Currency = "Rs";
        }
        public class Notification
        {
            public const string Success = "Success!";
            public const string RegisterSuccessfully = "Register Successfully, Now you can login!";
            public const string OrderPlacedSuccessfully = "Order Placed Successfully!";
            public const string LoginSuccessfully = "Login Successfully!";
            public const string ServerError = "Server Error!";
            public const string InvalidUsernameOrPassword = "Invalid Username or Password!";
        }

        public class Route
        {
            public const string Login = "/login";
            public const string Products = "/";
            public const string OrderHistory = "/orders";
            public const string Home = Products;//"/";
            public const string About = "/about";
        }
    }
}
