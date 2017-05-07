app.controller('printPaymentConfirmationController', ['$scope', '$window', function ($scope, $window) {
    var vm = this;
    vm.showAchSection = false;
    vm.showCardSection = false;
    vm.paymentDate;
    vm.paymentAmount;
    vm.fee;
    vm.totalAmount;
    vm.accountNumber;
    vm.fullName;
    vm.bankAccountName;
    vm.bankName;
    vm.routingNumber;
    vm.bankAccountNumber;
    vm.accountType;
    vm.confirmationNumber;
    vm.cardName;
    vm.cardType;
    vm.cardNumber;
    vm.expiryDate;
    vm.expiryYear;
    vm.address;
    vm.city;
    vm.state;
    vm.zip;
    
    vm.paymentTransactionData;

    vm.init = function ()
    {
        //vm.paymentTransactionData = PaymentConfirmationService.getPaymentTransactionData();
        vm.paymentTransactionData = JSON.parse(localStorage.paymentTransactionData);
        
        if (vm.paymentTransactionData)
        {
            vm.paymentDate = vm.paymentTransactionData.paymentDate;
            vm.paymentAmount = vm.paymentTransactionData.paymentAmount;
            vm.fee = vm.paymentTransactionData.fee;
            vm.totalAmount = vm.paymentTransactionData.totalAmount;
            vm.accountNumber = vm.paymentTransactionData.accountNumber;
            vm.fullName = vm.paymentTransactionData.fullName;
            vm.confirmationNumber = vm.paymentTransactionData.confirmationNumber;

            //ach transaction
            if (vm.paymentTransactionData.routingNumber) {
                vm.bankAccountName = vm.paymentTransactionData.bankAccountName;
                vm.bankName = vm.paymentTransactionData.bankName;
                vm.routingNumber = vm.paymentTransactionData.routingNumber;
                vm.bankAccountNumber = vm.paymentTransactionData.bankAccountNumber;
                vm.accountType = vm.paymentTransactionData.accountType;

                vm.showAchSection = true;
                vm.showCardSection = false;
            }
            else {
                vm.cardName = vm.paymentTransactionData.cardName;
                vm.cardType = vm.paymentTransactionData.cardType;
                vm.cardNumber = vm.paymentTransactionData.cardNumber;
                vm.expiryDate = vm.paymentTransactionData.expiryDate;
                vm.expiryYear = vm.paymentTransactionData.expiryYear;
                vm.address = vm.paymentTransactionData.address;
                vm.city = vm.paymentTransactionData.city;
                vm.state = vm.paymentTransactionData.state;
                vm.zip = vm.paymentTransactionData.zip;

                vm.showAchSection = false;
                vm.showCardSection = true;
            }
           
            $scope.$watch(angular.bind(this, function () {
                return vm.accountNumber; // `this` IS the `this` above!!
            }), function (newVal, oldVal) {
                $window.print();
            });
        }
    }

    vm.init();
}]);