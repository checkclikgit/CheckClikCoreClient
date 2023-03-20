using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class ShiftTimingDTO
    {
        public int WeekDay { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsOpen24Hrs { get; set; }
        public bool IsClose { get; set; }
    }
}