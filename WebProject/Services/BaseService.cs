using WebProject.Controllers;
using WebProject.DbContextLayer;

namespace WebProject.Services
{
    public class BaseService
    {
       
        protected readonly AppDbContext _context;

        protected readonly ILogger<BaseService> _logger;
        public BaseService(AppDbContext context , ILogger<BaseService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
    }
}
