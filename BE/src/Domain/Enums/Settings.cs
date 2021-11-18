using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace Domain.Enums
{
    public static class Settings
    {
        public static string GetBookCoversPath(IHostEnvironment hostEnvironment)
        {
            return Path.Combine(hostEnvironment.ContentRootPath.ToString(), "wwwroot", "Media", "Images", "BookCovers");
        }
        public static string GetAuthorAvatarsPath(IHostEnvironment hostEnvironment)
        {
            return Path.Combine(hostEnvironment.ContentRootPath.ToString(), "wwwroot", "Media", "Images", "UserAvatars");
        }
        public static readonly List<ImageFormat> AllowedImageFormats = new()
        {
            ImageFormat.Png,
            ImageFormat.Jpeg,
        };
    }
}
