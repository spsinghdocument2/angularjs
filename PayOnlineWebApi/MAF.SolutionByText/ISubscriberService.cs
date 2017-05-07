namespace MAF.SolutionByText
{
    public interface ISubscriberService
    {
        ResponseMessage UnSubscribe(string phoneNumber);
        ResponseMessage UpdateSubscriber(string phoneNumber, string accountNumber);
    }
}
