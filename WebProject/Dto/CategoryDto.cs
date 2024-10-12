using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebProject.Entites;
using WebProject.ModelValidation;

namespace WebProject.Dto
{
    public class CategoryDto
    {
     
        public string ID { get; set; }

        public string Name { get; set; }

        public string ParentCategoryID { get; set; }

        public virtual Category ParentCategory { set; get; }

        public string Slug { get; set; }

        
        public string Level { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsFeatured { get; set; }
  
        public virtual ICollection<ProductDto> Products { get; set; }
    }
}
