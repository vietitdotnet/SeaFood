
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebProject.DbContextLayer;

namespace WebProject.Areas.Identity.Pages.Role
{

    /*[Authorize(Policy = "Administrator")]*/
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;

        protected readonly AppDbContext _context;
        protected readonly ILogger<RolePageModel> _logger;


        [TempData]
        public string StatusMessage { get; set; }

        public RolePageModel(RoleManager<IdentityRole> roleManager, ILogger<RolePageModel> logger , AppDbContext context)
        {
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
        }

    }
}
