using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class AssignValuesToDTO
    {
        public static void AssingDToValues(ErrorLogDTO dto, Exception ex, string _pageurl, string _timezone)
        {
            // var repository = HttpContext.Current.User as UserPrincipal;
            StackTrace st = new StackTrace(ex, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(0);

            //Get the file name
            string fileName = frame.GetFileName();

            //Get the method name
            string methodName = frame.GetMethod().Name;

            //Get the line number from the stack frame
            int line = frame.GetFileLineNumber();

            //Get the column number
            int col = frame.GetFileColumnNumber();
            //if (repository != null)
            //{
            //    dto.userId = repository.userid;
            //}
            //else
            //{
            //    dto.userId = "";
            //}
            dto.userId = "999";
            dto.ExcMessage = ex.Message;
            dto.ExcSource = ex.Source;
            dto.ExcStackTrace = ex.StackTrace;
            dto.ExcType = "";
            dto.LineNo = line;
            dto.methodName = methodName;
            dto.pageUrl = _pageurl;
            dto.timezone = _timezone;

        }
    }
}