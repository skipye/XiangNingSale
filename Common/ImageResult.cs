using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Common
{
    public class ImageResult : ActionResult
    {
        private readonly string _format;
        private ImageFormat imageFormat;
        private readonly string _noFilePath;
        public readonly byte[] _buffer;
        public ImageResult(byte[] buffer, string noFilePath = "/images/peo.jpg", string format = "png")
        {

            _format = format.ToLower();
            _noFilePath = noFilePath;
            _buffer = buffer;

        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            switch (_format)
            {
                case "bmp": context.HttpContext.Response.ContentType = "image/bmp"; imageFormat = ImageFormat.Bmp; break;
                case "gif": context.HttpContext.Response.ContentType = "image/gif"; imageFormat = ImageFormat.Gif; break;
                case "jpeg": context.HttpContext.Response.ContentType = "image/jpeg"; imageFormat = ImageFormat.Jpeg; break;
                case "png": context.HttpContext.Response.ContentType = "image/png"; imageFormat = ImageFormat.Png; break;
                case "tiff": context.HttpContext.Response.ContentType = "image/tiff"; imageFormat = ImageFormat.Tiff; break;
                case "jpg": context.HttpContext.Response.ContentType = "image/jpg"; imageFormat = ImageFormat.Jpeg; break;
                default: context.HttpContext.Response.ContentType = "image/png"; imageFormat = ImageFormat.Png; break;
            }
            try
            {
                if (_buffer == null)
                {
                    context.HttpContext.Response.TransmitFile(_noFilePath);
                }
                else
                {
                    context.HttpContext.Response.Cache.SetNoStore();
                    context.HttpContext.Response.BinaryWrite(_buffer);
                }
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.AddHeader("CRIC.ERROR", ex.Message);
                context.HttpContext.Response.AddHeader("Content-type", "image/jpg");
                context.HttpContext.Response.TransmitFile(_noFilePath);
            }
        }
    }
}
