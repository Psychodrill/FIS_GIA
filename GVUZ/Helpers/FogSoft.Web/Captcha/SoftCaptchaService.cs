using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Простейшая реализация <see cref="ICaptchaService" /> .
    /// </summary>
    public class SoftCaptchaService : ICaptchaService
    {
        private const string SessionKeyPrefix = "_CAPTCHA_";
        private readonly ICaptchaOptionsService _optionsService;

        public SoftCaptchaService(ICaptchaOptionsService optionsService)
        {
            if (optionsService == null) throw new ArgumentNullException("optionsService");
            _optionsService = optionsService;
        }

        public Bitmap CreateBitmap(CaptchaItem item)
        {
            if (item == null) throw new ArgumentNullException("item");
            return GenerateBitmap(item);
        }

        public void CacheCaptcha(HttpContextBase context, CaptchaItem captchaItem)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (captchaItem == null) throw new ArgumentNullException("captchaItem");
            if (context.Session == null) throw new ArgumentException("Session unavailable.");

            context.Session[SessionKeyPrefix + captchaItem.Uid] = captchaItem;
        }

        public CaptchaItem GetCachedCaptcha(HttpContextBase context, string uid = null)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (context.Session == null) return null;
            if (uid == null)
                uid = context.Request.Form[_optionsService.UidHiddenField];

            string key = GetSessionKey(uid);
            if (key == null)
                return null;

            return context.Session[key] as CaptchaItem;
        }

        public void RemoveCachedCaptcha(HttpContextBase context)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (context.Session == null) return;

            string key = GetSessionKey(context.Request.Form[_optionsService.UidHiddenField]);
            context.Session.Remove(key);
        }

        public void Save(CaptchaItem options, Stream stream)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (stream == null) throw new ArgumentNullException("stream");
            using (Image image = GenerateBitmap(options))
            {
                image.Save(stream, ImageFormat.Jpeg);
            }
        }

        private string GetSessionKey(string uid)
        {
            return string.IsNullOrEmpty(uid) ? null : SessionKeyPrefix + uid;
        }

        private Bitmap GenerateBitmap(CaptchaItem captchaItem)
        {
            var random = new Random();
            var bitmap = new Bitmap(captchaItem.Width, captchaItem.Height, PixelFormat.Format32bppArgb);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, captchaItem.Width, captchaItem.Height);

            HatchBrush hatchBrush = null;
            Font font = null;

            try
            {
                hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
                graphics.FillRectangle(hatchBrush, rect);

                font = SetUpTextFont(captchaItem, graphics, rect);

                StringFormat format = SetUpTextFormat();

                // Create a path using the text and warp it randomly.
                GraphicsPath path = CreateGraphicsPath(captchaItem, rect, font, format, random);

                // Draw the text.
                hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
                graphics.FillPath(hatchBrush, path);

                AddRandomNoise(hatchBrush, graphics, random, rect);
            }
            finally
            {
                if (font != null)
                    font.Dispose();
                if (hatchBrush != null)
                    hatchBrush.Dispose();

                graphics.Dispose();
            }
            return bitmap;
        }

        private static void AddRandomNoise(HatchBrush hatchBrush, Graphics graphics, Random random, Rectangle rect)
        {
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int) (rect.Width*rect.Height/30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m/50);
                int h = random.Next(m/50);
                graphics.FillEllipse(hatchBrush, x, y, w, h);
            }
        }

        private static StringFormat SetUpTextFormat()
        {
            return new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center};
        }

        private Font SetUpTextFont(CaptchaItem options, Graphics g, Rectangle rect)
        {
            // TODO: use const instead of magic numbers after algorithm clarifying
            const float fixSize = 1.2f;
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(options.FamilyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(options.Text, font);
            } while (size.Width > fixSize*rect.Width && size.Height > fixSize*rect.Height);

            return font;
        }

        private GraphicsPath CreateGraphicsPath
            (CaptchaItem options, Rectangle rect, Font font, StringFormat format, Random random)
        {
            // TODO: clarify algorithm (in future, make more complex, if possible - e.g. curve instead of line)

            var path = new GraphicsPath();
            path.AddString(options.Text, font.FontFamily, (int) font.Style, font.Size, rect, format);

            const float v = 4F;
            PointF[] points =
                {
                    new PointF(random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                    new PointF(rect.Width - random.Next(rect.Width)/v, random.Next(rect.Height)/v),
                    new PointF(random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v),
                    new PointF(rect.Width - random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v)
                };
            var matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            return path;
        }
    }
}