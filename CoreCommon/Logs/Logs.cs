
using CoreCommon.Extensions;
using CoreCommon.LinkZipkin;
using NLog;

namespace CoreCommon.Logs
{
    public class Log
    {
        private static Logger _logger= LogManager.GetCurrentClassLogger();

        public static void Debug(string content)
        {
            _logger.Debug(content);
        } 

        public static void Info(string content)
        {
            _logger.Info(content);
        }

        public static void Warn(string content)
        {
            _logger.Warn(content);
        }

        public static void Error(string content)
        {
            _logger.Error(content);
        }

        public static void Fatal(string content)
        {
            _logger.Fatal(content);
        }
    }
}
