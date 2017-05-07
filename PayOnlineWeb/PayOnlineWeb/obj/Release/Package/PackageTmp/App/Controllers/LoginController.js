app.controller('LoginController', ['$scope', '$rootScope', '$location', 'LoginFactory', 'WebApiService', '$remember', function ($scope, $rootScope, $location, LoginFactory, WebApiService,$remember) {
    $scope.onKeyPressResult = 0;
    $rootScope.PaymentBlock = true;
    $rootScope.ACHBlock = "";
    $scope.remember ={value: false};
    $scope.loading = false;
    $scope.new = { collection: {} };
    (function () {
        if ($rootScope.loggedIn == true)
        {
            $rootScope.loggedIn = false;            
        }
        getRemember();
    })();
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.login = function () {
        $scope.loading = true;
        $scope.btndisabled = true;
        $scope.model = '';
        LoginFactory.loginUser($scope.new.collection, $scope.baseAddress).then(function (data) {
            $scope.btndisabled = false;
            var accountNumber = $scope.new.collection.AccountNumber;
            var email = $scope.new.collection.Email;
            if (Boolean(data.IsSuccess)) {
                $rootScope.hideMenu = "";
                $rootScope.leftMenu = true;
                $rootScope.userProfile = true;
                $rootScope.loggedIn = true;
                sessionStorage.setItem('ACNCheck', data.ACNStatus);
                sessionStorage.setItem('AccountNumber', accountNumber);
                sessionStorage.setItem('Email', email);
                setRememberMe();
                CheckNSFStatus(accountNumber);
                if (Boolean(data.IsBitReset)) $location.path('/TemporaryPassword');
                else $location.path('/LoanPayment');
            }
            else {
                $scope.model = data.Message;
            }
            $scope.loading = false;
        },
         function (err) {
             if (err.Message == "The request is invalid") {
                 $scope.btndisabled = false;
                 $scope.model = err.Message;
             }
             else {
                 $scope.btndisabled = false;
                 $scope.model = "Server connection problem";
             }
             $scope.loading = false;
         });
    };

    $scope.RemoveError = function ()
    {
        $scope.model = "";
    }

    $scope.Authenticate = function () {
        $location.path('/Authenticate');       
    };

    $scope.ForgotPassword = function () {
        $location.path('/ForgotPassword');
    };

    $scope.ResetPassword = function () {
        $location.path('/ResetPassword');
    };

    $scope.Cancel = function ()
    {   
        if ($scope.onKeyPressResult == 13)
        {
            $scope.onKeyPressResult = 0;
            return;
        }
        else
        {
            $scope.new.collection = {};
            $scope.new.collection.Email = '';
        }
    }

    $scope.onKeyPressResult = "";
    $scope.onKeyPress = function ($event)
    {
        if ($event.keyCode == 13)
        {           
            $scope.onKeyPressResult = $event.keyCode;
            $scope.login();
        }
    };

    function getRemember() {
        var accountNumber = $remember('AccountNumber');
        var email = $remember('Email');
        var rememberMe = $remember('RememberMe');
        if (accountNumber && email && rememberMe) {
            $scope.remember.value = rememberMe;
            $scope.new.collection.AccountNumber = accountNumber;
            $scope.new.collection.Email = email;
        }
    };

    function setRememberMe()
    {
        if ($scope.remember.value) {
            $remember('AccountNumber', { value: $scope.new.collection.AccountNumber, expires:1, session:false });
            $remember('Email', { value: $scope.new.collection.Email, expires: 1, session: false });
            $remember('RememberMe', { value: $scope.remember.value, expires: 1, session: false });
        }
        else
        {
            $remember('AccountNumber', '');
            $remember('Email', '');
            $remember('RememberMe', '');
        }
    };

    function CheckNSFStatus(accountNumber) {
        LoginFactory.CheckNSFStatus(accountNumber, $scope.baseAddress).then(function (data) {
            if (JSON.parse(data.data) == "Blocked") {
                $rootScope.ACHBlock = "Blocked";
            }
            else if (JSON.parse(data.data) == "ABlocked") {
                $rootScope.PaymentBlock = false;
            }
        }, function (error) {
            if (error == "") {
                $scope.model = "Server connection problem";
            }
            else {
                $scope.model = error.Message;;
            }
        });
    };
}]);
