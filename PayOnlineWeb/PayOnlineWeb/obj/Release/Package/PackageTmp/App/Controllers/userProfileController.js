
app.controller('userProfileController', ['$scope', '$rootScope', '$location', '$http', 'userProfileFactory', 'textNotificationService', 'WebApiService', function ($scope, $rootScope, $location, $http, userProfileFactory,textNotificationService, WebApiService) {
    $scope.Qstn1 = false;
    $scope.Qstn2 = false;
    $scope.Qstn3 = false;
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.AccountNumber = sessionStorage.getItem('AccountNumber');
    updateQuestionsJsionGet();
    updateSecurityAnswerGet();

    $scope.GetSecurityQuestionsData = {};
   
    $scope.SecurityID = '';
    $scope.SecurityID2 = '';
    $scope.SecurityID3 = '';

    $scope.disableTextNumber = false;
    $scope.disableTextNumber2 = false;
    $scope.loading = true;
    $scope.EditPanel = false;
    $scope.MainPanel = true;
    $scope.PrimaryNumberVisible = false;
    $scope.TextNumberVisible = false;
    $scope.TextNumber2Visible = false;
    $scope.AddressVisible = false;
  
    function bindUserProfile(){
        userProfileFactory.UserInfo($scope.AccountNumber, $scope.baseAddress).then(function (data) {
            $scope.FullName = data.FullName;
            $scope.Email = data.Email;
            $scope.AccountNumber = data.AccountNumber;
            $scope.Year = data.Year;
            $scope.Model = data.Model;
            $scope.DaysPastDue = data.DaysPastDue;
            $scope.PastDueAmount = '$' + data.PastDueAmount;
            $scope.LastPaymentAmount = '$' + data.LastPaymentAmount;
         
            $scope.securityIdFirst = data.SecurityID;
            $scope.securityIdSecond = data.SecurityID2;
            $scope.SecurityIdThird = data.SecurityID3
            $scope.Answer = data.Answer;
            $scope.Answer2 = data.Answer2;
            $scope.Answer3 = data.Answer3;
            $scope.Make = data.Make;
            $scope.PaymentFrequency = data.PaymentFrequency == "W" ? "Weekly" : data.PaymentFrequency == "M" ? "Monthly" : data.PaymentFrequency == "Y" ? "Yearly" : data.PaymentFrequency == "B" ? "Bi-Monthly" : data.PaymentFrequency == "S" ? "Semi-Monthly" : data.PaymentFrequency == "D" ? "Daily" : data.PaymentFrequency;
            if (data.CellNumber != null) { $scope.CellNumber = data.CellNumber.substr(0, 12); $scope.PrimaryNumberVisible = true; }
            if (data.TextNumber != null && data.TextNumber != '') { $scope.TextNumber = data.TextNumber.substr(0, 12); $scope.TextNumberVisible = true; }
            if (data.TextNumber2 != null && data.TextNumber2 != '') { $scope.TextNumber2 = data.TextNumber2.substr(0, 12); $scope.TextNumber2Visible = true; }
            if (data.Address1 != null) { $scope.Address1 = data.Address1; $scope.AddressVisible = true; }            
            $scope.Address2 = data.Address2;
            $scope.City = data.City;
            $scope.State = data.State;
            $scope.ZipCode = data.Zip;                               
            bindStateList(data.State);
            var collection = $scope.GetSecurityQuestionsData;
            for (var rec in collection) {
                if (collection[rec].value == $scope.securityIdFirst) {
                    $scope.SecurityIdFirst = collection[rec].SecurityQuestions;

                    $scope.Qstn1 = true;
                }
                if (collection[rec].value == $scope.securityIdSecond) {
                    $scope.SecurityIdSecond = collection[rec].SecurityQuestions;

                    $scope.Qstn2 = true;

                }
                if (collection[rec].value == $scope.SecurityIdThird) {
                    $scope.SecurityIdThird = collection[rec].SecurityQuestions;

                    $scope.Qstn3 = true;
                }
            }

            //Check opt-in for text numbers
            textNotificationService.getUserDetails($scope.AccountNumber, $scope.baseAddress).success(function (data) {
                if (data.length > 0) {
                    for (var counter = 0; counter < data.length; counter++) {
                        if (data[counter].TextNumber === $scope.TextNumber)
                            $scope.disableTextNumber = true;
                        else if (data[counter].TextNumber === $scope.TextNumber2)
                            $scope.disableTextNumber2 = true;
                    }
                }
                $scope.loading = false;
            }).error(function (data) {
                $scope.model = data.replace(/"/g, "");
                $scope.loading = false;
            });
        },
           function (err) {
               $scope.loading = false;
               $scope.model = "Get User Profile Failed!";
     });
    }

    (function () {
        bindUserProfile();
    })();

    $scope.toggleAccordian = function () {
        document.querySelector('#collapseOne').classList.toggle('in');
        document.querySelector('#accordianAnchor').classList.toggle('collapsed');
    };

    var bindStateList = function (selectedState)
    {
        userProfileFactory.StateList().then(function (data) {
            var states = data.States;
            var selectedOption = states.find(function (state) { return state.Value === selectedState; });
            $scope.StateList = states;
            $scope.selectedState = selectedOption;
        },
          function (err) {
              $scope.ErrorMessage = "Get User Profile Failed!";
          });
    }

    function updateSecurityAnswerGet() {
   
       var AccountNumber = { "AccountNumber": $scope.AccountNumber }
        userProfileFactory.SecurityAnswer(AccountNumber, $scope.baseAddress).then(function (data) {
           $scope.ErrorMessage = "";
           BindDropDown(data);
       },
          function (error) {
            
         
              if (error == "") {
               
                 $scope.ErrorMessage = "Server connection problem";
             }
             else {
              
                 $scope.ErrorMessage = error.Message;;
             }
             
         });
    };

    function updateQuestionsJsionGet() {

     
        userProfileFactory.SecurityQuestion().then(function (data) {
        
         $scope.GetSecurityQuestionsData = data.SecurityTable;
     },
          function (err) {
        
              $scope.ErrorMessage = "Get User Profile Failed!";
       });
 };

    $scope.uploadImage = function (files) {
        var ext = files[0].name.match(/\.(.+)$/)[1];
        if (angular.lowercase(ext) === 'jpg' || angular.lowercase(ext) === 'jpeg' || angular.lowercase(ext) === 'png') {
            alert("Valid File Format");
        }
        else {
            alert("Invalid File Format");
        }
    }
  
    $scope.EditDetails=function()
    {
        $scope.loading = true;
        $scope.EditPanel = true;
        $scope.MainPanel = false;
        $scope.loading = false;
    }

    function closeEditing() {
        bindUserProfile();
        $scope.loading = true;
        $scope.EditPanel = false;
        $scope.MainPanel = true;
        $scope.loading = false;
    };

    $scope.CancelDetails = function () {
        closeEditing();
    };


    $scope.submit = function ()
    {
        //validate state
        if ($scope.selectedState == "" || $scope.selectedState == undefined) {
            $scope.ErrorState = "Please select State."
            return;
        }
        var AccountNumbers = $scope.AccountNumber;
        var CellNumber = $scope.CellNumber.replace(/-/g, "");
        var TextNumber = $scope.TextNumber ? $scope.TextNumber.replace(/-/g, "") :$scope.TextNumber;
        var TextNumber2 = $scope.TextNumber2 != '' && $scope.TextNumber2 != undefined ? $scope.TextNumber2.replace(/-/g, "") : $scope.TextNumber2;
        if (!TextNumber || (TextNumber && TextNumber.length == 10)) {
            if (!TextNumber2 || (TextNumber2 && TextNumber2.length == 10)) {
                $scope.loading = true;

                var Address1 = $scope.Address1;
                var Email = $scope.Email;
               var Image = dataURItoBlob($scope.ImgFile);
                //console.log(dataURItoBlob($scope.ImgFile));
                //console.log($scope.ImgFile)

                var UserConfirmationdata = { "AccountNumber": AccountNumbers, "CellNumber": CellNumber, "TextNumber": TextNumber, "TextNumber2": TextNumber2, "Address1": Address1, "Image": $scope.ImgFile, "Email": Email, "Image": "", "Address2": $scope.Address2, "City": $scope.City, "State": $scope.selectedState.Value, "Zip": $scope.ZipCode };

                userProfileFactory.UserInfoConfirm({ model: UserConfirmationdata, files: dataURItoBlob($scope.ImgFile) }, $scope.baseAddress).success(function (data) {
                    $scope.confirmation = "Submit Successfully";
                    $scope.loading = false;
                    closeEditing();
                }).error(function (data) {
                    $scope.confirmation = data.replace(/"/g, "");
                    $scope.loading = false;
                });
            }
            else {
                $scope.confirmation = "Please enter valid secondary text number!";
            }
        }
        else {
            $scope.confirmation = "Please enter valid primary text number!";
        }

    }


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
    }

 function BindDropDown(data)
 {
   
     $scope.SecurityID = data.SecurityID == "0" ? "" : data.SecurityID ==null ? "" :  data.SecurityID == "" ? "" :  data.SecurityID;
     $scope.SecurityID2 = data.SecurityID2 == "0" ? "" : data.SecurityID2 == null ? "" : data.SecurityID2 == "" ? "" : data.SecurityID2;
     $scope.SecurityID3 = data.SecurityID3 == "0" ? "" : data.SecurityID3 == null ? "" : data.SecurityID3 == "" ? "" : data.SecurityID3;

        $scope.Answer = data.Answer;
        $scope.Answer2 = data.Answer2;
        $scope.Answer3 = data.Answer3;
    }

    $scope.updateQuestions = function () {

        if ($scope.SecurityID == $scope.SecurityID2) {
            $scope.QuestionError2 = "Security question match";
            return;
        }
        else if ($scope.SecurityID2 == $scope.SecurityID3 && $scope.SecurityID2 != '' && $scope.SecurityID2 != undefined) {
            $scope.QuestionError3 = "Security question match";
            return;
        }
        else if ($scope.SecurityID3 == $scope.SecurityID) {
            $scope.QuestionError3 = "Security question match";
            return;
        }
       
        var AccountNumber = $scope.AccountNumber;
        var SecurityID = $scope.SecurityID;
        var SecurityID2 = $scope.SecurityID2;
        var SecurityID3 = $scope.SecurityID3;

        var Answer = $scope.Answer;
        var Answer2 = $scope.Answer2;
        var Answer3 = $scope.Answer3;

        var validate = /^(?!.*([A-Za-z0-9])\1{2})[A-Za-z0-9]{2,50}$/;
        if (!validate.test(Answer)) {
            $scope.validateError = "Answer does not meet above requirements.";

            return;
        }
        else if (!validate.test(Answer2)) {
            $scope.validateError2 = "Answer does not meet above requirements.";
            return;
        }
        else if (!validate.test(Answer3))
        {
            $scope.validateError3 = "Answer does not meet above requirements.";
            return;
        }


        var updateData = { "AccountNumber": AccountNumber, "SecurityID": SecurityID, "Answer": Answer, "SecurityID2": SecurityID2, "Answer2": Answer2, "SecurityID3": SecurityID3, "Answer3": Answer3 };

      
        $scope.btnDisble = true;
        $scope.loading = true;
        userProfileFactory.UpdateAnswer(updateData, $scope.baseAddress).then(function (data) {
            $scope.btnDisble = false;
            $scope.loading = false;
         
            var message = JSON.parse(data);
            if (message == "complete")
            {
                $scope.message = "Your details have been Updated successfully.";
            }
            else
            {
                
                $scope.ErrorMessage = "Update Failed.";
            }

        },
          function (err)
          {
              $scope.loading = false;
         
              $scope.btnDisble = false;
              $scope.ErrorMessage = "Get User Profile Failed!";
          });



    }


    $scope.ErrorRemoveQuestionFirst = function ()
    {
        $scope.QuestionError2 = '';
    }

    $scope.ErrorRemoveQuestionSecond = function () {
        $scope.QuestionError3 = '';
    }

    $scope.validateErrorRemove = function () {
        $scope.validateError = "";
    }
    $scope.validateErrorRemove2 = function () {
        $scope.validateError2 = "";

    }
    $scope.validateErrorRemove3 = function () {
        $scope.validateError3 = "";
    }
    $scope.names = ["john", "bill", "charlie", "robert", "alban", "oscar", "marie", "celine", "brad", "drew", "rebecca", "michel", "francis", "jean", "paul", "pierre", "nicolas", "alfred", "gerard", "louis", "albert", "edouard", "benoit", "guillaume", "nicolas", "joseph"];

    $scope.removeStateError = function () {
        $scope.ErrorState = "";
    };
    $scope.cancel = function () {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/LoanPayment');
    }

}]);




