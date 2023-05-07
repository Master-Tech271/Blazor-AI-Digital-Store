using DataModels;
using DataModels.Models;
using DataModels.Services;
using DataModels.Services.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebApp.Components
{
    public partial class Feedback
    {
        [Inject]
        public HelperClass HelperClass { get; set; } = default!;
        [Inject]
        public IUserService UserService { get; set; } = default!;
        [Inject]
        public DialogService DialogService { get; set; } = default!;
        [Parameter]
        public OrderFeedbackModel OrderFeedbackModel { get; set; } = new();

        public Variant Variant { get; set; } = HelperClass.InputVariant;

        public async Task SubmitAsync()
        {
            var result = await UserService.UpsertFeedback(OrderFeedbackModel);
            if (result)
            {
                HelperClass.ShowNotification(NotificationSeverity.Success, AppConstants.Notification.Success);
            }
            else
            {
                HelperClass.ShowNotification(NotificationSeverity.Error, AppConstants.Notification.ServerError);
            }
            DialogService.Close();
        }
    }
}
