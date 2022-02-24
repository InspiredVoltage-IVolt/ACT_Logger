using ACT.Core.Extensions;
using ACT.Core.Interfaces.Common;
using ACT.Core.Types.PluginPackage;

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
    ///    J = -654356631   - USED
    ///    K = -268956631   - USED
    ///    L = -111367331   - USED
    /// </summary>
    public static class ACT_Logger_ManagementEngine
    {
        #region Enums + Enum Support Functions
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

        #endregion

        #region Important Variables

        /// <summary>
        /// Holds the Current Loaded Configuration RAW JSON Data
        /// </summary>
        private static string _RawJSON = null;

        /// <summary>
        /// Active Base Path - Only Altered By a Plugin Override
        /// </summary>
        internal static string ActiveBasePath = System_BaseDirectory;

        /// <summary>
        /// Holds The Plugin Class Once Loaded
        /// </summary>
        internal static I_ErrorLoggable Plugin = null;

        /// <summary>
        /// Plugin Package Definition Variable
        /// </summary>
        internal static ACT_Plugin_Package_Definition PluginPackage = null;

        /// <summary>
        /// Plugin Active DLL Full Path
        /// </summary>
        internal static string Plugin_FullDLLPath = null;

        /// <summary>
        /// Plugin Full ClassName
        /// </summary>
        internal static string Plugin_FullClassName = null;

        #endregion

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

        #region Directory Information Variables
        internal static string System_BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        internal static string System_BaseResourcesDirectory { get { return System_BaseDirectory.EnsureDirectoryFormat() + "Resources\\"; } }
        internal static string System_BaseACT_LoggerDirectory { get { return System_BaseResourcesDirectory.EnsureDirectoryFormat() + "ACT_Logger\\"; } }
        internal static string System_BasePluginPackagesDirectory { get { return System_BaseDirectory.EnsureDirectoryFormat() + "Plugins\\"; } }
        #endregion

        #region Configuration Data Variables
        internal static Configuration.ADONet_ConfigurationData ADOConfig = null;
        internal static Configuration.FileSystem_ConfigurationData FileSystemConfig = null;
        internal static Configuration.WebService_ConfigurationData WebServiceConfig = null;
        internal static Configuration.WindowsEvent_ConfigurationData WindowsEventConfig = null;
        #endregion

        public static SystemTypes? Active_Configuration_Type = null;
        public static string UsedConfigurationFile = null;
        public static string RawJSON { get { return _RawJSON; } internal set { _RawJSON = value; } }

        internal static bool IsReady { get { return ACT_Logger_ManagementEngine.Active_Configuration_Type != null ? true : false; } }
        internal static bool CheckStatus()
        {
            if (!IsReady) { ACT_Logger_ManagementEngine.LoadConfigurationFile(); }
            if (!IsReady) { return false; }

            return true;
        }

        internal static bool ValidatePaths()
        {
            if (Directory.Exists(System_BaseDirectory) == false) { return false; }
            if (Directory.Exists(System_BaseResourcesDirectory) == false) { return false; }
            if (Directory.Exists(System_BaseACT_LoggerDirectory) == false) { return false; }
            if (Directory.Exists(System_BasePluginPackagesDirectory) == false) { return false; }

            return true;
        }

        /// <summary>
        /// Load All The configuration File Information Into the Appropriate Variable.
        /// </summary>
        /// <exception cref="Exception">No Config Files Found, Invalid Config Files Found, Error Loading Config File. (</exception>
        internal static void LoadConfigurationFile()
        {
            if (ValidatePaths() == false) { throw new Exception("Defined Paths Are Not All Found: (-162256631)"); }

            var _tmpFileList = new List<string>();

            var _SearchData = Directory.GetFiles(System_BaseACT_LoggerDirectory, "config.json", SearchOption.AllDirectories);

            if (_SearchData.Count() != 1) { throw new Exception("Error Locating Single Configuration File. (1554981925)"); }

            UsedConfigurationFile = _SearchData.First();

            /*  
            // REMOVE OLD CODE
            while (!File.Exists(UsedConfigurationFile))
            {
                _tmpFileList.Remove(UsedConfigurationFile);
                if (_tmpFileList.Count() > 0) { UsedConfigurationFile = _tmpFileList.First(); }
                else { throw new Exception("No Valid Configuration File Found (1554981928)"); }
            }
            */

            try
            {
                // Read All JSON Data
                _RawJSON = File.ReadAllText(UsedConfigurationFile);

                if (_RawJSON.Contains("FileSystem"))
                {
                    Active_Configuration_Type = SystemTypes.FileSystem;
                    FileSystemConfig = Configuration.FileSystem_ConfigurationData.FromJson(_RawJSON);
                    if (FileSystemConfig.FilesystemConfig.Plugin_Package.NullOrEmpty() == false)
                    {

                        "plugin-package": "ACT_Example_Package\\plugin_package_info.json",
                    "plugin-full-classname": "",
                            }
                }
                if (_RawJSON.Contains("WebService"))
                {
                    Active_Configuration_Type = SystemTypes.WebService;
                    WebServiceConfig = Configuration.WebService_ConfigurationData.FromJson(_RawJSON);
                }
                if (_RawJSON.Contains("WindowsEventLog"))
                {
                    Active_Configuration_Type = SystemTypes.WindowsEventLog;
                    WindowsEventConfig = Configuration.WindowsEvent_ConfigurationData.FromJson(_RawJSON);
                }
                if (_RawJSON.Contains("ADONetDatabase"))
                {
                    Active_Configuration_Type = SystemTypes.ADODatabase;
                    ADOConfig = Configuration.ADONet_ConfigurationData.FromJson(_RawJSON);
                }
            }
            catch (Exception ex)
            {
                Active_Configuration_Type = null;
                throw new Exception("Error Locating - Loading Configuration Data (1554981927)", ex);
            }
        }

        /// <summary>
        /// Load Plugin Package
        /// </summary>
        /// <param name="pluginData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static bool Load_PluginPackage(ACT_Plugin_Package_Definition pluginData, string FullClassName)
        {
            if (pluginData == null || FullClassName.NullOrEmpty()) { throw new Exception("Plugin Data Is InValid (-943516310)"); }
            if (pluginData.Included_Plugin_Type_Implementations == null || pluginData.Included_Plugin_Type_Implementations.Count == 0 || FullClassName.NullOrEmpty()) { throw new Exception("Plugin Data Is Invalid (-943516310)"); }

            // if (IsReady == false) { throw new Exception("System Is Not Ready. Please Check Configuration File (1554981930)"); }

            PluginPackage = pluginData;

            if (PluginPackage.Included_Plugin_Type_Implementations.Exists(x => x.FullClassName == FullClassName) == false)
            {
                throw new Exception("Unidentified Configuration Type Found: " + FullClassName + "(-162256631)");
            }

            if (pluginData.FilesystemConfig.FilesystemRoot.NullOrEmpty() == false)
            {
                if (pluginData.FilesystemConfig.FilesystemRoot.DirectoryExists())
                {
                    var _PluginSearchFiles = pluginData.FilesystemConfig.FilesystemRoot.FindAllFileReferencesInPath(pluginData.FilesystemConfig.DllFilename, true);
                    if (_PluginSearchFiles == null || _PluginSearchFiles.Count() != 1)
                    {
                        throw new Exception("Unable to Locate The Plugin DLL: " + pluginData.FilesystemConfig.DllFilename.TryToString("") + " (-654356631)");
                    }
                    Plugin_FullDLLPath = _PluginSearchFiles.First();
                }
                else { throw new Exception("Unable to Locate The Plugin DLL: " + pluginData.FilesystemConfig.DllFilename.TryToString("") + " (-654356631)"); }
            }
            else { }


            // Load the Assembly from the Found File
            var _tmpAssembly = System.Reflection.Assembly.LoadFrom(Plugin_FullDLLPath);

            // Create Instance
            try { Plugin = (I_ErrorLoggable)_tmpAssembly.CreateInstance(Plugin_FullClassName); }
            catch (Exception ex)
            {
                throw new Exception("Invalid Class Name - Plugin Not Able To Create an Instance (-540231783)", ex);
            }


            // IF Plugin Is Null Load The Plugin From The Settings Defined
            if (Plugin == null)
            {
                // IF Unique Path Exists - Set the Active Base PAth tp the Defined Plugin Override Location
                if (System.IO.Directory.Exists(ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot))
                {
                    ActiveBasePath = ACT_Logger_ManagementEngine.FileSystemConfig.FilesystemConfig.FilesystemRoot;
                }

                // Get All The Files That Match The Plugin Information DLLFileName
                var _PluginFilesFound = System.IO.Directory.GetFiles(ActiveBasePath, PluginPackage.DllFileName, SearchOption.AllDirectories);

                // If More than one file or no Files are Found Reset the ActiveBasePAth and throw an Exception
                if (_PluginFilesFound.Count() != 1)
                {
                    ActiveBasePath = ACT_Logger_ManagementEngine.System_BaseDirectory;
                    throw new Exception("Error - Path Contains Multiple DLLs with the same name (-2106315724)");
                }



                // Loop over all Keys looking for I_ErrorLoggable
                foreach (Type t in PluginPackage.TypesAndClassNames.Keys)
                {
                    if (t is I_ErrorLoggable)
                    {
                        Plugin_FullClassName = PluginPackage.TypesAndClassNames[t].OrderBy(x => x.Key).First().Value;

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
