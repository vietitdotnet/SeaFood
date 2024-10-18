using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject.Entites
{
    public class Order
    {
        [Key]
        public string ID { get; set; }
        
        [Required] 
        public string CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [Required]
        public string ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [Display(Name = "Size")]
        public int? SpecificationID { get; set; }

        [Display(Name = "Ghi chú")]
        [DataType(DataType.Text)]
        [StringLength(350, ErrorMessage = "{0} có độ dài tối đa {1} kí tự")]
        public string Description { set; get;}

        public DateTime OrderDate { get; set;} = DateTime.Now;

        public bool IsApproval { get; set; }

        public bool IsRecycleBin { get; set; }


    }
}

