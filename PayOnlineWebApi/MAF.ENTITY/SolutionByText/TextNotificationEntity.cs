namespace MAF.ENTITY.SolutionByText
{
    public class TextNotificationEntity
    {
        public string AccountNumber { get; set; }
        public string TextNumber { get; set; }
        public string Pin { get; set; }
        public bool IsPaymentReminder { get; set; }
        public bool IsPaymentConfirmation { get; set; }
        public bool IsPayByText { get; set; }
        public bool IsCommunicationByText { get; set; }
        public bool IsPrimaryTextNumber { get; set; }
    }
}
