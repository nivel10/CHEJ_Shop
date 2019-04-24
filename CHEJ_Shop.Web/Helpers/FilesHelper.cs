namespace CHEJ_Shop.Web.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FilesHelper
    {
        public static async Task<string> FileUpServer(IFormFile _imageFile)
        {
            try
            {
                var path = string.Empty;

                if (_imageFile != null && _imageFile.Length > 0)
                {
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\Products", 
                        _imageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await _imageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{_imageFile.FileName}";
                }

                return path;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}