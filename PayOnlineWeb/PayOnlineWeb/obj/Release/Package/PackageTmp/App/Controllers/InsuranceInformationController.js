app.controller('InsuranceInformationController', ['$scope', '$rootScope', '$location', 'InsuranceInformationService', 'WebApiService', function ($scope, $rootScope, $location, InsuranceInformationService, WebApiService)
{
    $scope.InsuranceCompanyName ="";
    $scope.InsuranceAddress ="";
    $scope.InsuranceCity ="";
    $scope.InsuranceState ="";
    $scope.InsuranceZip = "";

    $scope.AgencyCompanyName ="";
    $scope.AgencyAddress ="";
    $scope.AgencyCity ="";
    $scope.AgencyState ="";
    $scope.AgencyZip ="";

    $scope.InsuredName ="";
    $scope.InsuredAddress ="";
    $scope.InsuredCity ="";
    $scope.InsuredState ="";
    $scope.InsuredZip = "";

    $scope.PolicyNumber ="";
    $scope.EffectiveDate ="";
    $scope.ExpirationDate = "";



    $scope.Year ="";
    $scope.Make ="";
    $scope.Model = "";
    $scope.MakeModel = ""
    $scope.InsuranceCardPath = "";
    $scope.VehicleIdentificationNumber ="";
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.btndisabled = false;
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.message = "";
    $scope.loading = true;
    var stateList = [];
    $scope.StateList = [];
    $scope.searchInsuranceCompanyName = "";
    //var InsuranceCompanyList = [];
    //$scope.InsuranceCompanyList = [];

    $scope.chkuserProfileaddress =
  {
      value: false
  };
    var bindInsuranceComapnyList = function () {

        InsuranceInformationService.GetInsuranceCompanyNameList($scope.baseAddress).then(function (data) {
            $scope.InsuranceCompanyList = data;
          bindInsurance();
        },
          function (err) {
              $scope.ErrorMessage = "Get Insurance Company Name List Failed!";
          });
    };

    var bindStateList = function () {
        InsuranceInformationService.GetStateList().then(function (data) {
            stateList = data.States;
            $scope.StateList = stateList;
        },
          function (err) {
              $scope.ErrorMessage = "Get State List Failed!";
          });
    };

    bindInsuranceComapnyList();
    bindStateList();

   


  
    var bindInsurance = function () 
        {
            $scope.loading = true;
            InsuranceInformationService.GetInsuranceInformation($scope.AccountNumber, $scope.baseAddress).then(function (data)
            {
             
                $scope.searchInsuranceCompanyName = data.InsuranceCompanyName;
                $scope.InsuranceCompanyName = $scope.InsuranceCompanyList.find(function (CompanyName) { return CompanyName.InsuranceCompanyName === data.InsuranceCompanyName; });

                $scope.InsuranceAddress = data.InsuranceCompanyAddress;
                $scope.InsuranceCity = data.InsuranceCompanyCity;
                $scope.InsuranceState = stateList.find(function (state) { return state.Value === data.InsuranceCompanyState; });
                $scope.InsuranceZip = data.InsuranceCompanyZip;

                $scope.AgencyCompanyName = data.InsuranceAgencyName;
                $scope.AgencyAddress = data.InsuranceAgencyAddress;
                $scope.AgencyCity = data.InsuranceAgencyCity;
                $scope.AgencyState = stateList.find(function (state) { return state.Value === data.InsuranceAgencyState; });
                $scope.AgencyZip = data.InsuranceAgencyZip;

                $scope.InsuredName = data.InsuredName;
                $scope.InsuredAddress = data.InsuredAddress;
                $scope.InsuredCity = data.InsuredCity;
                $scope.InsuredState = stateList.find(function (state) { return state.Value === data.InsuredState; });
                $scope.InsuredZip = data.InsuredZip;

                $scope.PolicyNumber = data.InsPolicyNo;
         
                $scope.EffectiveDate = data.InsEffDate == null ? "" : data.InsEffDate;
                $scope.ExpirationDate = data.InsExpDate == null ? "" : data.InsExpDate;

                $scope.Year = data.VehYear;
                $scope.Make = data.VehModel;
                $scope.Model = data.VehType;
                $scope.MakeModel = $scope.Make + "/" + $scope.Model
                $scope.VehicleIdentificationNumber = data.VehVin;


                if ($scope.EffectiveDate != "") {
                    var EffectiveDate = $scope.EffectiveDate;
                    var date = new Date(EffectiveDate);
                    $scope.EffectiveDate = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                }
                if ($scope.ExpirationDate != "") {
                    var ExpirationDate = $scope.ExpirationDate;
                    var date2 = new Date(ExpirationDate);
                    $scope.ExpirationDate = (date2.getMonth() + 1) + '/' + date2.getDate() + '/' + date2.getFullYear();
                }

                $scope.loading = false;

            }, function (error)
            {
                $scope.btndisabled = false;
               
                if (error == "") {
                    $scope.mainInsuranceInformationError = "Server connection problem";
                }
                else {
                    $scope.mainInsuranceInformationError = error.Message;;
                }
            });
        };

 
 

    $scope.InsuranceInformationSave = function ()
    {
        $scope.loading = true;
        var regexCardNumber = /^\d*$/;
        var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
        var regexEmail = /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/;
       
        $scope.btndisabled = true;
       
        var regexZip = /^(\d{5}-\d{4}|\d{5})$/;
        if ($scope.InsuranceCompanyName == "" || $scope.InsuranceCompanyName == undefined) {
            $scope.ErrorInsuranceCompanyName = "Please Select Insurance Company Name."
            $scope.submittedInsuranceCompanyName = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuranceAddress == "" || $scope.InsuranceAddress == undefined) {
            $scope.ErrorInsuranceAddress = "Please Enter Address."
            $scope.submittedInsuranceAddress = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuranceCity == "" || $scope.InsuranceCity == undefined) {
            $scope.ErrorInsuranceCity = "Please Enter City."
            $scope.submittedInsuranceCity = "submittedInvalid";
         $scope.btndisabled = false;
            return;
        }

        //else if ($scope.InsuranceState.Value == null ) {
        //    $scope.ErrorInsuranceState = "Please Enter State."
        //    $scope.submittedInsuranceState = "submittedInvalid";
        //    $scope.Loadname = ""; $scope.btndisabled = false;
        //    return;
        //}


        else if ($scope.InsuranceState == "" || $scope.InsuranceState == undefined) {
            $scope.ErrorInsuranceState = "Please Select State."
            $scope.submittedInsuranceState = "submittedInvalid";
         $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuranceZip == "" || $scope.InsuranceZip == undefined) {
            $scope.ErrorInsuranceZip = "Please Enter Zip."
            $scope.submittedInsuranceZip = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        

        else if (!regexZip.test($scope.InsuranceZip)) { $scope.ErrorInsuranceZip = "Enter Only Zip Number"; $scope.submittedInsuranceZip = "submittedInvalid"; $scope.btndisabled = false; return; }
        else if ($scope.AgencyCompanyName == "" || $scope.AgencyCompanyName == undefined) {
            $scope.ErrorAgencyCompanyName = "Please Enter Agency Company Name."
            $scope.submittedAgencyCompanyName = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.AgencyAddress == "" || $scope.AgencyAddress == undefined) {
            $scope.ErrorAgencyAddress = "Please Enter Address.";
            $scope.submittedAgencyAddress = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.AgencyCity == "" || $scope.AgencyCity == undefined) {
            $scope.ErrorAgencyCity = "Please Enter City."
            $scope.submittedAgencyCity = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.AgencyState == "" || $scope.AgencyState == undefined) {
            $scope.ErrorAgencyState = "Please Select State."
            $scope.submittedAgencyState = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.AgencyZip == "" || $scope.AgencyZip == undefined) {
            $scope.ErrorAgencyZip = "Please Enter Zip."
            $scope.submittedAgencyZip = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
       
        else if (!regexZip.test($scope.AgencyZip)) { $scope.ErrorAgencyZip = "Enter Only Zip Number"; $scope.submittedAgencyZip = "submittedInvalid";  $scope.btndisabled = false; return; }
        else if ($scope.PolicyNumber == "" || $scope.PolicyNumber == undefined) {
            $scope.ErrorPolicyNumber = "Please Enter Policy Number."
            $scope.submittedPolicyNumber = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.EffectiveDate == "" || $scope.EffectiveDate == undefined)
        {
            $scope.ErrorEffectiveDate = "Please Enter Effective Date."
            $scope.submittedEffectiveDate = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.ExpirationDate == "" || $scope.ExpirationDate == undefined)
        {
            $scope.ErrorExpirationDate = "Please Enter Expiration Date."
            $scope.submittedExpirationDate = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuredName == "" || $scope.InsuredName == undefined) {
            $scope.ErrorInsuredName = "Please Enter Insured name."
            $scope.submittedInsuredName = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuredAddress == "" || $scope.InsuredAddress == undefined) {
            $scope.ErrorInsuredAddress = "Please Enter Address.";
            $scope.submittedInsuredAddress = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        else if ($scope.InsuredCity == "" || $scope.InsuredCity == undefined) {
            $scope.ErrorInsuredCity = "Please Enter City.";
            $scope.submittedInsuredCity = "submittedInvalid";
           $scope.btndisabled = false;
            return;
        }
        //else if ($scope.InsuredState.Value == "" || $scope.InsuredState.Value == undefined) {
        else if (!$scope.InsuredState) {
            $scope.ErrorInsuredState = "Please Select State.";
            $scope.submittedInsuredState = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
       
        else if ($scope.InsuredZip == "" || $scope.InsuredZip == undefined) {
            $scope.ErrorInsuredZip = "Please Enter Zip.";
            $scope.submittedInsuredZip = "submittedInvalid";
            $scope.btndisabled = false;
            return;
        }
       
        else if (!regexZip.test($scope.InsuredZip)) { $scope.ErrorInsuredZip = "Enter Only Zip Number"; $scope.submittedInsuredZip = "submittedInvalid";  $scope.btndisabled = false; return; }


        var collection = { 'AccountNumber': $scope.AccountNumber, 'InsuranceCompanyName': $scope.InsuranceCompanyName.InsuranceCompanyName, 'InsuranceAddress': $scope.InsuranceAddress, 'InsuranceCity': $scope.InsuranceCity, 'InsuranceState': $scope.InsuranceState.Value, 'InsuranceZip': $scope.InsuranceZip, 'AgencyCompanyName': $scope.AgencyCompanyName, 'AgencyAddress': $scope.AgencyAddress, 'AgencyCity': $scope.AgencyCity, 'AgencyState': $scope.AgencyState.Value, 'AgencyZip': $scope.AgencyZip, 'PolicyNumber': $scope.PolicyNumber, 'EffectiveDate': $scope.EffectiveDate, 'ExpirationDate': $scope.ExpirationDate, 'Year': $scope.Year, 'Make': $scope.Make, 'Model': $scope.Model, 'VehicleIdentificationNumber': $scope.VehicleIdentificationNumber, 'InsuredName': $scope.InsuredName, 'InsuredAddress': $scope.InsuredAddress, 'InsuredCity': $scope.InsuredCity, 'InsuredState': $scope.InsuredState.Value, 'InsuredZip': $scope.InsuredZip, 'InsuranceCardPath': $scope.ImgFile };


        InsuranceInformationService.SaveInsuranceInformation({ model: collection, files: dataURItoBlob($scope.ImgFile) }, $scope.baseAddress).then(function (data) {
       

            $scope.btndisabled = false;
         
            if (JSON.parse(data.data) == "Insurance Information is saved successfully.")
            {
                $scope.message = JSON.parse(data.data);
                $scope.InsuranceInformationError = "";
            }
            else
            {
                $scope.InsuranceInformationError = JSON.parse(data.data);
                $scope.message = "";
            }
            $scope.loading = false;
            
        }, function (error)
        {
            $scope.loading = false;
             $scope.btndisabled = false;
           
             if (error == "") {
                 $scope.InsuranceInformationError = "Server connection problem";
                 $scope.message = "";
             }
             else
             {
                 $scope.InsuranceInformationError = "Somthing went wrong !";
             //$scope.InsuranceInformationError = error.data;
                 $scope.message = "";
             }
         });



           


    };
    //function to convert the dataURI
    function dataURItoBlob(dataURI) {
        if (typeof dataURI === "undefined") {
            return null;
        }
        var binary = atob(dataURI.split(',')[1]);
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
        var array = [];
        for (var i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        return new Blob([new Uint8Array(array)], {
            type: mimeString
        });
    };


    $scope.removeInsuranceCompanyName = function () {
        $scope.ErrorInsuranceCompanyName = "";
        $scope.submittedInsuranceCompanyName = "";

    };
    $scope.removeInsuranceAddress = function () {
        $scope.ErrorInsuranceAddress = "";
        $scope.submittedInsuranceAddress = "";

    };
    $scope.removeInsuranceCity = function () {
        $scope.ErrorInsuranceCity = "";
        $scope.submittedInsuranceCity = "";

    };
    $scope.removeInsuranceState = function () {
        $scope.ErrorInsuranceState = "";
        $scope.submittedInsuranceState = "";

    }; 
    $scope.removeInsuranceZip = function () {
        $scope.ErrorInsuranceZip = "";
        $scope.submittedInsuranceZip = "";

       
    };
    $scope.removeAgencyCompanyName = function () {
        $scope.ErrorAgencyCompanyName = ""
        $scope.submittedAgencyCompanyName = "";

    }; 
    $scope.removeAgencyAddress = function () {
        $scope.ErrorAgencyAddress = "";
        $scope.submittedAgencyAddress = "";

    };
    $scope.removeAgencyCity = function () {
        $scope.ErrorAgencyCity = "";
        $scope.submittedAgencyCity = "";

    };
    $scope.removeAgencyState = function () {
        $scope.ErrorAgencyState = "";
        $scope.submittedAgencyState = "";

    };
    $scope.removeAgencyZip = function () {
        $scope.ErrorAgencyZip = "";
        $scope.submittedAgencyZip = "";

    };
    $scope.removeAgencyPolicyNumber = function ()
    {
        $scope.ErrorPolicyNumber = "";
        $scope.submittedPolicyNumber = "";

    };
    $scope.removeEffectiveDate = function () {

        $scope.ErrorEffectiveDate = "";
        $scope.submittedEffectiveDate = "";

    };
    $scope.removeExpirationDate = function ()
    {

        $scope.ErrorExpirationDate = ""
        $scope.submittedExpirationDate = "";

    };
    $scope.removeInsuredName = function () {

        $scope.ErrorInsuredName = ""
        $scope.submittedInsuredName = "";

    };
    $scope.removeInsuredAddress = function () {
        $scope.ErrorInsuredAddress = "";
        $scope.submittedInsuredAddress = "";

    };

    $scope.removeInsuredCity = function () {
        $scope.ErrorInsuredCity = "";
        $scope.submittedInsuredCity = "";

    };
    $scope.removeInsuredState = function () {
        $scope.ErrorInsuredState = "";
        $scope.submittedInsuredState = "";

    };
   
    $scope.removeInsuredZip = function () {
        $scope.ErrorInsuredZip = "";
        $scope.submittedInsuredZip = "";

    };
    $scope.GetAddress = function ()
    {
    
        if ($scope.chkuserProfileaddress.value == true)
        {
            GetUserProfileAddress();
        }
        else
        {
            $scope.InsuredName = "";
            $scope.InsuredAddress = "";
            $scope.InsuredCity = "";
            $scope.InsuredState = "";
            $scope.InsuredZip = "";
        }
    };


    function GetUserProfileAddress() {
        $scope.loading = true;
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        InsuranceInformationService.GetUserProfileAddress(AccountNumber, BaseAddress).then(function (data) {
            $scope.InsuredName = data.FullName;
            $scope.InsuredAddress = data.Address1;
            $scope.InsuredCity = data.City;
            //  $scope.InsuredState = data.State;
            $scope.InsuredZip = data.Zip;
            $scope.loading = false;

            $scope.InsuredState = stateList.find(function (state) { return state.Value === data.State; });
        }, function (error) {
            if (error == "") {
                $scope.mainInsuranceInformationError = "Server connection problem";
            }
            else {
                $scope.mainInsuranceInformationError = error.Message;;
            }
        });
    };
   
}]);