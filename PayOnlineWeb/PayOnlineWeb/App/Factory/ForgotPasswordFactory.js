app.factory('ForgotPasswordFactory', ['$http', function ($http) {
    var url = "";
    return {
       
        verifyUser: function (collection, baseAddress) {
            url = baseAddress + "/ForgotPassword/POST";
            return $http.post(url, collection);
        }, 

       
        GetSecurityQuestions: function ()
        {
         
            url = "data/SecurityQuestionsTable.json";
            return $http.get(url);
        },

        temporaryPasswordSendMailUser: function (collection, baseAddress)
        {
            url = baseAddress + "/ForgotPassword/ForgotTemporaryPasswordSendMail";
            return $http.post(url, collection);
        },
        paymentAgreementFee : function () {

            url = "data/PaymentAgreement.json";
            return $http.get(url);
        },
   

        DuplicatePayment: function (accountNumber, baseAddress)
        {
            
             url = baseAddress + "/LoanPayment/DuplicatePayment/" + accountNumber;
             return $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
        },

    }
}]);