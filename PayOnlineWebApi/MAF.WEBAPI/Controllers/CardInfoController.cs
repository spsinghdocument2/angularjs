using MAF.BAL;
using MAF.ENTITY.CyberSource;
using System;
using System.Web.Http;


namespace MAF.WEBAPI.Controllers
{
     [Authorize]
    [RoutePrefix("api/CardInfo")]
    public class CardInfoController : ApiController
    {
        // Get interface Account Information
        private readonly ICardInfo ICardInfo;
        //This property datamember is used to create object of Account Information
        CardInfoEntity cardInfoEntity = null;

        public CardInfoController(ICardInfo iCardInfo)
            {
                // Create object Payonline(Account Information)
                ICardInfo = iCardInfo;
            }

        #region User Card Information Get
        //Get the Card Information  function
        [HttpGet]
        [Route("GetCardInfo/{accountNumber:int}")]
        public IHttpActionResult GetCardInfo(int accountNumber)
        {
            try
            {
                //  This private datamember is used to create object of Account Information
                cardInfoEntity = new CardInfoEntity();


                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(accountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    return BadRequest(message);

                }
                else
                {
                    // search account Information get
                    //cardInfoEntity = ICardInfo.GetCardInfo(Convert.ToString(accountNumber));
                    accountNumber = 311676;
                    cardInfoEntity = ICardInfo.GetCardInfo(Convert.ToString(accountNumber));
                    //send web api  account Information
                    return Ok(cardInfoEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Card Profile Controller in Get Card Details Action Method", ex);
                return null;
            }
        }

        #endregion

    }
}
