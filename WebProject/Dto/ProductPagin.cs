using WebProject.Entites;

namespace WebProject.Dto
{
    public class ProductPagin
    {
        public IEnumerable<ProductDto> Products { get; set; }

        public IEnumerable<Product> IndexProducts { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
