using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customer
{
    public static class StringCaseUtil
    {
        public static string ToTitleCase(this string s)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string strTitleCase = textInfo.ToTitleCase(s);
            if (strTitleCase.EndsWith("Gb") || strTitleCase.EndsWith("Mb") || strTitleCase.EndsWith("Kb"))
                strTitleCase = strTitleCase.Replace("Gb", "GB").Replace("Mb", "MB").Replace("Kb", "KB");
            return strTitleCase;
        }
    }
}