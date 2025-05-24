using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Laundrify.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageOrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ManageOrdersModel> _logger;

        public ManageOrdersModel(ApplicationDbContext context, ILogger<ManageOrdersModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} status updated to {Status}", orderId, status);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteOrderAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    _logger.LogWarning($"Order not found with ID: {id}");
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToPage();
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully deleted order {id}");
                TempData["SuccessMessage"] = "Order deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {OrderId}: {Message}", id, ex.Message);
                TempData["ErrorMessage"] = "An error occurred while deleting the order.";
            }

            return RedirectToPage();
        }
    }
} 