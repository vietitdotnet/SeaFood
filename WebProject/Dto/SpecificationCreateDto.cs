using System.ComponentModel.DataAnnotations;

namespace WebProject.Dto
{
    public class SpecificationCreateDto
    {
        public int ID { get; set; }

       
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải lớn hơn hoặc bằng 1")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        [Display(Name = "Kích thước (con/kí)")]
        public int Size { get; set; }

      
        [Display(Name = "Giá bán")]
        [Range(100, int.MaxValue, ErrorMessage = "{0} phải lớn hơn hoặc bằng 100")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        public decimal Price { get; set; }

        [Required]
        public string ProductID { get; set; }

        public string ProductName { get; set; }

        
    }
}
