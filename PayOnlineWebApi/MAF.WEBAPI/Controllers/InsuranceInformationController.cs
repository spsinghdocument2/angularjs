using MAF.BAL;
using MAF.ENTITY;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MAF.WEBAPI.Controllers
{
    [RoutePrefix("api/InsuranceInformation")]
    public class InsuranceInformationController : ApiController
    {

        private readonly IInsuranceInformation IInsuranceInformation;
        InsuranceInformationEntity insuranceInformation = null;
        public InsuranceInformationController(IInsuranceInformation iInsuranceInformation)
        {
            IInsuranceInformation = iInsuranceInformation;
        }


        #region Insurance Information post
        /// <summary>
        /////Save Insurance Information  function
        /// </summary>
        /// <param name="InsuranceInformationEntity">InsuranceInformationEntity</param>
        /// <returns>string</returns>
        [HttpPost]
        [Route("SaveInsuranceInformation")]
        public async Task<HttpResponseMessage> SaveInsuranceInformation()
        //  public IHttpActionResult SaveInsuranceInformation(InsuranceInformationEntity insuranceInformation)
        {
            try
            {

                string message = string.Empty;
                Dictionary<string, string> extensionLookup = new Dictionary<string, string>()
                    {
                        {"image/png", ".png"},
                        {"image/jpeg", ".jpeg"},
                        {"image/gid", ".gif"}
                    };

                var root = HttpContext.Current.Server.MapPath("~/images/InsuranceCardUploads");
                if (!System.IO.Directory.Exists(root))
                {
                    System.IO.Directory.CreateDirectory(root);
                }

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
               // var root = HttpContext.Current.Server.MapPath("~/images/cardUploads");

               // var root = HttpContext.Current.Server.MapPath("~/images/uploads");
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                if (result.FormData["model"] == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var modelData = result.FormData["model"];


                InsuranceInformationEntity model = JsonConvert.DeserializeObject<InsuranceInformationEntity>(modelData);

                //get the files
                foreach (var fileData in result.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;

                    fileName = Guid.NewGuid().ToString() + extensionLookup[fileData.Headers.ContentType.ToString()];
                    model.InsuranceCardPath = "~/images/uploads/" + fileName;
                    File.Move(fileData.LocalFileName, Path.Combine(root, fileName));
                }



                if (model == null)
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);


                }

                if (string.IsNullOrWhiteSpace(model.AccountNumber) || string.IsNullOrWhiteSpace(model.InsuranceCompanyName) || string.IsNullOrWhiteSpace(model.InsuranceAddress) || string.IsNullOrWhiteSpace(model.InsuranceCity) || string.IsNullOrWhiteSpace(model.InsuranceState) || string.IsNullOrWhiteSpace(model.InsuranceZip)
                    || string.IsNullOrWhiteSpace(model.AgencyCompanyName) || string.IsNullOrWhiteSpace(model.AgencyAddress) || string.IsNullOrWhiteSpace(model.AgencyCity) || string.IsNullOrWhiteSpace(model.AgencyState) || string.IsNullOrWhiteSpace(model.AgencyZip)
                   || string.IsNullOrWhiteSpace(model.InsuredName) || string.IsNullOrWhiteSpace(model.InsuredAddress) || string.IsNullOrWhiteSpace(model.InsuredCity) || string.IsNullOrWhiteSpace(model.InsuredState) || string.IsNullOrWhiteSpace(model.InsuredZip)
                     || string.IsNullOrWhiteSpace(model.PolicyNumber) || string.IsNullOrWhiteSpace(model.EffectiveDate) || string.IsNullOrWhiteSpace(model.ExpirationDate))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return Request.CreateResponse(HttpStatusCode.BadRequest, message);

                }
                else
                {
                    message = IInsuranceInformation.SaveInsuranceInformation(model);
                    return Request.CreateResponse(HttpStatusCode.OK, message);

                }

            }
            catch (Exception ex)
            {
                MAF.BAL.Logger.WriteErrorLog("Error in Insurance Information Controller Save Details Action Method", ex);
                return null;
            }
        }
        #endregion

        #region Get Insurance Information 
        /// <summary>
        /////Get Insurance Information  function
        /// </summary>
        /// <param name="AccountNumber">AccountNumber</param>
        /// <returns>all Get Insurance Information</returns>
        [Authorize]
        [HttpGet]
        [Route("GetInsuranceInformation/{AccountNumber:int}")]
        public IHttpActionResult GetInsuranceInformation(int AccountNumber)
        {
            try
            {
                //  This private datamember is used to create object of insurance Information
                insuranceInformation = new InsuranceInformationEntity();


                string message = string.Empty;
                if (string.IsNullOrWhiteSpace(Convert.ToString(AccountNumber)))
                {
                    message = string.Format(MAF.BAL.ResourceFile.Common.RequestInvalid);
                    // Error message send
                    return BadRequest(message);

                }
                else
                {

                    insuranceInformation = IInsuranceInformation.GetInsuranceInformation(Convert.ToString(AccountNumber));
                    //send web api  account Information
                    return Ok(insuranceInformation);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Insurance Information Get Details Action Method", ex);
                return null;
            }
        
        
        }

        #endregion

        
         #region Get Insurance Company List 
        /// <summary>
        /////Get Insurance Company List
        /// </summary>        
        /// <returns>all Get Insurance Company List</returns>
       [Authorize]
        [HttpGet]
        [Route("GetInsuranceCompanyList")]
        public IHttpActionResult GetInsuranceCompanyList()
        {
            try
            {
               
                //  This private datamember is used to create object of insurance Information
                List<InsuranceInformationEntity> insuranceCompanyList = new List<InsuranceInformationEntity>();


                 insuranceCompanyList = IInsuranceInformation.GetInsuranceCompanyList();
                //send web api  account Information
                return Ok(insuranceCompanyList);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog("Error in Insurance Information Get Insurance Company List Action Method", ex);
                return null;
            }

            
        }

        #endregion

    }
}
