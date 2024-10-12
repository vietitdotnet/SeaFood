namespace WebProject.FileManager
{
    public interface  IFileService
    {
        string PathRepresentWebRootPath(IObjectFolder objectFolder);
        string PathRepresentContentRootPath(IObjectFolder objectFolder);

        string HttpContextAccessorPathImgSrcIndex(IObjectFolder objectFolder, string UrlImg);

        string HttpContextAccessorPathDefaulImgSrcIndex(string UrlImg);
        Task<bool> CreateFileAsync(IObjectFolder objectFolder, IFormFile formFile, string fileName);

        Task<bool> DeleteFileAsync(IObjectFolder objectFolder, string Url);

        
    }
}
