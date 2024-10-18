using SixLabors.ImageSharp.PixelFormats;
using System.ComponentModel.DataAnnotations;
using WebProject.ModelValidation;

namespace WebProject.Dto
{
    public class OrderInformation
    {

        [Required(ErrorMessage = "{0} Không được bỏ trống")]

        [Display(Name ="Họ và tên")]

        [StringLength(50, ErrorMessage = "{0} chỉ tối đa {1}")]
        public string CustomerName { get; set; }


        [Required(ErrorMessage = "{0} Không được bỏ trống")]
        [StringLength(100 , ErrorMessage ="{0} chỉ tối đa {1}")]

        [Display(Name = "Địa chỉ cụ thể")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Tỉnh thành")]
        [Required(ErrorMessage ="{0} Không được bỏ trống")]
        [StringLength(100, ErrorMessage = "{0} chỉ tối đa {1}")]
        public string CustomerCity { get; set; }

        [Phone(ErrorMessage ="{0} Không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "{0} Không được bỏ trống")]
        [StringLength(20, ErrorMessage = "{0} chỉ tối đa {1} số")]      
        public string PhoneNumber { get; set; }
        public string ProductID { get; set; }

        public string ProductSlug { get; set; }

        [Display(Name ="Kích thước")]
        public int? SpecificationID { get; set; }

        [Display(Name = "Chi chú")]
        [DataType(DataType.Text)]
        [StringLength(350, ErrorMessage = "{0} có độ dài tối đa {1} kí tự")]
        public string Description { set; get; }
    }
}
