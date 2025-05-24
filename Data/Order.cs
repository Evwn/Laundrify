using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Laundrify.Models;

namespace Laundrify.Data
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public User Client { get; set; }
        [Required]
        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public string Status { get; set; } = "Pending";
        [Required]
        public DateTime DropOffDate { get; set; }
        [Required]
        public DateTime PickUpDate { get; set; }
    }
} 