using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Entites
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Mật khẩu hiển thị")]
        public string PassWordDisplay { get; set; }

        [Column(TypeName = "Char")]
        [StringLength(100)]
        public string Avartar { get; set; }

        [Display(Name = "Họ")]
        [StringLength(50, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Tên")]
        [StringLength(50, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { set; get; }

        [Display(Name = "Công ty")]
        [StringLength(50, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 5)]
        public string Company { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Mô tả bản thân")]
        public string Description { get; set; }

        [Display(Name = "Quê quán")]
        [StringLength(350, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 5)]
        public string NativePlace { get; set; }
    }
}
