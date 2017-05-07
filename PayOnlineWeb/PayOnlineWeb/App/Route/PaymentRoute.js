
     Route.config(function ($stateProvider, $urlRouterProvider) {

    // For any unmatched url, send to /route1
         $urlRouterProvider.otherwise("Login")

        $stateProvider
            .state('route1', {
                url: "route1",
                templateUrl: "Views/h1.html"
            })
            .state('route1.list', {
                url: "/list",
                templateUrl: "User1/route1.list.html"
               
            })

            .state('route2', {
                url: "route2",
                templateUrl: "Views/Login/LoginIframe.html",
                controller: 'LoginController'


             
            })
            .state('route2.list', {
                url: "/list",
                templateUrl: "User2/route2.list.html"
              
            })
})
