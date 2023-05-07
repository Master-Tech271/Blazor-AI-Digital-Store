using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class UserModel
    {
        public int Id { get; set; } = 0;
        [Required, StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(8)]
        public string Password { get; set; } = string.Empty;

    }
}
