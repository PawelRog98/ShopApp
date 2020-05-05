using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shop.Areas.Identity.Data;

namespace Shop.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ShopUser> _userManager;
        private readonly SignInManager<ShopUser> _signInManager;

        public IndexModel(
            UserManager<ShopUser> userManager,
            SignInManager<ShopUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string City { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Street")]
            public string Street { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "State")]
            public string State { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Country")]
            public string Country { get; set; }
        }

        private async Task LoadAsync(ShopUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName=user.LastName,
                City = user.City,
                Street = user.Street,
                PostalCode = user.PostalCode,
                State = user.State,
                Country=user.Country,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
            }

            if (Input.Street != user.Street)
            {
                user.Street = Input.Street;
            }

            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
            }

            if (Input.State != user.State)
            {
                user.State = Input.State;
            }

            if (Input.Country != user.Country)
            {
                user.Country = Input.Country;
            }
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
