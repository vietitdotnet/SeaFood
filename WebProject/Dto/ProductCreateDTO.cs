using System.ComponentModel.DataAnnotations;
using WebProject.ModelValidation;

namespace WebProject.Dto
{
    public class ProductCreateDTO
    {
        public ProductCreateDTO()
        {
            ID = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
        }

        [Key]
        [IdValidation]
        [StringLength(36, ErrorMessage = "{0} có độ dài tối đa {1} kí tự")]
        public string ID { get; set; }


        [Display(Name = "Tên")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        public string Name { get; set; }

        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        [StringLength(100, ErrorMessage = "{0} có độ dài tối đa {1} kí tự")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        [DataType(DataType.Text)]
        [StringLength(135, ErrorMessage = "{0} có độ dài tối đa {1} kí tự")]
        public string Description { set; get; }


        [Display(Name = "Giá bán")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        public decimal Price { get; set; }


        [Display(Name = "Ảnh")]
        public string ImageURL { get; set; }


        [Display(Name = "Nổi bật")]
        public bool IsFeatured { get; set; }


        [Display(Name = "Trạng thái hoặt động")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "{0} không được bỏ trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} có độ dài từ {1} đến {2} kí tự.")]
        [SlugValidation]
        public string Slug { get; set; }

        [Display(Name = "Bài viết")]
        
        public string Content { get; set; }

        [Display(Name = "Kích thước")]
        public bool IsSize { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }


        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Danh mục")]
        public string CategoryID { get; set; }

      

    }
}
