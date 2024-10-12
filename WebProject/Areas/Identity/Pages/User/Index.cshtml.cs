using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProject.DbContextLayer;
using WebProject.Entites;
using WebProject.Paging;

namespace WebProject.Areas.Identity.Pages.User
{
    public class IndexModel : UserPageModel
    {
        public IndexModel(UserManager<AppUser> userManager, ILogger<UserPageModel> logger, AppDbContext dbContext, RoleManager<IdentityRole> roleManager) : base(userManager, logger, dbContext, roleManager)
        {
        }

        public PagedList<UserAndRole> users { get; set; }

        public class UserAndRole : AppUser
        {
            public string UserRoles { get; set; }
        }

        //[BindProperty(SupportsGet = true)]
        //public string SearchPhone { get; set; }

        public async Task<IActionResult> OnGet([FromQuery] UserParameters userParameters, [FromQuery] string search)
        {

            var user = from u in _dbContext.Users
                       select u;

          
            PagedList<AppUser> pageUsers = PagedList<AppUser>.ToPagedList(user,
                        userParameters.PageNumber,
                        userParameters.PageSize);

            List<UserAndRole> derivedList = pageUsers.ToList().Select(x => new UserAndRole
            {

                Id = x.Id,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                LastName = x.LastName,
                FirstName = x.FirstName,

            }).ToList();

            users = new PagedList<UserAndRole>(derivedList, pageUsers.TotalCount, userParameters.PageNumber, userParameters.PageSize);

            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                item.UserRoles = string.Join(",", roles);

            }

            return Page();

        }
    }
}
