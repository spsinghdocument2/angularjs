app.controller('AutoPayAgreementController', ['$scope', '$routeParams', '$location', 'AutoPayAgreementService', 'WebApiService', function ($scope, $routeParams, $location, autoPayAgreementService, webApiService)
{
    $scope.switchWidgetForTerms = 'ACHTerms';
    $scope.accountNumber = '';
    $scope.transactionFee = '0.00';
    $scope.baseAddress = webApiService.baseAddress;
    $scope.termsAndConditionsError = '';
    $scope.checkBoxTermsAndConditions = false;
    $scope.disableEnrollButton = true;
    $scope.loading = false;
    $scope.errorMessage = '';
    $scope.showErrorMessage = false;
    $scope.confirmationNumber = '';
    $scope.showConfirmationPage = false;

    (function () {
        $scope.loading = true;
        $scope.showErrorMessage = false;

        $scope.accountNumber = $routeParams.accountNumber;
        autoPayAgreementService.getAutoPayPaymentMethod($scope.accountNumber, $scope.baseAddress).then(function (data) {
            if (data)
            {
                if (data.PaymentMethod === 'ACH')
                {
                    $scope.switchWidgetForTerms = 'ACHTerms';
                }
                else if(data.PaymentMethod === 'Card')
                {
                    $scope.switchWidgetForTerms = 'CardTerms';
                }
                $scope.transactionFee = data.Fee;
            }
            $scope.loading = false;
        }, function (error)
        {
            $scope.showErrorMessage = true;
            $scope.errorMessage = error.Message;
            $scope.loading = false;
        });
    })();

    $scope.termsAndConditionCheckboxChanged = function ()
    {
        if ($scope.checkBoxTermsAndConditions)
        {
            $scope.disableEnrollButton = false;
            $scope.termsAndConditionsError = '';
        }
        else
        {
            $scope.disableEnrollButton = true;
        }
    }

    $scope.enrollToAutoPay = function ()
    {
        $scope.loading = true;
        $scope.termsAndConditionsError = '';
        if ($scope.checkBoxTermsAndConditions) {
            autoPayAgreementService.enrollAutoPay({ 'AccountNumber': $scope.accountNumber }, $scope.baseAddress).then(function (data) {
                if (data) {
                    $scope.confirmationNumber = data.ConfirmationNumber;
                    $scope.showConfirmationPage = true;
                }
                $scope.loading = false;
            }, function (error) {
                $scope.showConfirmationPage = false;
                $scope.showErrorMessage = true;
                $scope.errorMessage = error.Message;
                $scope.loading = false;
            });
        }
        else {
            $scope.termsAndConditionsError = 'You must agree to above terms and conditions.';
        }
    };

}]);