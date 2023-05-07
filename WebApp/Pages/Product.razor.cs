using DataModels;
using DataModels.Models;
using DataModels.Services.Interface;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebApp.Pages
{
    public partial class Product
    {
        [Inject]
        public HelperClass HelperClass { get; set; } = default!;
        [Inject]
        public IUserService UserService { get; set; } = default!;
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        public List<ProductModel> RecommendedProducts { get; set; } = new List<ProductModel>();
        private AlignItems alignItems = AlignItems.Normal;
        private JustifyContent justifyContent = JustifyContent.Normal;
        private string gap = "1rem";
        protected override async Task OnInitializedAsync()
        {
            await HelperClass.IsAuthorizedAsync();
            HelperClass.ShowLoader();
            StateHasChanged();
            Products = await UserService.Products();
            RecommendedProducts = await UserService.Products(true);
            HelperClass.HideLoader();
            StateHasChanged();
            await base.OnInitializedAsync();
        }

        public async Task OrderNowAsync(ProductModel Product)
        {
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
                HelperClass.NavigateTo(AppConstants.Route.OrderHistory);
            }
            else
            {
                HelperClass.ShowNotification(NotificationSeverity.Error, AppConstants.Notification.ServerError);
            }
        }
    }
}
