using FootballShare.DAL.Repositories;
using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<SiteUser> _signInManager;
        private readonly UserManager<SiteUser> _userManager;
        private readonly ISiteUserRepository _userRepo;

        public IndexModel(
            UserManager<SiteUser> userManager,
            SignInManager<SiteUser> signInManager,
            IEmailSender emailSender,
            ISiteUserRepository userRepo)
        {
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public bool IsEmailConfirmed { get; set; }
        [Display(Name="Username")]
        public string UserName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get User data from repository
            SiteUser userData = await this._userRepo.GetAsync(user.Id.ToString());

            if (userData == null)
            {
                return NotFound($"Couldn't load information about user with ID '{user.Id}'.");

            }

            Input = new InputModel
            {
                DisplayName = userData.DisplayName,
                Email = userData.Email,
                FullName = userData.FullName
            };

            UserName = userData.UserName;
            IsEmailConfirmed = userData.EmailConfirmed;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get User data from repository
            SiteUser userData = await this._userRepo.GetAsync(user.Id.ToString());

            if (userData == null)
            {
                return NotFound($"Couldn't load information about user with ID '{user.Id}'.");

            }

            if (Input.DisplayName != userData.DisplayName)
            {
                userData.DisplayName = Input.DisplayName;
            }
            if (Input.Email != userData.Email)
            {
                userData.Email = Input.Email;
            }
            if (Input.FullName != userData.FullName)
            {
                userData.FullName = Input.FullName;
            }

            // Update user in database
            SiteUser result = await this._userRepo.UpdateAsync(userData, CancellationToken.None);

            if(result != null)
            {
                await _signInManager.RefreshSignInAsync(userData);
                StatusMessage = "Your profile has been updated";
            }
            else
            {
                ModelState.AddModelError("", "Failed to update your profile. Please try again.");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
