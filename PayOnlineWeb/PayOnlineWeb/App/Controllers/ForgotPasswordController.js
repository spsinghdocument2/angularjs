app.controller('ForgotPasswordController', ['$scope', '$rootScope', '$location', 'ForgotPasswordFactory', 'WebApiService', function ($scope, $rootScope, $location, ForgotPasswordFactory, WebApiService) {

    $scope.AccountNumber = '';
    $scope.Answer = '';
    $scope.Email = '';
    $scope.FullName = '';
    $scope.SecurityID = '';
    $scope.SecurityID2 = '';
    $scope.SecurityID3 = '';
    $scope.UsrAnswer = '';
    $scope.QuestionsID = '';
    $scope.QuestionsID2 = '';
    $scope.QuestionsID3 = '';
    $scope.QuestionsID4 = '';
    $scope.QuestionsID5 = '';
    $scope.Answer2 = '';
    $scope.Answer3 = '';
    $scope.Answer4 = '';
    $scope.Answer5 = '';

    $scope.Step1Class = "active circle_main2";
    $scope.Step2Class = "disabled circle_main2";
    $scope.Step3Class = "disabled";

    $scope.tabpane1Class = "tab-pane active";
    $scope.tabpane2Class = "tab-pane disabled";
    $scope.tabpane3Class = "tab-pane disabled";
    $scope.alertTemporaryPassword = "Temporary password will be sent to below email"

    $scope.buttonOk = false;
    $scope.levelEmail = true;
    $scope.textBoxEmail = true;
    $scope.buttonSend = true;
    $scope.btndone = true;
    $scope.btndoneOk = false;

    $scope.txtTwoAnswers = true;
    $scope.txtThreeAnswers = true;
    $scope.Forgot =
        {
            collection: {}
        };
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.reset = function ()
    {
        $scope.Forgot.collection = {};
    }
    $scope.verify = function ()
    {
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDisble = true;

        if (isNaN($scope.Forgot.collection.AccountNumber)) {
            $scope.Loadname = "";
            $scope.btnDisble = false;
            $scope.StepFirstError = "Must input numbers";
            return;
        }

        ForgotPasswordFactory.verifyUser($scope.Forgot.collection, $scope.baseAddress).success(function (data)
        {

            $scope.Loadname = "";
            $scope.btnDisble = false;
            if (data.Error != null) {
                $scope.Forgot.collection = {};
                $scope.Error2 = data.Error;
                return;
            }
            $scope.Forgot.collection = {};
            $scope.data = data.Error;
            allCllectionData(data);


        }).error(function (data)
        {
       
            if (data.Message == "The request is invalid")
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.StepFirstError = data.Message;
            }
            else
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.StepFirstError = "Server connection problem ";
            }

         
        });
    };
    function allCllectionData(data)
    {
        $scope.AccountNumber = data.AccountNumber;
        $scope.Answer = data.Answer;
        $scope.Email = data.Email;
        $scope.FullName = data.FullName;   

        var obj = JSON.parse(data.SecurityID == "" ? 0 : data.SecurityID);
        $scope.QuestionsID = obj;

    
        $scope.QuestionsID2 = JSON.parse(data.SecurityID2 == "" ? 0 : data.SecurityID2);
        $scope.QuestionsID3 = JSON.parse(data.SecurityID3 == "" ? 0 : data.SecurityID3);

        $scope.Answer2 = data.Answer2;
        $scope.Answer3 = data.Answer3;


        var qus = obj;
        if ($scope.QuestionsID2 == 0)
        {
            $scope.txtTwoAnswers = false;
            
        }
        if ($scope.QuestionsID3 == 0)
        {
            $scope.txtThreeAnswers = false;
        }
       
        ForgotPasswordFactory.GetSecurityQuestions().success(function (data) {
        
   
            var collection = data.SecurityTable;
            for (var rec in collection)
            {
                if (collection[rec].value == $scope.QuestionsID)
                {
                    $scope.SecurityID = collection[rec].SecurityQuestions;
                    
                }
                if (collection[rec].value == $scope.QuestionsID2)
                {
                    $scope.SecurityID2 = collection[rec].SecurityQuestions;

                }
                if (collection[rec].value == $scope.QuestionsID3)
                {
                    $scope.SecurityID3 = collection[rec].SecurityQuestions;

                }
                
            }
        }).error(function (data) {
           
            $scope.StepFirstError = "Json Load problem";

        });

        $scope.Step1Class = "disabled circle_main2";
        $scope.Step2Class = "active circle_main2";
        $scope.Step3Class = "disabled";

        $scope.tabpane1Class = "tab-pane disabled";
        $scope.tabpane2Class = "tab-pane active";
        $scope.tabpane3Class = "tab-pane disabled";

    }
    $scope.SendMailCollection = { collection: [] };

    $scope.verifyQuestion = function ()
    {
      
        $scope.Loadname = "glyphicon-refresh";
        $scope.btnDisble = true;


        var Answer = $scope.Answer;
        var Answer2 = $scope.Answer2;
        var Answer3 = $scope.Answer3;

        var QuestionsID = $scope.QuestionsID;
        var QuestionsID2 = $scope.QuestionsID2;
        var QuestionsID3 = $scope.QuestionsID3;

        var UserQuestionsId = $scope.UserQuestionsId;
        var UsrAnswer = $scope.UsrAnswer;

        if(QuestionsID == UserQuestionsId)
        {
            if(Answer == UsrAnswer)
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.wrongpassword = "";
              
                popTemporaryPassword();
                //$scope.Step1Class = "disabled circle_main2";
                //$scope.Step2Class = "disabled circle_main2";
                //$scope.Step3Class = "active";

                //$scope.tabpane1Class = "tab-pane disabled";
                //$scope.tabpane2Class = "tab-pane disabled";
                //$scope.tabpane3Class = "tab-pane active";
            }
            else
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.UsrAnswers3 = '';
                $scope.wrongpassword = "Your answer doesn't match our records";
                return
            }
        }
      
        else if(QuestionsID2 == UserQuestionsId)
        {
            if (Answer2 == UsrAnswer) {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.wrongpassword = "";
                popTemporaryPassword();
                //$scope.Step1Class = "disabled circle_main2";
                //$scope.Step2Class = "disabled circle_main2";
                //$scope.Step3Class = "active";

                //$scope.tabpane1Class = "tab-pane disabled";
                //$scope.tabpane2Class = "tab-pane disabled";
                //$scope.tabpane3Class = "tab-pane active";
            }
            else
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.UsrAnswers3 = '';
                $scope.wrongpassword = "Your answer doesn't match our records";
                return
            }
        }
        else if(QuestionsID3 == UserQuestionsId)
        {
            if (Answer3 == UsrAnswer) {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.wrongpassword = "";
                popTemporaryPassword();
                //$scope.Step1Class = "disabled circle_main2";
                //$scope.Step2Class = "disabled circle_main2";
                //$scope.Step3Class = "active";

                //$scope.tabpane1Class = "tab-pane disabled";
                //$scope.tabpane2Class = "tab-pane disabled";
                //$scope.tabpane3Class = "tab-pane active";
            }
            else
            {
                $scope.Loadname = "";
                $scope.btnDisble = false;
                $scope.UsrAnswers3 = '';
                $scope.wrongpassword = "Your answer doesn't match our records";
                return
            }
        }
       
        else
        {
            $scope.Loadname = "";
            $scope.btnDisble = false;
            $scope.UsrAnswers3 = '';
            $scope.wrongpassword = "Your answer doesn't match our records";
            return
        }
    }

    function popTemporaryPassword() {
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: false  // to prevent closing with Esc button (if you want this too)
        })
    };

    $scope.MafLogin = function () {
    
        $location.path('/login');
        window.location.reload();

    };
    $scope.CancelFirst = function () {

        $scope.Forgot.collection = {};
        $scope.Loadname = "";
        $scope.btnDisble = false;
    }
    $scope.CancelSecond = function () {
       
        $scope.UsrAnswer = '';
        $scope.UsrAnswers2 = '';
        $scope.UsrAnswers3 = '';
        $scope.Loadname = "";
        $scope.btnDisble = false;
    };

    (function () {


        ForgotPasswordFactory.GetSecurityQuestions().success(function (data) {
            $scope.AllData = data.SecurityTable;


        }).error(function (data) {

            $scope.Error2 = "Json Load problem";
        });

    })();


    $scope.UsrAnswersErrorMessageHideFirst = function () {

        $scope.wrongpassword = '';
    };
    $scope.UsrAnswersErrorMessageHideSecond = function () {
        $scope.wrongpassword2 = '';
    };
    $scope.UsrAnswersErrorMessageHideThird = function () {
        $scope.wrongpassword3 = '';
    };
    $scope.AccountNumberErrorMessageHide = function () {

        $scope.StepFirstError = '';
        $scope.Error2 = '';
    };

    $scope.verifyEmail = function()
    {

            var AccountNumber = $scope.AccountNumber;        
            var Email = $scope.Email;
            var FullName = $scope.FullName;

            var SendMailCollection = { "AccountNumber": AccountNumber, "FullName": FullName, "Email": Email };
            $scope.Loadname1 = "glyphicon-refresh";
            $scope.disableSendMail = true;
            ForgotPasswordFactory.temporaryPasswordSendMailUser(SendMailCollection, $scope.baseAddress).success(function (data) {
                
                $scope.Loadname1 = "";
                $scope.disableSendMail = false;
                $scope.btnDisble = false;
                var obj = JSON.parse(data);
                 $scope.data = obj;
                 if (obj == "mail send successfully")
                 {

                     $scope.buttonOk = true;
                     $scope.levelEmail = false;
                     $scope.textBoxEmail = false;
                     $scope.buttonSend = false;
                     $scope.alertTemporaryPassword = "Temporary password sent"
                     $scope.message = "Email has been Sent.When you receive your sign in information,follow the directions in the email to reset your temporary password."
                }
                else
                {
                     $scope.temporaryError = obj;
                }

            }).error(function (data) {

                if (data.Message == "The request is invalid") {
                    $scope.Loadname1 = "";
                    $scope.disableSendMail = false;
              
                    $scope.temporaryError = data.Message;
                }
                else {
                 
                    $scope.Loadname1 = "";
                    $scope.disableSendMail = false;
                    $scope.temporaryError = "Server connection problem ";
                }

            });




     //   }
      //  else
      //  {
       //     $scope.temporaryError ="Your email didn't match ";
     //   }
        

    }

    $scope.removeTemporaryError = function()
    {
        $scope.temporaryError = " ";
    }

    $scope.removeTemporaryReset = function ()
    {
      
        $scope.temporaryError = " ";
        $scope.TemporaryEmailId = "";
    }
}]);


