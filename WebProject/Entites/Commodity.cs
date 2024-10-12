using System.ComponentModel.DataAnnotations;
using WebProject.ModelValidation;

namespace WebProject.Entites
{
    public class Commodity
    {
        public Commodity()
        {
            ID = Guid.NewGuid().ToString().Substring(24);
        }

        [Key]
        public string ID { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} có độ dài từ {1} đến {2} kí tự.")]
        [SlugValidation]
        public string Slug { get; set; }
        
        public virtual ICollection<Product> Products { get; set;}
    }
}
