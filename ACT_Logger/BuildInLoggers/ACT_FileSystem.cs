using ACT.Core.Enums;
using ACT.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.Core.Logger.BuildInLoggers
{
    /// <summary>
    /// 
    ///  ACT File System Logger Master
    ///  
    ///  1785367045     - A - USED
    ///  -943516310     - B - USED
    ///  622567631      - C - USED
    ///  -2106315724    - D - USED
    ///  -540231783     - E - USED
    /// </summary>
    public class ACT_FileSystem_Logger : I_ErrorLoggable
    {
        internal static I_Plugin OverRide_ErrorLoggablePlugin = null;
        internal static I_ErrorLoggable Plugin = null;
        internal static string BasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
        internal static string Plugin_FullClassName = null;

        public void DLogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1785367045)"); }

        }

        public void LogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1785367045)"); }
        }

        public bool OverRidePlugin(I_Plugin pluginData)
        {
            return false;
        }

        public void QuickLog(string Information)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1785367045)"); }
        }
    }
}
