using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Esrp.Web.Auth
{
	/// <summary>
	/// Summary description for renew
	/// </summary>
	public class renew : IHttpHandler
	{
		private static readonly byte[] ImageData = new byte[]{0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xFF, 0xFF, 0xFF, 0x21, 0xF9, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x2C, 0x00, 0x00, 0x00, 0x00,
			0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x01, 0x44, 0x00, 0x3B};

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "image/gif";
			context.Response.BinaryWrite(ImageData);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}