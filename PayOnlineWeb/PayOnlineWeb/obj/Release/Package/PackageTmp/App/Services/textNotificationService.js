app.service('textNotificationService',['$http', function ($http) {

    this.getUserDetails = function (acountNumber, baseAddress) {
        url = baseAddress + "/TextNotification/GetUserDetails/" + acountNumber;
        return $http.get(url);
    }

    this.requestPin = function (collection, baseAddress) {
        url = baseAddress + "/TextNotification/RequestVerification";
        return $http.post(url, collection);
    }

    this.verifyPin = function (collection, baseAddress) {
        url = baseAddress + "/TextNotification/ConfirmVerification";
        return $http.post(url, collection);
    }
    this.unSubscribe = function (collection, baseAddress) {
        url = baseAddress + "/TextNotification/UnSubscribe";
        return $http.post(url, collection);
    }
    this.UpdateAdditionalNotifications = function (collection, baseAddress) {
        url = baseAddress + "/TextNotification/UpdateAdditionalNotifications";
        return $http.post(url, collection);
    }
    this.getSubscription = function (acountNumber, baseAddress) {
        url = baseAddress + "/TextNotification/GetSubscription/" + acountNumber;
        return $http.get(url);
    }
    this.getAdditionalNotifications = function (acountNumber, baseAddress) {
        url = baseAddress + "/TextNotification/GetAdditionalNotifications/" + acountNumber;
        return $http.get(url);
    }

    this.getTextNumbers = function (acountNumber, baseAddress) {
        url = baseAddress + "/TextNotification/GetTextNumberDetails/" + acountNumber;
        return $http.get(url);
    }
}]);

app.service('additionalNotificationsService', function additionalNotificationsService() {
    var additionalNotifications;

    var service = {       
        getNotifications: function () { return additionalNotifications; },
        setNotifications: function (value) { additionalNotifications = value; }
    };
    return service;
});

