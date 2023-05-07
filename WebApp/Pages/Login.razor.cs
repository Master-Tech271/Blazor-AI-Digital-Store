using DataModels;
using DataModels.Models;
using DataModels.Services.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;

namespace WebApp.Pages
{
    public partial class Login
    {
        [Inject]
        public HelperClass HelperClass { get; set; } = default!;
        [Inject]
        public IUserService UserService { get; set; } = default!;
        public UserModel User { get; set; } = new UserModel();
        public bool RememberMe { get; set; } = true;
        public bool IsLogin { get; set; } = true;
        public Variant Variant { get; set; } = HelperClass.InputVariant;
        public string CardClass
        {
            get
            {
                if (IsLogin)
                {
                    return "rz-my-12 rz-mx-auto rz-p-2 rz-p-md-12";
                }
                else
                {
                    return "rz-my-0 rz-mx-auto rz-p-2 rz-p-md-12";
                }
            }
        }
        protected override Task OnInitializedAsync()
        {
            if (HelperClass.IsAuthorized) 
                HelperClass.NavigateTo(AppConstants.Route.Home);
            return base.OnInitializedAsync();
        }



        async Task OnLoginAsync(LoginArgs args)
        {
            HelperClass.ShowLoader();
            User.Email = args.Username;
            User.Password = args.Password;
            var result = await UserService.Login(User);
            if (result.Id > 0)
            {
                await HelperClass.SetItemAsync(AppConstants.Authentication.LoginKey, result);
                await HelperClass.IsAuthorizedAsync(); 
                User = new();
                HelperClass.ShowNotification(NotificationSeverity.Success, AppConstants.Notification.LoginSuccessfully);
                HelperClass.NavigateTo(AppConstants.Route.Home);
            }
            else
            {
                HelperClass.ShowNotification(NotificationSeverity.Error, AppConstants.Notification.InvalidUsernameOrPassword);
            }
            HelperClass.HideLoader();
        }

        void OnRegister()
        {
            IsLogin = !IsLogin;
        }

        async Task RegisterAsync()
        {
            HelperClass.ShowLoader();
            var result = await UserService.Register(User);
            if (result)
            {
                User = new ();
                OnRegister();
                HelperClass.ShowNotification(NotificationSeverity.Success, AppConstants.Notification.RegisterSuccessfully);                
            }
            else
            {
                HelperClass.ShowNotification(NotificationSeverity.Error, AppConstants.Notification.ServerError);
            }
            HelperClass.HideLoader();
        }

        void OnResetPassword()
        {

        }
    }
}
