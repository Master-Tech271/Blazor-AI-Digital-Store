

using DataModels;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebApp.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public HelperClass HelperClass { get; set; } = default!;
        [Inject]
        public NotificationService NotificationService { get; set; } = default!;
        public bool SidebarExpanded { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            HelperClass.NotificationService = NotificationService;
            if (!await HelperClass.IsAuthorizedAsync())
            {
                HelperClass.NavigateTo(AppConstants.Route.Login);
            }
            HelperClass.AppStateChanged += () => StateHasChanged();
            await base.OnInitializedAsync();
        }
    }
}
