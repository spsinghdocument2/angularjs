
namespace MAF.BAL
{
    using log4net;
    using System;
    using System.Configuration;
    public class Logger
    {
        private static readonly ILog log = null;

        private static bool IsTracing;
        //private static bool IsErrorWriting;
        static Logger()
        {
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log4net.Config.XmlConfigurator.Configure();
            IsTracing = CanTracing();

        }


        public static void WriteTraceLog(string traceMessage)
        {
            try
            {
                //// if (ConfigurationManager.AppSettings["IsTracing"].ToString().ToUpper() == "YES")
                if (IsTracing)
                {
                    ////Logging info message.
                    log.Info(traceMessage);
                }
            }
            catch (Exception)
            {

            }
        }
        public static void WriteErrorLog(string error, Exception ex)
        {
            try
            {
                log.Error(error, ex);

            }
            catch (Exception)
            {
            }
        }

        #region IsTracing      -> Check if tracing is 'ON' in config file
        private static bool CanTracing()
        {
            ////Getting the method name as well as class name from where request came here
            bool flag = false;
            try
            {

                ////Getting the Tracing string from the config file.
                flag = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTracing"].ToString(), System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                ////Writing error in error file
                log.Error("Error", ex);
            }

            return flag;
        }

        #endregion

    }
}
