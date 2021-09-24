using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;

namespace Fbs.Web.Controls
{
    public partial class Captcha : System.Web.UI.UserControl,IValidator
    {
        private bool _isValid=false;
        private Random _random = new Random();
        private string _errorMessage;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["captcha"]))
            {
                this.Session["CaptchaImageText"] = GenerateRandomCode();
                // Create a CAPTCHA image using the text stored in the Session object.
                CaptchaImage ci = new CaptchaImage(this.Session["CaptchaImageText"].ToString(), 200, 50, "Century Schoolbook");

                // Change the response headers to output a JPEG image.
                this.Response.Clear();
                this.Response.ContentType = "image/jpeg";

                // Write the image to the response stream in JPEG format.
                ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

                // Dispose of the CAPTCHA image object.
                ci.Dispose();
                this.Response.End();
            }
            else
            {
                if (this.Request.QueryString.AllKeys.Length != 0)
                {
                    if (String.IsNullOrEmpty(this.Request.QueryString["captcha"]))
                        this.captchaImage.ImageUrl = this.Request.RawUrl + "&captcha=" + Guid.NewGuid().ToString();
                }
                else
                    this.captchaImage.ImageUrl = this.Request.RawUrl + "?captcha=" + Guid.NewGuid().ToString();
            }
        }

        //
        // Returns a string of six random digits.
        //
        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, this._random.Next(10).ToString());
            return s;
        }

        public string ErrorMessage
        {
            get
            {
                return this._errorMessage;
            }
            set
            {
                this.validator.ErrorMessage = value;
                this._errorMessage = value;
            }
        }

        public bool IsValid
        {
            get
            {
                return this._isValid;
            }
            set
            {
                this._isValid = value;
            }
        }

        public void Validate()
        {
            if ((this.Session["CaptchaImageText"] != null && this.CodeNumberTextBox.Text == this.Session["CaptchaImageText"].ToString()) || ConfigurationManager.AppSettings["DisableCaptchaValidation"]=="True")
            {
                this._isValid = true;
            }
            
        }

        protected void captchaValidate(object sender, ServerValidateEventArgs e)
        {
            this.Validate();
            e.IsValid = this.IsValid;
        }
    }

    /// <summary>
    /// Summary description for CaptchaImage.
    /// </summary>
    public class CaptchaImage
    {
        // Public properties (all read-only).
        public string Text
        {
            get { return this.text; }
        }

        public Bitmap Image
        {
            get { return this.image; }
        }
        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.height; }
        }

        // Internal properties.
        private string text;
        private int width;
        private int height;
        private string familyName;
        private Bitmap image;

        // For generating random numbers.
        private Random random = new Random();

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width and height.
        // ====================================================================
        public CaptchaImage(string s, int width, int height)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.GenerateImage();
        }

        // ====================================================================
        // Initializes a new instance of the CaptchaImage class using the
        // specified text, width, height and font family.
        // ====================================================================
        public CaptchaImage(string s, int width, int height, string familyName)
        {
            this.text = s;
            this.SetDimensions(width, height);
            this.SetFamilyName(familyName);
            this.GenerateImage();
        }

        // ====================================================================
        // This member overrides Object.Finalize.
        // ====================================================================
        ~CaptchaImage()
        {
            Dispose(false);
        }

        // ====================================================================
        // Releases all resources used by this object.
        // ====================================================================
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        // ====================================================================
        // Custom Dispose method to clean up unmanaged resources.
        // ====================================================================
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // Dispose of the bitmap.
                this.image.Dispose();
        }

        // ====================================================================
        // Sets the image width and height.
        // ====================================================================
        private void SetDimensions(int width, int height)
        {
            // Check the width and height.
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
            this.width = width;
            this.height = height;
        }

        // ====================================================================
        // Sets the font used for the image text.
        // ====================================================================
        private void SetFamilyName(string familyName)
        {
            // If the named font is not installed, default to a system font.
            try
            {
                Font font = new Font(this.familyName, 12F);
                this.familyName = familyName;
                font.Dispose();
            }
            catch (Exception ex)
            {
                this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
            }
        }

        // ====================================================================
        // Creates the bitmap image.
        // ====================================================================
        private void GenerateImage()
        {
            // Create a new 32-bit bitmap image.
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);

            // Fill in the background.
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;
            // Adjust the font size until the text fits within the image.
            do
            {
                fontSize--;
                font = new Font(this.familyName, fontSize, FontStyle.Bold);
                size = g.MeasureString(this.text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(this.text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 4F;
            PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Add some random noise.
            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            // Set the image.
            this.image = bitmap;
        }
    }
}