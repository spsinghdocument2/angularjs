app.controller('CardPaymentConfirmationController', ['$scope', '$rootScope', '$location', '$window', 'PaymentConfirmationService', 'WebApiService', function ($scope, $rootScope, $location, $window, PaymentConfirmationService, WebApiService) {
    $scope.AccountNumber = '';
    $scope.TokenId = '';
    $scope.FirstName = '';
    $scope.LastName = '';
    $scope.CardType = '';
    $scope.CardNumber = '';
    $scope.ExpiryDate = '';
    $scope.ExpiryYear = '';
    $scope.CvNumber = '';
    $scope.CurrentBalance = '';
    $scope.AmountPastDue = '';
    $scope.RegularAmountDue = '';
    $scope.OtherAmount = '';
    $scope.TotalAmountDue = '';
    $scope.DueDate = '';
    $scope.PrimaryNumber = '';
    $scope.Email = '';
    $scope.Address = '';
    $scope.City = '';
    $scope.State = '';
    $scope.Zip = '';
    $scope.FuturePaymentDate = '';
    $scope.PaymentType = '';
    $scope.CardPaymentAmount = '';
    $scope.loading = true;
    $scope.ViewTotalamount = '';
    $scope.errortext = false;
    $scope.SaveFuture = '';
    $scope.ConfirmationNumberShow = false;
    $scope.firstDiv = true;
    $scope.secondDiv = false;
    $scope.ViewFee = "";
    $scope.PaymentTimeOption = "";
    $scope.FutureAmount = "";
    $scope.FutureOtherAmount = "";
    $scope.SelectedPaymentAmountType;
    var CardPaymentConfirmationJsonFormat = JSON.stringify({});

    $scope.baseAddress = WebApiService.baseAddress;

    (function () {
        var holderData = PaymentConfirmationService.PaymentConfirmationGet();

        if (holderData == "")
        {
            $rootScope.leftMenu = false;
            $rootScope.userProfile = false;
            $rootScope.hideMenu = "hide";
            $location.path('/login');
            window.location.reload();
        }        
        var data = JSON.parse(holderData);
        $scope.AccountNumber = data.AccountNumber;
        $scope.CardName = data.CardName;
        $scope.FirstName = data.FirstName;
        $scope.LastName = data.LastName;
        $scope.CardType = data.CardType;
        $scope.CardNumber = data.CardNumber;
        $scope.CardLastDigit = data.CardNumber.substr(-4);
        $scope.ExpiryDate = data.ExpiryDate;
        $scope.ExpiryYear = data.ExpiryYear;
        $scope.CvNumber = data.CvNumber;
        $scope.CurrentBalance = data.CurrentBalance;
        $scope.AmountPastDue = data.CurrentBalance;
        $scope.RegularAmountDue = data.RegularAmountDue;
        $scope.OtherAmount = data.OtherAmount;
        $scope.TotalAmountDue = data.TotalAmountDue;
        $scope.DueDate = data.DueDate;
        $scope.PrimaryNumber = data.PrimaryNumber.replace(/-/g, "");
        $scope.Email = data.Email;
        $scope.Address = data.Address;
        $scope.City = data.City;
        $scope.State = data.State;
        $scope.Zip = data.Zip;
        $scope.PaymentDate = data.PaymentDate;
        $scope.CardPaymentAmount = parseFloat(data.CardPaymentAmount).toFixed(2);
        $scope.FuturePaymentDate = data.FuturePaymentDate;
        $scope.PaymentType = data.PaymentType;
        if ($scope.FuturePaymentDate != "") { $scope.PaymentDate = $scope.FuturePaymentDate; }
        $scope.SaveFuture = data.SaveFuture;
        $scope.LastName = data.LastName;
        $scope.TokenId = data.TokenId;
        $scope.SavedCards = data.SavedCards;
        $scope.SelectedPaymentAmountType = data.SelectedPaymentAmountType;
        
        $scope.PaymentTimeOption = data.PaymentTimeOption;
        $scope.FutureAmount = data.FutureAmount;
        $scope.FutureOtherAmount = data.FutureOtherAmount;

        PaymentConfirmationService.PaymentConfirmationHolder("");

    })();

    (function () {
        PaymentConfirmationService.CardFeeAmountGet($scope.AccountNumber, $scope.baseAddress).success(function (data) {
            var Fee = parseFloat(JSON.parse(data));
            var CardPaymentAmount = parseFloat($scope.CardPaymentAmount);
            $scope.Fee = Fee;
            $scope.ViewFee =  JSON.parse(data);
            $scope.totalamount = parseFloat(CardPaymentAmount + Fee);
            $scope.ViewTotalamount = parseFloat(CardPaymentAmount + Fee).toFixed(2);
            $scope.loading = false;

        }).error(function (error) {
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }
        });

    })();

  
    $scope.MAFPayment=function()
    {
        $location.path('/PaymentAgreement');
    }

    $scope.Back = function()
    {
    
        CardPaymentConfirmationJsonFormat = JSON.stringify({
            BillingName: $scope.BillingName, PaymentDate: $scope.PaymentDate, CardPaymentAmount: $scope.CardPaymentAmount,
            AccountNumber: $scope.AccountNumber, FullName: $scope.FullName, CardType: $scope.CardType, CardNumber: $scope.CardNumber,
            ExpiryDate: $scope.ExpiryDate, ExpiryYear: $scope.ExpiryYear, FirstName: $scope.FirstName, LastName: $scope.LastName, CurrentBalance: $scope.CurrentBalance,
            AmountPastDue: $scope.AmountPastDue, RegularAmountDue: $scope.RegularAmountDue, OtherAmount: $scope.OtherAmount,
            TotalAmountDue: $scope.TotalAmountDue, CvNumber: $scope.CvNumber, Email: $scope.Email, DueDate: $scope.DueDate,
            PrimaryNumber: $scope.PrimaryNumber.replace(/-/g, ""), Address: $scope.Address, City: $scope.City, FuturePaymentDate: $scope.FuturePaymentDate,
            City: $scope.City, PaymentType: $scope.PaymentType, State: $scope.State, Zip: $scope.Zip, SavedCards: $scope.SavedCards, SelectedPaymentAmountType: $scope.SelectedPaymentAmountType, PaymentTimeOption: $scope.PaymentTimeOption,FutureAmount:$scope.FutureAmount, FutureOtherAmount: $scope.FutureOtherAmount
            });
        

        $scope.result = PaymentConfirmationService.PaymentConfirmationHolder(CardPaymentConfirmationJsonFormat);
        if ($scope.result == "Holder data successfully") {

            $rootScope.loggedIn = true;
            $location.path('/PaymentAmount');
        }
    }

    $scope.CardPayment=function()
    {
        $scope.loading = true;;
        $scope.btnSubmit = true;
        if($scope.TokenId=='')
        {
            CardPayment();
        }
        else
        {
            SavedCardPayment();
        }
    }

    function CardPayment()
    {
        $scope.loading = true;
        $scope.Loadname = "glyphicon-refresh";
        var CardType = $scope.CardType;
        var cardPaymentConfirmationdata = {
            "CardName": $scope.CardName, "Fee": parseFloat($scope.ViewFee).toFixed(2), 
            "FirstName": $scope.FirstName, "LastName": $scope.LastName, "Street": $scope.Address, "City": $scope.City, "State": $scope.State, "PostelCode": $scope.Zip,
            "Email": $scope.Email, "CardNumber": $scope.CardNumber, "ExpirationMonth": $scope.ExpiryDate, "ExpirationYear": $scope.ExpiryYear, "FuturePaymentDate": $scope.FuturePaymentDate,
            "CardType": CardType, "CvNumber": $scope.CvNumber, "Amount": parseFloat($scope.ViewTotalamount).toFixed(2), "AccountNumber": $scope.AccountNumber, "PrimaryNumber": $scope.PrimaryNumber.replace(/-/g, ""), "Date": $scope.PaymentDate, "SaveFuture": $scope.SaveFuture
        };


        PaymentConfirmationService.CardPaymentConfirm(cardPaymentConfirmationdata, $scope.baseAddress).success(function (data) {
            $scope.btnSubmit = false;
            $scope.Loadname = '';
            var result = JSON.parse(data);
            $scope.loading = false;
            if (result.length == 10) {
                $scope.confirmation = JSON.parse(data);
                $scope.errortext = false;
                $scope.ConfirmationNumberShow = true;
                $scope.firstDiv = false;
                $scope.secondDiv = true;
                CardPaymentConfirmationAlert();
            }
            else {
                $scope.firstDiv = true;
                $scope.secondDiv = false;
                $scope.errortext = true;
                $scope.ConfirmationNumberShow = false;
                $scope.error = JSON.parse(data);
            }

        }).error(function (error) {
            $scope.Loadname = '';
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;;
            }


        });
    }

    function SavedCardPayment()
    {
        $scope.loading = true;
        $scope.Loadname = "glyphicon-refresh";
        var CardType = $scope.CardType;
        var cardPaymentConfirmationdata = {
            "CardName": $scope.CardName, "TokenId": $scope.TokenId, "Fee": parseFloat($scope.ViewFee).toFixed(2),
            "FirstName": $scope.FirstName, "LastName": $scope.LastName, "Street": $scope.Address, "City": $scope.City, "State": $scope.State, "PostelCode": $scope.Zip, 
            "Email": $scope.Email, "CardNumber": $scope.CardNumber, "ExpirationMonth": $scope.ExpiryDate, "ExpirationYear": $scope.ExpiryYear, "FuturePaymentDate": $scope.FuturePaymentDate,
            "CardType": CardType, "CvNumber": $scope.CvNumber, "Amount": parseFloat($scope.ViewTotalamount).toFixed(2), "AccountNumber": $scope.AccountNumber, "PrimaryNumber": $scope.PrimaryNumber.replace(/-/g, ""), "Date": $scope.PaymentDate, "SaveFuture": $scope.SaveFuture
        };


        PaymentConfirmationService.SavedCardPaymentConfirm(cardPaymentConfirmationdata, $scope.baseAddress).success(function (data) {
            $scope.btnSubmit = false;
            $scope.Loadname = '';
            var result = JSON.parse(data);
            $scope.loading = false;
            if (result.length == 10) {
                $scope.confirmation = JSON.parse(data);
                $scope.errortext = false;
                $scope.ConfirmationNumberShow = true;
                $scope.firstDiv = false;
                $scope.secondDiv = true;
                CardPaymentConfirmationAlert();
            }
            else {
                $scope.firstDiv = true;
                $scope.secondDiv = false;
                $scope.errortext = true;
                $scope.ConfirmationNumberShow = false;
                $scope.error = JSON.parse(data);
            }

        }).error(function (error) {
            $scope.Loadname = '';
            if (error == "") {

                $scope.Error = "Server connection problem";
            }
            else {

                $scope.Error = error.Message;
            }


        });
    }


    function CardPaymentConfirmationAlert() {
        $('#ConfirmationPaymentModal').modal({

            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    }

    $scope.AutoPay = function () {
        $location.path('/AutoPay');
    };
    $scope.Finish = function () {
        $location.path('/LoanPayment');
    };

    $scope.print = function () {
        var paymentTransactionDataCollection = JSON.stringify({
            accountNumber: $scope.AccountNumber,
            paymentDate: $scope.PaymentDate,
            paymentAmount: parseFloat($scope.CardPaymentAmount).toFixed(2),
            fee: $scope.ViewFee,
            totalAmount: parseFloat($scope.ViewTotalamount).toFixed(2),
            cardName: $scope.CardName,
            fullName: $scope.FirstName + ' ' + $scope.LastName,
            cardType:$scope.CardType  ,
            cardNumber:$scope.CardNumber,
            expiryDate: $scope.ExpiryDate,
            expiryYear: $scope.ExpiryYear,
            address:$scope.Address,
            city:$scope.City,
            state:$scope.State,
            zip: $scope.Zip,
            confirmationNumber: $scope.confirmation
        });

        localStorage.paymentTransactionData = paymentTransactionDataCollection;
        $window.open('/PrintConfirmation', '_blank');
    }

    this.setFuturePayWithOutAgreement = function () {
        futurePayService.setFuturePayWithAgreement(false);
    };
}]);