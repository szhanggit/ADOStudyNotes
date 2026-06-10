using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Services.Command.Media.Extensions
{
    public static class ImageDimensionExtension
    {
        public static string GetImageDimension(this IFormFile file)
        {
            string dimension;

            using (var img = Image.Load(file.OpenReadStream()))
            {
                dimension = $"({img.Height} x {img.Width})";
            }

            return dimension;
        }

        public static string GetImageHeight(this IFormFile file)
        {
            string height;

            using (var img = Image.Load(file.OpenReadStream()))
            {
                height = img.Height.ToString();
            }

            return height;
        }

        public static string GetImageWidth(this IFormFile file)
        {
            string width;

            using (var img = Image.Load(file.OpenReadStream()))
            {
                width = img.Width.ToString();
            }

            return width;
        }
    }
}
