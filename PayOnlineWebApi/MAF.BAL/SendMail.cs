using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MAF.BAL
{
    public class SendMail
    {

        private static readonly SendMail instance = new SendMail();

        public static SendMail Instance
        {
            get
            {
                return instance;
            }
        }

        string mailHtmlBody = @"<html>" +
                      " <body>" +
                      "<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#fff'>" +
                      "<tbody>" +
                      "<tr>" +
                      "<td>" +
                      "<table width='600' border='0' cellspacing='0' cellpadding='0' bgcolor='#FFFFFF' align=center style='border:1px solid #4a9dd4'>" +
                      "<tbody>" +
                      "<tr>" +
                      "<td style='text-align: center; padding: 20px 10px;border-top: 3px solid #2185c5;'><a href='http://www.midfinance.com' target='_blank'><img src=cid:myImageID width='200' height='61' border='0' ></a></td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td width='100%' align='left' valign='top' style='padding:0px 20px 10px;'>" +
                      "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                      "<tbody>" +
                      "<tr>" +
                      "<td height='25' align='left' valign='middle' ><font style='font-family: Arial, Helvetica, sans-serif; color:#353535; font-size:14px; font-weight:normal;'>Dear {0},</font></td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td height='25' align='left' valign='middle' ><font style='font-family: Arial, Helvetica, sans-serif; color:#353535; font-size:13px; font-weight:normal; line-height:22px;'>Greetings from Mid-Atlantic Finance Company!</font></td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td height='25' align='left' valign='middle' ><font style='font-family: Arial, Helvetica, sans-serif; color:#353535; font-size:13px; font-weight:normal; line-height:22px;'>{1}</font></td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td height='50' align='left' valign='middle' ><font style='font-family: Arial, Helvetica, sans-serif; color:#353535; font-size:13px; font-weight:normal; line-height:22px;'>Looking forward to more opportunities to be of service to you.</font></td>" +
                      "</tr>" +
                      "</tbody>" +
                      "</table>" +
                      "</td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td width='100%' align='left' valign='top' style='padding:0px 20px 10px;'>" +
                      "<table width='100%' border='0' cellspacing='0' cellpadding='0' style='margin-top:10px;'>" +
                      "<tr>" +
                      "<td height='25' align='left' valign='middle' ><font style='font-family: Arial, Helvetica, sans-serif; color:#353535; font-size:14px; line-height:22px;'>Thank you,<br/>" +
                      "Mid-Atlantic Finance Company<br/>" +
                      "Web Administrator<br/>" +
                      "</font></td>" +
                      "</tr>" +
                      "</table></td>" +
                      "</tr>" +
                      "<tr>" +
                      "<td>&nbsp;</td>" +
                      "</tr>" +
                      "</tbody>" +
                      "</table></td>" +
                      "</tr>" +
                      "</tbody>" +
                      "</table>" +
                      "</body>" +
                      "</html>";

        /// <summary>
        /// Method to send email based on mail message details
        /// </summary>
        /// <param name="mailTo">Mail To</param>
        /// <param name="mailSubject">Mail Subject</param>
        /// <param name="mailBody">Mail Body</param>
        private void Send(string mailTo, string mailSubject, string mailBody)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.Priority = MailPriority.Normal;
                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml = true;

                //create Alrternative HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");

                //Add Image
                var filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Images/logo.png");
                LinkedResource theEmailImage = new LinkedResource(filePath);
                theEmailImage.ContentId = "myImageID";

                //Add the Image to the Alternate view
                htmlView.LinkedResources.Add(theEmailImage);

                //Add view to the Email Message
                mailMessage.AlternateViews.Add(htmlView);

                //set the "from email" address and specify a friendly 'from' name
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailAddress"], "Mid-Atlantic Finance Company");

                //set the "to" email address
                mailMessage.To.Add(mailTo);

                //set the Email subject
                mailMessage.Subject = mailSubject;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["ServerAddress"]; //Or Your SMTP Server Address

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsSMTPServerWithCredentials"]))
                {
                    smtp.Credentials = new System.Net.NetworkCredential
                                        (ConfigurationManager.AppSettings["MailAddress"], ConfigurationManager.AppSettings["MailPassword"]); // ***use valid credentials***
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);

                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                }

                smtp.Send(mailMessage);
            }
            catch
            {
                throw;
            }
        }

        #region  send mail ChangePassword
        public string ChangePassword(string fullName, string accountNumber, string email, string newPassword)
        {
            string message = string.Empty;
            try
            {
                string subject = "MAF Online Payment Access";
                string body = string.Format(mailHtmlBody,fullName, "<br>We are sending your new login information as per your request. " +
                    "<br>You can now login using the following information.<br><br>MAF Account Number: <b>" + accountNumber +
                    "</b><br>Email (username):<b>" + email + "</b><br>New Password:<b>" + newPassword + "</b>");

                Send(email, subject, body);
                message = "mail send successfully";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;

        }
        #endregion

        #region  send mail ForgotPassword
        public string ForgotPassword(string fullName, string accountNumber, string email, string Password)
        {
            string message = string.Empty;
            try
            {
                string subject = "Online Payment Access";
                string body = string.Format(mailHtmlBody, fullName, "We are sending your login information as per your request. <br>You can now login using the following information.<br><br>MAF Account Number: <b>" + accountNumber + "</b><br>Email (username):<b>" + email + "</b><br>Password:<b>" + Password + "</b>");

                Send(email, subject, body);
                message = "mail send successfully";
            }
            catch (Exception exc)
            {
                message = exc.Message;
            }
            return message;
        }
        #endregion

        #region  send mail  registertion
        public string Register(string fullName, string accountNumber, string email, string Password)
        {
            string message = string.Empty;
            try
            {
                string subject = "MAF Online Payment Account";
                string body = string.Format(mailHtmlBody, fullName, "Thank you for choosing the online payment powered by CallPass Tech, LLC. We are sending your login information. <br>You can now login using the following information.<br><br>MAF Account Number: <b>" + accountNumber + "</b><br>Email (username):<b>" + email + "</b>");
                
                
                Send(email, subject, body);
                message = "saved successfully";
            }
            catch(Exception exc)
            {
                message = "Make sure your account hasn't been registered already.";
            }
            return message;
        }
        #endregion

        #region  send mail  Payment Confirmation
        public string PaymentConfirmation(string totalAmount, string confirmationNumber, string email, string BankHolder)
        {
            // DateTime dateTime = Convert.ToDateTime(paymentDate);
            DateTime dateTime = DateTime.Now;

            string date = dateTime.ToString("MM/dd/yyyy");
            string message = string.Empty;
            try
            {
                string body = string.Format(mailHtmlBody, BankHolder, "We have received payment of $" + Convert.ToDecimal(totalAmount) + " on " + date + ", your confirmation number is " + confirmationNumber + ".");
                string subject = "Payment received on your MAF Online Payment Account";

                Send(email, subject, body);
                message = "Mail sent successfully";
            }
            catch
            {
                message = "Make sure your account hasn't been registered already.";
            }
            return message;
        }
        #endregion

        #region OptInMail
        public void OptInMail(string email, string textNumber)
        {
            string message = string.Empty;

            try
            {
                string body = "Your verification pin has been sent on your text number XXX-XXX-" + textNumber.Substring(textNumber.Length - 4, 4) + ". Please enter your pin to verify and subscribe in Notifications By Text services.";
                string subject = "Text Number PIN Verification";

                Send(email, subject, body);
                message = "Submit successfully";
            }
            catch
            {
                 message = "Faild!.";
            }
            
        }
        #endregion

        #region VerifyPinMail
        public void VerifyPinMail(string email)
        {
            string message = string.Empty;

            try
            {
                string subject = "Text Number PIN Verification Confirmation";
                string body = "Your pin has been verified successfully and subscribed for Notifications By Text services";
                
                Send(email, subject, body);

                message = "Submit successfully";
            }
            catch
            {
                message = "Faild!.";
            }
           
        }
        #endregion

        #region OptOutMail
        public void OptOutMail(string email, string textNumber)
        {
            string message = string.Empty;

            try
            {
                string subject = "Text Number Unsubscribed Confirmation";
                string body = "Your text number XXX-XXX-" + textNumber.Substring(textNumber.Length - 4, 4) + " has been successfully unsubscribed from Notifications By Text services.";

                Send(email, subject, body);
                message = "Submit successfully";
            }
            catch
            {
                message = "Faild!.";
            }
            
        }
        #endregion
    }
}
