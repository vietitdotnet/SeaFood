using System.ComponentModel.DataAnnotations;

namespace WebProject.Entites
{
    public class Customer
    {
        public Customer() 
        {
            ID = Guid.NewGuid().ToString();
        }

        [Key]
        public string ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength (100)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNunber { get; set; }
    }
}
