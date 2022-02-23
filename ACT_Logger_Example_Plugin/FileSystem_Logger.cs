using ACT.Core.Enums;
using ACT.Core.Interfaces.Common;

namespace ACT.Applications.ACT_Logger.Plugins
{
    public class FileSystem_Logger : I_ErrorLoggable
    {
        /// <summary>
        /// Identifier Redundant Flag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "FileSystem";
        }

        public void DLogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            throw new NotImplementedException();
        }

        public void LogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            throw new NotImplementedException();
        }

        public bool OverRidePlugin(I_Plugin pluginData)
        {
            throw new NotImplementedException();
        }

        public void QuickLog(string Information)
        {
            throw new NotImplementedException();
        }
    }
}