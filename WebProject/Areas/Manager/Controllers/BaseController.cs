using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebProject.DbContextLayer;

namespace WebProject.Areas.Manager.Controllers
{
    [Area("manager")]
    public class BaseController : Controller
    {
        protected readonly AppDbContext _context;

        protected readonly ILogger<BaseController> _logger;

        [TempData]
        public string StatusMessage { get; set; }
        public BaseController(AppDbContext appDbContext, ILogger<BaseController> logger)
        {
           
            _context = appDbContext;
            _logger = logger;
           
        }
       
    }
}
