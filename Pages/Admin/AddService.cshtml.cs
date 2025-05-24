using Laundrify.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Laundrify.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddServiceModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Service Service { get; set; }
        public string Message { get; set; }
        public AddServiceModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet() { }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fill all fields correctly.";
                return Page();
            }
            _db.Services.Add(Service);
            _db.SaveChanges();
            return RedirectToPage("/Admin/Services");
        }
    }
} 