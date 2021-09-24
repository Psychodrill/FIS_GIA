using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.Controllers
{
	/// <summary>
	/// Получение набора данных из строки с JSON (эмуляция стандартного механизма, где его нужно вызвать вручную)
	/// </summary>
	public static class JsonRequestWrapper
	{
		private class JsonFakeContext : HttpContextBase
		{
			private class FakeRequest : HttpRequestBase
			{
				public override string ContentType
				{
					get
					{
						return "application/json";
					}

					set
					{
						base.ContentType = value;
					}
				}

				private readonly Stream _inputStream;
				public override Stream InputStream
				{
					get
					{
						return _inputStream;
					}
				}

				public FakeRequest(string request)
				{
                    if (request == null)
                    {
                        _inputStream = new MemoryStream();
                        return;
                    }
					_inputStream = new MemoryStream(Encoding.UTF8.GetBytes(request));
				}
			}

			private readonly FakeRequest _fakeRequest;
			public override HttpRequestBase Request
			{
				get
				{
					return _fakeRequest;
				}
			}

			public JsonFakeContext(string request)
			{
				_fakeRequest = new FakeRequest(request);
			}
		}

		public static IValueProvider GetValueProvider(string jsonData)
		{
			JsonValueProviderFactory factory = new JsonValueProviderFactory();
			ControllerContext cxt = new ControllerContext();
			cxt.HttpContext = new JsonFakeContext(jsonData);
			return factory.GetValueProvider(cxt);
		}
	}
}