app.provider('WebApiService', {
    $get: function () {
        return {
            baseAddress: 'http://localhost:49512/api'
        }
    }
});

//baseAddress: 'http://66.192.17.76:7075/api'
//baseAddress: 'http://192.168.12.218:8080/api'