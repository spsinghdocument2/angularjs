app.directive('leftmenuDirective', function () {
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        template: '<div id="leftMenuId" ng-show="leftMenu" class="left-sidebar">' +
            ' <div id="scroller" class="sidebar-holder">' +
            '<ul id="scroll_div" class="nav  nav-list">' +
           ' <li class="user_namenav">' +
           '<img class="user_img" src="{{ProfilePicture}}">' +
           '<span class="user_namemail hidden-minibar"> {{BankHolde}} <br/> <span class="font12">{{Email}}</span>' +
            '</span>' + 
            '</li>' +
            '  <li class="nav-toggle">' +
            '<span class="menu_text">Menu</span>' +
            '  <button class="btn  btn-nav-toggle text-primary" ng-click="hideShowSidebar()">' +
            '<img src="images/menu_image.png" />' +
            ' </button>' +
            '</li>' +
            '<li id=accountId ng-click="AccountOperationFirst()"  class={{accountActive}}><a href="" ng-click="MAFHome()" data-placement="right" title="You can see account details here." ><img src="images/acounts.png" /><span class="hidden-minibar"> Accounts </span> </a> </li>' +
            '<li  id=mafPayment ng-click="MAFPaymentOperation()" class={{MAFPaymentActive}} ng-show="PaymentBlock"><a href="" ng-click="MAFPayment()"  title="You can do one-time payment and setup the future pay here."><img src="images/maf_payment.png" /><span class="hidden-minibar">Make MAF Payment </span> </a> </li>' +
            '   <li ng-show="PaymentBlock" ng-click="FuturePayOperation()" class={{FuturePayActive}}> <a href="" ng-click="FuturePay()" title="You can setup future pay here."> <img src="images/future_pay.png"/><span class="hidden-minibar">Set Future Pay </span> </a> </li>' +
            '<li ng-click="PaymentHistoryOperation()" class={{PaymentHistoryActive}}><a href="" ng-click="PaymentHistory()"  title="You can see payment history here."><img src="images/history_reprt_icon.png" /><span class="hidden-minibar"> Payment History </span> </a>' +
           ' <li ng-show="PaymentBlock" ng-click="PayByTextOperation()" class={{PayByTextActive}} ><a  href="" ng-click="PayByText()" title="You can setup pay by text here."><img src="images/pay_by_text.png"/><span class="hidden-minibar"> Pay By Text </span> </a>' +
            '<li ng-click="PaymentScheduleSearch()" class={{PaymentScheduleSearchActive}}><a href="" ng-click="PaymentScheduleSearchOpen()" title="You can search the schedule payments here."><img src="images/calender.png"/><span class="hidden-minibar">Search Payment Schedule </span> </a>' +
            '<li data-toggle="collapse" data-target="#user_setting" > <a href="" title="You can see all settings here."> <img src="images/user_setting.png"/><span class="hidden-minibar"> My User Settings </span> <i class="fa fa-angle-down arrown_down" aria-hidden="true"></i></a> </li>' +
            '<ul class="sub-menu collapse" id="user_setting">' +
             '<li ng-click="UserProfileOperation()" class={{UserProfileActive}}  ><a ng-click="UserProfile()" href="" title="You can see and edit user profile here."><img src="images/user.png"/><span class="hidden-minibar">User Profile</span></a></li>' +      
            ' <li ng-click="ChangePasswordOperation()"  class={{changePasswordActive}}><a ng-click="MainResetPassword()" href="" title="You can change password here."><img src="images/change_password.png"/><span class="hidden-minibar">Change My Password</span></a></li>' +
            ' <li ng-click="UpdateSecurityQuestionOperation()"  class={{UpdateSecurityQuestionActive}} ><a href="" ng-click="UpdateSecurityQuestion()" title="You can update security questions here."><img src="images/security-question.png"/><span class="hidden-minibar">Update Security Question</span></a></li>' +
            '<li ng-click="ManageCardOperation()" class={{ManageCardActive}}  ><a ng-click="ManageCard()" href="" title="You can manage Debit/CC here."><img src="images/managemycard.png"/><span class="hidden-minibar">Manage My Card</span></a></li>' +
            '<li ng-click="ManageACHOperation()" class={{ManageACHActive}}  ><a ng-click="ManageACH()" href="" title="You can manage ACH here."><img src="images/manage_ach.png"/><span class="hidden-minibar">Manage My ACH</span></a></li>' +
            ' <li ng-click="NotificationByTextManageOperation()"  class={{NotificationByTextManageActive}} ><a href="" ng-click="NotificationByTextManage()" title="You can setup and manage notification by text here."><img src="images/notification.png"/><span class="hidden-minibar">Notifications By Text</span></a></li>' +
                   ' <li  ng-click="InsuranceInformationOperation()" class={{InsuranceInformationActive}}  ><a href="" ng-click="InsuranceInformation()" title="You can see and edit insurance information here."><img src="images/insurance_icon.png"/><span class="hidden-minibar">Insurance Information</span></a></li>' +
            ' </ul>' +
            '<li ng-show="PaymentBlock"  ng-click="AutoPayOperation()"  class={{AutoPayActive}}> <a  ng-click="AutoPay()" href="" title="You can setup autopay here."> <img src="images/auto_pay.png"/><span class="hidden-minibar"> AutoPay </span> </a> </li>' +
            '   <li> <a href="" ng-click="Mainlogout()" title="You can logout here."> <img src="images/sign_out.png"/><span class="hidden-minibar"> Logout </span> </a> </li>' +             
            '</ul>' +
            ' </div>' +
            ' <script type="text/javascript">alert();$("#scroll_div").niceScroll();</script>' +
            '</div>'
        , link: function () {
            $("#scroll_div").niceScroll();                      
        }
    }
});

app.directive('alertList', function () {
    return {
        restrict: 'E', // Element directive
        replace: true,
        transclude: true,
        scope: { alert: '=alertData' },
        template: "<li><div class='dropdown-messages-box'>" +
                                "<div class='media-body'>"+
                                    "<strong>{{alert.Head}}</strong><br>"+
                                     "<small class='text-muted'>{{alert.Description}}<a  href=''   ng-show=\"alert.Action !='NoAction'\" ng-click='$parent.Action(alert.Action);'>{{alert.ActionHeader}}</a>.</small>" +
                                    
                               " </div>"+
                   "</div></li>"
                  // +
                // '<li class="divider"></li>'

    };
});

