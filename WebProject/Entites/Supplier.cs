namespace WebProject.Entites
{
    public class Supplier
    {
        public Supplier()
        {
            ID = Guid.NewGuid().ToString();
          
        }

        public string ID { get; set; }

        public string SupplierCode { get; set; } //Mã nhà cung cấp để định danh duy nhất.

        public string Name { get; set; } //Tên người liên hệ chính của nhà cung cấp.
    
        public string ContactTitle { get; set; }  //Chức danh của người liên hệ (ví dụ: Giám đốc bán hàng, Quản lý).

        public string TaxID { get; set; } // Mã số thuế

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public decimal Rating { get; set; }
        public string Fax { get; set; }

        public bool IsActive { get; set; } // trạng thái hoặt động

        public string Notes { get; set; } // Ghi chú thêm 

        public virtual ICollection<Product> Products { get; set; }


    }
}
