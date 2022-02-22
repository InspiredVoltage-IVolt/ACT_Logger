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
    ///  Main Built in ACT_Logger Class
    /// 
    ///  - A 
    ///  - B
    ///  - C
    ///  - D    
    /// </summary>
    public class ACT_ADONet_Logger : I_ErrorLoggable
    {
        internal static I_Plugin OverRide_ErrorLoggablePlugin = null;

        public void DLogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (-620587607)"); }
        }

        public void LogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (-620587607)"); }
        }

        public bool OverRidePlugin(I_Plugin pluginData)
        {
            if (pluginData == null) { throw new Exception("Plugin Data Is Null (-620587610)"); }
            if (pluginData.TypesAndClassNames.ContainsKey(typeof(I_ErrorLoggable)))
            {
                OverRide_ErrorLoggablePlugin = pluginData;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void QuickLog(string Information)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (-620587607)"); }
        }
    }
}
