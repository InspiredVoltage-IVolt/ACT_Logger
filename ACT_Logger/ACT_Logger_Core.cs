using ACT.Core.Enums;
using ACT.Core.Interfaces.Common;
using ACT.Core.Logger;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT._
{
    /// <summary>
    ///  MAIN ACT Error Entry Point
    ///     
    ///     ERROR NUMBERS
    ///         1883237528   - A - USED
    ///         317153587    - B - USED
    ///         -1248930354  - C - USED
    ///         1123722641   - D
    /// </summary>
    public static class Log
    {
        static I_ErrorLoggable _CurrentLogger = null;

        public static void DLogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1883237528)"); }

# if DEBUG5
            if (ACT_Logger_ManagementEngine.LoadedConfigurationType == ACT_Logger_ManagementEngine.SystemTypes.FileSystem)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.DLogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.LoadedConfigurationType == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.DLogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.LoadedConfigurationType == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.DLogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.LoadedConfigurationType == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.DLogError(className, summary, ex, additionInformation, errorType);
            }
            else
            {
                throw new Exception("Odd Error Unknown Logger Type: (-1248930354)");
            }
#else
            return;
#endif
        }

        public static void LogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1883237528)"); }

            if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.FileSystem)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.LogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.LogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.LogError(className, summary, ex, additionInformation, errorType);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.LogError(className, summary, ex, additionInformation, errorType);
            }
            else
            {
                throw new Exception("Odd Error Unknown Logger Type: (-1248930354)");
            }
        }

        public static bool OverRidePlugin(I_Plugin pluginData)
        {
            if (pluginData == null) { throw new Exception("Plugin Data Is Null (317153587)"); }
            if (pluginData.TypesAndClassNames.ContainsKey(typeof(I_ErrorLoggable)) == false) { return false; }

            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1883237528)"); }

            if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.FileSystem)
            {
                if (_CurrentLogger == null) { _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger(); }
                return _CurrentLogger.OverRidePlugin(pluginData);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                if (_CurrentLogger == null) { _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger(); }
                return _CurrentLogger.OverRidePlugin(pluginData);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                if (_CurrentLogger == null) { _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger(); }
                return _CurrentLogger.OverRidePlugin(pluginData);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                if (_CurrentLogger == null) { _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger(); }
                return _CurrentLogger.OverRidePlugin(pluginData);
            }
            else
            {
                throw new Exception("Odd Error Unknown Logger Type: (-1248930354)");
            }

        }

        public static void QuickLog(string Information)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1883237528)"); }

            if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.FileSystem)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.QuickLog(Information);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.QuickLog(Information);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.QuickLog(Information);
            }
            else if (ACT_Logger_ManagementEngine.Active_Configuration_Type == ACT_Logger_ManagementEngine.SystemTypes.WindowsEventLog)
            {
                _CurrentLogger = new ACT.Core.Logger.BuildInLoggers.ACT_FileSystem_Logger();
                _CurrentLogger.QuickLog(Information);
            }
            else
            {
                throw new Exception("Odd Error Unknown Logger Type: (-1248930354)");
            }
        }
    }
}
