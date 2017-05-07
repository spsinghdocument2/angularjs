app.controller('AuthenticateController', ['$scope', 'AuthenticateFactory', 'WebApiService', '$location', '$rootScope', function ($scope, AuthenticateFactory, WebApiService, $location, $rootScope) {
    $scope.new2 =
        {
            verify: {}
        };
    $scope.AccountNumber = "";
    $scope.AccountHolder = "";

    $scope.baseAddress = WebApiService.baseAddress;

    $scope.Step1Class = "active";
    $scope.Step2Class = "disabled";
    $scope.Step3Class = "disabled";

    $scope.tabpane1Class = "tab-pane active";
    $scope.tabpane2Class = "tab-pane disabled";
    $scope.tabpane3Class = "tab-pane disabled";
    $scope.previous = function ()
    {
       
        $scope.Step1Class = "active";
        $scope.Step2Class = "disabled";
        $scope.Step3Class = "disabled";

        $scope.tabpane1Class = "tab-pane active";
        $scope.tabpane2Class = "tab-pane disabled";
        $scope.tabpane3Class = "tab-pane disabled";
    }
    $scope.checkboxModel = {
        value1: false
       
    };
    $scope.checkboxModel2 = {
        value1: false

    };
    $scope.chkBoxVerify2 =function()
    {
        var checkdSteap1 = $scope.checkboxModel.value1;
       
        if (checkdSteap1 == true) {

            $scope.checkBox2 = "";
            
        }

    }

    $scope.step2Verify = function()
    {
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDisble2 = true;
        var checkdSteap1 = $scope.checkboxModel.value1;
        
        if (checkdSteap1 == false) {
            $scope.Loadname = "";
            $scope.btnDisble2 = false;
            $scope.checkBox2 = "Make sure you check the confirmation box.";
            return;
        }

        $scope.Step1Class = "disabled";
        $scope.Step2Class = "disabled";
        $scope.Step3Class = "active";

        $scope.tabpane1Class = "tab-pane disabled";
        $scope.tabpane2Class = "tab-pane disabled";
        $scope.tabpane3Class = "tab-pane active";
        $scope.Loadname = "";
        $scope.btnDisble2 = false;
    }

    $scope.step3Verify = function()
    {
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDisble3 = true;
        var checkdSteap1 = $scope.checkboxModel2.value1;

        if (checkdSteap1 == false) {
            $scope.Loadname = "";
            $scope.btnDisble3 = false;
            $scope.checkBox3 = "Make sure you check the confirmation box.";
            return;
        }
        else {
            var fullName = $scope.AccountHolder;
            var mafAccountNumber = $scope.AccountNumber;
            sessionStorage.setItem('FullName', fullName);
            sessionStorage.setItem('MafAccountNumber', mafAccountNumber);
            $rootScope.CreateAccount = true;
            $scope.Loadname = "";
            $scope.btnDisble3 = false;
            $location.path('/CreateAccount');
        }

    }

    $scope.chkboxverify3 = function () {
     
        var checkdSteap1 = $scope.checkboxModel2.value1;

        if (checkdSteap1 == true) {

            $scope.checkBox3 = "";

        }

    }
    $scope.cancelStep3 = function()
    {
        $scope.Step1Class = "active";
        $scope.Step2Class = "disabled";
        $scope.Step3Class = "disabled";

        $scope.tabpane1Class = "tab-pane active";
        $scope.tabpane2Class = "tab-pane disabled";
        $scope.tabpane3Class = "tab-pane disabled";
        $scope.new2.verify = angular.copy(copyData);
        $scope.form.$setPristine();
        $scope.checkboxModel.value1 = false;
        $scope.checkboxModel2.value1 = false;
    }
    $scope.step2previous = function () {
        $scope.Loadname = "";
        $scope.Step1Class = "disabled";
        $scope.Step2Class = "active";
        $scope.Step3Class = "disabled";

        $scope.tabpane1Class = "tab-pane disabled";
        $scope.tabpane2Class = "tab-pane active";
        $scope.tabpane3Class = "tab-pane disabled";

    }
    var copyData = angular.copy($scope.new2.verify);

    $scope.Steap1Cancel = function ()
    {
        $scope.new2.verify = angular.copy(copyData);
        $scope.form.$setPristine();

       // $('#verifyForm')[0].reset();
        $scope.Loadname = "";
        $scope.btnDisble = false;
        //$scope.new2.verify = {};
    }

    $scope.verify = function () {
        $scope.Loadname = "glyphicon-refresh";

        $scope.btnDisble = true;

        AuthenticateFactory.VerifyUser($scope.new2.verify, $scope.baseAddress).then(function (data) {
            $scope.Loadname = "";
            $scope.btnDisble = false;
            $scope.Error = data.Error;
           // $scope.new2.verify = {};
            if (data.Error == null) {
                $scope.Step1Class = "disabled";
                $scope.Step2Class = "active";
                $scope.Step3Class = "disabled";

                $scope.tabpane1Class = "tab-pane disabled";
                $scope.tabpane2Class = "tab-pane active";
                $scope.tabpane3Class = "tab-pane disabled";
                verifed(data);

            }
        },
        function (err)
        {
            if (err == "")

            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.Error = "Server connection problem";

            }
            else
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.Error = err.Message;
            }

            

        });
            
        };
         function verifed(data)
         {
             $scope.AccountNumber = data.AccountNumber;
             $scope.AccountHolder = data.AccountHolder;
             $scope.VehicleYear = data.VehicleYear;
             $scope.Make = data.Make;
             $scope.Model = data.Model;
             $scope.MakeandModel = $scope.Make + " " + $scope.Model;

         }
         $scope.login = function()
         {
             $location.path('/login');
         }
         $scope.MafLogin = function () {
             $location.path('/login');
         }

}]);


