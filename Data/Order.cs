using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laundrify.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public User Client { get; set; } = null!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime DropOffDate { get; set; }
        public DateTime PickUpDate { get; set; }
    }
} 