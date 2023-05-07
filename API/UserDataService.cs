using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SQLORM;

namespace API
{
    public class UserDataService
    {
        private readonly ILogger<UserDataService> _logger;

        public UserDataService(ILogger<UserDataService> log)
        {
            _logger = log;
        }

       
        #region User Api


        [FunctionName("RegisterUser")]
        [OpenApiOperation(operationId: "RegisterUser", tags: new[] { "user" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDataService), Required = true, Description = "The user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> RegisterUser(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "RegisterUser")] HttpRequest req,
    ILogger log)
        {
            try
            {
                // Read the request body and deserialize the user object
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(requestBody);

                // Insert the user into the database
                var sql = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password)";
                var rowsAffected = SQLData.Execute(sql, user);

                // Return a success message
                return new OkObjectResult($"User '{user.FirstName} {user.LastName}' registered successfully!");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error registering user");
                return new StatusCodeResult(500);
            }
        }

        //login
        [FunctionName("Login")]
        [OpenApiOperation(operationId: "Login", tags: new[] { "login" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDataService), Required = true, Description = "The user object")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Login(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Login")] HttpRequest req,
    ILogger log)
        {
            try
            {
                /*if (!HelperClass.ValidateToken(req))
                    return new UnauthorizedResult();*/
                // Read the request body and deserialize the user object
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(requestBody);

                // Insert the user into the database
                var sql = "SELECT TOP 1 * FROM Users WHERE Email=@Email and Password=@Password";
                user = SQLData.QuerySingleOrDefault<UserModel>(sql, user) ?? new UserModel();


                // Return a success message
                return new OkObjectResult(JsonConvert.SerializeObject(user));
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error registering user");
                return new StatusCodeResult(500);
            }
        }



        [FunctionName("Products")]
        [OpenApiOperation(operationId: "Products", tags: new[] { "product" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiParameter(name: "Recommended", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "The **Recommended** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Products(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Products")] HttpRequest req,
    ILogger log)
        {
            try
            {
                bool Recommended = Convert.ToBoolean(req.Query["Recommended"].ToString());
                var User = HelperClass.ValidateToken(req);
                if (User.Id <= 0)
                    return new UnauthorizedResult();
                string sql = string.Empty;
                List<ProductModel> Products = new();
                if (Recommended)
                {
                    sql = "SELECT * FROM Product WHERE Id IN (select ProductId from RecommendedProducts WHERE UserId = @Id)";
                    Products = SQLData.Query<ProductModel>(sql, User).ToList();
                }
                else
                {
                    sql = "SELECT * FROM Product";
                    Products = SQLData.Query<ProductModel>(sql).ToList();
                }



                // Return a success message
                return new OkObjectResult(JsonConvert.SerializeObject(Products));
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Products Error!");
                return new StatusCodeResult(500);
            }
        }



        [FunctionName("PlaceOrder")]
        [OpenApiOperation(operationId: "PlaceOrder", tags: new[] { "placeorder" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> PlaceOrder(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "PlaceOrder")] HttpRequest req,
    ILogger log)
        {
            try
            {
                if (HelperClass.ValidateToken(req).Id <= 0)
                    return new UnauthorizedResult();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(requestBody);

                var sql = "INSERT INTO Orders (ProductId, UserId, Price) VALUES (@ProductId, @UserId, @Price);";
                int count = SQLData.Execute(sql, order);


                // Return a success message
                return new OkObjectResult(count > 0 ? true : false);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Order Not Placed!");
                return new StatusCodeResult(500);
            }
        }

        //UpsertFeedback
        [FunctionName("UpsertFeedback")]
        [OpenApiOperation(operationId: "UpsertFeedback", tags: new[] { "upsertfeedback" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> UpsertFeedback(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpsertFeedback")] HttpRequest req,
    ILogger log)
        {
            try
            {
                if (HelperClass.ValidateToken(req).Id <= 0)
                    return new UnauthorizedResult();

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var order = JsonConvert.DeserializeObject<OrderFeedbackModel>(requestBody);

                var sql = "MERGE INTO OrderFeedback AS target USING (SELECT @ProductId, @UserId, @Feedback, @ReviewStar) AS source(ProductId, UserId, Feedback, ReviewStar) ON target.ProductId = source.ProductId AND target.UserId = source.UserId WHEN MATCHED THEN UPDATE SET target.Feedback = source.Feedback, target.ReviewStar = source.ReviewStar WHEN NOT MATCHED THEN INSERT (ProductId, UserId, Feedback, ReviewStar) VALUES (source.ProductId, source.UserId, source.Feedback, source.ReviewStar);";
                int count = SQLData.Execute(sql, order);


                // Return a success message
                return new OkObjectResult(count > 0 ? true : false);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Order Not Placed!");
                return new StatusCodeResult(500);
            }
        }


        [FunctionName("OrderHistory")]
        [OpenApiOperation(operationId: "OrderHistory", tags: new[] { "orderhistory" })]
        [OpenApiSecurity("basic_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Basic)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> OrderHistory(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "OrderHistory")] HttpRequest req,
    ILogger log)
        {
            List<OrderHistoryModel> PlaceOrders = new();
            try
            {
                var User = HelperClass.ValidateToken(req);
                if (User.Id <= 0)
                    return new UnauthorizedResult();

                var sql = "SELECT p.Id ProductId, o.UserId, SUM(o.Price) TotalPrice, p.Image ProductImage, p.Name ProductName, p.Description ProductDescription, COUNT(o.ProductId) AS OrderCount, f.Feedback, f.ReviewStar FROM Orders o INNER JOIN Product p ON o.ProductId = p.Id LEFT JOIN OrderFeedback f ON o.UserId = f.UserId AND o.ProductId = f.ProductId WHERE o.Userid = @Id GROUP BY p.Id, o.UserId, o.Price, p.Image, p.Name, p.Description, f.Feedback, f.ReviewStar ORDER BY p.Id DESC";

                PlaceOrders = SQLData.Query<OrderHistoryModel>(sql, User).ToList();


                // Return a success message
                return new OkObjectResult(PlaceOrders);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Order Not Placed!");
                return new StatusCodeResult(500);
            }
        }


        #endregion



    }
}

