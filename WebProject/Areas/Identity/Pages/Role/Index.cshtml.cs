
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using WebProject.Areas.Identity.Pages.Role;
using WebProject.DbContextLayer;

namespace WebProject.Areas.Identity.Pages.Role
{

    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, ILogger<IndexModel> logger, AppDbContext context) : base(roleManager, logger, context)
        {
        }

        public List<RoleAddClamis> roleClamis { get; set; }
        

        public class RoleAddClamis : IdentityRole
        {
            public string[] Clamis { get; set; }

        }

        public async Task OnGet()
        {
            roleClamis = await _roleManager.Roles.Select(r => new RoleAddClamis() { 
            
                Id = r.Id,
                Name = r.Name

            }).ToListAsync();

            foreach (var item in roleClamis)
            {
                var clamis = await _roleManager.GetClaimsAsync(item);
                var stringClamis = clamis.Select(c => c.Type + "=" + c.Value).ToArray();
                item.Clamis = stringClamis;
            }
        }
    }
}
