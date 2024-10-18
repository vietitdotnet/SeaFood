using Microsoft.AspNetCore.Mvc;
using WebProject.DbContextLayer;

namespace WebProject.Services.OrderService
{
    public class OrderService : BaseService , IOrderService
    {
        public OrderService(AppDbContext context, ILogger<BaseService> logger) : base(context, logger)
        {
        }

        public string SetId(string contain = "ID")
        {
            var count = _context.Orders.Count();
            if (count == 0)
            {
                return contain + "100000";
            }
            else
            {
                var temp = Convert.ToInt32(_context.Orders.ToList()[count - 1].ID.ToString().Substring(contain.Length));

                temp = temp + 1;

                return contain + temp.ToString();
            }
        }
    }
}
