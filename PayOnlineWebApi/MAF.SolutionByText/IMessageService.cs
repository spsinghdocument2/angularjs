using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAF.SolutionByText
{
    public interface IMessageService
    {
        ResponseMessage RequestVBT(string phoneNumber);
        ResponseMessage ConfirmVBT(string phoneNumber, string pin);
        ResponseMessage SendMessage(string accountNumber, string message);
    }
}
