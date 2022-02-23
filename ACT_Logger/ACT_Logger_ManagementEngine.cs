using ACT.Core.Extensions;
using ACT.Core.Interfaces.Common;
using ACT.Core.Logger.Types;
using ACT.Core.Types.PluginPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACT.Core.Logger
{
    /// <summary>
    ///  Internal Management Class - Helps Loads Configuration Data.
    /// FILE ERROR CODES
    ///    A = 1554981925   - USED
    ///    B = 1554981928   - USED
    ///    C = 1554981927   - USED
    ///    D = 1554981930   - USED
    ///    E = -943516310   - USED
    ///    F = 622567631    - USED
    ///    G = -2106315724  - USED
    ///    H = -540231783   - USED
    ///    I = -162256631   - USED
    /// 
    /// </summary>
    public static class ACT_Logger_ManagementEngine
    {
        #region Enums
        [Flags]
        public enum SystemTypes
        {
            FileSystem = 0, ADODatabase = 1, WebService = 2, WindowsEventLog = 3
        }

        [Flags]
        public enum EncryptionMethods
        {
            WindowsMachineProtectedData, WindowsUserProtectedData, ACTDefault, CustomIEncryptionPlugin
        }
        #endregion

        private static string _RawJSON = null;

        internal static string System_BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        internal static Configuration.ADONet_ConfigurationData ADOConfig = null;
        internal static Configuration.FileSystem_ConfigurationData FileSystemConfig = null;
        internal static Configuration.WebService_ConfigurationData WebServiceConfig = null;
        internal static Configuration.WindowsEvent_ConfigurationData WindowsEventConfig = null;

        internal static string ActiveBasePath = System_BaseDirectory;

        internal static I_ErrorLoggable Plugin = null;
        internal static Plugin_Assembly_Information OverRide_ErrorLoggablePlugin = null;
        internal static string Plugin_FullClassName = null;

        #region READONLY CONSTANTS
        public static readonly string LOGSYSTEMTYPES_FILESYSTEM = "FileSystem";
        public static readonly string LOGSYSTEMTYPES_ADONETDATABASE = "ADONetDatabase";
        public static readonly string LOGSYSTEMTYPES_WEBSERVICE = "WebService";
        public static readonly string LOGSYSTEMTYPES_WINDOWSEVENTLOG = "WindowsEventLog";

        public static readonly string ENCRYPTION_METHOD_WINDOWSMACHINE_PROTECTED = "Microsoft-ProtectedData-Machine";
        public static readonly string ENCRYPTION_METHOD_WINDOWS_USER_PROTECTED = "Microsoft-ProtectedData-User";
        public static readonly string ENCRYPTION_METHOD_ACTDEFAULT = "ACT-Security-Default";
        public static readonly string ENCRYPTION_METHOD_CUSTOM_PLUGIN = "ACT-Custom-Plugin";
        #endregion


        public static SystemTypes? Active_Configuration_Type = null;
        public static List<string> ConfigurationFilesFound = new List<string>();
        public static string UsedConfigurationFile = null;
        public static string RawJSON { get { return _RawJSON; } internal set { _RawJSON = value; } }


        /// <summary>
        /// Get System Types By Enum
        /// </summary>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static string GetSystemTypeString(SystemTypes typ)
        {
            if (typ == SystemTypes.FileSystem) { return LOGSYSTEMTYPES_FILESYSTEM; }
            else if (typ == SystemTypes.ADODatabase) { return LOGSYSTEMTYPES_ADONETDATABASE; }
            else if (typ == SystemTypes.WebService) { return LOGSYSTEMTYPES_WEBSERVICE; }
            else if (typ == SystemTypes.WindowsEventLog) { return LOGSYSTEMTYPES_WINDOWSEVENTLOG; }
            else { return null; }
        }

        /// <summary>
        /// Get Encryption Method String By Enum
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetEncryptionMethodsString(EncryptionMethods method)
        {
            if (method == EncryptionMethods.WindowsMachineProtectedData) { return ENCRYPTION_METHOD_WINDOWSMACHINE_PROTECTED; }
            else if (method == EncryptionMethods.WindowsUserProtectedData) { return ENCRYPTION_METHOD_WINDOWS_USER_PROTECTED; }
            else if (method == EncryptionMethods.ACTDefault) { return ENCRYPTION_METHOD_ACTDEFAULT; }
            else if (method == EncryptionMethods.CustomIEncryptionPlugin) { return ENCRYPTION_METHOD_CUSTOM_PLUGIN; }
            else { return null; }
        }

        /// <summary>
        /// Load All The configuration File Information Into the Appropriate Variable.
        /// </summary>
        /// <exception cref="Exception">No Config Files Found, Invalid Config Files Found, Error Loading Config File. (</exception>
        internal static void LoadConfigurationFile()
        {
            string _PathSearchBase = AppDomain.CurrentDomain.BaseDirectory + "Resources\\";
            var _tmpFileList = new List<string>();

            var _SearchData = System.IO.Directory.GetFiles(ActiveBasePath, "config.json", SearchOption.AllDirectories);

            if (_SearchData.Count() != 1) { throw new Exception("Error Locating Single Configuration File. (1554981925)"); }

            ConfigurationFilesFound.AddRange(_SearchData);

            _tmpFileList.AddRange(ConfigurationFilesFound);

            UsedConfigurationFile = _SearchData.First();

            while (!System.IO.File.Exists(UsedConfigurationFile))
            {
                _tmpFileList.Remove(UsedConfigurationFile);
                if (_tmpFileList.Count() > 0) { UsedConfigurationFile = _tmpFileList.First(); }
                else { throw new Exception("No Valid Configuration File Found (1554981928)"); }
            }

            try
            {
                _RawJSON = System.IO.File.ReadAllText(UsedConfigurationFile);

                if (_RawJSON.Contains("FileSystem")) { Active_Configuration_Type = SystemTypes.FileSystem; FileSystemConfig = Configuration.FileSystem_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("WebService")) { Active_Configuration_Type = SystemTypes.WebService; WebServiceConfig = Configuration.WebService_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("WindowsEventLog")) { Active_Configuration_Type = SystemTypes.WindowsEventLog; WindowsEventConfig = Configuration.WindowsEvent_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("ADONetDatabase")) { Active_Configuration_Type = SystemTypes.ADODatabase; ADOConfig = Configuration.ADONet_ConfigurationData.FromJson(_RawJSON); }
            }
            catch (Exception ex)
            {
                Active_Configuration_Type = null;
                throw new Exception("Error Locating - Loading Configuration Data (1554981927)", ex);
            }
        }

        internal static bool IsReady { get { return ACT_Logger_ManagementEngine.Active_Configuration_Type != null ? true : false; } }

        internal static bool CheckStatus()
        {
            if (!IsReady) { ACT_Logger_ManagementEngine.LoadConfigurationFile(); }
            if (!IsReady) { return false; }

            return true;
        }

        internal static bool LoadOverRide_Plugin(ACT_Plugin_Package_Definition pluginData)
        {
            if (pluginData == null) { throw new Exception("Plugin Data Is Null (-943516310)"); }
            if (pluginData.Included_Plugin_Type_Implementations.First()..NullOrEmpty()) { return false; }

            // SAVE CURRENT STATE
            SystemTypes? _OriginalType = Active_Configuration_Type;

            if (IsReady == false) { throw new Exception("System Is Not Ready. Please Check Configuration File (1554981930)"); }
           
            OverRide_ErrorLoggablePlugin = pluginData;             
            
            #region SET ACTIVE CONFIG TYPE

            if (OverRide_ErrorLoggablePlugin.SystemTypeIdentifier == LOGSYSTEMTYPES_FILESYSTEM)
            {
                Active_Configuration_Type = SystemTypes.FileSystem;
            }
            else if (OverRide_ErrorLoggablePlugin.SystemTypeIdentifier == LOGSYSTEMTYPES_ADONETDATABASE)
            {
                Active_Configuration_Type = SystemTypes.ADODatabase;
            }
            else if (OverRide_ErrorLoggablePlugin.SystemTypeIdentifier == LOGSYSTEMTYPES_WEBSERVICE)
            {
                Active_Configuration_Type = SystemTypes.WebService;
            }
            else if (OverRide_ErrorLoggablePlugin.SystemTypeIdentifier == LOGSYSTEMTYPES_WINDOWSEVENTLOG)
            {
                Active_Configuration_Type = SystemTypes.WindowsEventLog;
            }
            else
            {
                throw new Exception("Unidentified Configuration Type Found: " + OverRide_ErrorLoggablePlugin.SystemTypeIdentifier + "(-162256631)");
            }

            #endregion

            // IF Plugin Is Null Load The Plugin From The Settings Defined
            if (Plugin == null)
            {
                // IF Unique Path Exists - Set the Active Base PAth tp the Defined Plugin Override Location
                if (System.IO.Directory.Exists(ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot))
                {
                    ActiveBasePath = ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot;
                }

                // Get All The Files That Match The Plugin Information DLLFileName
                var _PluginFilesFound = System.IO.Directory.GetFiles(ActiveBasePath, OverRide_ErrorLoggablePlugin.DllFileName, SearchOption.AllDirectories);

                // If More than one file or no Files are Found Reset the ActiveBasePAth and throw an Exception
                if (_PluginFilesFound.Count() != 1)
                {
                    ActiveBasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
                    throw new Exception("Error - Path Contains Multiple DLLs with the same name (-2106315724)");
                }

                // Load the Assembly from the Found File
                var _tmpAssembly = System.Reflection.Assembly.LoadFrom(_PluginFilesFound[0]);

                // Loop over all Keys looking for I_ErrorLoggable
                foreach (Type t in OverRide_ErrorLoggablePlugin.TypesAndClassNames.Keys)
                {
                    if (t is I_ErrorLoggable)
                    {
                        Plugin_FullClassName = OverRide_ErrorLoggablePlugin.TypesAndClassNames[t].OrderBy(x => x.Key).First().Value;

                        if (Plugin_FullClassName == null)
                        {
                            ActiveBasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
                            Active_Configuration_Type = _OriginalType;
                            throw new Exception("Invalid Class Name (-540231783)");
                        }

                        try { Plugin = (I_ErrorLoggable)_tmpAssembly.CreateInstance(Plugin_FullClassName); }
                        catch (Exception ex)
                        {
                            ActiveBasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
                            Active_Configuration_Type = _OriginalType;
                            throw new Exception("Invalid Class Name - Plugin Not Able To Create an Instance (-540231783)", ex);
                        }

                        if (Plugin != null) { return true; }
                    }
                }
            }

            // If it Gets Here Then it Failed and We Reset All Adjusted Settings.
            ActiveBasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
            Active_Configuration_Type = _OriginalType;
            return false;
        }
    }
}
