using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FogSoft.Import
{
	/// <summary>
	/// Implements CSV reading.</summary>
	public class CsvReader<T> : StructuredReader<T> where T : Record, new()
	{
		private bool _skipHeaders;
		private readonly bool _useDoubleQuotes;
		private readonly TextReader _textReader;
		private readonly char _separator;
		private readonly bool _hasRows;

		public CsvReader(Stream stream, Encoding encoding = null, bool skipHeaders = false,
			bool useDoubleQuotes = true)
		{
			if (stream == null) throw new ArgumentNullException("stream");

			_skipHeaders = skipHeaders;
			_useDoubleQuotes = useDoubleQuotes;
			// ReSharper disable AssignNullToNotNullAttribute
			_textReader = new StreamReader(stream, encoding ?? Encoding.Default, true);
			// ReSharper restore AssignNullToNotNullAttribute
			_separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," ? ';' : ',';
			_hasRows = _textReader.Peek() != -1;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_textReader.Dispose();
		}

		public override bool Read()
		{
lStart:
			string line = _textReader.ReadLine();
			if (line == null || string.IsNullOrWhiteSpace(line.Replace(";", "")) )
			{
				SetCurrentRecordToNull();
				return false;
			}

			if (_skipHeaders)
			{
				_skipHeaders = false;
				line = _textReader.ReadLine();
				if (line == null)
					return false;
			}

			CreateCurrentRecord(Split(line).ToArray());
			
			if (Current.SkipThisRecord)
				goto lStart;

			return true;
		}

		public override bool HasRows
		{
			get { return _hasRows; }
		}

		private List<string> Split(string line)
		{
			List<string> list = new List<string>();

			bool hasOpenQuote = false;
			bool hasEscapedQuote = false;
			bool isNewValue = true;
			int start = 0;
			int i;
			for (i = 0; i < line.Length; i++)
			{
				char c = line[i];
				if (isNewValue)
				{
					if (c == _separator)
					{
						list.Add(null);
						continue;
					}
					hasOpenQuote = c == '"';
					hasEscapedQuote = false;
					start = i;
					isNewValue = false;
					continue;
				}
				if (!hasOpenQuote)
				{
					if (c != _separator) continue;
					list.Add(line.Substring(start, i-start));
					isNewValue = true;
					continue;
				}
				// has open quote, so we should care about double quotes and closing quotes
				if (c != '"') continue;

				if (_useDoubleQuotes)
				{
					if (i < line.Length - 1 && line[i + 1] == '"')
					{
						hasEscapedQuote = true;
						i++;
						continue;
					}
				}
				else
				{
					if (i > 0 && line[i - 1] == '\\')
					{
						hasEscapedQuote = true;
						continue;
					}
				}
				if (i < line.Length - 1 && line[i + 1] != _separator)
					throw new InvalidOperationException(Errors.NoSeparatorAfterClosingQuotes);

				string substring = line.Substring(start + 1, i - start - 1);
				if (hasEscapedQuote)
					substring = substring.Replace(_useDoubleQuotes ? "\"\"" : "\\\"", "\"");
				list.Add(substring);
				isNewValue = true;
				if (i < line.Length - 1) i++; // пропускаем разделитель
			}
			if (!hasOpenQuote && !isNewValue)
				list.Add(line.Substring(start, i - start));
				
			if (line.EndsWith(_separator.ToString()))
				list.Add(null);
			return list;
		}
	}
}
