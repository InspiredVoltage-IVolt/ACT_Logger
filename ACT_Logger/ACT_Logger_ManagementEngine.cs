using ACT.Core.Extensions;
using ACT.Core.Interfaces.Common;
using ACT.Core.Types.PluginPackage;
using System.Linq;

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
        /// Holds the Current Loaded Configuration RAW JSON Data (Either Default Config or Plugin Config)
        /// </summary>
        private static string _RawJSON = null;

        /// <summary>
        /// Active Base Path = Resources\\ACT_Logger\\
        /// </summary>
        internal static string ActiveBasePath = System_BaseACT_LoggerDirectory;

        /// <summary>
        /// Defines if the Plugin isdefined and being used
        /// </summary>
        public static bool UsingPlugin { get; internal set; }

        #region Plugin Variables
        /// <summary>
        /// Holds The Plugin Class Once Loaded
        /// </summary>
        internal static I_ErrorLoggable Plugin = null;

        /// <summary>
        /// Holds the Plugin Package Assembly
        /// </summary>
        internal static System.Reflection.Assembly _PluginAssembly = null;

        /// <summary>
        /// Plugin Package Definition Variable
        /// </summary>
        internal static ACT_Plugin_Package_Definition Plugin_Package_Data = null;

        /// <summary>
        /// Plugin Configuration Path
        /// </summary>
        internal static string Plugin_Configuration_Path = null;

        /// <summary>
        /// Plugin Full Configuration File Path
        /// </summary>
        internal static string Plugin_Full_Configuration_File_Path = null;

        /// <summary>
        /// Plugin Active DLL Full Path
        /// </summary>
        internal static string Plugin_Full_Dll_Path = null;

        /// <summary>
        /// Plugin Full ClassName
        /// </summary>
        internal static string Plugin_Full_Class_Name = null;

        #endregion

        /// <summary>
        /// Simple Is Ready Checks For Active_Configuration_Type
        /// </summary>        
        internal static bool IsReady { get { return ACT_Logger_ManagementEngine.Active_Configuration_Type != null ? true : false; } }

        /// <summary>
        /// Checks the Statis To Ensure everying Loaded Correctly
        /// </summary>
        /// <returns></returns>
        internal static bool CheckStatus()
        {
            if (!IsReady)
            {
                ACT_Logger_ManagementEngine.LoadConfigurationFile();
                if (!IsReady) { return false; }
            }
            else
            {
                if (UsingPlugin)
                {
                    if (Plugin == null) { return false; }
                    if (Plugin is I_ErrorLoggable) { return true; }
                }
            }
            return true;
        }

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

        /// <summary>
        /// Returns the current Plugin Package Assembly
        /// </summary>
        public static System.Reflection.Assembly PluginAssembly { get { return _PluginAssembly; } }

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
        internal static bool Load_PluginPackage(string PluginPackageName, string FullClassName)
        {
            if (PluginPackageName.NullOrEmpty() || FullClassName.NullOrEmpty()) { throw new Exception("INVALID PARAMETERS: (-100)"); }

            string _CurrentRawJSON = _RawJSON;

            Plugin_Configuration_Path = System_BasePluginPackagesDirectory.EnsureDirectoryFormat() + PluginPackageName + "\\";

            if (Plugin_Configuration_Path.DirectoryExists() == false) { throw new Exception("Unable to Locate The Plugin Package Configuration Path: " + Plugin_Configuration_Path + " (-654356631)"); }
            Plugin_Full_Configuration_File_Path = Plugin_Configuration_Path + "plugin_package_info.json";
            if (Plugin_Full_Configuration_File_Path.FileExists() == false) { throw new Exception("Unable to Locate The Plugin Package Configuration File: " + Plugin_Full_Configuration_File_Path + " (-6543566312)"); }

            _RawJSON = Plugin_Full_Configuration_File_Path.ReadAllText();

            // TRY AND LOAD PLUGIN CONFIGURATION DATA
            try { Plugin_Package_Data = ACT_Plugin_Package_Definition.FromJson(_RawJSON); } catch { throw new Exception("Plugin Data Is InValid (-943516310)"); }

            // IF PLUGIN PACKAGE DATA DIDNT LOAD PROPERLY OR DLLFileName is Null Or Empty
            if (Plugin_Package_Data == null || Plugin_Package_Data.FilesystemConfig.DllFilename.NullOrEmpty()) { throw new Exception("Plugin Data Is InValid (Missing DLL File Name) (-943516310)"); }

            // IF PLUGIN PACKAGE DATA DOESNT CONTAIN DEFINED IMPLEMENTATIONS
            if (Plugin_Package_Data.Included_Plugin_Type_Implementations == null || Plugin_Package_Data.Included_Plugin_Type_Implementations.Count == 0) { throw new Exception("Plugin Data Is Invalid (No Implementations Defined) (-943516310)"); }

            // IF PLUGIN PACKAGED DOESNT DEFINE THE DLLFileName
            if (Plugin_Package_Data.FilesystemConfig.DllFilename.ToLower().EndsWith(".dll") == false) { throw new Exception("Plugin Data Is Invalid DLLFileName:(" + Plugin_Package_Data.FilesystemConfig.DllFilename + ") (-943516310)"); }

            // Set Default Plugin DLL Path
            Plugin_Full_Dll_Path = Plugin_Configuration_Path.EnsureDirectoryFormat() + Plugin_Package_Data.FilesystemConfig.DllFilename;

            // Check For Plugin DLL in Both Original Path and Configurable Path
            if (Plugin_Full_Dll_Path.FileExists() == false)
            {
                if (Plugin_Package_Data.FilesystemConfig.FilesystemRoot.NullOrEmpty() || Plugin_Package_Data.FilesystemConfig.FilesystemRoot.DirectoryExists() == false)
                {
                    throw new Exception("Plugin Data Is Invalid Defined File System Path:(" + Plugin_Package_Data.FilesystemConfig.FilesystemRoot + ") (-943516310)");
                }
                else
                {
                    Plugin_Full_Dll_Path = Plugin_Package_Data.FilesystemConfig.FilesystemRoot.EnsureDirectoryFormat() + Plugin_Package_Data.FilesystemConfig.DllFilename;
                    if (Plugin_Full_Dll_Path.FileExists() == false) { throw new Exception("Plugin Data Is Unable To File PluginDLL:(" + Plugin_Full_Dll_Path + ") (-943516310)"); }
                }
            }

            // Load the Assembly from the Found File
            try { _PluginAssembly = System.Reflection.Assembly.LoadFrom(Plugin_Full_Dll_Path); }
            catch { throw new Exception("Unable to create the Assembly: " + Plugin_Full_Dll_Path + " (-540231783)"); }

            // Create Instance
            try { Plugin = (I_ErrorLoggable)_PluginAssembly.CreateInstance(Plugin_Full_Class_Name); }
            catch (Exception ex) { throw new Exception("Invalid ClassName/DLL - Plugin Not Able To Create an Instance: " + Plugin_Full_Dll_Path + " (-540231783)", ex); }

            // IF Plugin Is Null Load The Plugin From The Settings Defined
            if (Plugin == null) { throw new Exception("Invalid ClassName/DLL - Plugin Not Able To Create an Instance -- NULL: " + Plugin_Full_Dll_Path + " (-540231783)"); }

            UsingPlugin = true;
            return true;
        }

        public static void RemovePlugin()
        {
            UsingPlugin = false;
            Plugin = null;
            _PluginAssembly = null;
            Plugin_Package_Data = null;
            Plugin_Configuration_Path = null;
            Plugin_Full_Configuration_File_Path = null;
            Plugin_Full_Dll_Path = null;
            Plugin_Full_Class_Name = null;
        }
    }
}
