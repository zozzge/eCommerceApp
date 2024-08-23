using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }  
        public string Email { get; set; }  
        public string PasswordHash { get; set; }  
    }
}
