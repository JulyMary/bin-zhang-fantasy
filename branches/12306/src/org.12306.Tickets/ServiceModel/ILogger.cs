using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.ServiceModel
{
    [Serializable]
    public enum MessageImportance {High, Normal, Low}

    public interface ILogger
    {
        void LogMessage(string category, MessageImportance importance, string message, params object[] messageArgs);
        void LogMessage(string category, string message, params object[] messageArgs);
        void LogWarning(string category, Exception exception, MessageImportance importance, string message, params object[] messageArgs);
        void LogWarning(string category, MessageImportance importance, string message, params object[] messageArgs);
        void LogWarning(string category, string message, params object[] messageArgs);
        void LogError(string category, Exception exception, string message, params object[] messageArgs);
        void LogError(string category, string message, params object[] messageArgs);

        void AddListener(ILogListener listener);
        void RemoveListener(ILogListener listener);

    }

    public static class ILoggerExtension
    {
        public static void SafeLogMessage(this ILogger logger, string category, MessageImportance importance, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogMessage(category, importance, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogMessage(this ILogger logger, string category, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogMessage(category, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogWarning(this ILogger logger, string category, Exception exception, MessageImportance importance, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogWarning(category, exception, importance, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogWarning(this ILogger logger, string category, MessageImportance importance, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogWarning(category, importance, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogWarning(this ILogger logger, string category, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogWarning(category, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogError(this ILogger logger, string category, Exception exception, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogError(category, exception, string.Format(message, messageArgs));
            }
        }
        public static void SafeLogError(this ILogger logger, string category, string message, params object[] messageArgs)
        {
            if (logger != null)
            {
                logger.LogError(category, string.Format(message, messageArgs));
            }
        }
    }

    public interface ILogListener
    {
        void OnMessage(string category, MessageImportance importance, string message);
        void OnWaring(string category, Exception exception, MessageImportance importance, string message);
        void OnError(string category, Exception exception, string message);
    }

    
}
