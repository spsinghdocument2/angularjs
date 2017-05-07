using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;
using MAF.CyberSource.Client;
using MAF.CyberSource.Client.SoapServiceReference;
using System.Security.Cryptography;
using MAF.ENTITY.CyberSource;

namespace CyberSource.CyberService
{
    public class CyberService
	{        

        public  string TokenizationPaymentsSubscription(CardInfoEntity objCardInfo)
        {
            RequestMessage request = new RequestMessage();

            // Credit Card Authorization
            request.ccAuthService = new CCAuthService();
            request.ccAuthService.run = "true";

            // Credit Card Capture
            request.ccCaptureService = new CCCaptureService();
            request.ccCaptureService.run = "true";

            request.paySubscriptionCreateService = new PaySubscriptionCreateService();
            request.paySubscriptionCreateService.run = "true";
            // add required fields

            request.merchantReferenceCode = objCardInfo.MerchantReferenceCode;
            //request.merchantID = "1111";

            BillTo billTo = new BillTo();
            billTo.firstName = objCardInfo.FirstName;
            billTo.lastName = objCardInfo.LastName;
            billTo.street1 = objCardInfo.Street;
            billTo.city = objCardInfo.City;
            billTo.state = objCardInfo.State;
            billTo.postalCode = objCardInfo.PostelCode;
            billTo.country = "United States";
            billTo.customerID = objCardInfo.AccountNumber;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            billTo.email = objCardInfo.Email;
            billTo.ipAddress = objCardInfo.IpAddress;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            request.billTo = billTo;

            Card card = new Card();
            card.accountNumber = objCardInfo.CardNumber;
            card.expirationMonth = objCardInfo.ExpirationMonth;
            card.expirationYear = objCardInfo.ExpirationYear;
            card.cardType = objCardInfo.CardType;
            card.cvNumber = objCardInfo.CvNumber;
            request.card = card;


            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "USD";
            purchaseTotals.grandTotalAmount = objCardInfo.Amount;
            request.purchaseTotals = purchaseTotals;            

            RecurringSubscriptionInfo recurring = new RecurringSubscriptionInfo();
            recurring.frequency = "on-demand";
            request.recurringSubscriptionInfo = recurring;
            // add optional fields here per your business needs

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);
                return ProcessReply(reply);
            }
       

           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {               
              return  HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                return HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                return HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                return HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                return HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                return HandleException(e);
            }
        }
        
        public string TokenizationPayments(CardInfoEntity objCardInfo)
        {
            RequestMessage request = new RequestMessage();

            // Credit Card Authorization
            request.ccAuthService = new CCAuthService();
            request.ccAuthService.run = "true";

            // Credit Card Capture
            request.ccCaptureService = new CCCaptureService();
            request.ccCaptureService.run = "true";

            // add required fields

            request.merchantReferenceCode = objCardInfo.MerchantReferenceCode;
            //request.merchantID = "1111";

            BillTo billTo = new BillTo();
            billTo.firstName = objCardInfo.FirstName;
            billTo.lastName = objCardInfo.LastName;
            billTo.street1 = objCardInfo.Street;
            billTo.city = objCardInfo.City;
            billTo.state = objCardInfo.State;
            billTo.postalCode = objCardInfo.PostelCode;
            billTo.country = "United States";
            billTo.customerID = objCardInfo.AccountNumber;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            billTo.email = objCardInfo.Email;
            billTo.ipAddress = objCardInfo.IpAddress;
            request.billTo = billTo;

            Card card = new Card();
            card.accountNumber = objCardInfo.CardNumber;
            card.expirationMonth = objCardInfo.ExpirationMonth;
            card.expirationYear = objCardInfo.ExpirationYear;
            card.cardType = objCardInfo.CardType;
            card.cvNumber = objCardInfo.CvNumber;
            request.card = card;


            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "USD";
            purchaseTotals.grandTotalAmount = objCardInfo.Amount;
            request.purchaseTotals = purchaseTotals;

            RecurringSubscriptionInfo recurring = new RecurringSubscriptionInfo();
            recurring.frequency = "on-demand";
            request.recurringSubscriptionInfo = recurring;
            // add optional fields here per your business needs

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);
                return GetTokenizationContent(reply);
            }


           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                return HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                return HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                return HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                return HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                return HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public string TokenizationSubscription(CardInfoEntity objCardInfo)
        {
            RequestMessage request = new RequestMessage();

            request.paySubscriptionCreateService = new PaySubscriptionCreateService();
            request.paySubscriptionCreateService.run = "true";

            // add required fields

            request.merchantReferenceCode = objCardInfo.MerchantReferenceCode;
            //request.merchantID = "1111";

            BillTo billTo = new BillTo();
            billTo.firstName = objCardInfo.FirstName;
            billTo.lastName = objCardInfo.LastName;
            billTo.street1 = objCardInfo.Street;
            billTo.city = objCardInfo.City;
            billTo.state = objCardInfo.State;
            billTo.postalCode = objCardInfo.PostelCode;
            billTo.country = "United States";
            billTo.customerID = objCardInfo.AccountNumber;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            billTo.email = objCardInfo.Email;
            billTo.ipAddress = objCardInfo.IpAddress;
            request.billTo = billTo;

            Card card = new Card();
            card.accountNumber = objCardInfo.CardNumber;
            card.expirationMonth = objCardInfo.ExpirationMonth;
            card.expirationYear = objCardInfo.ExpirationYear;
            card.cardType = objCardInfo.CardType;
            //card.cvNumber = objCardInfo.CvNumber;
            request.card = card;


            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "USD";
            //purchaseTotals.grandTotalAmount = objCardInfo.Amount;  //commented because don't require when creating subscription
            purchaseTotals.grandTotalAmount = "0.0";
            request.purchaseTotals = purchaseTotals;

            RecurringSubscriptionInfo recurring = new RecurringSubscriptionInfo();
            recurring.frequency = "on-demand";
            request.recurringSubscriptionInfo = recurring;
            // add optional fields here per your business needs

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);
                return ProcessReply(reply);
            }


           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                return HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                return HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                return HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                return HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                return HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        public  string RePayment(CardInfoEntity objCardInfo)
        {
            RequestMessage request = new RequestMessage();

            // add required fields
            request.ccAuthService = new CCAuthService();
            request.ccAuthService.run = "true";

            // Credit Card Capture
            request.ccCaptureService = new CCCaptureService();
            request.ccCaptureService.run = "true";

            // Update Card Information
            request.paySubscriptionUpdateService = new PaySubscriptionUpdateService();
            request.paySubscriptionUpdateService.run = "true";

            request.merchantReferenceCode = objCardInfo.MerchantReferenceCode;

            BillTo billTo = new BillTo();
            billTo.firstName = objCardInfo.FirstName;
            billTo.lastName = objCardInfo.LastName;
            billTo.street1 = objCardInfo.Street;
            billTo.city = objCardInfo.City;
            billTo.state = objCardInfo.State;
            billTo.postalCode = objCardInfo.PostelCode;
            billTo.country = "United States";
            billTo.customerID = objCardInfo.AccountNumber;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            billTo.email = objCardInfo.Email;
            billTo.ipAddress = objCardInfo.IpAddress;
            request.billTo = billTo;

            PaymentNetworkToken network = new PaymentNetworkToken();
            network.transactionType = "1";
            request.paymentNetworkToken = network;
             
            RecurringSubscriptionInfo recurring = new RecurringSubscriptionInfo();
            recurring.subscriptionID = objCardInfo.TokenId;
            request.recurringSubscriptionInfo = recurring;

            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "USD";
            purchaseTotals.grandTotalAmount = objCardInfo.Amount;
            request.purchaseTotals = purchaseTotals;            

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);
                return  GetTokenizationContent(reply);
            }

           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                return HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                return HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                return HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                return HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                return HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                return HandleException(e);
            }

        }

        public string UpdateSubscription(CardInfoEntity objCardInfo)
        {
            RequestMessage request = new RequestMessage();

            request.paySubscriptionUpdateService = new PaySubscriptionUpdateService();
            request.paySubscriptionUpdateService.run = "true";

            // add required fields

            request.merchantReferenceCode = objCardInfo.MerchantReferenceCode;
            //request.merchantID = "1111";

            BillTo billTo = new BillTo();
            billTo.firstName = objCardInfo.FirstName;
            billTo.lastName = objCardInfo.LastName;
            billTo.street1 = objCardInfo.Street;
            billTo.city = objCardInfo.City;
            billTo.state = objCardInfo.State;
            billTo.postalCode = objCardInfo.PostelCode;
            billTo.country = "United States";
            billTo.customerID = objCardInfo.AccountNumber;
            billTo.phoneNumber = objCardInfo.PrimaryNumber;
            billTo.email = objCardInfo.Email;
            billTo.ipAddress = objCardInfo.IpAddress;
            request.billTo = billTo;

            PaymentNetworkToken network = new PaymentNetworkToken();
            network.transactionType = "1";
            request.paymentNetworkToken = network;

            RecurringSubscriptionInfo recurring = new RecurringSubscriptionInfo();
            recurring.subscriptionID = objCardInfo.TokenId;
            request.recurringSubscriptionInfo = recurring;
            // add optional fields here per your business needs

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);
                return GetTokenizationContent(reply);
            }


           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                return HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                return HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                return HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                return HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                return HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        [Obsolete]
        public static void RecurringPayments()
        {
            RequestMessage request = new RequestMessage();

            // we will let the client pick up the merchantID
            // from the config file.  In multi-merchant scenarios,
            // you would set a merchantID in each request.

            // this sample requests auth and capture

            // Credit Card Authorization
            request.ccAuthService = new CCAuthService();
            request.ccAuthService.run = "true";

            // Credit Card Capture
            request.ccCaptureService = new CCCaptureService();
            request.ccCaptureService.run = "true";

            request.paySubscriptionCreateService = new PaySubscriptionCreateService();
            request.paySubscriptionCreateService.run = "true";
            // add required fields

            request.merchantReferenceCode = "12345678";

            BillTo billTo = new BillTo();
            billTo.firstName = "John";
            billTo.lastName = "Doe";
            billTo.street1 = "1295 Charleston Road";
            billTo.city = "Mountain View";
            billTo.state = "CA";
            billTo.postalCode = "94043";
            billTo.country = "US";
            billTo.email = "null@cybersource.com";
            billTo.ipAddress = "10.7.111.111";
            request.billTo = billTo;

            Card card = new Card();
            card.accountNumber = "4111111111111111";
            card.expirationMonth = "12";
            card.expirationYear = "2020";
            card.cardType = "001";
            request.card = card;


            PurchaseTotals purchaseTotals = new PurchaseTotals();
            purchaseTotals.currency = "USD";
            request.purchaseTotals = purchaseTotals;

            // there are two items in this sample
            request.item = new Item[2];

            Item item = new Item();
            item.id = "0";
            item.unitPrice = "12.34";
            request.item[0] = item;

            item = new Item();
            item.id = "1";
            item.unitPrice = "56.78";
            request.item[1] = item;

            RecurringSubscriptionInfo recurringSubscriptionInfo = new RecurringSubscriptionInfo();
            recurringSubscriptionInfo.amount = "50";
            recurringSubscriptionInfo.frequency = "Monthly";
            recurringSubscriptionInfo.numberOfPayments = "3";
            recurringSubscriptionInfo.startDate = "20160730";

            request.recurringSubscriptionInfo = recurringSubscriptionInfo;
            // add optional fields here per your business needs

            try
            {
                ReplyMessage reply = SoapClient.RunTransaction(request);                
                ProcessReply(reply);
            }            

           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                HandleWebException(we);
            }
            //Any other exception!
            catch (Exception e)
            {
                HandleException(e);
            }

            Console.WriteLine("Press Return to end...");
            Console.ReadLine();
        }

		private static string ProcessReply( ReplyMessage reply )
		{    		
			string content  = GetContent( reply );			 
            return content;
		}

		private static string GetContent( ReplyMessage reply )
		{

			int reasonCode = int.Parse( reply.reasonCode );
			switch (reasonCode)
			{
					// Success
				case 100: return (""+ reasonCode+" " + reply.requestID +" SubscriptionID: " + reply.paySubscriptionCreateReply.subscriptionID);
                case 101: return ("" + reasonCode + " " + reply.requestID + " Required field(s) are missing: " + EnumerateValues(reply.missingField));
                case 102: return ("" + reasonCode + " " + reply.requestID + " Field(s) are invalid: " + EnumerateValues(reply.invalidField));
                case 204: return ("" + reasonCode + " " + reply.requestID + " Insufficient funds in the account.  Please use a different card or select another form of payment.");
                case 110: return ("" + reasonCode + " " + reply.requestID + " Partial amount approved.");
                case 150: return ("" + reasonCode + " " + reply.requestID + " General system failure.");
                case 151: return ("" + reasonCode + " " + reply.requestID + " The request was received but there was a server timeout.");
                case 152: return ("" + reasonCode + " " + reply.requestID + " The request was received, but a service did not finish running in time.");
                case 200: return ("" + reasonCode + " " + reply.requestID + " The authorization request was approved by the issuing bank but declined by CyberSource.");
                case 201: return ("" + reasonCode + " " + reply.requestID + " The issuing bank has questions about the request. Please contact your bank.");
                case 202: return ("" + reasonCode + " " + reply.requestID + " Expired card.");
                case 203: return ("" + reasonCode + " " + reply.requestID + " Decline card.");
                case 205: return ("" + reasonCode + " " + reply.requestID + " Stolen or lost card.");
                case 207: return ("" + reasonCode + " " + reply.requestID + " Issuing bank unavailable.");
                case 208: return ("" + reasonCode + " " + reply.requestID + " Inactive card or card not authorized.");
                case 210: return ("" + reasonCode + " " + reply.requestID + " The card has reached the credit limit.");
                case 211: return ("" + reasonCode + " " + reply.requestID + " Invalid card verification number.");
                case 220: return ("" + reasonCode + " " + reply.requestID + " The processor declined the request based on a general issue with the customer’s account.");
                case 221: return ("" + reasonCode + " " + reply.requestID + " The customer matched an entry on the processor’s negative file.");
                case 222: return ("" + reasonCode + " " + reply.requestID + " The customer’s bank account is frozen.");
                case 230: return ("" + reasonCode + " " + reply.requestID + " The authorization request was approved by the issuing bank but declined by CyberSource");
                case 231: return ("" + reasonCode + " " + reply.requestID + " Invalid card number.");
                case 232: return ("" + reasonCode + " " + reply.requestID + " The card type is not accepted by the payment processor.");
                case 233: return ("" + reasonCode + " " + reply.requestID + " General decline by the processor.");
                case 234: return ("" + reasonCode + " " + reply.requestID + " There is a problem with your CyberSource merchant configuration. Please contact customer care.");
                case 236: return ("" + reasonCode + " " + reply.requestID + " Processor failure.");
                case 240: return ("" + reasonCode + " " + reply.requestID + " The card type sent is invalid.");
                case 250: return ("" + reasonCode + " " + reply.requestID + " The request was received, but there was a timeout at the payment processor."); 
				default:
					return( String.Empty );
			}	
		}
        private static string GetTokenizationContent(ReplyMessage reply)
        {

            int reasonCode = int.Parse(reply.reasonCode);
            switch (reasonCode)
            {
                // Success
                case 100: return ("" + reasonCode + " " + reply.requestID);
                case 101: return ("" + reasonCode + " " + reply.requestID + " Required field(s) are missing: " + EnumerateValues(reply.missingField));
                case 102: return ("" + reasonCode + " " + reply.requestID + " Field(s) are invalid: " + EnumerateValues(reply.invalidField));
                case 204: return ("" + reasonCode + " " + reply.requestID + " Insufficient funds in the account.  Please use a different card or select another form of payment.");
                case 110: return ("" + reasonCode + " " + reply.requestID + " Partial amount approved.");
                case 150: return ("" + reasonCode + " " + reply.requestID + " General system failure.");
                case 151: return ("" + reasonCode + " " + reply.requestID + " The request was received but there was a server timeout.");
                case 152: return ("" + reasonCode + " " + reply.requestID + " The request was received, but a service did not finish running in time.");
                case 200: return ("" + reasonCode + " " + reply.requestID + " The authorization request was approved by the issuing bank but declined by CyberSource.");
                case 201: return ("" + reasonCode + " " + reply.requestID + " The issuing bank has questions about the request. Please contact your bank.");
                case 202: return ("" + reasonCode + " " + reply.requestID + " Expired card.");
                case 203: return ("" + reasonCode + " " + reply.requestID + " Decline card.");
                case 205: return ("" + reasonCode + " " + reply.requestID + " Stolen or lost card.");
                case 207: return ("" + reasonCode + " " + reply.requestID + " Issuing bank unavailable.");
                case 208: return ("" + reasonCode + " " + reply.requestID + " Inactive card or card not authorized.");
                case 210: return ("" + reasonCode + " " + reply.requestID + " The card has reached the credit limit.");
                case 211: return ("" + reasonCode + " " + reply.requestID + " Invalid card verification number.");
                case 220: return ("" + reasonCode + " " + reply.requestID + " The processor declined the request based on a general issue with the customer’s account.");
                case 221: return ("" + reasonCode + " " + reply.requestID + " The customer matched an entry on the processor’s negative file.");
                case 222: return ("" + reasonCode + " " + reply.requestID + " The customer’s bank account is frozen.");
                case 230: return ("" + reasonCode + " " + reply.requestID + " The authorization request was approved by the issuing bank but declined by CyberSource");
                case 231: return ("" + reasonCode + " " + reply.requestID + " Invalid card number.");
                case 232: return ("" + reasonCode + " " + reply.requestID + " The card type is not accepted by the payment processor.");
                case 233: return ("" + reasonCode + " " + reply.requestID + " General decline by the processor.");
                case 234: return ("" + reasonCode + " " + reply.requestID + " There is a problem with your CyberSource merchant configuration. Please contact customer care.");
                case 236: return ("" + reasonCode + " " + reply.requestID + " Processor failure.");
                case 240: return ("" + reasonCode + " " + reply.requestID + " The card type sent is invalid.");
                case 250: return ("" + reasonCode + " " + reply.requestID + " The request was received, but there was a timeout at the payment processor.");
                default:
                    return (String.Empty);
            }
        }

        private static string HandleException(Exception e)
        {
            string content = String.Format("500 0000000000000000000000 Server Error.");

            return content;
        }

        private static string HandleSecurityException(MessageSecurityException e)
        {
            string content = String.Format("500 0000000000000000000000 Server Error.");
            return content;
        }
        
        private static string HandleCryptoException(Exception e)
        {
            string content = String.Format("500 0000000000000000000000 Server Error.");
            return content;
        }
		
        private static string HandleWebException( WebException we )
		{
            string content = String.Format("501 0000000000000000000000 Transaction Faild.");
            return content;
		}

		private static string EnumerateValues( string[] array )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (string val in array)
			{
				sb.Append( val + "\n" );
			}

			return( sb.ToString() );
		}

		private static bool IsCriticalError( WebException we )
		{
			switch (we.Status)
			{
				case WebExceptionStatus.ProtocolError:
					if (we.Response != null)
					{
						HttpWebResponse response 
							= (HttpWebResponse) we.Response;

						// GatewayTimeout may be returned if you are
						// connecting through a proxy server.
						return(	response.StatusCode ==
							HttpStatusCode.GatewayTimeout );
					
					}

					// In case of ProtocolError, the Response property
					// should always be present.  In the unlikely case 
					// that it is not, we assume something went wrong
					// along the way and to be safe, treat it as a
					// critical error.
					return( true );

				case WebExceptionStatus.ConnectFailure:
				case WebExceptionStatus.NameResolutionFailure:
				case WebExceptionStatus.ProxyNameResolutionFailure:
				case WebExceptionStatus.SendFailure:
					return( false );

				default:
					return( true );
			}
		}
	}
}
