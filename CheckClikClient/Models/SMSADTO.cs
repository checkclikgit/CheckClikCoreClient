using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class SMSADTO
    {
        public string passKey { get; set; }
        public string refNo { get; set; }
        public string sentDate { get; set; }
        public string idNo { get; set; }
        public string cName { get; set; }
        public string Cntry { get; set; }
        public string cCity { get; set; }
        public string cZip { get; set; }
        public string cPOBox { get; set; }
        public string cMobile { get; set; }
        public string cTel1 { get; set; }
        public string cTel2 { get; set; }
        public string cAddr1 { get; set; }
        public string cAddr2 { get; set; }
        public string shipType { get; set; }
        public int PCs { get; set; }
        public string cEmail { get; set; }
        public string carrValue { get; set; }
        public string carrCurr { get; set; }
        public string codAmt { get; set; }
        public string weight { get; set; }
        public string custVal { get; set; }
        public string custCurr { get; set; }
        public string insrAmt { get; set; }
        public string insrCurr { get; set; }
        public string itemDesc { get; set; }
        public string sName { get; set; }
        public string sContact { get; set; }
        public string sAddr1 { get; set; }
        public string sAddr2 { get; set; }
        public string sCity { get; set; }
        public string sPhone { get; set; }
        public string sCntry { get; set; }
        public DateTime prefDelvDate { get; set; }
        public string gpsPoints { get; set; }
        public string awbNo { get; set; }
        public string reason { get; set; }
    }
}