
app.controller('textNotificationController', ['$scope', '$rootScope', '$location', 'textNotificationService', 'additionalNotificationsService', 'WebApiService', function ($scope, $rootScope, $location, textNotificationService, additionalNotificationsService, WebApiService) {
    $scope.showErrorTerms = false;
    $scope.checkboxValidation =
    {
        value: false,
    };
    $scope.isPaymentReminder = true;
    $scope.isPaymentConfirmation = true;
    $scope.isPayByText = true;
    $scope.isCommunicationByText = true;
    $scope.baseAddress = WebApiService.baseAddress;
    $scope.accountNumber = sessionStorage.getItem('AccountNumber');
    $scope.errorMessage = '';
    $scope.showErrorMessage = false;
    $scope.successMessage = '';
    $scope.showSuccessMessage = false;

    $scope.showVerifyButton = true;
    $scope.showVerifyProgress = false;
    $scope.showErrorVerify = false;


    $scope.showOptInSwitchWidget = '';
    $scope.loading = true;
    $scope.showNextButton = true;
    $scope.showButtonProgress = false;
    $scope.btnNextDisabled = false;

    //steps
    $scope.Step1Class = "active";
    $scope.Step2Class = "disabled";
    $scope.Step3Class = "disabled";

    $scope.tabpane1Class = "tab-pane active pad_top0";
    $scope.tabpane2Class = "tab-pane disabled pad_top0";
    $scope.tabpane3Class = "tab-pane disabled pad_top0";

    $scope.Step1 = true;
    $scope.Step2 = false;
    $scope.Step3 = false;

    $scope.showAddTextNumber = false;
    var optedInTextNumberItem = function () {
        this.textNumber = '';
        this.paymentReminder = false;
        this.paymentConfirmation = false;
        this.payByText = false;
        this.communicationByText = false;
        this.counter;
    };
    var optedInTextNumberItems = [];
    $scope.optedInTextNumber = {};
    $scope.showAddNewTextNumber = false;


    var loadGrid = function () {
        optedInTextNumberItems.splice(0, optedInTextNumberItems.length);
        textNotificationService.getUserDetails($scope.accountNumber, $scope.baseAddress).success(function (data) {
            if (data.length === 0) {
                //redirect to opt in
                $rootScope.loggedIn = true;
                $location.path('/NotificationByText/OptIn');

                textNotificationService.getTextNumbers($scope.accountNumber, $scope.baseAddress).success(function (data) {
                    if (data.length > 0) {
                        $scope.TextNumber = GetMobile(data[0]);
                        $scope.btnNextDisabled = false;
                    }
                    else {
                        $scope.btnNextDisabled = true;
                    }
                    $scope.loading = false;
                }).error(function (data) {
                    $scope.errorMessage = JSON.parse(data);
                    $scope.showErrorMessage = true;
                    $scope.loading = false;
                });
            }
            else {
                
            for (var counter = 0; counter < data.length; counter++) {
                var item = new optedInTextNumberItem();
                item.textNumber = GetMobile(data[counter].TextNumber);
                item.paymentReminder = data[counter].IsPaymentReminder;
                item.paymentConfirmation = data[counter].IsPaymentConfirmation;
                item.payByText = data[counter].IsPayByText;
                item.communicationByText = data[counter].IsCommunicationByText;
                item.counter = counter;
                optedInTextNumberItems.push(item);
            }
            $scope.optedInTextNumber = optedInTextNumberItems;
            if (optedInTextNumberItems.length < 2)
                $scope.showAddTextNumber = true;
            else
                $scope.showAddNewTextNumber = false;
            
            }
            $scope.loading = false;

        }).error(function (data) {
            $scope.errorMessage = JSON.parse(data);
            $scope.showErrorMessage = true;
            $scope.loading = false;
        });
        $rootScope.NotificationByTextManageLoad = false;
    };

    (function () {
        if (!$rootScope.NotificationByTextManageLoad) {
            textNotificationService.getTextNumbers($scope.accountNumber, $scope.baseAddress).success(function (data)
            {
                if (data.length > 0)
                {
                    $scope.TextNumber = GetMobile(data[0]);
                    $scope.btnNextDisabled = false;
                }
                else
                {
                    $scope.btnNextDisabled = true;
                }
                $scope.loading = false;
            }).error(function (data) {
                $scope.errorMessage = JSON.parse(data);
                $scope.showErrorMessage = true;
                $scope.loading = false;
            });
        }
        else {
            loadGrid();
        }
    })();

    $scope.startOptIn = function ()
    {
        //redirect to notifications by text opt in
        $rootScope.loggedIn = true;
        $location.path('/NotificationByText/OptIn');
    }

    $scope.requestPin = function () {
        $scope.showErrorMessage = false;
        $scope.loading = true;
        var textNumber = $scope.TextNumber;
        if (textNumber != '' && textNumber != undefined) {
            textNumber = textNumber.replace(/-/g, "");
            if (textNumber != '' && textNumber != undefined && textNumber.length == 10) {
                if ($scope.checkboxValidation.value === true) {
                    $scope.showErrorTerms = false;
                    var collection = { "AccountNumber": $scope.accountNumber, "TextNumber": textNumber };
                    textNotificationService.requestPin(collection, $scope.baseAddress).success(function (data) {

                        $scope.Step1Class = "disabled";
                        $scope.Step2Class = "active";
                        $scope.Step3Class = "disabled";

                        $scope.tabpane1Class = "tab-pane disabled pad_top0";
                        $scope.tabpane2Class = "tab-pane active pad_top0";
                        $scope.tabpane3Class = "tab-pane disabled pad_top0";

                        $scope.Step1 = false;
                        $scope.Step2 = true;
                        $scope.Step3 = false;

                        $scope.loading = false;
                    }).error(function (data) {
                        $scope.errorMessage = JSON.parse(data);
                        $scope.showErrorMessage = true;
                        //$scope.showButtonProgress = false;
                        //$scope.showNextButton = true;
                        $scope.loading = false;
                    });
                }
                else {
                    $scope.errorTerms = "You must agree to above Terms & Conditions.";
                    $scope.showErrorTerms = true;
                    $scope.loading = false;
                }
            }
            else {
                $scope.errorMessage = "Please enter valid text number!";
                $scope.showErrorMessage = true;
                $scope.loading = false;
            }
        }
        else {
            $scope.errorMessage = "Please enter valid text number!";
            $scope.showErrorMessage = true;
            $scope.loading = false;
        }
    };

    $scope.additionalNotifications = function ()
    {
        $scope.loading = true;
        var collection = {
            "isPaymentReminder": $scope.isPaymentReminder,
            "isPaymentConfirmation": $scope.isPaymentConfirmation,
            "isPayByText": $scope.isPayByText,
            "isCommunicationByText": $scope.isCommunicationByText
        };
        additionalNotificationsService.setNotifications(collection);

        $scope.isPaymentReminder = true;
        $scope.isPaymentConfirmation = true;
        $scope.isPayByText = true;
        $scope.isCommunicationByText = true;

        //$scope.showButtonProgress = true;
        $scope.Step1Class = "disabled";
        $scope.Step2Class = "disabled";
        $scope.Step3Class = "active";

        $scope.tabpane1Class = "tab-pane disabled pad_top0";
        $scope.tabpane2Class = "tab-pane disabled pad_top0";
        $scope.tabpane3Class = "tab-pane active pad_top0";

        $scope.Step1 = false;
        $scope.Step2 = false;
        $scope.Step3 = true;
        //$scope.showButtonProgress = false;
        $scope.loading = false;
    }

    function GetMobile(val) {
        return val;
    };

    $scope.confirmPin = function (textNumber, pin, fromWhere) {
        $scope.showSuccessMessage = false;
        $scope.loading = true;

        var TextNumber = textNumber.replace(/-/g, "");
        if (pin != '' && pin != undefined) {
            $scope.showErrorVerify = false;
            var collectionNotifications = additionalNotificationsService.getNotifications();
            var collection = {
                "AccountNumber": $scope.accountNumber,
                "Pin": pin,
                "TextNumber": TextNumber,
                "IsPaymentReminder": collectionNotifications.isPaymentReminder,
                "IsPaymentConfirmation": collectionNotifications.isPaymentConfirmation,
                "IsPayByText": collectionNotifications.isPayByText,
                "IsCommunicationByText": collectionNotifications.isCommunicationByText
            };
            textNotificationService.verifyPin(collection, $scope.baseAddress).success(function (data) {
                $scope.showErrorVerify = false;
                $scope.loading = false;
                $scope.successMessage = "Pin verification for " + textNumber + " is successful.";
                $scope.showSuccessMessage = true;
                if (fromWhere === 'PayByText') {
                    $('#notificationsByTextModal').modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();

                    $scope.$parent.getTextNumbers();
                }
                else{
                //redirect to notifications by text manage
                $rootScope.loggedIn = true;
                $rootScope.NotificationByTextManageLoad = true;
                $location.path('/NotificationByText/Manage');
                }
            }).error(function (data,status) {
                if (status === 501)
                {
                    $scope.optInErrorMessage = JSON.parse(data);
                    $('#optInErrorModal').modal('show');
                }
                else
                {
                    $scope.errorVerify = JSON.parse(data);
                    $scope.showErrorVerify = true;
                }               
                $scope.loading = false;
                $scope.showVerifyProgress = false;
            });
        }
        else {
            $scope.errorVerify = "Please enter pin!";
            $scope.showErrorVerify = true;
            $scope.loading = false;
        }
    };

    $scope.showOptOutConfirmation = function (textNumber)
    {
        $scope.optOutNumber = textNumber;
    }

    //unsubscribe
    $scope.unSubscribe = function () {
        $scope.showSuccessMessage = false;
        $scope.loading = true;
        var textNumber = $scope.optOutNumber.replace(/-/g, "");

        var collection = { "AccountNumber": $scope.accountNumber, "TextNumber": textNumber };
        textNotificationService.unSubscribe(collection, $scope.baseAddress).success(function (data) {
            $scope.successMessage = "Opt-Out from all Notifications By Text for " + $scope.optOutNumber + " is successful.";
            $scope.showSuccessMessage = true;
            $scope.loading = false;

            //refresh the grid
            for (var i = optedInTextNumberItems.length - 1; i >= 0; i--) {
                if (optedInTextNumberItems[i].textNumber.replace(/-/g, "") === textNumber) {
                    optedInTextNumberItems.splice(i, 1);
                }
            }
            if (optedInTextNumberItems.length < 2 && optedInTextNumberItems.length > 0)
                $scope.showAddTextNumber = true;
            else if (optedInTextNumberItems.length === 0)
            {
                $scope.LoadOptIn = true;
                //redirect to notifications by text opt in
                $rootScope.loggedIn = true;
                $location.path('/NotificationByText/OptIn');
            }
        }).error(function (data) {
            $scope.errorMessage = JSON.parse(data);
            $scope.showErrorMessage = true;
            $scope.loading = false;
        });
    };

    $scope.manageAdditionalNotifications = function () {
        $scope.showSuccessMessage = false;
        $scope.loading = true;
        var textNumber = $scope.TextNumber.replace(/-/g, "");

        var collection = {
            "AccountNumber": $scope.accountNumber,
            "TextNumber": textNumber,
            "IsPaymentReminder": $scope.isPaymentReminder,
            "IsPaymentConfirmation": $scope.isPaymentConfirmation,
            "IsPayByText": $scope.isPayByText,
            "IsCommunicationByText": $scope.isCommunicationByText
        };
        textNotificationService.UpdateAdditionalNotifications(collection, $scope.baseAddress).success(function (data) {
            $scope.successMessage = "Additional Notifications saved successfully.";
            $scope.showSuccessMessage = true;
            $scope.loading = false;
            //refresh the grid
            loadGrid();
         
        }).error(function (data) {
            $scope.errorMessage = JSON.parse(data);
            $scope.showErrorMessage = true;
            $scope.loading = false;
        });
    };

    $scope.viewAdditionalNotifications = function (data) {
        $scope.TextNumber = data.textNumber;
        $scope.isPaymentReminder = data.paymentReminder;
        $scope.isPaymentConfirmation = data.paymentConfirmation;
        $scope.isPayByText = data.payByText;
        $scope.isCommunicationByText = data.communicationByText;
    };

    $scope.textNumberChanged = function () {
        var textNumber = $scope.TextNumber;
        if (textNumber && textNumber.replace(/-/g, "").length === 10) {
                $scope.btnNextDisabled = false;
        }
        else
            $scope.btnNextDisabled = true;
    }

    $scope.redirectToPayByText = function () {
        if ($rootScope.PaymentBlock) {
            $rootScope.loggedIn = true;
            $rootScope.leftMenu = true;
            $rootScope.userProfile = true;
            $location.path('/PayByText');
        }
    };
    $scope.redirectToNotificationsByText = function () {
        $('#optInErrorModal').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $rootScope.NotificationByTextManageLoad = true;
        $location.path('/NotificationByText/Manage');
    };
}]);


