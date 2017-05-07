
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MAF.WEBAPI.Resolver;
using MAF.BAL;
using MAF.SolutionByText;

namespace MAF.WEBAPI.App_Start
{
    public static class Bootstrapper
    {

        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ILogin, Login>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticate, Authenticate>(new HierarchicalLifetimeManager());
            container.RegisterType<IRegister, Register>(new HierarchicalLifetimeManager());
            container.RegisterType<IForgotPassword, ForgotPassword>(new HierarchicalLifetimeManager());
            container.RegisterType<IChangePassword, ChangePassword>(new HierarchicalLifetimeManager());
            container.RegisterType<IPayOnline, PayOnline>(new HierarchicalLifetimeManager());
            container.RegisterType<IPaymentConfirmation, PaymentConfirmation>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoanPayment, LoanPayment>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserProfile, UserProfile>(new HierarchicalLifetimeManager());
            container.RegisterType<IDataContext, DataContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IGeneralService, GeneralService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMessageService, MessageService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISubscriberService, SubscriberService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAutoPay, AutoPay>(new HierarchicalLifetimeManager());
            container.RegisterType<IInsuranceInformation, InsuranceInformation>(new HierarchicalLifetimeManager());
            container.RegisterType<IAutoPayAgreement, AutoPayAgreement>(new HierarchicalLifetimeManager());
            container.RegisterType<IUtility, Utility>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuturePay, FuturePay>(new HierarchicalLifetimeManager());
            container.RegisterType<IPayByText, PayByText>(new HierarchicalLifetimeManager());
            container.RegisterType<IAchPayment, AchPayment>(new HierarchicalLifetimeManager());
            container.RegisterType<ICardPayment, CardPayment>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}