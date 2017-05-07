app.controller('RegisterController', ['$scope', '$rootScope', '$location', 'RegisterFactory', 'WebApiService', function ($scope, $rootScope, $location, RegisterFactory, WebApiService) {
    // $scope.FullName = sessionStorage.getItem('FullName');
    // $scope.MafAccountNumber = sessionStorage.getItem('MafAccountNumber');
    $scope.buttonReset = true;
    $scope.buttonSave = true;
    $scope.buttonok = false;
    $scope.ErrorMessage = '';
   
    $scope.Register =
        {
            collection: {}
        };
    function sesionGet() {
        $scope.Register.collection.AccountNumber = sessionStorage.getItem('MafAccountNumber');
        $scope.Register.collection.FullName = sessionStorage.getItem('FullName');
    }
    sesionGet();
    $scope.baseAddress = WebApiService.baseAddress;

    var oriPerson = angular.copy($scope.Register.collection);
    $scope.RegisterReset = function ()
    {


        $scope.Register.collection = angular.copy(oriPerson);
        $scope.form.$setPristine();

        $scope.Loadname = "";
        $scope.btnDisble = false;
        sesionGet();

    }

    $scope.save = function () {
      
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDisble = true;
        if ($scope.Register.collection.SecurityID == $scope.Register.collection.SecurityID2)
        {
            $scope.Loadname = "";
            $scope.btnDisble = false;
            $scope.QuestionError2 = "Security question match";
            return;
            
        }
        else if ($scope.Register.collection.SecurityID2 == $scope.Register.collection.SecurityID3)
        {
            $scope.Loadname = "";
            $scope.btnDisble = false;
            $scope.QuestionError3 = "Security question match";
            return;
        }
        else if ($scope.Register.collection.SecurityID3 == $scope.Register.collection.SecurityID)
        {
            $scope.Loadname = "";
            $scope.btnDisble = false;
             $scope.QuestionError3 = "Security question match";
            return;
        }

        else
        {
            RegisterFactory.SaveUser($scope.Register.collection, $scope.baseAddress).then(function (data)
            {
         
                $scope.Loadname = "";
                $scope.btnDisble = false;
                var obj = JSON.parse(data);
                $scope.data = obj;
                if ($scope.data == "Registration Successful")
                {
                 //   alert("Registration Successful");
                    //  $location.path('/login');
                    $scope.buttonReset = false;
                    $scope.buttonSave = false;
                    $scope.buttonok = true;
                    $scope.ErrorMessage = '';
                    $scope.message = "Registration Successful";
                    $scope.Register.collection = angular.copy(oriPerson);
                    $scope.form.$setPristine();
                    sesionGet();
                }
                if ($scope.data == "Make sure your account hasn't been registered already.")
                {
                    $scope.ErrorMessage = "Make sure your account hasn't been registered already.";
                }
               // $scope.Register.collection = {};
               // $scope.Password = '';
               
                sesionGet();

            }, function (err)
            {
               
                if (err == "")
                {
                    $scope.Loadname = "";
                    $scope.btnDisble = false;
                    $scope.ErrorMessage = "Server connection problem";
                }
                else
                {
                    $scope.Loadname = "";
                    $scope.btnDisble = false;
                    $scope.ErrorMessage = err.Message;;
                }
                

            });
        }
    };

    (function () {
      
        RegisterFactory.GetSecurityQuestions().then(function (data) {
            $scope.AllData = data.SecurityTable;
           
        }, function (err) { $scope.ErrorMessage = "Json Load problem"; })

})();
    $scope.MafLogin = function () {
        $location.path('/login');
    }
    $scope.ErrorQuestionMessageHideSecond = function ()
    {
  
        $scope.QuestionError2 = '';
    }
    $scope.ErrorQuestionMessageHideThird = function ()
    {
        $scope.QuestionError3 = '';
    }

}]);


