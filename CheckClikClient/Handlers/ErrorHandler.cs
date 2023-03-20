using Customer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace CheckClikClient.Handlers
{
    public class ErrorHandler
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ErrorHandler(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public void WriteError(ErrorLogDTO obj, string errorMessage)
        {
            try
            {
                string path = "~/ErrorFiles/" +
                DateTime.Today.ToString("MM-dd-yyyy") + ".txt";

                //if(!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                //{

                //    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                //}
                //Path.Combine(_env.ContentRootPath, "App_Data/Files")
                if (!File.Exists(Path.Combine(_env.ContentRootPath, path)))
                {

                    File.Create(Path.Combine(_env.ContentRootPath, path)).Close();
                }
                using (StreamWriter w = File.AppendText(Path.Combine(_env.ContentRootPath, path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}",
                    DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string err = "Error in: " +
                    //System.Web.HttpContext.Current.Request.Url.ToString() +
                    _httpContextAccessor.HttpContext.Request.PathBase.ToString() +
                    ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.WriteLine("__________________________");
                    w.Flush();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                //WriteError(ex.Message);
            }

        }
    }
}