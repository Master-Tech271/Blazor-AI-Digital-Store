using DataModels.Models;
using DataModels.Services.Interface;
using DataModels;
using Microsoft.AspNetCore.Components;
using Radzen;
using WebApp.Components;

namespace WebApp.Pages
{
    public partial class OrderHistory
    {
        [Inject]
        public HelperClass HelperClass { get; set; } = default!;
        [Inject]
        public DialogService DialogService { get; set; } = default!;
        [Inject]
        public IUserService UserService { get; set; } = default!;
        public List<OrderHistoryModel> OrderHistoryModel { get; set; } = new List<OrderHistoryModel>();
        private AlignItems alignItems = AlignItems.Normal;
        private JustifyContent justifyContent = JustifyContent.Normal;
        private string gap = "1rem";
        protected override async Task OnInitializedAsync()
        {
            await HelperClass.IsAuthorizedAsync();
            HelperClass.ShowLoader();
            OrderHistoryModel = await UserService.OrderHistory();
            HelperClass.HideLoader();
            StateHasChanged();
            await base.OnInitializedAsync();
        }

        public async Task FeedbackAsync(OrderHistoryModel orderHistoryModel)
        {
            OrderFeedbackModel OrderFeedbackModel = new OrderFeedbackModel
            {
                UserId = orderHistoryModel.UserId,
                ProductId = orderHistoryModel.ProductId,
                Feedback = orderHistoryModel.Feedback,
                ReviewStar = orderHistoryModel.ReviewStar,
            };
            await DialogService.OpenAsync<Feedback>($"Feedback {orderHistoryModel.ProductName}", new Dictionary<string, object>() { { "OrderFeedbackModel", OrderFeedbackModel } } ,
              new DialogOptions() { Width = "700px", Height = "512px", Resizable = false, Draggable = false });
            HelperClass.ShowLoader();
            OrderHistoryModel = await UserService.OrderHistory();
            HelperClass.HideLoader();
        }

        public async Task OrderNowAsync(ProductModel Product)
        {
            HelperClass.ShowLoader();
            OrderModel Order = new OrderModel
            {
                ProductId = Product.Id,
                UserId = Helper.User.Id,
                Price = Product.Price
            };
            var result = await UserService.PlaceOrder(Order);
            if (result)
            {
                HelperClass.ShowNotification(NotificationSeverity.Success, AppConstants.Notification.OrderPlacedSuccessfully);
            }
            else
            {
                HelperClass.ShowNotification(NotificationSeverity.Error, AppConstants.Notification.ServerError);
            }
            HelperClass.HideLoader();
        }
    }
}
