
app.factory('ChangePasswordFactory',['$http' ,function ($http) {
    var url = "";
    return {

        VerifyUser: function (collection, baseAddress)
        {
           

            url = baseAddress + "/ChangePassword/changePasswordVerify";
            return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
        },
        continueUser: function (collection, baseAddress) {

            url = baseAddress + "/ChangePassword/passwordReset";
            return $http.post(url, collection, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } });
        },
    }
}]);

