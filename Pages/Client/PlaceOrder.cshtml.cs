using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Laundrify.Pages.Client
{
    [Authorize(Roles = "Client")]
    public class PlaceOrderModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PlaceOrderModel> _logger;

        [BindProperty]
        public int ServiceId { get; set; }
        [BindProperty]
        public decimal Quantity { get; set; }
        [BindProperty]
        public DateTime DropOffDate { get; set; }
        [BindProperty]
        public DateTime PickUpDate { get; set; }
        public string Message { get; set; }

        public PlaceOrderModel(ApplicationDbContext db, ILogger<PlaceOrderModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid: {ModelState}", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    Message = "Please fill all fields correctly.";
                    return Page();
                }

                if (ServiceId == 0 || Quantity <= 0 || DropOffDate == default || PickUpDate == default)
                {
                    _logger.LogWarning("Invalid input values: ServiceId={ServiceId}, Quantity={Quantity}, DropOffDate={DropOffDate}, PickUpDate={PickUpDate}",
                        ServiceId, Quantity, DropOffDate, PickUpDate);
                    Message = "Please fill all fields correctly.";
                    return Page();
                }

                var userEmail = User.Identity?.Name;
                _logger.LogInformation("Attempting to place order for user: {UserEmail}", userEmail);

                var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {UserEmail}", userEmail);
                    return RedirectToPage("/Account/Login");
                }

                var service = _db.Services.FirstOrDefault(s => s.Id == ServiceId);
                if (service == null)
                {
                    _logger.LogWarning("Service not found for ID: {ServiceId}", ServiceId);
                    Message = "Selected service not found.";
                    return Page();
                }

                var order = new Order
                {
                    ClientId = user.Id,
                    ServiceId = ServiceId,
                    Quantity = Quantity,
                    Status = "Pending",
                    DropOffDate = DropOffDate,
                    PickUpDate = PickUpDate
                };

                _logger.LogInformation("Creating new order: ClientId={ClientId}, ServiceId={ServiceId}, Quantity={Quantity}",
                    order.ClientId, order.ServiceId, order.Quantity);

                _db.Orders.Add(order);
                var result = await _db.SaveChangesAsync();
                _logger.LogInformation("Save result: {Result} rows affected", result);

                return RedirectToPage("/Client/MyOrders");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while placing order");
                Message = "An error occurred while saving your order. Please try again.";
                return Page();
            }
        }
    }
} 