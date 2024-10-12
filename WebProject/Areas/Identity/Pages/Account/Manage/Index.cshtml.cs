// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProject.Entites;

namespace WebProject.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Họ")]
            [StringLength(10, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string LastName { get; set; }

            [Display(Name = "Tên")]
            [StringLength(50, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
            [Required(ErrorMessage = "{0} không được bỏ trống")]

            public string FirstName { get; set; }

            [Display(Name = "Ngày sinh")]
            public DateTime? BirthDate { set; get; }

            [Display(Name = "Công ty")]
            [StringLength(50, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 5)]
            public string Company { get; set; }

           
            [Display(Name = "Quê quán")]
            [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 5)]
            public string NativePlace { get; set; }

            [StringLength(100)]
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Mô tả bản thân")]

            [StringLength(350, ErrorMessage = "{0} dài tối đa {1} ký tự.")]
            public string Description { get; set; }
        }

        private async Task LoadAsync(AppUser appuser)
        {
            var user = await _userManager.GetUserAsync(User);

            Username = user.UserName;

            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                LastName = user.LastName,
                FirstName = user.FirstName,
                NativePlace = user.NativePlace,
                Company = user.Company,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Description = user.Description,

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

            /*var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }*/

            // Cập nhật các trường bổ sung

            user.LastName = Input.LastName;
            user.FirstName = Input.FirstName;
            user.Company = Input.Company;
            user.Address = Input.Address;
            user.BirthDate = Input.BirthDate;
            user.Description = Input.Description;

            user.NativePlace = Input.NativePlace;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Hồ sơ của bạn đã cập nhật";

            return RedirectToPage();
        }
    }
}
