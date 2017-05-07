app.controller('manageSavedPaymentDetailsController', ['$scope', '$rootScope', '$location', 'manageSavedPaymentDetailsService', 'WebApiService', function ($scope, $rootScope, $location, manageSavedPaymentDetailsService, WebApiService) {
    //------------------------------------- Global Section --------------------------------------------------
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.chkLastBillingAddress = false;
    $scope.chkUserProfileAddress = false;
    var chkBilling = 0, chkUserProfile = 0;
    var savedCardItem = function () {
        this.TokenId = '';
        this.CardNumber = '';
        this.CardType = '';
        this.CardName = '';
        this.CardExpiry = '';
        this.Selected = false;
    };
    var savedCardItems = [];
    $scope.savedCards = {};
    var savedAchItem = function () {
        this.BankABA = '';
        this.BankAccountName = '';
        this.BankAcctType = '';
        this.BankHolder = '';
        this.BankName = '';
        this.BankAcctNo = '';
        this.achEmail = '';
        this.PrimaryNumber = '';
        this.BankAcctNoView = '';
        this.counter = '';
        this.ID = '';
        this.Selected = false;
    };
    var savedAchItems = [];
    $scope.savedAch = {};
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

    //-------------------------------------- Main Section ---------------------------------------------------
    (function () {
        GetSavedCardDetails();
        BindSavedAchDetails();
    })();

    //------------------------------------- Card Section --------------------------------------------------

        function GetSavedCardDetails() {
        increaseLoadingCounter();
        manageSavedPaymentDetailsService.GetSavedCardDetails($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            savedCardItems = [];
            for (var counter = 0; counter < data.data.length; counter++) {
                var item = new savedCardItem();
                item.TokenId = data.data[counter].TokenId;
                item.CardNumber = data.data[counter].CardNumber;
                item.CardType = data.data[counter].CardType;
                item.CardName = data.data[counter].CardName;
                item.CardExpiry = data.data[counter].CardExpiry;
                if (counter === data.data.length - 1) {
                    item.Selected = true;
                    GetLastBillingAddress(item.TokenId);
                }
                savedCardItems.push(item);
            }
            $scope.savedCards = savedCardItems;
            if ($scope.savedCards.length > 0) {
                $scope.NewAddress = false;
                $scope.SavedAddress = true;
                $scope.SavedCard = true;
                $scope.SavedCardBlank = false;
            }
            else
            {
                $scope.SavedCard = false;
                $scope.SavedCardBlank = true;
            }
            decreaseLoadingCounter();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };

    $scope.radioSavedCard = function (data) {
        savedCardItems.forEach(function (item) {
            if (item.TokenId === data.TokenId) {
                item.Selected = true;
                GetLastBillingAddress(item.TokenId);
                chkBilling = 0, chkUserProfile = 0;
            }
            else { item.Selected = false; }
        });
        $scope.SavedAddress = true;
        $scope.NewAddress = false;
        ClearBillingAddress();
    }

    function bindStateList() {
        manageSavedPaymentDetailsService.GetStateList().then(function (data) {
            var selectedState = $scope.State;
            var states = data.data.States;
            var selectedOption = states.find(function (state) { return state.Value === selectedState; });
            $scope.StateList = states;
            $scope.selectedState = selectedOption;
        },
          function (err) {
              $scope.ErrorMessage = "Get State Failed!";
          });
    };

    $scope.GetAddress = function () {        
        if ($scope.chkLastBillAddress == true && chkBilling == 0) {
            chkBilling = 1;
            chkUserProfile = 0;
            $scope.chkuserProfileaddress = false;
            GetLastBillingAddress(0);
        }
        else if ($scope.chkuserProfileaddress == true && chkUserProfile == 0) {
            chkUserProfile = 1;
            chkBilling = 0;
            $scope.chkLastBillAddress = false;
            GetUserProfileAddress(0);
        }
        else {
            chkBilling = 0, chkUserProfile = 0;
            ClearBillingAddress();
        }
    };
    function ClearBillingAddress() {
        $scope.CardEmail = '';
        $scope.Address = '';
        $scope.PrimaryNumber = '';
        $scope.Address = '';
        $scope.City = '';
        $scope.LastName = '';
        $scope.FirstName = '';
        $scope.Zip = '';
        $scope.selectedState = '';
        $scope.errorFirstName = "";
        $scope.errorLastName = "";
        $scope.errorPrimaryNumber = "";
        $scope.errorCardEmail = "";
        $scope.errorAddress = "";
        $scope.errorselectedState = "";
        $scope.errorZip = "";
        $scope.chkLastBillAddress = false;
        $scope.chkuserProfileaddress = false;
        $scope.error = "";
        $scope.success = "";
    };

    function GetUserProfileAddress() {
        increaseLoadingCounter();
        manageSavedPaymentDetailsService.GetUserProfileAddress($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            $scope.FirstName = data.data.FullName.substring(0, data.data.FullName.lastIndexOf(" "));
            $scope.LastName = data.data.FullName.substring(data.data.FullName.lastIndexOf(" ") + 1);
            $scope.CardEmail = data.data.Email;
            $scope.PrimaryNumber = data.data.CellNumber;
            $scope.Address = data.data.Address1;
            $scope.City = data.data.City;
            $scope.State = data.data.State;
            $scope.Zip = data.data.Zip;
            bindStateList();
            decreaseLoadingCounter();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };

    function GetLastBillingAddress(tokenId) {
        increaseLoadingCounter();
        var AccountNumber = $scope.AccountNumber;
        var BaseAddress = $scope.baseAddress;
        manageSavedPaymentDetailsService.GetBillingAddress(AccountNumber, tokenId, BaseAddress).success(function (data) {
            $scope.FirstName = data.FirstName;
            $scope.LastName = data.LastName;
            $scope.PrimaryNumber = data.PrimaryNumber;
            $scope.CardEmail = data.EmailID;
            $scope.Address = data.Address;
            $scope.City = data.City;
            $scope.State = data.State;
            $scope.Zip = data.Zip;
            bindStateList();
            decreaseLoadingCounter();
        }).error(function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };

    $scope.EditAddress = function () {
        $scope.SavedAddress = false;
        $scope.NewAddress = true;
    };

    $scope.DeleteCard = function () {        
        manageSavedPaymentDetailsService.DeleteCard($scope.TokenId, $scope.baseAddress).then(function (data) {
            $('#CardConfirmModal .close').click();
            GetSavedCardDetails();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };
    $scope.DeleteConfirm = function (data) {
        $scope.TokenId = data.TokenId;
        PopCardDelete();
    };

    function PopCardDelete() {
        $('#CardConfirmModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    };

    function AddressValidation() {
        var returnValue = true;
        var regexEmail = /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/;
        var regaxZip = /^(\d{5}-\d{4}|\d{5})$/;        

        if ($scope.FirstName == "" || $scope.FirstName == undefined) { $scope.errorFirstName = "Please Enter First Name"; return returnValue = false; }
        else { $scope.errorFirstName = ""; returnValue = true; }

        if ($scope.LastName == "" || $scope.LastName == undefined) { $scope.errorLastName = "Please Enter Last Name"; return returnValue = false; }
        else { $scope.errorLastName = ""; returnValue = true; }

        if ($scope.PrimaryNumber == "" || $scope.PrimaryNumber == undefined) { $scope.errorPrimaryNumber = "Please Enter Primary Number"; return returnValue = false; }
        else { $scope.errorPrimaryNumber = ""; returnValue = true; }

        if ($scope.CardEmail == "" || $scope.CardEmail == undefined) { $scope.errorCardEmail = "Please Enter Email"; return returnValue = false; }
        else if (!regexEmail.test($scope.CardEmail)) { $scope.errorCardEmail = "Not an email!"; return returnValue = false; }
        else { $scope.errorCardEmail = ""; returnValue = true; }

        if ($scope.Address == "" || $scope.Address == undefined) { $scope.errorAddress = "Please Enter Address"; return returnValue = false; }
        else { $scope.errorAddress = ""; returnValue = true; }

        if ($scope.City == "" || $scope.City == undefined) { $scope.errorCity = "Please Enter City"; return returnValue = false; }
        else { $scope.errorCity = ""; returnValue = true; }

        if ($scope.selectedState == "" || $scope.selectedState == undefined) { $scope.errorselectedState = "Please Please Select State"; return returnValue = false; }
        else { $scope.errorselectedState = ""; returnValue = true; }

        if ($scope.Zip == "" || $scope.Zip == undefined) { $scope.errorZip = "Please Enter Zip"; return returnValue = false; }
        else if (!regaxZip.test($scope.Zip)) { $scope.errorZip = "Not an Zip!"; return returnValue = false; }
        else { $scope.errorZip = ""; returnValue = true; }


        return true;
    }

    $scope.SaveCardAddress = function () {
        if(AddressValidation()==true)
        {
            increaseLoadingCounter();
            savedCardItems.forEach(function (item) {
                if (item.Selected) {
                    $scope.TokenId = item.TokenId;
                    $scope.CvNumber = item.CVV;
                    $scope.CardName = item.CardName;
                    $scope.ExpiryMonth = item.CardExpiry.substring(0, 2);
                    $scope.ExpiryYear = item.CardExpiry.substring(3, 7);
                    $scope.CardNumber = item.CardNumber;
                    $scope.CardLastDigit = item.CardNumber;
                    $scope.CardType = item.CardType == "V" ? "001" : "002";
                }
            });            
            var SaveCardAddressData = {
                "CardName": $scope.CardName, "TokenId": $scope.TokenId,
                "FirstName": $scope.FirstName, "LastName": $scope.LastName, "Street": $scope.Address, "City": $scope.City, "State": $scope.State, "PostelCode": $scope.Zip,
                "Email": $scope.CardEmail, "CardNumber": $scope.CardNumber, "ExpirationMonth": $scope.ExpiryMonth, "ExpirationYear": $scope.ExpiryYear,
                "CardType": $scope.CardType, "AccountNumber": $scope.AccountNumber, "PrimaryNumber": $scope.PrimaryNumber.replace(/-/g, "")
            };

            manageSavedPaymentDetailsService.SaveCardAddress(SaveCardAddressData, $scope.baseAddress).success(function (data) {
                var result = JSON.parse(data);
                if (result.length == 10) $scope.success = "Saved Successfully."
                else $scope.error = result;                                   
                decreaseLoadingCounter();
            }).error(function (error) {
                decreaseLoadingCounter();
                if (error == "") {
                    $scope.error = "Server connection problem";
                }
                else {
                    $scope.error = error.Message;;
                }
            });
        }
    };


    //-----------------------------------  ACH Section -----------------------------------------------//

    function BindSavedAchDetails() {
        increaseLoadingCounter();
        manageSavedPaymentDetailsService.GetSavedAchDetails($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            savedAchItems = [];
            for (var counter = 0; counter < data.data.length; counter++) {
                var item = new savedAchItem();
                item.BankABA = data.data[counter].BankABA;
                item.BankAccountName = data.data[counter].BankHolder;
                item.BankAcctType = data.data[counter].BankAcctType == "P" ? "Checking" : "Savings";
                item.BankHolder = data.data[counter].CreatedBy;
                item.BankName = data.data[counter].BankName;
                item.BankAcctNo = data.data[counter].BankAcctNo;
                item.achEmail = data.data[counter].Email;
                item.PrimaryNumber = data.data[counter].PrimaryNumber;
                item.BankAcctNoView = "XXXXX-" + data.data[counter].BankAcctNo.substr(-4);
                item.ID = data.data[counter].ID;
                item.counter = counter;
                if (counter === data.data.length - 1)
                    item.Selected = true;
                savedAchItems.push(item);
            }
            $scope.savedAch = savedAchItems;

            if (savedAchItems) {
                if (savedAchItems.length > 0) {
                    $scope.savedACHBlank = false;
                    $scope.savedACH = true;
                }
                else {                    
                    $scope.savedACH = false;
                    $scope.savedACHBlank = true;
                }
            }
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

    $scope.DeleteACH = function () {
        manageSavedPaymentDetailsService.DeleteSavedAch($scope.ACHId,$scope.AccountNumber, $scope.baseAddress).then(function (data) {
            $('#ACHConfirmModal .close').click();
            BindSavedAchDetails();
        }, function (error) {
            if (error == "") {
                $scope.Error = "Server connection problem";
            }
            else {
                $scope.Error = error.Message;;
            }
        });
    };
    $scope.ACHDeleteConfirm = function (data) {
        $scope.ACHId = data.ID;
        ACHDeletePopUp();
    };

    function ACHDeletePopUp() {
        $('#ACHConfirmModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    };


}]);