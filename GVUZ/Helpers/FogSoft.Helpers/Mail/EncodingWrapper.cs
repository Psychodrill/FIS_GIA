using System;
using System.Text;

namespace FogSoft.Helpers.Mail
{
    public abstract class EncodingWrapper : Encoding
    {
        private readonly Encoding _encoding = GetEncoding(1251);

        protected EncodingWrapper(Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");

            _encoding = encoding;
        }

        public override string BodyName
        {
            get { return _encoding.BodyName; }
        }

        public override string EncodingName
        {
            get { return _encoding.EncodingName; }
        }

        public override string HeaderName
        {
            get { return _encoding.HeaderName; }
        }

        public override string WebName
        {
            get { return _encoding.WebName; }
        }

        public override int WindowsCodePage
        {
            get { return _encoding.WindowsCodePage; }
        }

        public override bool IsBrowserDisplay
        {
            get { return _encoding.IsBrowserDisplay; }
        }

        public override bool IsBrowserSave
        {
            get { return _encoding.IsBrowserSave; }
        }

        public override bool IsMailNewsDisplay
        {
            get { return _encoding.IsMailNewsDisplay; }
        }

        public override bool IsMailNewsSave
        {
            get { return _encoding.IsMailNewsSave; }
        }

        public override bool IsSingleByte
        {
            get { return _encoding.IsSingleByte; }
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

        public override byte[] GetPreamble()
        {
            return _encoding.GetPreamble();
        }

        public override int GetByteCount(char[] chars)
        {
            return _encoding.GetByteCount(chars);
        }

        public override int GetByteCount(string s)
        {
            return _encoding.GetByteCount(s);
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return _encoding.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return _encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return _encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override string GetString(byte[] bytes)
        {
            return _encoding.GetString(bytes);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return _encoding.GetCharCount(bytes, index, count);
        }

        public override Decoder GetDecoder()
        {
            return _encoding.GetDecoder();
        }

        public override Encoder GetEncoder()
        {
            return _encoding.GetEncoder();
        }

        public override int GetMaxByteCount(int charCount)
        {
            return _encoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return _encoding.GetMaxCharCount(byteCount);
        }

        public override bool IsAlwaysNormalized(NormalizationForm form)
        {
            return _encoding.IsAlwaysNormalized(form);
        }
    }
}