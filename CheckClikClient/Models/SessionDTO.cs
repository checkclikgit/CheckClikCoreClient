using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models
{
    public class SessionDTO
    {
        public int BranchId { get; set; }
        public int StoreId { get; set; }
		public long UserId { get; set; } = 0;
        public bool IsLogin{ get; set; }
        public string UserName { get; set; }
        public int CartBranchId { get; set; }
        public int CartItems { get; set; }
        public string UserProfile { get; set; }
        public int StoreType { get; set; }
		public string UserChatId { get; set; } = "";
        public string UserGroupChatId { get; set; }
    }
}