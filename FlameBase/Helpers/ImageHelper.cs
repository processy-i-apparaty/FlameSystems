using System.IO;
using System.Windows.Media.Imaging;

namespace FlameBase.Helpers
{
    public static class ImageHelper
    {
        public static void SaveImage(string filename, BitmapSource bitmapSource)
        {
            if (string.IsNullOrEmpty(filename)) return;
            var imageType = Path.GetExtension(filename).Trim('.');
            BitmapEncoder encoder;
            switch (imageType.ToLower())
            {
                case "png":
                    encoder = new PngBitmapEncoder();
                    break;
                case "jpg":
                case "jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case "bmp":
                    encoder = new BmpBitmapEncoder();
                    break;
                case "tiff":
                    encoder = new TiffBitmapEncoder();
                    break;
                case "gif":
                    encoder = new GifBitmapEncoder();
                    break;
                case "wmb":
                    encoder = new WmpBitmapEncoder();
                    break;
                default:
                    return;
            }

            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }
    }
}