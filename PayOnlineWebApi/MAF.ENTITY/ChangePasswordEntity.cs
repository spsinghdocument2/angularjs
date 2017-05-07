using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public class ChangePasswordEntity
    {
        public string AccountNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public string NewPassword { get; set; }
        public string BitReset { get; set; }
         
    }
    
}
