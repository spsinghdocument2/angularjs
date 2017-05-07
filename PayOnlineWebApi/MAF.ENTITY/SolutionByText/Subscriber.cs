
namespace MAF.ENTITY.SolutionByText
{
    public class Subscriber
    {
        public int SubscriberID { get; set; }
        public string AccountNumber { get; set; }
        public string TextNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool DoNotText { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPaymentReminderNotification { get; set; }
        public bool IsPaymentConfirmationNotification { get; set; }
        public bool IsPayByTextNotification { get; set; }
        public bool IsCommunicationByTextNotification { get; set; }
    }
}
