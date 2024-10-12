
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebProject.DbContextLayer;
using WebProject.Entites;

namespace WebProject.Areas.Identity.Pages.User
{
    public class DetailModel : UserPageModel
    {
        public DetailModel(UserManager<AppUser> userManager,
            ILogger<UserPageModel> logger,
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager) : base(userManager, logger, dbContext, roleManager)
        {
        }

        public async Task<IActionResult> OnGet([FromQuery] string id)
        {


            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);


            if (user is null)
            {
                return NotFound("Không tìm thấy người dùng");
            }

            Input = new InputModel
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                LastName = user.LastName,
                FirstName = user.FirstName,
                NativePlace = user.NativePlace,
                Company = user.Company,
                Address = user.Address,
                BirthDate = user.BirthDate,
                Description = user.Description,

            };


            var roles = await  _userManager.GetRolesAsync(user);

            Input.UserRoles = string.Join(",", roles);

            return Page();
            
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 

            public string Id { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string Email { get; set; }

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

            public string UserRoles { get; set; }
        }




    }
}
