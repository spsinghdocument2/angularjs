
app.controller('ChangePasswordController', ['$scope', 'ChangePasswordFactory', 'WebApiService', '$location', '$rootScope', function ($scope, ChangePasswordFactory, WebApiService, $location, $rootScope) {
    $scope.AccountNumber = '';
    $scope.Email = '';
    $scope.FullName = '';
    $scope.Password = '';
    // $scope.NewPassword = '';
    $scope.OldPassword = '';
    $scope.currentPasswordDisabled = false;
    $scope.newPasswordDisabled = false;
    $scope.confirmNewPasswordDisabled = false;
    $scope.editButtionVisible = false;
    $scope.saveButtionVisible = true;
    $scope.resetButtionVisible = true;

    $scope.baseAddress = WebApiService.baseAddress;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    $scope.changePasswordVerify = function ()
    {
        $scope.loading = true;
        var AccountNumber = $scope.AccountNumber;
        var Password = $scope.Currentpassword;
        $scope.form.$invalid = true;
        var dataHolder = { "AccountNumber": AccountNumber, "Password": Password }
        $scope.Loadname = "glyphicon-refresh";
 
        ChangePasswordFactory.VerifyUser(dataHolder, $scope.baseAddress).success(function (data) {
            $scope.loading = false;
            $scope.Loadname = "";
            $scope.Error = data.Error;

            allData(data)
        }).error(function (data)
        {
        
            $scope.loading = false;
            if (data.Message != null)
            {
                $scope.Error = data.Message;
                $scope.Loadname = "";
                return;
            }
            else
            {
                $scope.Loadname = "";
                $scope.Error = "Server connection problem ";
            }
            $scope.form.$invalid = false;
            $scope.Loadname = "";
           
        });
    };

    function sendMail()
    {
        $scope.loading = true;
        $scope.Loadname = "glyphicon-refresh";
        var AccountNumber = $scope.AccountNumber;
        var Email = $scope.Email;
        var FullName = $scope.FullName;
        var NewPassword = $scope.NewPassword;
        var BitReset = "0";

        var SendMailCollection = { "AccountNumber": AccountNumber, "Email": Email, "FullName": FullName, "NewPassword": NewPassword, 'BitReset': BitReset };

        $scope.Loadname = "glyphicon-refresh";
         ChangePasswordFactory.continueUser(SendMailCollection, $scope.baseAddress).success(function (data) {
             $scope.form.$invalid = false;
             $scope.Loadname = "";
             $scope.loading = false;
            var Getdata = JSON.parse(data);
            if (Getdata == "mail send successfully")
            {
                $scope.currentPasswordDisabled = true;
                $scope.newPasswordDisabled = true;
                $scope.confirmNewPasswordDisabled = true;
                $scope.updateMessage = "Your password has been changed and send to your e-mail address.";
                $scope.editButtionVisible = true;
                $scope.saveButtionVisible = false;
                $scope.resetButtionVisible = false;
            }

         }).error(function (data) {
             $scope.loading = false;
            $scope.Loadname = "";
            $scope.Loadname = "";
            $scope.form.$invalid = false;
            if (data != "") {
                $scope.Error = data.Message;
            }
            else
            {
                $scope.Error = "Server connection problem ";
            }
        });


    };


    function allData(data) {
       
        if (data.Error != null)
        {
            $scope.Error = data.Error;
            $scope.form.$invalid = false;
            return;
        }


        else if (data.Password != $scope.Currentpassword)
        {
            $scope.Error = "Current Password didn't match";
            $scope.form.$invalid = false;
            return;
        }
        else
        {


            $scope.AccountNumber = data.AccountNumber;
            $scope.Email = data.Email;
            $scope.FullName = data.FullName;
            $scope.Password = data.Password;
            sendMail();

        }
    }
    var copyCurrentpassword = angular.copy($scope.Currentpassword);
    var copyNewPassword = angular.copy($scope.NewPassword);
    var copyConfirmNewPassword = angular.copy($scope.ConfirmNewPassword);

    $scope.Reset = function ()
    {
        //$scope.Currentpassword = angular.copy(copyCurrentpassword);
        //$scope.NewPassword = angular.copy(copyNewPassword);
        //$scope.ConfirmNewPassword = angular.copy(copyConfirmNewPassword);
        $scope.Currentpassword = '';
        $scope.NewPassword = '';
        $scope.ConfirmNewPassword = '';
        $scope.form.$setPristine();

       // $scope.Currentpassword = '';
       // $scope.NewPassword = '';
      //  $scope.ConfirmNewPassword = '';
    }

    $scope.Edit = function ()
    {
        $scope.currentPasswordDisabled = false;
        $scope.newPasswordDisabled = false;
        $scope.confirmNewPasswordDisabled = false;
        $scope.updateMessage = '';
        $scope.editButtionVisible = false;
        $scope.saveButtionVisible = true;
        $scope.resetButtionVisible = true;
    };

    $scope.buttonOk = function()
    {
        var accountNumber = $scope.AccountNumber;
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $rootScope.hideMenu = "";
        sessionStorage.setItem('AccountNumber', accountNumber);
        $location.path('/LoanPayment');
       

    }

    $scope.ErrorRemove = function()
    {
        $scope.Error = "";
    }

}]);


