using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using log4net;
using log4net.Config;

namespace GVUZ.ImportService2016.Core.Main.Log
{
	/// <summary>
	/// log4net:HostName, log4net:Identity, log4net:UserName
	/// </summary>
	public static class LogHelper
	{
		public static readonly ILog Log = LogManager.GetLogger("GlobalLogger");

		public static void InitLoging()
		{
			InitLoging(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
		}

		public static void InitLoging(string configFilePath)
		{
			XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));

            //GlobalContext.Properties["RawUrl"] = new RawUrl();
            //GlobalContext.Properties["UrlReferrer"] = new UrlReferrer();
            //GlobalContext.Properties["UserHostAddress"] = new UserHostAddress();
            //GlobalContext.Properties["UserHostName"] = new UserHostName();
            //GlobalContext.Properties["UserAgent"] = new UserAgent();
            GlobalContext.Properties["MachineName"] = new MachineName();
            GlobalContext.Properties["BaseAppPath"] = new BaseApplicationPath();
		}

		public static void LogCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
			{
				if (Log.IsFatalEnabled)
				{
					Log.Fatal("Fatal Exception (background thread)",
										(e.ExceptionObject as Exception) ?? new Exception(e.ExceptionObject.ToString()));
				}
			}
			else
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error("Unhandled Exception in Domain",
										(e.ExceptionObject as Exception) ?? new Exception(e.ExceptionObject.ToString()));
				}
			}
		}

		
	}

	public class GCAllocatedBytesHelper
	{
		public override string ToString()
		{
			return GC.GetTotalMemory(true).ToString();
		}
	}

	public class MachineName
	{
		public override string ToString()
		{
			return Environment.MachineName;
		}
	}

	public class ExecutingAssembly
	{
		public override string ToString()
		{
			return Assembly.GetExecutingAssembly().FullName;
		}
	}

	public class AppDomainName
	{
		public override string ToString()
		{
			return AppDomain.CurrentDomain.FriendlyName;
		}
	}

    public class BaseApplicationPath
    {
        public override string ToString()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }

	public class WindowsIdentityName
	{
		public override string ToString()
		{
			return WindowsIdentity.GetCurrent().Name;
		}
	}

    //public class HttpRuntimeProperty
    //{
    //    protected static bool IsAspNetApp
    //    {
    //        get { return HostingEnvironment.IsHosted && HttpContext.Current != null; }
    //    }

    //    protected static bool HasRequest
    //    {
    //        get { return HostingEnvironment.IsHosted && HttpContext.Current != null && HttpContext.Current.Request != null; }
    //    }
    //}

    //public class RawUrl : HttpRuntimeProperty
    //{
    //    [DebuggerStepThrough]
    //    public override string ToString()
    //    {
    //        string result = null;
    //        try
    //        {
    //            result = HasRequest ? HttpContext.Current.Request.RawUrl : null;
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //}

    //public class UrlReferrer : HttpRuntimeProperty
    //{
    //    public static readonly ILog Log = LogManager.GetLogger("GlobalLogger");

    //    [DebuggerStepThrough]
    //    public override string ToString()
    //    {
    //        string absoluteUri = null;
    //        try
    //        {
    //            absoluteUri = (HasRequest && HttpContext.Current.Request.UrlReferrer != null)
    //                ? HttpContext.Current.Request.UrlReferrer.AbsoluteUri
    //                : null;
    //        }
    //        catch (UriFormatException ex)
    //        {
    //            if (Log.IsInfoEnabled)
    //                Log.Info(ex);
    //        }
    //        catch
    //        {
    //        }

    //        return absoluteUri;
    //    }
    //}

    //public class UserHostAddress : HttpRuntimeProperty
    //{
    //    [DebuggerStepThrough]
    //    public override string ToString()
    //    {
    //        string result = null;
    //        try
    //        {
    //            result = HasRequest ? HttpContext.Current.Request.UserHostAddress : null;
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //}

    //public class UserHostName : HttpRuntimeProperty
    //{
    //    [DebuggerStepThrough]
    //    public override string ToString()
    //    {
    //        string result = null;
    //        try
    //        {
    //            result = HasRequest ? HttpContext.Current.Request.UserHostName : null;
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //}

    //public class UserAgent : HttpRuntimeProperty
    //{
    //    [DebuggerStepThrough]
    //    public override string ToString()
    //    {
    //        string result = null;
    //        try
    //        {
    //            result = HasRequest ? HttpContext.Current.Request.UserAgent : null;
    //        }
    //        catch
    //        {
    //        }
    //        return result;
    //    }
    //}
}
