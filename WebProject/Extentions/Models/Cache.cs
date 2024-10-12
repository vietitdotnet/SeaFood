namespace WebProject.Extentions.Models
{
   
    public class Cache
    {
        public static string keyProduct = "_product";
        public static string keyCategory = "_category";

        public static string keyPolicy = "_policy";
        public Dictionary<string, string> ObjCache { get; set; }
        public Cache()
        {
            ObjCache = new Dictionary<string, string>();
            ObjCache.Add("_product", "Sản Phẩm");
            ObjCache.Add("_category", "Danh mục");
            ObjCache.Add("_policy", "Chính Sách");

        }
    }
    
}
