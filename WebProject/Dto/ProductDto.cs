using WebProject.Entites;

namespace WebProject.Dto
{
    public class ProductDto
    {
        public string ID { get; set; }

        public string SKU { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsSize { get; set; }

        public decimal CostPrice { get; set; }

        public string ImageURL { get; set; }

        public bool IsActive { get; set; }

        public Category Category { get; set; }
    }
}
