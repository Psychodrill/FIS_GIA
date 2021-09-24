using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GVUZ.Helper;

namespace GVUZ.Web.Portlets
{
	public static class HtmlPartialRenderer
	{
		public static string SimplePager(this HtmlHelper helper, int currentPage, int pageCount,
		                                 string urlTemplate, string pagerClass, string pagerAction)
		{
			if (currentPage < 0) currentPage = 1;
			if (pageCount < 0) pageCount = 0;
			if (pageCount == 1) //не показываем пейджер
				return "";

			var pager = new PagerBuilder(urlTemplate)
			            	{
			            		PagerClass = pagerClass,
			            		PagerAction = pagerAction,
			            		PagerPage = currentPage
			            	};

			if (currentPage > 1)
			{
				pager.AddPage("&lt;&lt;", 1, "", "setPage(1)");
				pager.AddPage("&lt;", currentPage - 1, "", "setPage(" + (currentPage - 1).ToString() + ");");
			}

			int start = Math.Max(currentPage - 2, 1);
			int end = Math.Min(pageCount, currentPage + 2);

			for (var i = start; i <= end; i++)
				pager.AddPage(i.ToString(), i, i == currentPage ? "current" : "", "setPage(" + i + ");");

			if (currentPage < pageCount)
			{
				pager.AddPage("&gt;", currentPage + 1, "", "setPage(" + (currentPage + 1).ToString() + ");");
				pager.AddPage("&gt;&gt;", pageCount, "", "setPage(" + pageCount + ");");
			}

			return pager.ToString();
		}

	}

}