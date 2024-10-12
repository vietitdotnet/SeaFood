using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebProject.ModelValidation;

namespace WebProject.Entites
{
    public class Category
    {

        public Category()
        {
            ID = Guid.NewGuid().ToString().Substring(24);
        }

        [Key]
        public string ID { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        public string Name { get; set; }

        public string ParentCategoryID { get; set; }

        [ForeignKey("ParentCategoryID")]
        [Display(Name = "Danh mục cha")]
        public virtual Category ParentCategory { set; get; }

        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} có độ dài từ {1} đến {2} kí tự.")]
        [SlugValidation]
        public string Slug { get; set; }

        [Display(Name = "Cấp")]
        public string Level { get; set; }
        public int DisplayOrder { get; set; }

        [Display(Name = "Nổi bật")]
        public bool IsFeatured { get; set; }
        public virtual ICollection<Category> CategoryChildrens { get; set; }

        public virtual ICollection<Product> Products { get; set; }
      
    }
}
