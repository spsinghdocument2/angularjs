app.service('InsuranceInformationService', ['$http', '$q', function ($http, $q) {
    var url = "";

    //this.SaveInsuranceInformation = function (collection, baseAddress) {

    //    url = baseAddress + "/InsuranceInformation/SaveInsuranceInformation";
    //    var deferred = $q.defer();
    //    $http.post(url, collection).success(function (response) {

    //        deferred.resolve(response);
    //    })
    //    .error(function (err, status) {

    //        deferred.reject(err);
    //    });
    //    return deferred.promise;

    //};


    this.SaveInsuranceInformation = function (collectionData, baseAddress) {

        url = baseAddress + "/InsuranceInformation/SaveInsuranceInformation";
        //return $http.post(url, collectionData, {
        //    transformRequest: angular.identity,
        //    headers: {
        //        'Content-Type': undefined
        //    }
        //});
        return $http({
            method: 'POST',
            url: url,
            //IMPORTANT!!! You might think this should be set to 'multipart/form-data' 
            // but this is not true because when we are sending up files the request 
            // needs to include a 'boundary' parameter which identifies the boundary 
            // name between parts in this multi-part request and setting the Content-type 
            // manually will not set this boundary parameter. For whatever reason, 
            // setting the Content-type to 'false' will force the request to automatically
            // populate the headers properly including the boundary parameter.
            headers: { 'Content-Type': undefined },
            //This method will allow us to change how the data is sent up to the server
            // for which we'll need to encapsulate the model data in 'FormData'
            transformRequest: function (data) {
                console.log(data);
                var formData = new FormData();
                //need to convert our json object to a string version of json otherwise
                // the browser will do a 'toString()' on the object which will result 
                // in the value '[Object object]' on the server.
                formData.append("model", angular.toJson(data.model));
                //now add all of the assigned files
                formData.append("file", data.files);
                return formData;
            },
            //Create an object that contains the model and files which will be transformed
            // in the above transformRequest method
            data: collectionData
        });

    }





    this.GetInsuranceInformation = function (accountNumber, baseAddress)
    {

        url = baseAddress + "/InsuranceInformation/GetInsuranceInformation/" + accountNumber;
        var deferred = $q.defer();
        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response)
        {

            deferred.resolve(response);
        })
        .error(function (err, status) {

            deferred.reject(err);
        });
        return deferred.promise;

    };

    this.GetUserProfileAddress = function (accountNumber, baseAddress) {
        url = baseAddress + "/UserProfile/GetUserProfile/" + accountNumber;
        deferred = $q.defer();
        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);
        })
        .error(function (err, status) {
            //deferred.reject(null);
            deferred.reject(err);
        });
        return deferred.promise;

    }


    this.GetStateList = function () {

        var deferred = $q.defer();
        url = "data/State.json";

        $http.get(url).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    this.GetInsuranceCompanyNameList = function (baseAddress) {

        var deferred = $q.defer();
        url = baseAddress + "/InsuranceInformation/GetInsuranceCompanyList";

        $http.get(url, { headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem('Token') } }).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

}]);