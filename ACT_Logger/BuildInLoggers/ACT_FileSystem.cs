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
        internal static string BasePath = ACT_Logger_ManagementEngine.BaseDirectory;
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
            if (pluginData == null) { throw new Exception("Plugin Data Is Null (-943516310)"); }

            if (pluginData.TypesAndClassNames.ContainsKey(typeof(I_ErrorLoggable))) { OverRide_ErrorLoggablePlugin = pluginData; }
            else { throw new Exception("Plugin Does Not Define I_ErrorLoggable as a Supported Type.  Please Fix Configuration (622567631)"); }

            if (OverRide_ErrorLoggablePlugin == null) { return false; }

            if (Plugin == null)
            {
                if (System.IO.Directory.Exists(ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot))
                {
                    BasePath = ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot;
                }

                var _PluginFilesFound = System.IO.Directory.GetFiles(BasePath, OverRide_ErrorLoggablePlugin.DLLFileName);
                if (_PluginFilesFound.Count() != 1)
                {
                    BasePath = ACT_Logger_ManagementEngine.BaseDirectory;
                    throw new Exception("Error - Path Contains Multiple DLLs with the same name (-2106315724)");
                }

                var _tmpAssembly = System.Reflection.Assembly.LoadFrom(_PluginFilesFound[0]);

                foreach (Type t in OverRide_ErrorLoggablePlugin.TypesAndClassNames.Keys)
                {
                    if (t is I_ErrorLoggable)
                    {
                        if (OverRide_ErrorLoggablePlugin.SubIdentifier == ACT_Logger_ManagementEngine.LOGSYSTEMTYPES_FILESYSTEM)
                        {
                            Plugin_FullClassName = OverRide_ErrorLoggablePlugin.TypesAndClassNames[t].OrderBy(x => x.Key).First().Value;
                            if (Plugin_FullClassName == null)
                            {
                                BasePath = ACT_Logger_ManagementEngine.BaseDirectory;
                                throw new Exception("Invalid Class Name (-540231783)");
                            }

                            try { Plugin = (I_ErrorLoggable)_tmpAssembly.CreateInstance(Plugin_FullClassName); }
                            catch (Exception ex)
                            {
                                BasePath = ACT_Logger_ManagementEngine.BaseDirectory;
                                throw new Exception("Invalid Class Name (-540231783)", ex);
                            }

                            if (Plugin != null) { return true; }
                        }
                    }
                }
            }

            BasePath = ACT_Logger_ManagementEngine.BaseDirectory;
            return false;
        }

        public void QuickLog(string Information)
        {
            if (!ACT_Logger_ManagementEngine.CheckStatus()) { throw new Exception("Error: Configuration File Not Loaded. System Not Ready (1785367045)"); }
        }
    }
}
