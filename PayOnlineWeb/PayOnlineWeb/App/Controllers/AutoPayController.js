app.controller('AutoPayController', ['$scope', '$route', '$rootScope', '$location', 'AutoPayService', 'paymentAmountService', 'WebApiService', function ($scope, $route, $rootScope, $location, AutoPayService,paymentAmountService, WebApiService)
{
    
    $scope.ShowGridview = false;
    $scope.DeleteTokenId = "";
    $scope.Country = "United States";
    $scope.OtherAmount = '';
    $scope.TokenId = "";
    $scope.showSwitchWidget = '';
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.CheckACN = sessionStorage.getItem('ACNCheck');
    $scope.PaymentMethod ={name: 'ACH'};
    $scope.AchDetail ={name: 'NewAchDetail'}; 
    $scope.AccountType ={name: 'Checking'}
    $scope.CardType ={Name: 'Visa'};
    $scope.TypeOfDebitCredit ={Name: 'New'};
    $scope.checkBoxTermsAndConditions ={value: false};
    $scope.checkBoxTermsAndConditionsPaymentAmount  ={value: false};
    $scope.AchSaveAccountInformationFuture ={value: true};  
    $scope.DebitCreditSaveAccountInformationFuture ={value: true};
    $scope.chkuserProfileaddress ={value: false};
    $scope.chkLastBillAddress ={value: false};
    $scope.ACHRadio = false;
    $scope.newCardView = true;
    $scope.BillingAddress = true;
    $scope.cardPayWith = false;
    $scope.PaymentTypeACH = false;
    $scope.achGridDiv = false;
    $scope.CardTypeData = "";
    $scope.CardFee = "";
    $scope.CardNumberView = "";
    $scope.CardTotalAmount = "";
    $scope.AchAmountSubmit = true;
    $scope.AchGrid = "";
    $scope.confirmation = "";
    $scope.btndisabled = false;
    $scope.Loadname = "";
    $scope.loading = true;
    $scope.Email = sessionStorage.getItem('Email');
    $scope.BankRoutingNumber = "";
    $scope.BankAccountName = "";
    $scope.CheckingAccountNumber = "";
    $scope.AccountHolderName = "";
    $scope.OtherAmount = "";
    $scope.PavmentDate = new Date();
    $scope.CardName = "";
    $scope.CardNumber = "";
    $scope.ExpiryDat = "";
    $scope.ExpiryYear = "";
    $scope.CvNumber = "";
    $scope.BillingName = "";
    $scope.BillingLastName = "";
    $scope.IsActiveAchDetail = false;
    $scope.PrimaryNumber = "";
    $scope.Address = "";
    $scope.City = "";
    $scope.State = "";
    $scope.Zip = "";
    $scope.HolderPrimaryNumber = "";
    $scope.deleteAutopayAccountNumber = "";
    $scope.AutoPayIsActive = false;
    $scope.RemainingTerm = "";
    $scope.CardExpiry = "";
    $scope.StateList = [];
    $scope.stateSelection = { selectedState: '' };

    //--------------------Ach ------------------------------------
    $scope.ConfirmationAchDueDate = "";
    $scope.ConfirmationAchAmountDue = "";
    $scope.ConfirmationAchFee = "";
    $scope.ConfirmationAchTotalAmount = "";
    $scope.ConfirmationAchMafAccountNumber = "";
    $scope.ConfirmationAchNameOnTheAch = "";
    $scope.ConfirmationAchBankName = "";
    $scope.ConfirmationAchBankRoutingNumber = "";
    $scope.ConfirmationAchBankAccountNumber = "";
    $scope.ConfirmationAchAccountType = "";
    $scope.ConfirmationDebitCreditFrequency = "";
    //  $scope.ConfirmationAchSaveAccountInformationFuture = false;
    $scope.ConfirmationAchAccountTypeView = "";
    $scope.ConfirmationAchBankAccountNumberView = "";
    $scope.ConfirmationAchEmail = "";
    $scope.ConfirmationAchPavmentDate = "";
    $scope.ConfirmationAchPrimaryNumber = "";
    $scope.ConfirmationAchBankAccountName = "";

    $scope.HoldAchOtherAmount = "";
    $scope.HoldAchFee = "";
    $scope.HoldAchAccountHolderName = "";
    $scope.HoldAchBankName = "";
    $scope.HoldAchBankRoutingNumber = "";
    $scope.HoldAchCheckingAccountNumber = "";
    $scope.HoldAchAccountType = "";
    $scope.HoldAchEmail = "";
    $scope.HoldAchPrimaryNumber = "";
    $scope.HoldAchBankAccountName = "";
    $scope.nameState = "";
    $scope.saveAch = false;
    $scope.saveCard = false;
    //--------------------CC/DD ----------------------------------

    $scope.ConfirmationDebitCreditAmountDue = "";
    $scope.ConfirmationDebitCreditDueDate = "";
    $scope.ConfirmationDebitCreditFrequency = "";
    $scope.ConfirmationDebitCreditCardType = "";
    $scope.ConfirmationDebitCreditCardName = "";
    $scope.ConfirmationDebitCreditCardNumber = "";
    $scope.ConfirmationDebitCreditExpiryDate = "";
    $scope.ConfirmationDebitCreditExpiryYear = "";
    $scope.ConfirmationDebitCreditCvNumber = "";
    $scope.ConfirmationDebitCreditSaveAccountInformationFuture = false;
    $scope.ConfirmationDebitCreditBillingName = "";
    $scope.ConfirmationDebitCreditPrimaryNumber = "";
    $scope.ConfirmationDebitCreditEmail = "";
    $scope.ConfirmationDebitCreditAddress = "";
    $scope.ConfirmationDebitCreditCity = "";
    $scope.ConfirmationDebitCreditState = "";
    $scope.ConfirmationDebitCreditZip = "";
    $scope.ConfirmationDebitCreditFee = 0;

    $scope.Status = "";
    $scope.ScheduleId = "";
    $scope.dataHolderGrid = {};
    var dataArrayGrid = [];   

    var DetailGrid = function ()
    {
        this.ScheduleId = '';
        this.TokenId = '';
        this.ScheduleDate = '';
        this.ScheduleMethod = '';
        this.IsSchedule = false;
        this.IsReccuring = false;
        this.CreatedDate = '';
        this.IsActive = false;
        this.ConfirmationID = '';
        this.AcctNo = '';
        this.TranDate = '';
        this.TranPayment = '';
        this.TranFee = '';
        this.BankABA = '';
        this.BankAcctNo = '';
        this.BankName = '';
        this.BankHolder = '';
        this.BankAcctType = '';
        this.Status = '';
        this.SaveAccountFuture = false;
        this.BankAccountName = '';
        this.paymentFrequency = '';
        this.BankAcctNoView = '';
        
    };
    $scope.TableBankABA = false;
    var loadingCounter = 0;
    function increaseLoadingCounter() {
        if (loadingCounter === 0)
            $scope.loading = true;
        loadingCounter++;
    }
    function decreaseLoadingCounter() {
        if (loadingCounter > 0) {
            loadingCounter--;
            if (loadingCounter === 0)
                $scope.loading = false;
        }
    }
    (function () {
        increaseLoadingCounter();
        AutoPayService.GetAutoPayDelete($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            dataArrayGrid = [];
            $scope.dataHolderGrid = {};            
            if (data.length > 0) {
                for (var counter = 0; counter < data.length; counter++) {
                    var item = new DetailGrid();                  
                    item.ScheduleId = data[counter].ScheduleId;
                    item.TokenId = data[counter].TokenId;
                    item.ScheduleDate = data[counter].ScheduledDate;
                    item.ScheduleMethod = data[counter].ScheduleMethod;
                    item.IsSchedule = data[counter].IsSchedule;
                    item.IsReccuring = data[counter].IsReccuring;
                    item.CreatedDate = data[counter].CreatedDate;
                    item.IsActive = data[counter].IsActive;
                    item.ConfirmationID = data[counter].ConfirmationID;
                    item.AcctNo = data[counter].AcctNo;
                    item.TranDate = data[counter].TranDate;
                    item.TranPayment = data[counter].TranPayment;
                    item.TranFee = data[counter].TranFee;
                    item.BankABA = data[counter].BankABA;
                    item.BankAcctNo = data[counter].BankAcctNo;
                    item.BankName = data[counter].BankName;
                    item.BankHolder = data[counter].BankHolder;                   
                    item.BankAcctType = data[counter].BankAcctType == "S" ? "ach.png" : data[counter].BankAcctType == "P" ? "ach.png" : data[counter].BankAcctType == "V" ? "visa_icon.jpg" : "master_card.jpg";
                    item.Status = data[counter].Status;
                    item.SaveAccountFuture = data[counter].SaveAccountFuture;
                    item.BankAccountName = data[counter].BankAccountName;
                    item.paymentFrequency = data[counter].paymentFrequency == "W" ? "Weekly" : data[counter].paymentFrequency == "M" ? "Monthly" : data[counter].paymentFrequency == "Y" ? "Yearly" : data[counter].paymentFrequency == "B" ? "Bi-Weekly" : data[counter].paymentFrequency == "S" ? "Semi-Monthly" : data[counter].paymentFrequency == "D" ? "Daily" : data[counter].paymentFrequency;
                    item.BankAcctNoView = data[counter].BankAcctType == "S" || data[counter].BankAcctType == "P" ? data[counter].BankAcctNo : "XXXX-XXXX-XXXX-" + data[counter].BankAcctNo.substr(-4);
                    $scope.AutoPayIsActive = data[counter].IsActive;
                    $scope.TableBankABA = data[counter].BankAcctType == "S" ? true : data[counter].BankAcctType == "P" ? true : data[counter].BankAcctType == "V" ? false : false;
                    $scope.bankAccountNameView = data[counter].BankAcctType == "S" ? "Bank Account #" : data[counter].BankAcctType == "P" ? "Bank Account #" : data[counter].BankAcctType == "V" ? "Card #" : "Card #";
                    item.counter = counter;
                    dataArrayGrid.push(item);
                }
                $scope.dataHolderGrid = dataArrayGrid;
                $scope.showSwitchWidget = 'StartAutoPayGrid';                
            }
            else {
                $scope.showSwitchWidget = 'StartAutoPay';

                if ($scope.CheckACN == "C" || $rootScope.ACHBlock === "Blocked") {
                    $scope.ACHRadio = true;
                    $scope.PaymentMethod.name = "DebitCredit";
                    PaymentByDebitCreditCardOnly();
                }
            }
            decreaseLoadingCounter();
        },
           function (error) {
               decreaseLoadingCounter();
               if (error == "") {

                   $scope.startLandingError = "Server connection problem";
               }
               else {
                   $scope.startLandingError = error.Message;
               }
           });
    })();
    
    $scope.autoPayDuplicatePayment = function () {
        increaseLoadingCounter();
        AutoPayService.GetAutoPayDuplicatePayment($scope.AccountNumber, $scope.baseAddress).success(function (data) {
            $scope.AutoPayDueDate = data[0].AutoPayDueDate;
            $scope.FuturePayDate = data[0].FuturePayDate;
            decreaseLoadingCounter();
            if ($scope.Status != "UpdateAutoPay") {
                if (data[0].OneTime == "True" && data[0].Scheduled == "True") {
                    $scope.ShowOneTime = true;
                    $scope.ShowScheduled = true;
                    AutoPayDuplicatePopup();
                    return false;
                }
                else if (data[0].OneTime == "True") {
                    $scope.ShowOneTime = true;
                    $scope.ShowScheduled = false;
                    AutoPayDuplicatePopup();
                    return false;
                }
                else if (data[0].Scheduled == "True") {
                    $scope.ShowOneTime = false;
                    $scope.ShowScheduled = true;
                    AutoPayDuplicatePopup();
                    return false;
                }
                else $scope.StartAutoPay();
            }
            else
            {
                $scope.StartAutoPay();
            }
           
        }
      ).error(function (err) {
          decreaseLoadingCounter();
          if (err == "") {
              $scope.Error = "Server connection problem";
          }
          else {

              $scope.Error = err.Message;;
          }
      });
    }

    function AutoPayDuplicatePopup() {
        $('#AlertModel').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    $scope.cancelAlert = function () {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $route.reload();
    }

    var bindStateList = function ()
    {
        increaseLoadingCounter();
        AutoPayService.GetStateList().then(function (data) {
           
            var selectedState = $scope.State;
            var states = data.States;
            var selectedOption = states.find(function (state) { return state.Value === selectedState; });
         
            $scope.StateList = states;
            $scope.stateSelection.selectedState = selectedOption;
            decreaseLoadingCounter();
        },
          function (err) {
              decreaseLoadingCounter();
              $scope.ErrorMessage = "Get State Failed!";
          });
    }


    var savedCardItems = [];
    $scope.savedCards = {};

    var savedCardItem = function ()
    {
        this.TokenId = '';
        this.CardNumber = '';
        this.CardType = '';
        this.CardName = '';
        this.CardExpiry = '';
        this.CVV = '';
        this.Selected = false;
        this.counter;
    };

    function ClearValidation() {
        $('#CardName').css('border-color', '#e6e6e6');
        $('#CardNumber').css('border-color', '#e6e6e6');
        $('#ExpiryDate').css('border-color', '#e6e6e6');
        $('#ExpiryYear').css('border-color', '#e6e6e6');
        $('#CvNumber').css('border-color', '#e6e6e6');

    };

    function ClearBillingAddress() {
        $scope.BillingName = '';
        $scope.BillingLastName = '';
        $scope.PrimaryNumber = '';
        $scope.Email = '';
        $scope.Address = '';
        $scope.City = '';
        $scope.State = '';
        $scope.stateSelection.selectedState = '';
        $scope.Zip = '';
    };

    function GetLastBillingAddress(tokenId) {
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        AutoPayService.GetBillingAddress(AccountNumber, tokenId, BaseAddress).success(function (data) {
            $scope.BillingName = data.FirstName;
            $scope.BillingLastName = data.LastName;
            $scope.PrimaryNumber = data.PrimaryNumber;
            $scope.Email = data.EmailID;
            $scope.Address = data.Address;
            $scope.City = data.City;
            $scope.State = data.State;
            $scope.Zip = data.Zip;
            bindStateList();
            decreaseLoadingCounter();
        }).error(function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    }

    function DisableNewCard() {
        $scope.TypeOfDebitCredit.Name = 'Saved';
        $scope.CardType.Name = "";
        $scope.SaveCardDisabled = true;
        $scope.CardName = "";
        $scope.CardNumber = "";
        $scope.ExpiryYear = "";
        $scope.ExpiryDate = "";
        $scope.CvNumber = "";
        ClearValidation();
    };

    function ClearValidation() {
        $('#CardName').css('border-color', '#e6e6e6');
        $('#CardNumber').css('border-color', '#e6e6e6');
        $('#ExpiryDate').css('border-color', '#e6e6e6');
        $('#ExpiryYear').css('border-color', '#e6e6e6');
        $('#CvNumber').css('border-color', '#e6e6e6');

    };

    $scope.PaymentTypeDebitCredit = function () {
        PaymentByDebitCreditCardOnly();
        $scope.GetCardFee();
    };
    function PaymentByDebitCreditCardOnly() {
        //  ClearBillingAddress();
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        paymentAmountService.GetCardDetails(AccountNumber, BaseAddress).then(function (data) {          
            savedCardItems = [];
            var isActive = 0;
            for (var counter = 0; counter < data.length; counter++)
            {
                var item = new savedCardItem();
                item.TokenId = data[counter].TokenId;
                item.CardNumber = data[counter].CardNumber;
                item.CardType = data[counter].CardType;
                item.CardName = data[counter].CardName;
                item.CardExpiry = data[counter].CardExpiry;
                item.counter = counter;
               
                /////////////////////////////////////Saurabh
                if ($scope.Status === "UpdateAutoPay" && data[counter].TokenId === $scope.TokenIdEdit) {
                    isActive = 1;
                    item.Selected = true;
                    GetLastBillingAddress(item.TokenId);
                }
                if (counter === data.length - 1 && isActive === 0)
                {
                    item.Selected = true;
                    GetLastBillingAddress(item.TokenId);
                }

                ////////////////////////////////////////////////Rahul

                //if ($scope.Status === "UpdateAutoPay")
                //{
                //    if (item.TokenId === $scope.TokenIdEdit) {                        
                //        item.Selected = true;
                //        GetLastBillingAddress(item.TokenId);
                //    }
                //}
                //if (counter === data.length - 1)
                //{
                //    item.Selected = true;
                //    GetLastBillingAddress(item.TokenId);
                //}
                
                savedCardItems.push(item);
            }
            $scope.savedCards = savedCardItems;
      
            if ($scope.savedCards.length > 0)
            {
                $scope.cardPayWith = true;
                $scope.TypeOfDebitCredit.Name = 'Saved';
                $scope.SaveCardInfo = true;
                $scope.newCardView = false;
                $scope.BillingAddress = false;
                $scope.SavedBillingAddress = true;
            }
            else
            {
                $scope.cardPayWith = false;

                $scope.SaveCardInfo = false;
                $scope.newCardView = true;
                $scope.BillingAddress = true;
                $scope.SavedBillingAddress = false;
                $scope.TypeOfDebitCredit.Name = "New";
               
                $scope.BillingName = "";
                $scope.BillingLastName = "";
                $scope.PrimaryNumber = "";
                $scope.Email = "";
                $scope.Address = "";
                $scope.City = "";
                // $scope.stateSelection = "";
                $scope.Zip = "";
                $scope.chkuserProfileaddress.value = false;
                $scope.chkLastBillAddress.value = false;
            }
            bindStateList();
            decreaseLoadingCounter();
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };
       
    $scope.GetConfirmationPayment = function () {
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.Ach3StepError = "";
        $scope.confirmation = "";
        increaseLoadingCounter();
        AutoPayService.GetAutoPayDelete($scope.AccountNumber, $scope.baseAddress).then(function (data) {            
            dataArrayGrid = [];
            $scope.dataHolderGrid = {};

            if (data.length > 0) {
                for (var counter = 0; counter < data.length; counter++) {
                    var item = new DetailGrid();
                    item.ScheduleId = data[counter].ScheduleId;
                    item.TokenId = data[counter].TokenId;
                    item.ScheduleDate = data[counter].ScheduledDate;
                    item.ScheduleMethod = data[counter].ScheduleMethod;
                    item.IsSchedule = data[counter].IsSchedule;
                    item.IsReccuring = data[counter].IsReccuring;
                    item.CreatedDate = data[counter].CreatedDate;
                    item.IsActive = data[counter].IsActive;
                    item.ConfirmationID = data[counter].ConfirmationID;
                    item.AcctNo = data[counter].AcctNo;
                    item.TranDate = data[counter].TranDate;
                    item.TranPayment = data[counter].TranPayment;
                    item.TranFee = data[counter].TranFee;
                    item.BankABA = data[counter].BankABA;
                    item.BankAcctNo = data[counter].BankAcctNo;
                    item.BankName = data[counter].BankName;
                    item.BankHolder = data[counter].BankHolder;                                        
                    item.BankAcctType = data[counter].BankAcctType == "S" ? "ach.png" : data[counter].BankAcctType == "P" ? "ach.png" : data[counter].BankAcctType == "V" ? "visa_icon.jpg" : "master_card.jpg";
                    item.Status = data[counter].Status;
                    item.SaveAccountFuture = data[counter].SaveAccountFuture;
                    item.BankAccountName = data[counter].BankAccountName;
                    item.paymentFrequency = data[counter].paymentFrequency == "W" ? "Weekly" : data[counter].paymentFrequency == "M" ? "Monthly" : data[counter].paymentFrequency == "Y" ? "Yearly" : data[counter].paymentFrequency == "B" ? "Bi-Weekly" : data[counter].paymentFrequency == "S" ? "Semi-Monthly" : data[counter].paymentFrequency == "D" ? "Daily" : data[counter].paymentFrequency;
                    item.BankAcctNoView = data[counter].BankAcctType == "S" || data[counter].BankAcctType == "P" ? data[counter].BankAcctNo : "XXXX-XXXX-XXXX-" + data[counter].BankAcctNo.substr(-4);
                    item.counter = counter;
                    $scope.AutoPayIsActive = data[counter].IsActive;
                    $scope.TableBankABA = data[counter].BankAcctType == "S" ? true : data[counter].BankAcctType == "P" ? true : data[counter].BankAcctType == "V" ? false : false;
                    $scope.bankAccountNameView = data[counter].BankAcctType == "S" ? "Bank Account #" : data[counter].BankAcctType == "P" ? "Bank Account #" : data[counter].BankAcctType == "V" ? "Card #" : "Card #";
                    dataArrayGrid.push(item);

                }

                $scope.dataHolderGrid = dataArrayGrid;               
                $scope.confirmation = "";
                $scope.AchAmountSubmit = true;
                $scope.checkBoxTermsAndConditions.value = false;
                $scope.checkBoxTermsAndConditionsPaymentAmount.value = false;
                $scope.AchSaveAccountInformationFuture.value = false;
                $scope.DebitCreditSaveAccountInformationFuture.value = false;

                $scope.termsAndConditionsError = "";
                $scope.FirstError = "";
                $scope.CardError = "";
                $scope.CardMessage = "";
                $scope.Ach3StepError = "";
                $scope.confirmation = "";
                $scope.CardError = "";
                $scope.CardMessage = "";
                $scope.MainError = "";
                $scope.MainError2 = "";
                $scope.PaymentAmountTermsAndConditionsError = "";

                $scope.CardName = "";
                $scope.CardNumber = "";
                $scope.ExpiryDate = "";
                $scope.ExpiryYear = "";
                $scope.CvNumbe = "";

                $scope.AchDetail.name = 'NewAchDetail';
                $scope.AccountType.name = '';
                $scope.AchDisabled = false;
                $scope.PrimaryNumber = "";
                $scope.Email = "";
                $scope.CheckingAccountNumbe = "";
                $scope.CheckingAccountNumber = "";
                $scope.BankRoutingNumber = "";
                $scope.AchSaveAccountInformationFuture = true;
                $scope.BankAccountName = "";

                $scope.Status = "";
                $scope.ScheduleId = "";
                $scope.showSwitchWidget = 'StartAutoPayGrid';
            }
            else
            {

                $scope.confirmation = "";
                $scope.AchAmountSubmit = true;
                $scope.showSwitchWidget = 'StartAutoPay';
            }
            decreaseLoadingCounter();

        },
           function (error) {

               decreaseLoadingCounter();
               if (error == "") {

                   $scope.startLandingError = "Server connection problem";
               }
               else {
                   $scope.startLandingError = error.Message;
               }

           });

    };

    $scope.AutopayDelete = function(data)
    {
        $scope.deleteAutopayAccountNumber = data.AcctNo;             
        popAutoPayDelete();
    }

    $scope.StartAutoPay = function () {
        increaseLoadingCounter();
      //  $scope.Loadname = "glyphicon-refresh";
        $scope.btndisabled = true;
        AutoPayService.GetAccountInformation($scope.AccountNumber, $scope.baseAddress).then(function (data) {            
            GetAccountInformation(data);
            $scope.Loadname = "";
            $scope.btndisabled = false;
            $scope.showSwitchWidget = 'FirstDivStep1';
            decreaseLoadingCounter();
        }, function (error) {
     decreaseLoadingCounter();
     $scope.Loadname = "";
     $scope.btndisabled = false;
     if (error == "") {

         $scope.LandingError = "Server connection problem";
     }
     else {
         $scope.LandingError = error;
     }

 });
    };
   
    $scope.ChoosePaymentMethod = function ()
    {            
        var paymentMethod = $scope.PaymentMethod.name;
        if ($scope.checkBoxTermsAndConditions.value == false) {
            $scope.termsAndConditionsError = "You must agree to above terms and conditions.";
            return;
        }
      

        $scope.btndisabled = true;
      //  $scope.Loadname = "glyphicon-refresh";
        if (paymentMethod == "ACH")
        {
            $scope.IsActiveAchDetail = true;
            GetAchDetail();
          
           
        }
        if (paymentMethod == "DebitCredit")
        {
            $scope.PaymentTypeDebitCredit();
            $scope.showSwitchWidget = 'SecondDivStep2DebitCredit';

            $scope.btndisabled = false;
            $scope.Loadname = "";
        }

    };
    $scope.RemovetermsAndConditionsError = function () {
        $scope.termsAndConditionsError = "";
    };
    $scope.BackAchPaymentAmount = function () {
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.Ach3StepError = "";
        $scope.confirmation = "";
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.MainError = "";
        $scope.MainError2 = "";
        $scope.PaymentAmountTermsAndConditionsError = "";
        $scope.showSwitchWidget = 'FirstDivStep1';
        $scope.checkBoxTermsAndConditions.value = false;
        $scope.checkBoxTermsAndConditionsPaymentAmount.value = false;
    };

    //Remove Error

    $scope.removeErrorPrimaryNumber = function () {
        $scope.errorPrimaryNumber = "";
        $scope.submittedPrimaryNumber = "";
    };
    $scope.removeErrorBankAccountName = function () {
        $scope.errorBankAccountName = "";
        $scope.submittedBankAccountName = "";
    };

    $scope.removeErrorCheckingAccountNumber = function () {
        $scope.errorCheckingAccountNumber = "";
        $scope.submittedCheckingAccountNumber = "";
    };
    $scope.removeErrorRouting = function()
    {
        $scope.ErrorRouting = "";
        $scope.submittedRouting = "";
    }
    $scope.removeerrorEmail = function()
    {
        $scope.errorEmail = "";
        $scope.submittedEmail = "";

    }
   

    $scope.radioSavedAhc = function (data)
    {
        
       
        dataArray.forEach(function (item) {
            
            if (item.ID === data.ID) {
                
                item.Selected = true;
                $scope.HoldAchOtherAmount = data.TranPayment;
                $scope.HoldAchFee = data.TranFee;
                $scope.HoldAchAccountHolderName = data.BankHolder;
                $scope.HoldAchBankName = data.BankName;
                $scope.HoldAchBankRoutingNumber = data.BankABA;
                $scope.HoldAchCheckingAccountNumber = data.BankAcctNo;
                $scope.HoldAchAccountType = data.BankAcctType == "Checking" ? "P" : "S";
                $scope.HoldAchEmail = data.Email;
                $scope.HoldAchPrimaryNumber = data.PrimaryNumber;
                $scope.HoldAchBankAccountName = data.BankAccountName;
                $scope.AchGrid = data.BankAcctType;
             
            }
            else { item.Selected = false; }
        });

      
        $scope.saveAch = true;
        $scope.MainError2 = "";      
        $scope.AchDetail.name = 'SavedAchDetail';


        $scope.AccountType.name = '';
        $scope.AchDisabled = true;
        $scope.PrimaryNumber = "";
        $scope.Email = "";
        $scope.CheckingAccountNumbe = "";
        $scope.CheckingAccountNumber = "";
        $scope.BankRoutingNumber = "";
        $scope.AchSaveAccountInformationFuture = false;
        $scope.BankAccountName = "";

        $scope.submittedBankAccountName = "";
        $scope.errorBankAccountName = "";
        $scope.submittedBankAccountName = "";
        $scope.errorBankAccountName = "";
        $scope.submittedBankAccountName = "";
        $scope.errorCheckingAccountNumber = "";
        $scope.submittedRouting = "";
        $scope.ErrorRouting = "";
        $scope.submittedEmail = "";
        $scope.errorEmail = "";
        $scope.submittedInvalid = "";
        $scope.ErrorCardName = "";
        $scope.submittedInvalid2 = "";
        $scope.ErrorCardNumber = "";
        $scope.submittedExpiryDate = "";
        $scope.ErrorExpiryDate = "";
        $scope.submittedExpiryYear = "";
        $scope.ErrorExpiryYear = "";
        $scope.submittedCvNumber = "";
        $scope.ErrorCvNumber = "";
        $scope.submittedBillingName = "";
        $scope.ErrorBillingName = "";
        $scope.submittedBillinLastgName = "";
        $scope.ErrorBillingLastName = "";
        $scope.submittedPrimaryNumber = "";
        $scope.ErrorPrimaryNumber = "";
        $scope.submittedEmail = "";
        $scope.ErrorEmail = "";
        $scope.submittedAddress = "";
        $scope.ErrorAddress = "";
        $scope.submittedCity = "";
        $scope.ErrorCity = "";
        $scope.submittedState = "";
        $scope.ErrorState = "";
        $scope.submittedZip = "";
        $scope.ErrorZip = "";

        $scope.errorPrimaryNumber = "";
        $scope.submittedCheckingAccountNumber = "";

        $scope.PaymentAmountTermsAndConditionsError = "";
    }

    $scope.AchPaymentAmountContinue = function ()
    {
        $scope.MainError2 = "";
        if ($scope.checkBoxTermsAndConditionsPaymentAmount.value == false) { $scope.PaymentAmountTermsAndConditionsError = "You must agree to above terms and conditions."; return; }
        if ($scope.DueDate == "") {
            $scope.PaymentAmountTermsAndConditionsError = "No Billing Due Date";
            return;
        }

        if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
        { $scope.MainError2 = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }

        if ($scope.AchDetail.name == "")
        {
            $scope.MainError2 = "Please select ACH detail"
            return;

        }
        $scope.GetAchFee();
        $scope.autopaySchedule();
        if ($scope.AchDetail.name == 'NewAchDetail')
        {
            NewAchAmount();
        }
       
        if ($scope.AchDetail.name == 'SavedAchDetail')
        {              
            ConfirmationAchAmount();
            // $scope.AchDetail.name = "";
        }
    
      
    };

  
    $scope.dataHolder = {};
    var dataArray = [];
    var Detail = function () {
        this.AcctNo = '';
        this.BankABA = '';
        this.BankAccountName = '';
        this.BankAcctType = '';
        this.BankHolder = '';
        this.BankName = '';
        this.ConfirmationID = '';
        this.ID = '';
        this.SaveAccountFuture = false;
        this.TranDate = '';
        this.TranFee = '';
        this.TranPayment = '';
        this.BankAcctNo = '';
        this.BankAcctNoView = '';
        this.Email = '';
        this.counter;
        this.PrimaryNumber = '';
        this.id = '';
    };

    
    $scope.Ach2stepError = "";
    function NewAchAmount() {
        
        $scope.btndisabled = true;
       // $scope.Loadname = "glyphicon-refresh";
        var CheckingAccountNumberregex = /^\d*$/;
        var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
        var regexBankRoutingNumber = /^(\d+){9,}$/;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if ($scope.PrimaryNumber == "" || $scope.PrimaryNumber == undefined) { $scope.errorPrimaryNumber = "Please Enter Account Holder Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }

        else if (!regexPrimaryNumber.test($scope.PrimaryNumber)) { $scope.errorPrimaryNumber = "Enter Only Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if ($scope.BankAccountName == "" || $scope.BankAccountName == undefined) { $scope.errorBankAccountName = "Please Enter Bank Account Name"; $scope.submittedBankAccountName = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if ($scope.CheckingAccountNumber == "" || $scope.CheckingAccountNumber == undefined) { $scope.errorCheckingAccountNumber = "Please Enter " + $scope.AccountType.name + " Account Number"; $scope.submittedCheckingAccountNumber = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; };

        if (!CheckingAccountNumberregex.test($scope.CheckingAccountNumber)) { $scope.errorCheckingAccountNumber = "Enter Only Numbers"; $scope.submittedCheckingAccountNumber = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if ($scope.BankRoutingNumber == "" || $scope.BankRoutingNumber == undefined) { $scope.ErrorRouting = "Please Enter Bank Routing Number"; $scope.submittedRouting = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if (!regexBankRoutingNumber.test($scope.BankRoutingNumber)) { $scope.ErrorRouting = "Must be 9 numeric values"; $scope.submittedRouting = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }

        else if ($scope.Email == "" || $scope.Email == undefined) { $scope.errorEmail = "Please Enter Email"; $scope.submittedEmail = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if (!regexEmail.test($scope.Email)) { $scope.errorEmail = "Not an email!"; $scope.submittedEmail = "submittedInvalid"; $scope.btndisabled = false; $scope.Loadname = ""; return; }
        else if ($scope.checkBoxTermsAndConditionsPaymentAmount.value == false) { $scope.PaymentAmountTermsAndConditionsError = "You must agree to above terms and conditions."; $scope.btndisabled = false; $scope.Loadname = ""; return; }

        var CheckingAccountNumber = $scope.CheckingAccountNumber;
        var Amount = $scope.TotalAmountDue;
        var AccountNumber = $scope.AccountNumber;
        var Routing = $scope.BankRoutingNumber;
        var collection = { "CheckingAccountNumber": CheckingAccountNumber, "Amount": Amount, "AccountNumber": AccountNumber, "Routing": Routing, "FuturePaymentDate": $scope.DueDate, "PaymentMethod": "ACH" };
        increaseLoadingCounter();
       
        AutoPayService.CheckRoutingNumber(collection, $scope.baseAddress).then(function (data) {
     
            if (data.Error != null) {
                $scope.MainError2 = data.Error;

                $scope.btndisabled = false;
                $scope.Loadname = ""
                decreaseLoadingCounter();
                return;
            }
        
            if (data.ErrorRouting != null) {

                $scope.MainError2 = data.ErrorRouting;
                $scope.btndisabled = false; $scope.Loadname = "";
                decreaseLoadingCounter();
                return;
            }
            else {
                $scope.ConfirmationAchDueDate = $scope.DueDate;
                $scope.ConfirmationAchAmountDue = $scope.OtherAmount;
                //  $scope.ConfirmationAchFee = "0.00";
                // 
                //  $scope.ConfirmationAchTotalAmount = parseFloat($scope.ConfirmationAchFee) + parseFloat($scope.OtherAmount);
                $scope.ConfirmationAchMafAccountNumber = $scope.AccountNumber;
                $scope.ConfirmationAchNameOnTheAch = $scope.AccountHolderName;
                $scope.ConfirmationAchBankName = data.BankName;
                $scope.ConfirmationAchBankRoutingNumber = $scope.BankRoutingNumber;
                $scope.ConfirmationAchBankAccountNumber = $scope.CheckingAccountNumber;           
                $scope.ConfirmationAchAccountType = $scope.AccountType.name == 'Checking' ? "P" : "S";             
                $scope.ConfirmationDebitCreditFrequency = $scope.Frequency;
                // $scope.ConfirmationAchSaveAccountInformationFuture = $scope.AchSaveAccountInformationFuture;
                $scope.ConfirmationAchEmail = $scope.Email;
                $scope.ConfirmationAchAccountTypeView = $scope.AccountType.name == 'Checking' ? "ACH - Checking Account" : "ACH - Savings Account";
               // $scope.ConfirmationAchBankAccountNumberView = $scope.CheckingAccountNumber.substr(-4);
                $scope.ConfirmationAchBankAccountNumberView = $scope.CheckingAccountNumber;
                $scope.ConfirmationAchPavmentDate = $rootScope.TodayDate;
                $scope.ConfirmationAchBankAccountName = $scope.BankAccountName;
                $scope.ConfirmationAchPrimaryNumber = $scope.PrimaryNumber;
                $scope.btndisabled = false;
                $scope.Loadname = ""
                $scope.showSwitchWidget = 'thirdDivStep3ACH';

            }
            decreaseLoadingCounter();
        },


        function (error) {
       
            decreaseLoadingCounter();
            $scope.btndisabled = false; $scope.Loadname = "";

            if (error == "") {


                $scope.MainError = "Server connection problem";
            }
            else {


                $scope.MainError = error.Message;;
            }
        });




    };

    function ConfirmationAchAmount()
    {       
        var CheckingAccountNumber = $scope.HoldAchCheckingAccountNumber;
        var Amount = $scope.TotalAmountDue;
        var AccountNumber = $scope.AccountNumber;
        var Routing = $scope.HoldAchBankRoutingNumber;
        var collection = { "CheckingAccountNumber": CheckingAccountNumber, "Amount": Amount, "AccountNumber": AccountNumber, "Routing": Routing, "FuturePaymentDate": $scope.DueDate };
        increaseLoadingCounter();

        AutoPayService.CheckRoutingNumber(collection, $scope.baseAddress).then(function (data) {
            if (data.Error != null) {
                $scope.MainError2 = data.Error;

                $scope.btndisabled = false;
                $scope.Loadname = ""
                decreaseLoadingCounter();
                return;
            }

            if (data.ErrorRouting != null) {

                $scope.MainError2 = data.ErrorRouting;
                $scope.btndisabled = false; $scope.Loadname = "";
                decreaseLoadingCounter();
                return;
            }
            else
            {
                //if ($scope.saveAch == false)
                //{
                //    $scope.PaymentAmountTermsAndConditionsError = "SELECT YOUR SAVED ACH DETAIL.";
                //    return;
                //}
                $scope.btndisabled = true;
                // $scope.Loadname = "glyphicon-refresh";
                $scope.ConfirmationAchDueDate = $scope.DueDate;
                $scope.ConfirmationAchAmountDue = $scope.OtherAmount;

                $scope.ConfirmationAchMafAccountNumber = $scope.AccountNumber;
                $scope.ConfirmationAchNameOnTheAch = $scope.HoldAchAccountHolderName;
                $scope.ConfirmationAchBankName = $scope.HoldAchBankName;
                $scope.ConfirmationAchBankRoutingNumber = $scope.HoldAchBankRoutingNumber;
                $scope.ConfirmationAchBankAccountNumber = $scope.HoldAchCheckingAccountNumber;
                $scope.ConfirmationAchAccountType = $scope.HoldAchAccountType;
                $scope.ConfirmationDebitCreditFrequency = $scope.Frequency;
                //  $scope.ConfirmationAchSaveAccountInformationFuture = $scope.AchSaveAccountInformationFuture;
                $scope.ConfirmationAchEmail = $scope.HoldAchEmail;
                $scope.ConfirmationAchPavmentDate = $scope.PavmentDate;
                $scope.ConfirmationAchBankAccountName = $scope.HoldAchBankAccountName;
                $scope.ConfirmationAchPrimaryNumber = $scope.HoldAchPrimaryNumber;
                $scope.ConfirmationAchAccountTypeView = $scope.HoldAchAccountType == 'P' ? "ACH - Checking Account" : "ACH - Savings Account";
               // $scope.ConfirmationAchBankAccountNumberView = $scope.HoldAchCheckingAccountNumber.substr(-4);
                $scope.ConfirmationAchBankAccountNumberView = $scope.HoldAchCheckingAccountNumber;
                $scope.btndisabled = false;
                $scope.Loadname = "";
                $scope.showSwitchWidget = 'thirdDivStep3ACH';

            }
            decreaseLoadingCounter();
        },


        function (error) {
          
            decreaseLoadingCounter();
            $scope.btndisabled = false; $scope.Loadname = "";

            if (error == "") {


                $scope.MainError = "Server connection problem";
            }
            else {


                $scope.MainError = error.Message;;
            }
        });
       

    }

    $scope.SavedAchAmount = function ()
    {
  
        increaseLoadingCounter();
        $scope.confirmation = "";
        $scope.btndisabled = true;
       // $scope.Loadname = "glyphicon-refresh";
        // $scope.btnSubmit = true;
        var AccountNumber = $scope.ConfirmationAchMafAccountNumber;
        var PaymentAmount = $scope.ConfirmationAchAmountDue;
        var FeeAmoun = $scope.ConfirmationAchFee;
        var RountingNumber = $scope.ConfirmationAchBankRoutingNumber;
        var BankAccountNumber = $scope.ConfirmationAchBankAccountNumber;
        var BankName = $scope.ConfirmationAchBankName;
        var BankHolder = $scope.ConfirmationAchNameOnTheAch;
        var AccountType = $scope.ConfirmationAchAccountType;
        var UpdatedPhone = $scope.ConfirmationAchPrimaryNumber;
        var UpdatedPhoneDate = $scope.ConfirmationAchPavmentDate;       
        var Email = $scope.ConfirmationAchEmail;
     
        //  var SaveAccountFuture = $scope.AchSaveAccountInformationFuture.value == true ? "1" : "0";
        var SaveAccountFuture = "1";
        var BankAccountName = $scope.ConfirmationAchBankAccountName;
        var paymentFrequency = $rootScope.PaymentFrequency2;
        var Status = $scope.Status;
        var ScheduleId = $scope.ScheduleId;
     
        var paymentConfirmationdata = { "AccountNumber": AccountNumber, "PaymentAmount": PaymentAmount, "FeeAmoun": FeeAmoun, "RountingNumber": RountingNumber, "BankAccountNumber": BankAccountNumber, "BankName": BankName, "BankHolder": BankHolder, "AccountType": AccountType, "UpdatedPhone": UpdatedPhone, "UpdatedPhoneDate": UpdatedPhoneDate, "Email": Email, "SaveAccountFuture": SaveAccountFuture, "BankAccountName": BankAccountName, "paymentFrequency": paymentFrequency, "Status": Status, "ScheduleId": ScheduleId };



        AutoPayService.AchPaymentSave(paymentConfirmationdata, $scope.baseAddress).then(function (data)
        {
            
            GetAchDetail();
            $scope.btndisabled = false;
            $scope.Loadname = "";
            // $scope.confirmation = "AutoPay setup is successfull.";

            PaymentConfirmationAlert();
            $scope.AchAmountSubmit = false;
            $scope.Status = "";
            $scope.ScheduleId = "";

            $scope.CardError = "";
            $scope.CardMessage = "";
            $scope.Ach3StepError = "";
            $scope.confirmation = "";
            decreaseLoadingCounter();
        },


        function (error)
        {
            decreaseLoadingCounter();
            $scope.btndisabled = false;
            $scope.Loadname = "";
            if (error == "") {


                $scope.Ach3StepError = "Server connection problem";
            }
            else {


                $scope.Ach3StepError = error.Message;
            }
        });




    }

    $scope.AchDeletePop = function (data)
    {
      
        $scope.HoldAchCheckingAccountNumber = data.id;
        popAchDelete();
    }

    $scope.AchDelete = function ()
    {
        increaseLoadingCounter();
        var id = $scope.HoldAchCheckingAccountNumber;
       // $scope.Loadname = "glyphicon-refresh";
        $scope.btndisabled = true;
        paymentAmountService.DeleteSavedAch(id, $scope.AccountNumber, $scope.baseAddress).then(function (data)
        {
            $scope.IsActiveAchDetail = true;
            $scope.btndisabled = false;
            $scope.Loadname = "";                   
            GetAchDetail();
            $('#myModal2 .close').click();           
            decreaseLoadingCounter();
        },

        function (error)
        {
            decreaseLoadingCounter();
            $scope.btndisabled = false;
            $scope.Loadname = "";
            if (error == "")
            {
                $scope.AchDelete = "Server connection problem";
            }
            else
            {
                $scope.AchDelete = error.Message;;
            }
        });   
    }

    function popAchDelete()
    {
        $('#myModal2').modal({

            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    function popAutoPayDelete() {
        $('#myModal4').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    function PopCardDelete() {
        $('#myModal3').modal({

            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    function GetAchDetail()
    {
        
        increaseLoadingCounter();
        AutoPayService.GetAchDetail($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            var isActive = 0;
            dataArray = [];
            $scope.dataHolder = {};
            for (var counter = 0; counter < data.length; counter++)
            {
                var item = new Detail();
                item.AcctNo = data[counter].AcctNo;
                item.BankABA = data[counter].BankABA;
                item.BankAccountName = data[counter].BankHolder;
                item.BankAcctType = data[counter].BankAcctType == "P" ? "Checking" : "Savings";
                item.BankHolder = data[counter].CreatedBy;
                item.BankName = data[counter].BankName;
                item.ConfirmationID = data[counter].ConfirmationID;              
                item.ID = data[counter].ID;
                item.SaveAccountFuture = data[counter].SaveAccountFuture;
                item.TranDate = data[counter].TranDate;
                item.TranFee = data[counter].TranFee;
                item.TranPayment = data[counter].TranPayment;
                item.BankAcctNo = data[counter].BankAcctNo;
                item.Email = data[counter].Email;
                item.PrimaryNumber = data[counter].PrimaryNumber;
                item.BankAcctNoView = "XXXXX-" + data[counter].BankAcctNo.substr(-4);
                item.counter = counter;
               
                item.id = data[counter].ID;
                $scope.PaymentTypeACH = true;
                dataArray.push(item);
                ///////////////////////////////// saurabh 
                if ($scope.Status === "UpdateAutoPay" && data[counter].BankAcctNo === $scope.BankAcctNoEdit) {
                    isActive = 1;
                    item.Selected = true;
                    $scope.radioSavedAhc(item)
                }

                if (counter === data.length - 1 && isActive === 0) {
                    item.Selected = true;

                    $scope.radioSavedAhc(item)
                }

                ////////////////////////////////////// rahul

                //if ($scope.Status === "UpdateAutoPay")
                //{
                //    if (item.BankAcctNo === $scope.BankAcctNoEdit) {
                //        item.Selected = true;
                //        $scope.radioSavedAhc(item)
                //    }
                //}               
                //if (counter === data.length - 1)
                //{
                //    item.Selected = true;
                 
                //    $scope.radioSavedAhc(item)
                //}

             
            }
            $scope.dataHolder = dataArray;
           
            if ($scope.dataHolder.length > 0)
            {
                $scope.AchDetail.name = "SavedAchDetail";
                $scope.achGridDiv = true;
                $scope.achNewPaymentDetail = false;
                $scope.PaymentTypeACH = true;
            }
            else 
            {
                $scope.AccountType.name = "Checking";
                $scope.PaymentTypeACH = false;
                $scope.achNewPaymentDetail = true;
                $scope.PaymentTypeACH = false;
                $scope.achGridDiv = false;
                $scope.AchDetail.name = "NewAchDetail";

               
            }
            if ($scope.IsActiveAchDetail === true)
            {
                $scope.btndisabled = false;
                $scope.Loadname = "";
                $scope.showSwitchWidget = 'SecondDiv2AHC';
                $scope.IsActiveAchDetail = false;
            }
            
           
            decreaseLoadingCounter();
        },
               function (error)
               {
                   $scope.btndisabled = false;
                   $scope.Loadname = "";
                   decreaseLoadingCounter()
                   if (error == "")
                   {
                       $scope.FirstError = "Server connection problem";
                   }
                   else
                   {
                       $scope.FirstError = error.Message;
                   }
               });

    }

    $scope.removeErrorCvvTable = function ()
    {
        $scope.errorCvNumber = "";
    }

    $scope.DeleteAutoPayRecurring = function ()
    {

        increaseLoadingCounter();
      //  $scope.Loadname = "glyphicon-refresh";
        $scope.btndisabled = true;
        var accountNumber = $scope.deleteAutopayAccountNumber;
       
        AutoPayService.AutoPayDelete(accountNumber, $scope.baseAddress).then(function (data)
        {
            $scope.btndisabled = false;
            $scope.Loadname = "";
            $('#myModal4 .close').click();
            $scope.showSwitchWidget = 'StartAutoPay';               
          
            decreaseLoadingCounter();
        },

        function (error) {
            decreaseLoadingCounter();
            $scope.btndisabled = false;
            $scope.Loadname = "";
            if (error == "")
            {
                $scope.AutoPayDeleteError = "Server connection problem";
            }
            else {


                $scope.AutoPayDeleteError = error.Message;;
            }
        });

    }

    function NewCardPayment()
    {        
        var regexCvvNumber = /^\d*$/;
        var regexCardNumber = /^[0-9]{16,16}$/;
        var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var regexZip = /^(\d{5}-\d{4}|\d{5})$/;
        if ($scope.CardName == "" || $scope.CardName == undefined) {
            $scope.ErrorCardName = "Please Enter Name On Card."
            $scope.submittedInvalid = "submittedInvalid";
            $scope.Loadname = "";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.CardNumber == "" || $scope.CardNumber == undefined) {
            $scope.ErrorCardNumber = "Please Enter Card Number.";
            $scope.submittedInvalid2 = "submittedInvalid";
            $scope.Loadname = "";
            $scope.btndisabled = false;
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexCardNumber.test($scope.CardNumber)) { $scope.ErrorCardNumber = "Enter Only 16 Numbers"; $scope.submittedInvalid2 = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }
        else if ($scope.ExpiryDate == "" || $scope.ExpiryDate == undefined) {
            $scope.ErrorExpiryDate = "Please Select Your Expiry Date."
            $scope.submittedExpiryDate = "submittedInvalid";
            $scope.Loadname = "";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.ExpiryYear == "" || $scope.ExpiryYear == undefined) {
            $scope.ErrorExpiryYear = "Please Select Your Expiry Year."
            $scope.submittedExpiryYear = "submittedInvalid";
            $scope.Loadname = "";
            $scope.btndisabled = false;
            return;
        }
        else if ($scope.CvNumber == "" || $scope.CvNumber == undefined) {
            $scope.ErrorCvNumber = "Please Enter CVV Number."
            $scope.submittedCvNumber = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;

            return;
        }
        else if (!regexCvvNumber.test($scope.CvNumber)) { $scope.ErrorCvNumber = "Enter Only Numbers"; $scope.submittedCvNumber = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }
        else if ($scope.BillingName == "" || $scope.BillingName == undefined) {
            $scope.ErrorBillingName = "Please Enter First Name."
            $scope.submittedBillingName = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }

        else if ($scope.BillingLastName == "" || $scope.BillingLastName == undefined) {
            $scope.ErrorBillingLastName = "Please Enter Last Name."
            $scope.submittedBillinLastgName = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }

        else if ($scope.PrimaryNumber == "" || $scope.PrimaryNumber == undefined) {
            $scope.ErrorPrimaryNumber = "Please Enter Primary Number."
            $scope.submittedPrimaryNumber = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexPrimaryNumber.test($scope.PrimaryNumber)) { $scope.ErrorPrimaryNumber = "Enter Only Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }

        else if ($scope.Email == "" || $scope.Email == undefined) {
            $scope.ErrorEmail = "Please Enter Email."
            $scope.submittedEmail = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexEmail.test($scope.Email)) { $scope.ErrorEmail = "Not an email!"; $scope.submittedEmail = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }
        else if ($scope.Address == "" || $scope.Address == undefined) {
            $scope.ErrorAddress = "Please Enter Address."
            $scope.submittedAddress = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if ($scope.City == "" || $scope.City == undefined) {
            $scope.ErrorCity = "Please Enter City."
            $scope.submittedCity = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
       
        else if (!$scope.stateSelection.selectedState) {
            $scope.ErrorState = "Please Enter State."
            $scope.submittedState = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if ($scope.Zip == "" || $scope.Zip == undefined) {
            $scope.ErrorZip = "Please Enter Zip."
            $scope.submittedZip = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexZip.test($scope.Zip)) { $scope.ErrorZip = "Enter Only Zip Number"; $scope.submittedZip = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }

        else  if ($scope.checkBoxTermsAndConditionsPaymentAmount.value == false) {
            $scope.errorCvNumber = "You must agree to above terms and conditions.";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
      
        $scope.State = $scope.stateSelection.selectedState.Value;
  

        $scope.ConfirmationDebitCreditCardName = $scope.CardName;
        $scope.ConfirmationDebitCreditCardNumber = $scope.CardNumber;
        $scope.ConfirmationDebitCreditExpiryDate = $scope.ExpiryDate;
        $scope.ConfirmationDebitCreditExpiryYear = $scope.ExpiryYear;
        $scope.ConfirmationDebitCreditCvNumber = $scope.CvNumber;
        $scope.ConfirmationDebitCreditSaveAccountInformationFuture = $scope.DebitCreditSaveAccountInformationFuture.value;


        //  $scope.CardFee = "4.00";
        $scope.CardPaymentType = 'Credit/Debit';
        $scope.CardNumberView = $scope.CardNumber.substr(-4);
        $scope.CardTypeImg = $scope.CardType.Name == "Visa" ? "visa_icon" : "master_card";
        //  $scope.CardTotalAmount = parseFloat($scope.CardFee) + parseFloat($scope.OtherAmount);
        $scope.CardTypeData = $scope.CardType.Name 
        $scope.showSwitchWidget = 'thirddivstep3DebitCredit';
        $scope.Loadname = ""; $scope.btndisabled = false;
        $scope.TokenId = "";

    }

    function savedCardpayment()
    {       
        var regexCardNumber = /^\d*$/;
        var regexPrimaryNumber = /^[0-9+-]{10,12}$/;
        var regexEmail = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var regexZip = /^(\d{5}-\d{4}|\d{5})$/;     
        if ($scope.BillingName == "" || $scope.BillingName == undefined) {
            $scope.ErrorBillingName = "Please Enter Name."
            $scope.submittedBillingName = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }

        else if ($scope.BillingLastName == "" || $scope.BillingLastName == undefined) {
            $scope.ErrorBillingLastName = "Please Enter Last Name."
            $scope.submittedBillinLastgName = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }

        else if ($scope.PrimaryNumber == "" || $scope.PrimaryNumber == undefined) {
            $scope.ErrorPrimaryNumber = "Please Enter Primary Number."
            $scope.submittedPrimaryNumber = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexPrimaryNumber.test($scope.PrimaryNumber)) { $scope.ErrorPrimaryNumber = "Enter Only Primary Number"; $scope.submittedPrimaryNumber = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }

        else if ($scope.Email == "" || $scope.Email == undefined) {
            $scope.ErrorEmail = "Please Enter Email."
            $scope.submittedEmail = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexEmail.test($scope.Email)) { $scope.ErrorEmail = "Not an email!"; $scope.submittedEmail = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }
        else if ($scope.Address == "" || $scope.Address == undefined) {
            $scope.ErrorAddress = "Please Enter Address."
            $scope.submittedAddress = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if ($scope.City == "" || $scope.City == undefined) {
            $scope.ErrorCity = "Please Enter City."
            $scope.submittedCity = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!$scope.stateSelection.selectedState) {
            $scope.ErrorState = "Please Enter State."
            $scope.submittedState = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if ($scope.Zip == "" || $scope.Zip == undefined)
        {
            $scope.ErrorZip = "Please Enter Zip."
            $scope.submittedZip = "submittedInvalid";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;
        }
        else if (!regexZip.test($scope.Zip)) { $scope.ErrorZip = "Enter Only Zip Number"; $scope.submittedZip = "submittedInvalid"; $scope.Loadname = ""; $scope.btndisabled = false; return; }

        else  if ($scope.checkBoxTermsAndConditionsPaymentAmount.value == false) {
            $scope.errorCvNumber = "You must agree to above terms and conditions.";
            $scope.Loadname = ""; $scope.btndisabled = false;
            return;

        }
      
        $scope.State = $scope.stateSelection.selectedState.Value;
        // console.log(savedCardItems);
        savedCardItems.forEach(function (item)
        {
            
            if (item.Selected)
            {

                $scope.TokenId = item.TokenId;
                $scope.CardTypeImg = item.CardType == "Visa" ? "visa_icon" : "master_card";
                $scope.CardTypeData = item.CardType == "V" ? "Visa" : "Master";
                $scope.ConfirmationDebitCreditCardName = item.CardName;
                $scope.ConfirmationDebitCreditCardNumber = item.CardNumber;
                $scope.CardExpiry = item.CardExpiry;
                $scope.ConfirmationDebitCreditCvNumber = item.CVV;

            }
        });
        $scope.ConfirmationDebitCreditExpiryDate = $scope.CardExpiry.substring(0, 2);
        $scope.ConfirmationDebitCreditExpiryYear = $scope.CardExpiry.substring(3, 7);

        $scope.ConfirmationDebitCreditSaveAccountInformationFuture = $scope.DebitCreditSaveAccountInformationFuture.value;


        if ($scope.ConfirmationDebitCreditCvNumber == '') { $scope.errorCvNumber = "Please enter CVV number!";   $scope.Loadname = ""; $scope.btndisabled = false;return; } else { $scope.errorCvNumber = ""; }
        if (!regexCardNumber.test($scope.ConfirmationDebitCreditCvNumber)) { $scope.errorCvNumber = "Please Enter CVV Only Numbers"; $scope.Loadname = ""; $scope.btndisabled = false; return; }
        $scope.Loadname = ""; $scope.btndisabled = false;

        $scope.CardPaymentType = 'Credit/Debit';
        $scope.CardNumberView = $scope.ConfirmationDebitCreditCardNumber;
        $scope.CardTypeImg = $scope.CardTypeData == "Visa" ? "visa_icon" : "master_card"; 
        $scope.showSwitchWidget = 'thirddivstep3DebitCredit';
        $scope.Loadname = ""; $scope.btndisabled = false;
        var TokenId =   $scope.TokenId 
    }
    //-------------------------------------------------------Debit Credit PaymentAmount---------------------------------
    $scope.DebitCreditPaymentAmountContinue = function ()
    {
        if ($scope.DueDate == "")
        {
            $scope.errorCvNumber = "No Billing Due Date";
            return;
        }

        if (parseFloat($scope.MinimumAmount) > parseFloat($scope.TotalAmountDue) || parseFloat($scope.MaximumAmount) < parseFloat($scope.TotalAmountDue))
        { $scope.errorCvNumber = "Sorry we can't proccess your payment at this time. Please call Customer Service @ 800-793-9661 ext 600."; return; }
        // $scope.Loadname = "glyphicon-refresh";
        var collectiondata = { "AccountNumber": $scope.AccountNumber, "Amount": $scope.TotalAmountDue, "FuturePaymentDate": $scope.DueDate };
        AutoPayService.CheckValidCardTransaction(collectiondata, $scope.baseAddress).success(function (data) {
         if (JSON.parse(data) == "true")
         {

             $scope.btndisabled = true;
             if ($scope.TypeOfDebitCredit.Name == 'New') {

                 NewCardPayment();
                 $scope.autopaySchedule();
                 return;
             }
             else ($scope.TypeOfDebitCredit.Name = 'Saved')
             {

                 savedCardpayment();
                 $scope.autopaySchedule();

                 return;
             };
              
         }
            else
            {
                $scope.errorCvNumber = JSON.parse(data);
                return;
            }
         }).error(function (error) {
            
            if (error == "") { $scope.errorCvNumber = "Server connection problem"; return; }
            else { $scope.errorCvNumber = error.Message; return; }
        });



       
   
    };
    
    //--------------------Remove Error --------------
    $scope.RemoveErrorCardName = function () {
        $scope.ErrorCardName = ""
        $scope.submittedInvalid = "";

    };

    $scope.RemoveErrorCardNumber = function () {
        $scope.ErrorCardNumber = "";
        $scope.submittedInvalid2 = "";

    };

    $scope.RemoveExpiryDate = function () {
        $scope.ErrorExpiryDate = "";
        $scope.submittedExpiryDate = "";
    };

    $scope.RemoveExpiryYear = function () {
        $scope.ErrorExpiryYear = "";
        $scope.submittedExpiryYear = "";
    };

    $scope.RemoveCvNumber = function () {
        $scope.ErrorCvNumber = ""
        $scope.submittedCvNumber = "";
    };

    $scope.RemoveBillingName = function () {
        $scope.ErrorBillingName = "";
        $scope.submittedBillingName = "";
    };

    $scope.RemoveBillingLastName = function () {
        $scope.ErrorBillingLastName = "";
        $scope.submittedBillinLastgName = "";
    };

    $scope.RemovePrimaryNumber = function () {
        $scope.ErrorPrimaryNumber = ""
        $scope.submittedPrimaryNumber = "";
    };
    $scope.RemoveEmail = function () {
        $scope.ErrorEmail = ""
        $scope.submittedEmail = "";
    };
    $scope.RemoveAddress = function () {
        $scope.ErrorAddress = ""
        $scope.submittedAddress = "";
    };
    $scope.RemoveCity = function () {
        $scope.ErrorCity = ""
        $scope.submittedCity = "";
    };
    $scope.RemoveState = function () {
    
        $scope.ErrorState = ""
        $scope.submittedState = "";
    };
    $scope.RemoveZip = function () {
        $scope.ErrorZip = ""
        $scope.submittedZip = "";
    };
 
    //--------------------End --------------

    $scope.RemovePaymentAmounttermsAndConditionsError = function ()
    {
        $scope.PaymentAmountTermsAndConditionsError = '';
        $scope.errorCvNumber = '';
    }
    $scope.ACHPaymentConfirmationBack = function()
    {

        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.Ach3StepError = "";
        $scope.confirmation = "";
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.showSwitchWidget = 'SecondDiv2AHC';
        $scope.checkBoxTermsAndConditions.value = false;
        $scope.checkBoxTermsAndConditionsPaymentAmount.value = false;
        $scope.confirmation = "";
    }

    $scope.DebitCreditaymentConfirmationBack = function ()
    {
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.Ach3StepError = "";
        $scope.confirmation = "";
        $scope.CardError = "";
        $scope.CardMessage = "";
        $scope.CardName = "";
        $scope.CardNumber = "";
        $scope.ExpiryDate = "";
        $scope.ExpiryYear = "";
        $scope.CvNumber = "";
        $scope.CardType.Name = "Visa";
        // $scope.TypeOfDebitCredit.Name = "New";
        $scope.DebitCreditSaveAccountInformationFuture.value = true;
        $scope.showSwitchWidget = 'SecondDivStep2DebitCredit';
        $scope.checkBoxTermsAndConditions.value = false;
        $scope.checkBoxTermsAndConditionsPaymentAmount.value = false;
       
        if($scope.TypeOfDebitCredit.Name == "Saved")
            {
            if (savedCardItems)
            {
                if (savedCardItems.length > 0)
                {
                    savedCardItems.forEach(function (item) {
             
                        item.CVV = '';
                    });
                    $scope.savedCards = savedCardItems;
                    //  $scope.SaveCardInfo = true;
                    $scope.cardPayWith = true;
                    $scope.TypeOfDebitCredit.Name = 'Saved';
                    $scope.SaveCardInfo = true;
                    $scope.newCardView = false;
                    $scope.BillingAddress = false;
                    $scope.SavedBillingAddress = true;
               
                    //DisableNewCard();
                }
            }
    }

   };

   $scope.Cancel = function ()
   {
       
       
       $scope.termsAndConditionsError = "";
       $scope.FirstError = "";
       $scope.CardError = "";
       $scope.CardMessage = "";
       $scope.Ach3StepError = "";
       $scope.confirmation = "";
       $scope.CardError = "";
       $scope.CardMessage = "";
       $scope.MainError = "";
       $scope.MainError2 = "";
       $scope.PaymentAmountTermsAndConditionsError = "";

       $scope.CardName = "";
       $scope.CardNumber = "";
       $scope.ExpiryDate = "";
       $scope.ExpiryYear = "";
       $scope.CvNumbe = "";
       $scope.DebitCreditSaveAccountInformationFuture.value = true;
     //  $scope.showSwitchWidget = 'StartAutoPay';
       $scope.GetConfirmationPayment();

       $scope.checkBoxTermsAndConditions.value = false;
       $scope.checkBoxTermsAndConditionsPaymentAmount.value = false;
       $scope.confirmation = "";

       $scope.submittedBankAccountName = "";
       $scope.errorBankAccountName = "";
       $scope.submittedBankAccountName = "";
       $scope.errorBankAccountName = "";
       $scope.submittedBankAccountName = "";
       $scope.errorCheckingAccountNumber = "";
       $scope.submittedRouting = "";
       $scope.ErrorRouting = "";
       $scope.submittedEmail = "";
       $scope.errorEmail = "";
       $scope.submittedInvalid = "";
       $scope.ErrorCardName = "";
       $scope.submittedInvalid2 = "";
       $scope.ErrorCardNumber = "";
       $scope.submittedExpiryDate = "";
       $scope.ErrorExpiryDate = "";
       $scope.submittedExpiryYear = "";
       $scope.ErrorExpiryYear = "";
       $scope.submittedCvNumber = "";
       $scope.ErrorCvNumber = "";
       $scope.submittedBillingName = "";
       $scope.ErrorBillingName = "";
       $scope.submittedBillinLastgName = "";
       $scope.ErrorBillingLastName = "";
       $scope.submittedPrimaryNumber = "";
       $scope.ErrorPrimaryNumber = "";
       $scope.submittedEmail = "";
       $scope.ErrorEmail = "";
       $scope.submittedAddress = "";
       $scope.ErrorAddress = "";
       $scope.submittedCity = "";
       $scope.ErrorCity = "";
       $scope.submittedState = "";
       $scope.ErrorState = "";
       $scope.submittedZip = "";
       $scope.ErrorZip = "";
     

       $scope.errorPrimaryNumber = "";
       $scope.submittedCheckingAccountNumber = "";

       $scope.Status = "";
       $scope.ScheduleId = "";
   };


   function GetAccountInformation(data)
   {
       
       $scope.AccountHolderName = data.AccountHolder;
       $scope.PrimaryNumber = data.AccountHolderPhoneNumber;
       $scope.DueDate = data.DueDate;
       $scope.Frequency = $rootScope.PaymentFrequency;
      
       $scope.AmountDue = "$" + data.TotalAmountDue
       $scope.OtherAmount = data.OtherAmount;
       $scope.HolderPrimaryNumber = data.AccountHolderPhoneNumber;
       $scope.RemainingTerm = data.RemainingTerm;
       $rootScope.TodayDate = data.TodayDate;
       $scope.TotalAmountDue = data.TotalAmountDue;
       $scope.MinimumAmount = data.MinimumAmount;
       $scope.MaximumAmount = data.MaximumAmount;
   }

   $scope.checkedDebitCredit = false;
   $scope.SaveCardDisabled = false;
   $scope.radioSavedCard = function (data)
   {
       $scope.saveCard = true;
       savedCardItems.forEach(function (item)
       {
           if (item.TokenId === data.TokenId)
           {
               GetLastBillingAddress(item.TokenId);
               item.Selected = true;

           }
           else { item.Selected = false; }
       });
      // $scope.checkedDebitCredit = true;
       $scope.TypeOfDebitCredit.Name = 'Saved';
       $scope.CardType.Name = "";
       $scope.SaveCardDisabled = true;
       $scope.CardName = "";
       $scope.CardNumber = "";
       $scope.ExpiryYear = "";
       $scope.ExpiryDate = "";
       $scope.CvNumber = "";
       $scope.DebitCreditSaveAccountInformationFuture.value = true;

       $scope.submittedBankAccountName = "";
       $scope.errorBankAccountName = "";
       $scope.submittedBankAccountName = "";
       $scope.errorBankAccountName = "";
       $scope.submittedBankAccountName = "";
       $scope.errorCheckingAccountNumber = "";
       $scope.submittedRouting = "";
       $scope.ErrorRouting = "";
       $scope.submittedEmail = "";
       $scope.errorEmail = "";
       $scope.submittedInvalid = "";
       $scope.ErrorCardName = "";
       $scope.submittedInvalid2 = "";
       $scope.ErrorCardNumber = "";
       $scope.submittedExpiryDate = "";
       $scope.ErrorExpiryDate = "";
       $scope.submittedExpiryYear = "";
       $scope.ErrorExpiryYear = "";
       $scope.submittedCvNumber = "";
       $scope.ErrorCvNumber = "";
       $scope.submittedBillingName = "";
       $scope.ErrorBillingName = "";
       $scope.submittedBillinLastgName = "";
       $scope.ErrorBillingLastName = "";
       $scope.submittedPrimaryNumber = "";
       $scope.ErrorPrimaryNumber = "";
       $scope.submittedEmail = "";
       $scope.ErrorEmail = "";
       $scope.submittedAddress = "";
       $scope.ErrorAddress = "";
       $scope.submittedCity = "";
       $scope.ErrorCity = "";
       $scope.submittedState = "";
       $scope.ErrorState = "";
       $scope.submittedZip = "";
       $scope.ErrorZip = "";
       $scope.errorPrimaryNumber = "";
       $scope.submittedCheckingAccountNumber = "";
     
       $scope.SavedBillingAddress = true;
       $scope.BillingAddress = false;
   }

   $scope.radioNewCard = function ()
   {
       savedCardItems.forEach(function (item) {
           item.Selected = false;
       });
       $scope.BillingName = "";
       $scope.BillingLastName = "";
       $scope.PrimaryNumber = "";
       $scope.Email = "";
       $scope.Address = "";
       $scope.City = "";
       $scope.stateSelection.selectedState = "";
       $scope.Zip = "";
       
        $scope.checkedDebitCredit = false;
        $scope.TypeOfDebitCredit.Name = 'New';
        $scope.CardType.Name = "Visa";
        $scope.SaveCardDisabled = false;
        $scope.checkedDebitCredit = false;
        $scope.TypeOfDebitCredit.Name = 'New';
        $scope.CardType.Name = "Visa";
        $scope.SaveCardDisabled = false;

        $scope.BillingAddress = true;
        $scope.SaveCardInfo = false;
        $scope.newCardView = true;
        $scope.SavedBillingAddress = false;
   }
    $scope.SavedCardPayment = function()
    {
    
        increaseLoadingCounter();
       var Fee=  $scope.CardFee
      //  $scope.Loadname = "glyphicon-refresh";
        $scope.btndisabled = true;
        var paymentFrequency = $rootScope.PaymentFrequency2;;
        var Currency = "USD";

        var Status = $scope.Status;
        var ScheduleId = $scope.ScheduleId;
      
        var cardPaymentConfirmationdata = {
            
            TokenId: $scope.TokenId, "CardName": $scope.ConfirmationDebitCreditCardName, "Country": $scope.Country,
            "FirstName": $scope.BillingName, "LastName": $scope.BillingLastName, "Street": $scope.Address, "City": $scope.City, "State": $scope.stateSelection.selectedState.Value, "PostelCode": $scope.Zip,
            "Email": $scope.Email, "CardNumber": $scope.ConfirmationDebitCreditCardNumber, "ExpirationMonth": $scope.ConfirmationDebitCreditExpiryDate, "ExpirationYear": $scope.ConfirmationDebitCreditExpiryYear, 
            "CardType": $scope.CardTypeData, "CvNumber": $scope.ConfirmationDebitCreditCvNumber, "Currency": Currency, "Amount": $scope.CardTotalAmount, "AccountNumber": $scope.AccountNumber, "PrimaryNumber": $scope.PrimaryNumber, "Date": $scope.PavmentDate, "SaveFuture": $scope.checkBoxTermsAndConditionsPaymentAmount.value, "RemainingTerm": $scope.RemainingTerm, "paymentFrequency": paymentFrequency, "Fee": Fee, "Status": Status, "ScheduleId": ScheduleId

        };

       
        if ($scope.TokenId == "") {
            AutoPayService.CardAutoPaySchedule(cardPaymentConfirmationdata, $scope.baseAddress).then(function (data) {
                $scope.btndisabled = false;
                $scope.Loadname = "";
                
                if (JSON.parse(data) == "successfull")
                {       
                    $scope.AchAmountSubmit = false;
                    $scope.CardError = "";
                    $scope.CardMessage = "";
                    $scope.Ach3StepError = "";
                    $scope.confirmation = "";
                    PaymentConfirmationAlert();
                }
                else
                {                   
                    $scope.CardError = JSON.parse(data);
                }
                decreaseLoadingCounter();
            },

            function (error)
            {
               
                decreaseLoadingCounter();
                $scope.btndisabled = false;
                $scope.Loadname = "";
                if (error == "")
                {
                    $scope.CardError = "Server connection problem";
                }
                else
                {
                    $scope.CardError =  error.Message;
                }
            });
            return;
        }

        else
        {

            AutoPayService.SaveCardAutoPaySchedule(cardPaymentConfirmationdata, $scope.baseAddress).then(function (data) {
                $scope.btndisabled = false;
                $scope.Loadname = "";
               
                if (JSON.parse(data) == "successfull")
                {                 
                    $scope.AchAmountSubmit = false;
                    PaymentConfirmationAlert();
                    $scope.CardError = "";
                    $scope.CardMessage = "";
                    $scope.Ach3StepError = "";
                    $scope.confirmation = "";
                   
                }
                else
                {
                    $scope.CardError = JSON.parse(data);
                }
                decreaseLoadingCounter();
            },

          function (error) {
           
              decreaseLoadingCounter();
              $scope.btndisabled = false;
              $scope.Loadname = "";
              if (error == "") {
                  $scope.CardError = "Server connection problem";
              }
              else {


                  $scope.CardError = error.Message;
              }
          });
            return;
        }
    }
        
    function PaymentConfirmationAlert() {
        $('#ConfirmationPaymentModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    $scope.CardDeleteConfirm = function (data) {
     
        $scope.DeleteTokenId = data.TokenId;
        PopCardDelete();
    };
   
    $scope.DeleteCard = function () {

        increaseLoadingCounter();
      //  $scope.Loadname = "glyphicon-refresh";

        var BaseAddress = $scope.baseAddress;
        paymentAmountService.DeleteCard($scope.DeleteTokenId, BaseAddress).then(function (data) {

            $scope.PaymentTypeDebitCredit();
            decreaseLoadingCounter();
            $scope.Loadname = "";
            $('#myModal3 .close').click();
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.CardDelete = "Server connection problem";
            }
            else {
                $scope.CardDelete = error.Message;;
            }
        });
    };
    // $scope.ScheduleDueDate
    autopayScheduleGrid = [];
    $scope.autopayScheduleGrid = {};
    var autopayScheduleData = function ()
    {
        this.ScheduleDueDate = '';
        this.Amount = '';
    };
    $scope.autopaySchedule = function()
    {      
        autopayScheduleGrid = [];
        $scope.autopayScheduleGrid = {};
        var Days = $scope.Frequency == "Weekly" ? 7 : $scope.Frequency == "Monthly" ? 30 : $scope.Frequency == "Yearly" ? 365 : $scope.Frequency == "Bi-Weekly" ? 14 : $scope.Frequency == "Semi-Monthly" ? 15 : $scope.Frequency == "Daily" ? 1 : 0;
        var day = 0;
        //Create schedule for next due date
        var item = new autopayScheduleData();
        var dat = new Date($scope.AutoPayDueDate);
       
        item.ScheduleDueDate = dat;
        item.Amount = $scope.TotalAmountDue;

        // CReate schedule date for other terms
        autopayScheduleGrid.push(item);
        for (var counter = 0; counter < parseInt( $scope.RemainingTerm)-1; counter++)
        {         
           var item = new autopayScheduleData();           
           var dat = new Date($scope.AutoPayDueDate);
             day += Days ;
            var dat = dat.addDays(day);
            item.ScheduleDueDate = dat;
            item.Amount = $scope.OtherAmount;
          //  item.counter = counter;
            autopayScheduleGrid.push(item);

        }
        $scope.autopayScheduleGrid = autopayScheduleGrid;
    }


    Date.prototype.addDays = function (days)
    {
        var dat = new Date(this.valueOf());
        dat.setDate(dat.getDate() + days);
        return dat;
    }
    var dat = new Date();

    $scope.GetAchFee = function ()
    {
        increaseLoadingCounter();
        AutoPayService.GetAchFee($scope.AccountNumber, $scope.baseAddress).then(function (data)
        {

            $scope.ConfirmationAchFee = JSON.parse(data);

            $scope.ConfirmationAchTotalAmount = parseFloat($scope.ConfirmationAchFee) + parseFloat($scope.OtherAmount);
            decreaseLoadingCounter();
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }
        });
        

    };


    $scope.GetCardFee = function ()
    {
        increaseLoadingCounter();
        AutoPayService.GetCardFee($scope.AccountNumber, $scope.baseAddress).then(function (data) {

            $scope.CardFee = JSON.parse(data);
            $scope.CardTotalAmount = parseFloat($scope.CardFee) + parseFloat($scope.OtherAmount);
            $scope.ConfirmationAchTotalAmount = parseFloat($scope.ConfirmationAchFee) + parseFloat($scope.OtherAmount);
            decreaseLoadingCounter();
     
        }, function (error) {
            decreaseLoadingCounter();
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }
        });


    };

    $scope.achNewPaymentDetail = true;
    $scope.PaymentTypeACHSave = function ()
    {

        $scope.achGridDiv = true;
        $scope.achNewPaymentDetail = false;
    };


    $scope.PaymentTypeACHNew = function ()
    {
        $scope.achGridDiv = false;
        $scope.achNewPaymentDetail = true;
        $scope.MainError2 = "";
        $scope.AchDetail.name = 'NewAchDetail';
        $scope.AccountType.name = 'Checking';
        $scope.AchDisabled = false;
        $scope.PrimaryNumber = $scope.HolderPrimaryNumber;
        $scope.Email = sessionStorage.getItem('Email');
    };
    $scope.radioSavedCardShow = function () {
        PaymentByDebitCreditCardOnly();
        $scope.SaveCardInfo = true;
        $scope.newCardView = false;
        $scope.SavedBillingAddress = true;
        $scope.BillingAddress = false;
    };
    $scope.CardEditBillingAddress = function () {
        $scope.BillingAddress = true;
        $scope.SavedBillingAddress = false;

    };
    var chkBilling = 0, chkUserProfile = 0;
     $scope.GetAddress = function () {
        if ($scope.chkLastBillAddress.value == true && chkBilling == 0) {
            chkBilling = 1;
            chkUserProfile = 0;
            $scope.chkuserProfileaddress.value = false;
            GetLastBillingAddress(0);
        }
        else if ($scope.chkuserProfileaddress.value == true && chkUserProfile == 0) {
            chkUserProfile = 1;
            chkBilling = 0;
            $scope.chkLastBillAddress.value = false;
            GetUserProfileAddress(0);
        }
        else {
            chkBilling = 0, chkUserProfile = 0;
            ClearBillingAddress();
        }
    };


     function GetUserProfileAddress() {

         increaseLoadingCounter();
         var AccountNumber = $scope.AccountNumber;
         var BaseAddress = $scope.baseAddress;
         AutoPayService.GetUserProfileAddress(AccountNumber, BaseAddress).then(function (data) {
            
             $scope.BillingName = data.FullName.substring(0, data.FullName.lastIndexOf(" "));
             $scope.BillingLastName = data.FullName.substring(data.FullName.lastIndexOf(" ") + 1);
             $scope.PrimaryNumber = data.CellNumber;
             $scope.Email = data.Email;
             $scope.Address = data.Address1;
             $scope.City = data.City;
             $scope.stateSelection.selectedState = $scope.StateList.find(function (state) { return state.Value === data.State; });
             $scope.Zip = data.Zip;
             decreaseLoadingCounter();
         }, function (error)
         {
             decreaseLoadingCounter();
             if (error == "") {
                 $scope.errorCvNumber = "Server connection problem";
             }
             else {
                 $scope.errorCvNumber = error.Message;;
             }
         });
     }


    //////////////////////////////////////////////////////////////////////////////////////////////Autopay Edit ///////////////////////////////////
     $scope.TokenIdEdit = "";
     $scope.BankAcctNoEdit = "";

     $scope.AutoPayEdit = function (data)
     {
         $scope.Status = "UpdateAutoPay";
         $scope.autoPayDuplicatePayment();
         $scope.showSwitchWidget = 'FirstDivStep1';

        
         $scope.ScheduleId = data.ScheduleId;
         $scope.TokenIdEdit = data.TokenId;
         $scope.BankAcctNoEdit = data.BankAcctNo;
        

         if (data.ScheduleMethod == 'P' || data.ScheduleMethod == 'S')
         {
             $scope.PaymentMethod.name = "ACH";

         }
         else
         {
             $scope.PaymentMethod.name = "DebitCredit";
             $scope.PaymentTypeDebitCredit();
         }      

     };



}]);