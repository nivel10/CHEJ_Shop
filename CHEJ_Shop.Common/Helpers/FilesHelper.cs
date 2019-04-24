namespace CHEJ_Shop.Common.Helpers
{
    using Models;
    using System;
    using System.IO;

    public class FilesHelper
    {
        public static Response UplodaImage(
            MemoryStream _stream,
            string _folder,
            string _name)
        {
            try
            {
                _stream.Position = 0;
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    _folder,
                    _name);
                File.WriteAllBytes(path, _stream.ToArray());
                return new Response
                {
                    IsSuccess = true,
                    Message = "Method is ok...!!!",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}