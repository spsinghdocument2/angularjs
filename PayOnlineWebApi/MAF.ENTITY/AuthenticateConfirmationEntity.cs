using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAF.ENTITY
{
    public  class AuthenticateConfirmationEntity
    {
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string VehicleYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Error { get; set; }
    }
}
