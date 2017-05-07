app.controller('paymentAgreementController', ['$scope', '$rootScope', '$location', 'ForgotPasswordFactory', 'PaymentConfirmationService', 'WebApiService', function ($scope, $rootScope, $location, ForgotPasswordFactory,PaymentConfirmationService, WebApiService) {

    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
  
    $scope.BtnAchPaymentAgreementContinue = true;
    $scope.ShowOneTime = true;
    $scope.ShowScheduled = false;
   
    var AccountNumber = $scope.AccountNumber;
    $scope.baseAddress = WebApiService.baseAddress;
   
    $scope.Type = {
        name: 'Ach'
    };
    $scope.checkboxTermsConditions =
  {
      value: false

  };
    (function () {
        $scope.loading = true;
        PaymentConfirmationService.FeeAmountGet(AccountNumber, $scope.baseAddress).success(function (data) {
            $scope.fee = "$" + JSON.parse(data);
            $scope.loading = false;
            $scope.duplicatePayment();
        }
        ).error(function (error) {
            $scope.loading = false;
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                //  $scope.Error = error.Message;
                $scope.Error = "XML Load problem";
            }



        });

    })();

    function ACHPaymentAgreement()
    {
       
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/AccountInformation');
        
    }

    $scope.FuturePayAgreementContinue = function () {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/FuturePayAccountInfo');
    };
    function DebitCreditPaymentAgreement()
    {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/DebitCreditPaymentAgreement');

    }

    $scope.PaymentAgreement =function ()
    {
        
        if ($scope.Type.name == 'Ach')
        {
         
            ACHPaymentAgreement();
        }
        else
        {
            DebitCreditPaymentAgreement();
        }      
    }
    $scope.termsAndConditionsErrorRemove = function()
    {
        if ($scope.checkboxTermsConditions.value)
        {
            $scope.BtnAchPaymentAgreementContinue = false;
            $scope.termsAndConditionsError = '';
        }
        else
        {
            $scope.BtnAchPaymentAgreementContinue = true;
            $scope.termsAndConditionsError = '';
        }
    }

    $scope.AchAccountInformationNext = function ()
    {
        var termsAndConditions = $scope.checkboxTermsConditions.value;
        if (!termsAndConditions)
        {
            $scope.termsAndConditionsError = "You must agree to above terms and conditions";
            return;
        }
        else
        {
            $rootScope.sectionPayment = 0;

            $rootScope.loggedIn = true;
            $location.path('/AccountInformation')
        }

      };
    $scope.DebitCreditAccountInformationNext = function ()
    {
        var termsAndConditions = $scope.checkboxTermsConditions.value;


        if (termsAndConditions == true) {

            $scope.termsAndConditionsError = "You must agree to above terms and conditions";
            return;
        }
        else
        {
            $rootScope.sectionPayment = 0;
            $rootScope.loggedIn = true;
            $location.path('/DebitCreditAccountInformation');
        }

    };

    $scope.RemoveTermsAndConditionsNotAgree = function()
    {
      
        $scope.checkboxTermsConditions.value = false;
    }

    $scope.duplicatePayment = function()
    {
        $scope.loading = true;
        ForgotPasswordFactory.DuplicatePayment(AccountNumber, $scope.baseAddress).success(function (data)
        {
            
            if (data[0].OneTime == "True" && data[0].Scheduled == "True") {
                $scope.ShowOneTime = true;
                $scope.ShowScheduled = true;
                duplicatePopup();
            }
            else if (data[0].OneTime == "True") {
                $scope.ShowOneTime = true;
                $scope.ShowScheduled = false;
                duplicatePopup();
            }
            else if (data[0].Scheduled == "True")
            {
                $scope.ShowOneTime = false;
                $scope.ShowScheduled = true;
                duplicatePopup();
            }
          
            $scope.loading = false;
        }
      ).error(function (err)
      {
          $scope.loading = false;
          if (err == "")
          {
              $scope.Error = "Server connection problem";
          }
          else
          {
       
              $scope.Error = err.Message;;
          }      
      });

        

    }
    function duplicatePopup()
    {
        $('#myModal3').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }
    $scope.modalClass = "";
    $scope.cancel = function()
    {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/LoanPayment');             
    }


}]);