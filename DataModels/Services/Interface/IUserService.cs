using DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Services.Interface
{
    public interface IUserService
    {
        public Task<UserModel> Login(UserModel user);
        public Task<bool> Register(UserModel user);
        public Task<List<ProductModel>> Products(bool Recommended = false);
        public Task<bool> PlaceOrder(OrderModel order);
        public Task<List<OrderHistoryModel>> OrderHistory();
        public Task<bool> UpsertFeedback(OrderFeedbackModel orderFeedback);
    }
}
