using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Laundrify.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditOrderModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<EditOrderModel> _logger;

        [BindProperty]
        public Order Order { get; set; }

        public EditOrderModel(ApplicationDbContext db, ILogger<EditOrderModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult OnGet(int id)
        {
            Order = _db.Orders.Include(o => o.Client).Include(o => o.Service).FirstOrDefault(o => o.Id == id);
            if (Order == null)
                return RedirectToPage("/Admin/Orders");
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Received order update request");

            try
            {
                // Read the request body
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation($"Received request body: {body}");

                var json = JsonDocument.Parse(body);
                var orderElem = json.RootElement.GetProperty("Order");
                var orderId = orderElem.GetProperty("Id").GetInt32();
                var newStatus = orderElem.GetProperty("Status").GetString();

                _logger.LogInformation($"Processing update for Order ID: {orderId}, New Status: {newStatus}");

                var order = await _db.Orders.FindAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order not found with ID: {orderId}");
                    return new JsonResult(new { success = false, message = "Order not found" });
                }

                _logger.LogInformation($"Updating order {order.Id} status from {order.Status} to {newStatus}");
                order.Status = newStatus;
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated order {order.Id} status to {newStatus}");

                return new JsonResult(new { success = true, message = "Order updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order: {Message}", ex.Message);
                return new JsonResult(new { success = false, message = $"Error updating order: {ex.Message}" });
            }
        }
    }
} 