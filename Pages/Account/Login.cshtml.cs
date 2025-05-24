using Laundrify.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Laundrify.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(AuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Attempting login for user: {Email}", Input.Email);
                
                var result = await _authService.LoginAsync(Input.Email, Input.Password);
                if (result)
                {
                    _logger.LogInformation("User {Email} logged in successfully", Input.Email);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for user: {Email}", Input.Email);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
} 