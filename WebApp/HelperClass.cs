using DataModels;
using DataModels.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Text.Json;

namespace WebApp
{
    public class HelperClass
    {
        private readonly NavigationManager _navigationManager;
        public static NotificationService NotificationService { get; set; } = default!;
        private readonly IJSRuntime _jsRuntime;
        public static bool IsAuthorized { get; private set; } = false;

        public const Variant InputVariant = Variant.Flat;

        public static bool IsLoading { get; set; } = false;
        public static Action AppStateChanged { get; set; } = default!;

        public HelperClass(NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
        }

        public void NavigateTo(string url)
        {
            _navigationManager.NavigateTo(url);
        }

        public async Task LogoutAsync()
        {
            await ClearAsync();
            IsAuthorized = false;
            NavigateTo(AppConstants.Route.Login);
        }



        public async Task<bool> IsAuthorizedAsync(bool redirectToLogin = true)
        {
            UserModel user = await GetItemAsync<UserModel>(AppConstants.Authentication.LoginKey) ?? new UserModel();
            if (user.Id > 0)
            {
                IsAuthorized = true;
                Helper.User = user;
                return true;
            }
            if (redirectToLogin)
                NavigateTo(AppConstants.Route.Login);
            IsAuthorized = false;
            return false;
        }

        public void ShowNotification(NotificationSeverity notificationType, string message, string Tittle = null!, int duration = 4000)
        {
            if (NotificationService is null) return;
            if(Tittle is null)
            {
                switch(notificationType)
                {
                    case NotificationSeverity.Success:
                        Tittle = "Success";
                        break;
                    case NotificationSeverity.Error:
                        Tittle = "Error";
                        break;
                }
            }
            NotificationMessage notificationMessage = new NotificationMessage { Severity = notificationType, Summary = Tittle, Detail = message, Duration = duration };
            NotificationService.Notify(notificationMessage);
        }


        public async Task SetItemAsync<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.clear");
        }

        public void ShowLoader()
        {
            IsLoading = true;
            AppStateChanged?.Invoke();
        }
        public void HideLoader()
        {
            IsLoading = false;
            AppStateChanged?.Invoke();
        }
    }
}
