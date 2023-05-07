using DataModels.Models;
using Microsoft.AspNetCore.Components;

namespace WebApp.Components
{
    public partial class ProductCard
    {
        [Parameter]
        public ProductModel Product { get; set; } = new ProductModel();
        [Parameter]
        public EventCallback<ProductModel> OrderNow { get; set; } = default!;


        public async Task OrderNowAsync(ProductModel Product)
        {
            await OrderNow.InvokeAsync(Product);
        }
    }
}
