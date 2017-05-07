
namespace MAF.ENTITY
{
    public class CardDetailsEntity
    {
        public string TokenId{get; set;}
        public string CardName{get; set;}
        public string CardType{get; set;}
        public string CardNumber{get; set;}
        public string CardExpiry{get; set;}  
      
        public string AcctNo{get; set;}
        public string ConfirmationId { get; set; }
        public string RequestId{get; set;}
        public string ReasonCode{get; set;}
        public string Description{get; set;}
        public string Amount { get; set; }
        public string CardMaxLimit { get; set; }
    }
}
