using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Laundrify.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditServiceModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Service Service { get; set; }
        public string Message { get; set; }
        public EditServiceModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet(int id)
        {
            Service = _db.Services.FirstOrDefault(s => s.Id == id);
            if (Service == null)
                return RedirectToPage("/Admin/Services");
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fill all fields correctly.";
                return Page();
            }
            var service = _db.Services.FirstOrDefault(s => s.Id == Service.Id);
            if (service == null)
                return RedirectToPage("/Admin/Services");
            service.Name = Service.Name;
            service.Price = Service.Price;
            service.UnitType = Service.UnitType;
            _db.SaveChanges();
            return RedirectToPage("/Admin/Services");
        }
    }
} 