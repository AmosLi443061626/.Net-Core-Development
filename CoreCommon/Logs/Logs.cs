
using CoreCommon.Extensions;
using NLog;

namespace CoreCommon.Logs
{
    public class Log
    {
        private static Logger _logger= LogManager.GetCurrentClassLogger();

        public static void Debug(LogFormat content)
        {
            content._sysFlag = "debug";
            _logger.Debug(content.ToJson());
        } 

        public static void Info(LogFormat content)
        {
            content._sysFlag = "info";
            _logger.Info(content.ToJson());
        }

        public static void Warn(LogFormat content)
        {
            content._sysFlag = "warn";
            _logger.Warn(content.ToJson());
        }

        public static void Error(LogFormat content)
        {
            content._sysFlag = "error";
            _logger.Error(content.ToJson());
        }

        public static void Fatal(LogFormat content)
        {
            content._sysFlag = "fatal";
            _logger.Fatal(content.ToJson());
        }
    }
}
