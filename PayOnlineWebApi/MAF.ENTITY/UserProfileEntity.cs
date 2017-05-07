using System;

namespace MAF.ENTITY
{
    public class UserProfileEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; }
        public string ProfilePicture { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public int DaysPastDue { get; set; }
        public decimal PastDueAmount { get; set; }
        public decimal LastPaymentAmount { get; set; }
        public string CellNumber { get; set; }
        public string TextNumber { get; set; }
        public string TextNumber2 { get; set; } 
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PaymentFrequency { get; set; }
        public Byte SecurityID { get; set; }
        public Byte SecurityID2 { get; set; }
        public Byte SecurityID3 { get; set; }
        public string Answer { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }                             
        
    }
}
