using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Laundrify.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditOrderModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Order Order { get; set; }
        public EditOrderModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet(int id)
        {
            Order = _db.Orders.Include(o => o.Client).Include(o => o.Service).FirstOrDefault(o => o.Id == id);
            if (Order == null)
                return RedirectToPage("/Admin/Orders");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new { success = false, message = "Invalid model state" });
                }
                return Page();
            }

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == Order.Id);
            if (order == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new { success = false, message = "Order not found" });
                }
                return RedirectToPage("/Admin/Orders");
            }

            order.Status = Order.Status;
            await _db.SaveChangesAsync();

            // Check if the request is AJAX
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new JsonResult(new { success = true, message = "Order status updated successfully" });
            }

            return RedirectToPage("/Admin/Orders");
        }
    }
} 