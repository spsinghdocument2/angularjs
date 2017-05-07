app.config(function ($routeProvider, $locationProvider) {
    //app.config(function ($routeProvider) {

    $routeProvider
         .when('/', {
             templateUrl: '/Views/Login/login.html'
         })
        .when('/LoanPayment', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/LoanPayment/LoanPayment.html'
        })
        .when('/UserProfile', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/UserProfile/UserProfile.html'
        })
        .when('/InsuranceInformation', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/InsuranceInformation/InsuranceInformation.html'
        })
        .when('/NotificationByText/OptIn', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/TextNotification/OptIn.html'
        })
        .when('/NotificationByText/Manage', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/TextNotification/TextNotification.html'
        })
        .when('/login', {
            templateUrl: '/Views/Login/login.html',
            controller: 'LoginController'
        })
        .when('/CookiePolicy', {
            templateUrl: '/Views/CookiePolicy/CookiePolicy.html'
        })
        .when('/MAFDisclaimer', {
            templateUrl: '/Views/MAFDisclaimer/MAFDisclaimer.html'
        })
        .when('/PrivacyStatement', {
            templateUrl: '/Views/PrivacyStatement/PrivacyStatement.html'
        })
        .when('/TermsOfService', {
            templateUrl: '/Views/TermsOfService/TermsOfService.html'
        })
        .when('/Authenticate', {
            templateUrl: '/Views/Authenticate/Authenticate.html',
            controller: 'AuthenticateController'
        })
        .when('/ForgotPassword', {

            templateUrl: '/Views/ForgotPassword/ForgotPassword.html',
            controller: 'ForgotPasswordController'
        })
        .when('/ResetPassword', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/ResetPassword/ResetPassword.html',
            controller: 'ChangePasswordController'
        })
        .when('/DebitCreditAccountInformation', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/DebitCreditAccountInformation/DebitCreditAccountInformation.html'
        })
        .when('/CardInfo', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/CardInfo/CardInfo.html'
        })
        .when('/PaymentType', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentType/PaymentType.html'
        })
        .when('/CreateAccount', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.CreateAccount) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/CreateAccount/CreateAccount.html',
            controller: 'RegisterController'
        })
        .when('/PaymentAgreement', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentAgreement/PaymentAgreement.html'
        })
        .when('/DebitCreditPaymentAgreement', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/DebitCreditPaymentAgreement/DebitCreditPaymentAgreement.html'
        })
        .when('/AccountInformation', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/AccountInformation/AccountInformation.html'
        })
        .when('/PaymentAmount', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentAmount/PaymentAmount.html'
        })
        .when('/PaymentConfirmation', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentConfirmation/PaymentConfirmation.html'
        })
        .when('/UpdateSecurityQuestion', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/UpdateSecurityQuestion/UpdateSecurityQuestion.html'
        })
        .when('/PayByText', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PayByText/PayByText.html'
        })
        .when('/paymentHistory', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentHistory/PaymentHistory.html'
        }).when('/TemporaryPassword', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/TemporaryPassword/TemporaryPassword.html'
        })
         .when('/CardPaymentConfirmation', {
             resolve: {
                 "check": function ($location, $rootScope) {
                     if (!$rootScope.loggedIn) {
                         $location.path('/Views/Login/login.html');
                     }
                 }
             },
             templateUrl: '/Views/CardPaymentConfirmation/CardPaymentConfirmation.html'
         })
        .when('/PaymentScheduleSearch', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/PaymentScheduleSearch/PaymentScheduleSearch.html'
        })
        .when('/AutoPay', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');

                    }
                }
            },
            templateUrl: '/Views/AutoPay/AutoPay.html'
        })
        .when('/FuturePay', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');

                    }
                }
            },
            templateUrl: '/Views/FuturePay/FuturePay.html'
        })
        .when('/FuturePayAgreement', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');

                    }
                }
            },
            templateUrl: '/Views/FuturePay/FuturePayAgreement.html'
        })
         .when('/FuturePayAccountInfo', {
             resolve: {
                 "check": function ($location, $rootScope) {
                     if (!$rootScope.loggedIn) {
                         $location.path('/Views/Login/login.html');
                     }
                 }
             },
             templateUrl: '/Views/FuturePay/FuturePayAccountInfo.html'
         })
        .when('/ManageCard', {
            resolve: {
                "check": function ($location, $rootScope) {
                    if (!$rootScope.loggedIn) {
                        $location.path('/Views/Login/login.html');
                    }
                }
            },
            templateUrl: '/Views/ManageSavedPayment/ManageCard.html'
        })
         .when('/ManageACH', {
             resolve: {
                 "check": function ($location, $rootScope) {
                     if (!$rootScope.loggedIn) {
                         $location.path('/Views/Login/login.html');
                     }
                 }
             },
             templateUrl: '/Views/ManageSavedPayment/ManageACH.html'
         })
         .when('/KnowledgeBase', {            
             templateUrl:'/Views/KnowledgeBase/KnowledgeBase.html'
         })
         .when('/KnowledgeBasePayment', {
             templateUrl: '/Views/KnowledgeBase/KnowledgeBasePayment.html'
         })
          .when('/KnowledgeBaseCreateAccount', {
              templateUrl: '/Views/KnowledgeBase/KnowledgeBaseCreateAccount.html'
          })
     .when('/KnowledgeBaseForgetPassword', {
         templateUrl: '/Views/KnowledgeBase/KnowledgeBaseForgetPassword.html'
     })
     .when('/KnowledgeBaseUserProfile', {
         templateUrl: '/Views/KnowledgeBase/KnowledgeBaseUserProfile.html'
     })
     .when('/KnowledgeBaseChangePassword', {
         templateUrl: '/Views/KnowledgeBase/KnowledgeBaseChangePassword.html'
     })
    
        .when('/AutoPayAgreement/:accountNumber', {
            templateUrl: '/Views/AutoPayAgreement/AutoPayAgreement.html'
        })
    
        .when('/KnowledgeBaseUpdateSecurityQuestion', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseUpdateSecurityQuestion.html'
        })

        .when('/KnowledgeBaseManageCard', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseManageCard.html'
        })

        .when('/KnowledgeBaseManageAch', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseManageAch.html'
        })
   
        .when('/KnowledgeBaseFuturePayment', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseFuturePayment.html'
        })
    
        .when('/KnowledgeBaseInsuranceInformation', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseInsuranceInformation.html'
        })

        .when('/KnowledgeBaseMafAccountDetail', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseMafAccountDetail.html'
        })
    
        .when('/KnowledgeBasePaymentHistory', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBasePaymentHistory.html'
        })
    
        .when('/KnowledgeBaseSearchPaymentSchedule', {
            templateUrl: '/Views/KnowledgeBase/KnowledgeBaseSearchPaymentSchedule.html'
        })
    .when('/KnowledgeBaseAutoPay', {
        templateUrl: '/Views/KnowledgeBase/KnowledgeBaseAutoPay.html'
    })
    
        .when('/PrintConfirmation', {
            //resolve: {
            //    "check": function ($location, $rootScope) {
            //        if (!$rootScope.loggedIn) {
            //            $location.path('/Views/Login/login.html');
            //        }
            //    }
            //},
            templateUrl: '/Views/PaymentConfirmation/printConfirmation.html'
        })
        .otherwise({
            redirectTo: '/'
        });

    //enable HTML5mode to disable hashbang urls
    $locationProvider.html5Mode(true);
});