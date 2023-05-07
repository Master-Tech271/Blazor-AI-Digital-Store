using DataModels.Models;
using DataModels.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<UserModel> Login(UserModel user)
        {
            try
            {
                // Serialize the user object as JSON
                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Login", content);

                // If the response status code indicates success, return true
                if (response.IsSuccessStatusCode)
                {
                    user = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync()) ?? new UserModel();
                }

                return user;
            }
            catch (Exception ex)
            {
                return new UserModel();
            }
        }


        public async Task<bool> Register(UserModel user)
        {
            try
            {
                // Serialize the user object as JSON
                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/RegisterUser", content);

                // If the response status code indicates success, return true
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // If the response status code indicates an error, log the response and return false
                var errorMessage = await response.Content.ReadAsStringAsync();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<List<ProductModel>> Products(bool Recommended = false)
        {
            List<ProductModel> Products = new List<ProductModel>();
            try
            {
                Helper.InitializeHeader(_httpClient);
                var response = await _httpClient.GetAsync($"api/Products?Recommended={Recommended}");

                // If the response status code indicates success, return true
                if (response.IsSuccessStatusCode)
                {
                    Products = JsonConvert.DeserializeObject<List<ProductModel>>(await response.Content.ReadAsStringAsync()) ?? new();
                }

            }
            catch (Exception ex)
            {
            }
            return Products;
        }

        public async Task<bool> PlaceOrder(OrderModel order)
        {
            Helper.InitializeHeader(_httpClient);
            try
            {
                var response = await _httpClient.PostAsJsonAsync<OrderModel>("api/PlaceOrder", order);

                // If the response status code indicates success, return true
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // If the response status code indicates an error, log the response and return false
                var errorMessage = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public async Task<List<OrderHistoryModel>> OrderHistory()
        {
            Helper.InitializeHeader(_httpClient);
            List<OrderHistoryModel> PlaceOrders = new List<OrderHistoryModel>();
            try
            {
                PlaceOrders = await _httpClient.GetFromJsonAsync<List<OrderHistoryModel>>("api/OrderHistory") ?? new();


            }
            catch (Exception ex)
            {
            }
            return PlaceOrders;
        }

        public async Task<bool> UpsertFeedback(OrderFeedbackModel orderFeedback)
        {
            Helper.InitializeHeader(_httpClient);
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/UpsertFeedback", orderFeedback);

                // If the response status code indicates success, return true
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // If the response status code indicates an error, log the response and return false
                var errorMessage = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
            }

            return false;
        }
    }
}
