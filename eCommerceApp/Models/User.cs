using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }  
        public string Email { get; set; }  
        public string PasswordHash { get; set; }  
    }
}
