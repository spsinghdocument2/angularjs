using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MAF.ENTITY
{
      public  class LoginEntity
     {
        public string AccountNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ACNStatus { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsBitReset { get; set; }
        public string Message { get; set; }
    }
}
