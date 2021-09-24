namespace XtractPro.Multimedia
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    /// <summary>
    ///     CAPTCHA Image generation, on each Image property call,
    ///     based on all other customizable property values.
    /// </summary>
    [DefaultProperty("Text")]
    public class CaptchaImage
    {
        private readonly Random _random;

        public CaptchaImage() : this(new Random())
        {
            Text = "";
        }

        public CaptchaImage(Random random)
        {
            _random = random;
            Text = "";
        }

        #region Bitmap

        private int _height = 60;
        private int _width = 100;

        [Category("Bitmap")]
        [Description("Image Width")]
        public int Width
        {
            get { return _width; }
            set
            {
                if (value < 10 || value > 1000)
                    throw new Exception("Image Width must be between 10 and 1000");
                _width = value;
            }
        }

        [Category("Bitmap")]
        [Description("Image Height")]
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 10 || value > 1000)
                    throw new Exception("Image Height must be between 10 and 1000");
                _height = value;
            }
        }

        [Browsable(false)]
        [Category("Bitmap")]
        [Description("Generated image")]
        public Bitmap GetImage()
        {
            var bitmap = new Bitmap(_width, _height,
                                    PixelFormat.Format32bppPArgb);

            Graphics g = null;
            Font font = null;
            HatchBrush textBrush = null;
            HatchBrush backBrush = null;
            try
            {
                // Fill Background
                g = Graphics.FromImage(bitmap);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, _width, _height);
                textBrush = new HatchBrush(_textStyle,
                                           _textForeColor, _textBackColor);
                backBrush = new HatchBrush(_backStyle,
                                           _backForeColor, _backBackColor);
                g.FillRectangle(backBrush, rect);

                // Write Text
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                font = new Font(_fontName, _fontSize, _fontStyle);
                var path = new GraphicsPath();
                path.AddString(_text, font.FontFamily,
                               (int) font.Style, font.Size, rect, stringFormat);

                // Warp Text
                if (_warp > 0)
                {
                    var matrix = new Matrix();
                    float warp = 10 - _warp*6/100;
                    PointF[] points =
                        {
                            new PointF(_random.Next(rect.Width)/warp,
                                       _random.Next(rect.Height)/warp),
                            new PointF(rect.Width - _random.Next(rect.Width)/warp,
                                       _random.Next(rect.Height)/warp),
                            new PointF(_random.Next(rect.Width)/warp,
                                       rect.Height - _random.Next(rect.Height)/warp),
                            new PointF(rect.Width - _random.Next(rect.Width)/warp,
                                       rect.Height - _random.Next(rect.Height)/warp)
                        };
                    path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
                }
                g.FillPath(textBrush, path);

                // Random Noise over Text
                if (_noise > 0)
                {
                    var maxDim = Math.Max(rect.Width, rect.Height);
                    var radius = (maxDim*_noise/3000);
                    var maxGran = (rect.Width*rect.Height
                                   /(100 - (_noise >= 90 ? 90 : _noise)));

                    using (var pen = new Pen(textBrush, 1))
                        for (var i = 0; i < maxGran; i++)
                        {
                            var next = _random.Next(rect.Width);
                            var next1 = _random.Next(rect.Height);
                            var width = _random.Next(radius);
                            var height = _random.Next(radius);
                            g.DrawEllipse(pen, next, next1, width, height);
                        }
                }

                // Draw Lines over Text
                if (_lines > 0)
                {
                    var lines = (_lines/30) + 1;
                    using (var pen = new Pen(textBrush, 1))
                        for (var i = 0; i < lines; i++)
                        {
                            var points = new PointF[lines > 2 ? lines - 1 : 2];
                            for (var j = 0; j < points.Length; j++)
                                points[j] = new PointF(_random.Next(rect.Width),
                                                       _random.Next(rect.Height));
                            g.DrawCurve(pen, points, 1.75F);
                        }
                }
            }
            finally
            {
                // Dispose GDI+ objects
                if (g != null)
                    g.Dispose();
                if (font != null)
                    font.Dispose();
                if (textBrush != null)
                    textBrush.Dispose();
                if (backBrush != null)
                    backBrush.Dispose();
            }

            return bitmap;
        }

        #endregion

        #region Effects

        private int _lines = 100;
        private int _noise = 100;

        private int _warp = 100;

        [Category("Effects")]
        [Description("Noise Factor")]
        public int NoiseFactor
        {
            get { return _noise; }
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception("Noise Factor must be between 0 and 100");
                _noise = value;
            }
        }

        [Category("Effects")]
        [Description("Warp Factor")]
        public int WarpFactor
        {
            get { return _warp; }
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception("Warp Factor must be between 0 and 100");
                _warp = value;
            }
        }

        [Category("Effects")]
        [Description("Lines Factor")]
        public int LinesFactor
        {
            get { return _lines; }
            set
            {
                if (value < 0 || value > 100)
                    throw new Exception("Lines Factor must be between 0 and 100");
                _lines = value;
            }
        }

        #endregion

        #region Brushes

        private Color _backBackColor = Color.White;
        private Color _backForeColor = Color.LightGray;
        public HatchStyle _backStyle = HatchStyle.DottedGrid;
        private Color _textBackColor = Color.DarkGray;
        private Color _textForeColor = Color.Black;
        public HatchStyle _textStyle = HatchStyle.NarrowHorizontal;

        [Category("Hatch Brush Text")]
        [Description("Hatch Brush Text Style")]
        public HatchStyle TextStyle
        {
            get { return _textStyle; }
            set { _textStyle = value; }
        }

        [Category("Hatch Brush Text")]
        [Description("Hatch Brush Text Fore Color")]
        public Color TextForeColor
        {
            get { return _textForeColor; }
            set { _textForeColor = value; }
        }

        [Category("Hatch Brush Text")]
        [Description("Hatch Brush Text Background Color")]
        public Color TextBackColor
        {
            get { return _textBackColor; }
            set { _textBackColor = value; }
        }

        [Category("Hatch Brush Background")]
        [Description("Hatch Brush Background Style")]
        public HatchStyle BackgroundStyle
        {
            get { return _backStyle; }
            set { _backStyle = value; }
        }

        [Category("Hatch Brush Background")]
        [Description("Hatch Brush Background Fore Color")]
        public Color BackgroundForeColor
        {
            get { return _backForeColor; }
            set { _backForeColor = value; }
        }

        [Category("Hatch Brush Background")]
        [Description("Hatch Brush Background Background Color")]
        public Color BackgroundBackColor
        {
            get { return _backBackColor; }
            set { _backBackColor = value; }
        }

        #endregion

        #region Text

        private string _charSet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";
        private int _length = 5;
        private string _text = "";
        private bool _unique = true;

        [Category("Text"),
         Description("Text string. Empty to auto-generate random string.")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == null
                    || (value.Length != 0 && value.Length < 2)
                    || value.Length > 10)
                    throw new Exception("Text Length must be between 2 and 10");

                _text = value;
                if (_text.Length == 0)
                    for (var i = 0; i < _length; i++)
                    {
                        char c;
                        do
                            c = _charSet[_random.Next(0, _charSet.Length)]; while (_unique && _length <= _charSet.Length &&
                                                                                   _text.IndexOf(c) >= 0);
                        _text += c;
                    }
                _length = _text.Length;
            }
        }

        [Category("Text")]
        [Description("Character set for auto-text generation")]
        public string CharacterSet
        {
            get { return _charSet; }
            set
            {
                if (value == null || value.Length < 2)
                    throw new Exception("Minimum character set size is 2");
                _charSet = value;
                Text = "";
            }
        }

        [Category("Text")]
        [Description("Text Length")]
        public int Length
        {
            get { return _length; }
            set
            {
                if (value < 2 || value > 10)
                    throw new Exception("Text Length must be between 2 and 10");
                _length = value;
                Text = "";
            }
        }

        [Category("Text")]
        [Description("Unique characters or not")]
        public bool Unique
        {
            get { return _unique; }
            set
            {
                _unique = value;
                Text = "";
            }
        }

        #endregion

        #region Font

        private string _fontName = "Tahoma";

        private int _fontSize = 20;

        private FontStyle _fontStyle = FontStyle.Bold;

        [Category("Font")]
        [Description("Font Name")]
        [TypeConverter(typeof (FontConverter.FontNameConverter))]
        [Editor("System.Drawing.Design.FontNameEditor", typeof (UITypeEditor))]
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        [Category("Font")]
        [Description("Font Size")]
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        [Category("Font")]
        [Description("Font Style")]
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        #endregion
    }
}