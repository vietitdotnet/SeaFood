

using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace WebProject.FileManager
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _contextAccessor;
       
        public FileService(IWebHostEnvironment webHost , IHttpContextAccessor contextAccessor)
        {
            _webHost = webHost;
            _contextAccessor = contextAccessor;
        }


        public string PathRepresentWebRootPath(IObjectFolder objectFolder)
        {
            if (objectFolder != null)
            {
                return Path.Combine(_webHost.WebRootPath, objectFolder.GetFolderRootDirectory(), objectFolder.GetFileImage());
            }

            return null;
        }

        public  async Task<bool> CreateFileAsync(IObjectFolder objectFolder, IFormFile file, string fileName)
        {
         
            try
            {
                if (!Directory.Exists(PathRepresentWebRootPath(objectFolder)))
                {
                    Directory.CreateDirectory(PathRepresentWebRootPath(objectFolder));
                }

                var uploads = PathRepresentWebRootPath(objectFolder);

               
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                // Đường dẫn file đích với định dạng WebP
                var webpFilePath = Path.Combine(uploads, fileName);

                // Đường dẫn của file gốc
                var originalFilePath = Path.Combine(uploads, file.FileName);

                // Nén và lưu ảnh dưới định dạng WebP
                using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                    // Resize và nén ảnh
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(600, 0)
                    }));

                    // Lưu ảnh dưới định dạng WebP với chất lượng 75%
                    await image.SaveAsync(webpFilePath, new WebpEncoder { Quality = 60 });
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(IObjectFolder objectFolder, string Url)
        {

            var task = new Task<bool>(() => {

                try
                {
                    if (Url != null)
                    {
                        var path = Path.Combine(PathRepresentWebRootPath(objectFolder), Url);

                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            });

            task.Start();

            return await task;

        }
        
        public  string PathRepresentContentRootPath(IObjectFolder objectFolder)
        {
            if (objectFolder != null)
            {
                return Path.Combine(_webHost.ContentRootPath, objectFolder.GetFolderRootDirectory(), objectFolder.GetFileImage());
            }

            return null;
        }

        public  string HttpContextAccessorPathImgSrcIndex(IObjectFolder objectFolder, string UrlImg)
        {
            if (UrlImg != null && objectFolder != null)
            {
                return string.Format("{0}://{1}/{2}/{3}/{4}", _contextAccessor.HttpContext.Request.Scheme, _contextAccessor.HttpContext.Request.Host.ToString(), objectFolder.GetFolderRootDirectory(), objectFolder.GetFileImage(), UrlImg);
            }
            return null;
        }

        public string HttpContextAccessorPathDefaulImgSrcIndex(string UrlImg)
        {
            if (UrlImg != null)
            {
                return string.Format("{0}://{1}/{2}", _contextAccessor.HttpContext.Request.Scheme, _contextAccessor.HttpContext.Request.Host.ToString(),  UrlImg);
            }
            return null;
        }

        public static string GetUniqueFileName(string fileName)
        {

            var url = DateTime.Now.ToString("yymmssfff")
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);

            return url;
        }

        public static string GetUniqueFileWebp()
        {

            var url = DateTime.Now.ToString("yymmssfff")
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + ".webp";

            return url;
        }


    }
}
