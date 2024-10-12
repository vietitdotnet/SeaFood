namespace WebProject.FileManager
{
    public class ProductImg : ObjectFolder
    {
        private ProductImg() { }

        private static ProductImg _instance = null;

        public static ProductImg GetProductImg()
        {
            if (_instance is null)
            {
                return new ProductImg();
            }

            return _instance;
        }

        public override string GetFileImage()
        {
            return base.GetFileImage();
        }

        public override string GetFolderRootDirectory()
        {
            return base.GetFolderRootDirectory();
        }
    }
}
