using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace BBL.Feature.Form.Siriraj
{
    public static class ImageManipulate
    {
        public static Image Resize(this Image img, int srcX, int srcY, int srcWidth, int srcHeight, int dstWidth, int dstHeight)
        {
            var bmp = new Bitmap(dstWidth, dstHeight);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    var destRect = new Rectangle(0, 0, dstWidth, dstHeight);
                    graphics.DrawImage(img, destRect, srcX, srcY, srcWidth, srcHeight, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return bmp;
        }

        //public static Image ResizeProportional(this Image img, int width, int height, bool enlarge = false)
        //{
        //    double ratio = Math.Max(img.Width / (double)width, img.Height / (double)height);
        //    if (ratio < 1 && !enlarge) return img;
        //    return img.Resize(0, 0, img.Width, img.Height, M2.Round(img.Width / ratio), M2.Round(img.Height / ratio));
        //}

        public static Image ResizeCropExcess(this Image img, int dstWidth, int dstHeight)
        {
            double srcRatio = img.Width / (double)img.Height;
            double dstRatio = dstWidth / (double)dstHeight;
            int srcX, srcY, cropWidth, cropHeight;

            if (srcRatio < dstRatio) // trim top and bottom
            {
                cropHeight = dstHeight * img.Width / dstWidth;
                srcY = (img.Height - cropHeight) / 2;
                cropWidth = img.Width;
                srcX = 0;
            }
            else // trim left and right
            {
                cropWidth = dstWidth * img.Height / dstHeight;
                srcX = (img.Width - cropWidth) / 2;
                cropHeight = img.Height;
                srcY = 0;
            }

            return Resize(img, srcX, srcY, cropWidth, cropHeight, dstWidth, dstHeight);
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        //public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size size, bool preserveAspectRatio = true)
        //{
        //    int newWidth;
        //    int newHeight;
        //    if (preserveAspectRatio)
        //    {
        //        int originalWidth = image.Width;
        //        int originalHeight = image.Height;
        //        float percentWidth = (float)size.Width / (float)originalWidth;
        //        float percentHeight = (float)size.Height / (float)originalHeight;
        //        float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
        //        newWidth = (int)(originalWidth * percent);
        //        newHeight = (int)(originalHeight * percent);
        //    }
        //    else
        //    {
        //        newWidth = size.Width;
        //        newHeight = size.Height;
        //    }
        //    System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
        //    using (Graphics graphicsHandle = Graphics.FromImage(newImage))
        //    {
        //        graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
        //    }
        //    return newImage;
        //}

        //public class Size
        //{
        //    public int Width { get; set; }
        //    public int Height { get; set; }

        //    public Size(int p1, int p2)
        //    {
        //        this.Width = p1;
        //        this.Height = p2;
        //    }
        //}
    }
}