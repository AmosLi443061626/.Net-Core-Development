
using CoreCommon.Extensions;
using NLog;

namespace CoreCommon.Logs
{
    public class Log
    {
        private static Logger _logger= LogManager.GetCurrentClassLogger();

        public static void Debug(LogFormat content)
        {
            content.level = "DEBUG";
            _logger.Debug(content.ToJson());
        } 

        public static void Info(LogFormat content)
        {
            content.level = "INFO";
            _logger.Info(content.ToJson());
        }

        public static void Warn(LogFormat content)
        {
            content.level = "WARN";
            _logger.Warn(content.ToJson());
        }

        public static void Error(LogFormat content)
        {
            content.level = "ERROR";
            _logger.Error(content.ToJson());
        }

        public static void Fatal(LogFormat content)
        {
            content.level = "FATAL";
            _logger.Fatal(content.ToJson());
        }
    }
}
