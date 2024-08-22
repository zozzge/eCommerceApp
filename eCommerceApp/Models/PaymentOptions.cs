﻿using System.ComponentModel.DataAnnotations;

namespace eCommerceApp.Models
{
    public class PaymentOptions
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
