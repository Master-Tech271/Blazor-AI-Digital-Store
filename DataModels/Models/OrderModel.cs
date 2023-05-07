using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class OrderModel
    {
        public int Id { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }

    public class OrderFeedbackModel
    {
        public int Id { get; set; } = 0;
        public int ProductId { get; set; } = 0;
        public int UserId { get; set; } = 0;
        [Required, StringLength(500)]
        public string Feedback { get; set; } = string.Empty;
        [Range(0, 5)]
        public int ReviewStar { get; set; } = 0;

    }

    public class OrderHistoryModel
    {
        public int ProductId { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public string ProductImage { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string OrderCount { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public int ReviewStar { get; set; } = 0;
    }
}
