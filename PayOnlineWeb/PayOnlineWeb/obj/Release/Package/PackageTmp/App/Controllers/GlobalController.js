app.controller('GlobalController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location)
{
    $scope.ClassName = "home";    
    $rootScope.BankHolde = '';
    $rootScope.AchPaymentAmountCheck = true;
    $rootScope.NotificationByTextManageLoad = false;
    $rootScope.PaymentBlock = '';
  //  alert($rootScope.PaymentBlock);
    $scope.bodyClassAdd = function ()
    {
        $scope.ClassName = "home";
    }
   
    $scope.bodyClassRemove = function ()
    {
        
        $scope.ClassName = "";
    }
    $scope.mainholder = "main-holder";
    $scope.mainholderRemove = function ()
    {

        $scope.mainholder = "";
    };
    $scope.mainholderAdd = function ()
    {
        $scope.mainholder = "main-holder";
    } 
    $rootScope.leftMenu = false;
    $rootScope.userProfile = false;
    $rootScope.login = true;
    $rootScope.hideMenu = "hide";
    $scope.toggleButton = function () {
        if (window.screen.width <= 1024 || window.innerWidth <= 1024) {
            document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
            document.querySelector('.nav-toggle').classList.add('hide');
        }
        else
            document.querySelector('.nav-toggle').classList.remove('hide');
    }
    $scope.toggleSticky = function ()
    {
        document.querySelector('#help_sticky').classList.toggle('active');
    }
    $scope.hideShowSidebar = function ()
    {
        //document.querySelectorAll('.hidden-minibar').forEach(function(elem) {
        //    elem.classList.toggle('hide');
        //});
        var elems = document.querySelectorAll('.hidden-minibar');
        for (i = 0; i < elems.length; ++i) {
            elems[i].classList.toggle('hide');
        }

        document.querySelector('.site-holder').classList.toggle('mini-sidebar');
        document.querySelector('.contentTop').classList.toggle('header_widthmore');
        document.querySelector('.menu_text').classList.toggle('hide');
    }
    $scope.MAFPayment = function ()
    {
     $rootScope.loggedIn = true;
    $rootScope.leftMenu = true;
    $rootScope.userProfile = true; 
    document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
    $location.path('/PaymentAgreement');
    }
    $scope.InsuranceInformation = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/InsuranceInformation');
    }
  
    $scope.MAFHome = function () {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $location.path('/LoanPayment');
    }

    $scope.MAFHome = function () {
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $location.path('/LoanPayment');
    }
    
    $scope.paymentHistoryGoBack = true;
    $scope.UserProfile = function ()
    {
        document.querySelector('.nav-toggle').classList.remove('hide');
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/UserProfile');
    }
    $scope.UpdateSecurityQuestion = function ()
    {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/UpdateSecurityQuestion');
    }
    $scope.KnowledgeBase = function () {          
         $location.path('/KnowledgeBase');
     }
    $scope.AutoPay = function ()
    {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/AutoPay');
    }
    $scope.NotificationByTextOptOut = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/NotificationByText/OptIn');
    }

    $scope.NotificationByTextManage = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $rootScope.NotificationByTextManageLoad = true;
        $location.path('/NotificationByText/Manage');
    }

    $scope.Mainlogout = function ()
    {
        $rootScope.leftMenu = false;
        $rootScope.userProfile = false;
        $rootScope.loggedIn = false;
        $rootScope.hideMenu = "hide";

        $rootScope.accountActive = "active";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePay = "";
        $scope.MAFPaymentActive = "";

        $location.path('/login');
        //window.location.reload();
        
    }; 
    $scope.CookiePolicy = function () {
        $rootScope.leftMenu = false;
        $rootScope.userProfile = false;
        $rootScope.hideMenu = "hide";
        $location.path('/CookiePolicy');
        window.location.reload();
        
    }; 
    $scope.PrivacyStatement = function () {
        $rootScope.leftMenu = false;
        $rootScope.userProfile = false;
        $rootScope.hideMenu = "hide";
        $location.path('/PrivacyStatement');
        window.location.reload();

    };
    $scope.TermsOfService = function () {
        $rootScope.leftMenu = false;
        $rootScope.userProfile = false;
        $rootScope.hideMenu = "hide";
        $location.path('/TermsOfService');
        window.location.reload();

    };
    $scope.MAFDisclaimer = function () {
        $rootScope.leftMenu = false;
        $rootScope.userProfile = false;
        $rootScope.hideMenu = "hide";
        $location.path('/MAFDisclaimer');
        window.location.reload();

    };

    $scope.MainResetPassword = function ()
    {
        document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
        $rootScope.hideMenu = "";
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/ResetPassword');

    };
    $scope.PaymentHistory = function ()
    {
        
        document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
        $rootScope.hideMenu = "";
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        sessionStorage.setItem('AccountHolder', $scope.AccountHolder);
        sessionStorage.setItem('CurrentBalance', $scope.CurrentBalance);
        $scope.paymentHistoryGoBack = false;
        $location.path('/paymentHistory');

    };
    $scope.PaymentHistoryAchPaymentAmount = function () {
        document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        sessionStorage.setItem('AccountHolder', $scope.AccountHolder);
        sessionStorage.setItem('CurrentBalance', $scope.CurrentBalance);
        $scope.paymentHistoryGoBack = true;
        $location.path('/paymentHistory');

    };

    $scope.PayByText = function ()
    {
        document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
        $rootScope.hideMenu = "";
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/PayByText');
    };

    $scope.PaymentScheduleSearchOpen = function () {
        document.querySelector('.left-sidebar').classList.toggle('show-fullsidebar');
        $rootScope.hideMenu = "";
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/PaymentScheduleSearch');

    };
    $scope.FuturePay = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/FuturePayAgreement');
    }

    $scope.SetFuturePayWithoutAgreement = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/FuturePay');
    }

    $scope.ManageCard = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/ManageCard');
    }

    $scope.ManageACH = function () {
        document.querySelector('.left-sidebar').classList.remove('show-fullsidebar');
        $rootScope.loggedIn = true;
        $rootScope.leftMenu = true;
        $rootScope.userProfile = true;
        $location.path('/ManageACH');
    }
    
    $rootScope.accountActive = "active";
    $rootScope.MAFPaymentActive = "";
    $rootScope.payMyBillActive = "";
    $rootScope.PaymentHistoryActive = "";
    $scope.changePasswordActive = "";
    $scope.UpdateSecurityQuestionActive = "";
    $scope.UserProfileActive = "";
    $scope.PayByTextActive = "";
    $scope.NotificationByTextManageActive = "";
    $scope.PaymentScheduleSearchActive = "";
    $scope.AutoPayActive = "";
    $scope.InsuranceInformationActive = "";
    $scope.FuturePayActive = "";
    $scope.ManageCardActive = "";
    $scope.ManageACHActive = "";

    $scope.AccountOperationFirst = function ()
    {

        $rootScope.accountActive = "active";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.MAFPaymentOperation = function ()
    {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "active";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.AccountOperation = function ()
    {

        $rootScope.payMyBillActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "active";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.PaymentHistoryOperation = function ()
    {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "active";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.ChangePasswordOperation = function ()
    {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "active";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }

    $scope.UpdateSecurityQuestionOperation = function ()
    {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "active";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.UserProfileOperation = function ()
    {
        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "active";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }

    $scope.ManageCardOperation = function () {
        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "active";
        $scope.ManageACHActive = "";
    }

    $scope.ManageACHOperation = function () {
        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "active";
    }

    $scope.PayByTextOperation = function ()
    {
    
        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "active";
        $scope.NotificationByTextManageActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }

    $scope.NotificationByTextManageOperation = function () {
        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.NotificationByTextManageActive = "active";
        $scope.PaymentScheduleSearchActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }

    $scope.PaymentScheduleSearch = function () {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.PaymentScheduleSearchActive = "active";
        $scope.NotificationByTextManageActive = "";
        $scope.AutoPayActive = "";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.AutoPayOperation = function () {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.AutoPayActive = "active";
        $scope.InsuranceInformationActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
    }
    $scope.InsuranceInformationOperation = function () {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.AutoPayActive = "";
        $scope.FuturePayActive = "";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
        $scope.InsuranceInformationActive = "active";
    }
    $scope.FuturePayOperation = function () {

        $rootScope.accountActive = "";
        $rootScope.MAFPaymentActive = "";
        $rootScope.payMyBillActive = "";
        $rootScope.PaymentHistoryActive = "";
        $scope.changePasswordActive = "";
        $scope.UpdateSecurityQuestionActive = "";
        $scope.UserProfileActive = "";
        $scope.PayByTextActive = "";
        $scope.PaymentScheduleSearchActive = "";
        $scope.NotificationByTextManageActive = "";
        $scope.AutoPayActive = "";
        $scope.FuturePayActive = "active";
        $scope.ManageCardActive = "";
        $scope.ManageACHActive = "";
        $scope.InsuranceInformationActive = "";
    };
    $scope.Action = function (data)
    {
    
        if (data === "UserProfile")
        {
            $scope.UserProfile();
            $scope.UserProfileOperation();
        }
        else if (data === "UpdateSecurityQuestion")
        {
            $scope.UpdateSecurityQuestion();
            $scope.UpdateSecurityQuestionOperation();
        }
        else if (data === "InsuranceInformation")
        {
            $scope.InsuranceInformation();
            $scope.InsuranceInformationOperation();
        }
        else if (data === "MAFPayment") {
            $scope.MAFPayment();
            $scope.MAFPaymentOperation();
        }
    };



}]);

