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
    ///    A = 1554981925 - USED
    ///    B = 1554981928 - USED
    ///    C = 1554981927 - USED
    ///    D = 1554981930
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

        internal static string BaseDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; } }
        internal static Configuration.ADONet_ConfigurationData ADOConfig = null;
        internal static Configuration.FileSystem_ConfigurationData FileSystemConfig = null;
        internal static Configuration.WebService_ConfigurationData WebServiceConfig = null;
        internal static Configuration.WindowsEvent_ConfigurationData WindowsEventConfig = null;


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


        public static SystemTypes? LoadedConfigurationType = null;
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


            var _SearchData = System.IO.Directory.GetFiles(, "config.json", SearchOption.AllDirectories);

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

                if (_RawJSON.Contains("FileSystem")) { LoadedConfigurationType = SystemTypes.FileSystem; FileSystemConfig = Configuration.FileSystem_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("WebService")) { LoadedConfigurationType = SystemTypes.WebService; WebServiceConfig = Configuration.WebService_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("WindowsEventLog")) { LoadedConfigurationType = SystemTypes.WindowsEventLog; WindowsEventConfig = Configuration.WindowsEvent_ConfigurationData.FromJson(_RawJSON); }
                if (_RawJSON.Contains("ADONetDatabase")) { LoadedConfigurationType = SystemTypes.ADODatabase; ADOConfig = Configuration.ADONet_ConfigurationData.FromJson(_RawJSON); }
            }
            catch (Exception ex)
            {
                LoadedConfigurationType = null;
                throw new Exception("Error Locating - Loading Configuration Data (1554981927)", ex);
            }
        }

        internal static bool IsReady { get { return ACT_Logger_ManagementEngine.LoadedConfigurationType != null ? true : false; } }

        internal static bool CheckStatus(SystemTypes TypeStatus)
        {
            if (!IsReady) { ACT_Logger_ManagementEngine.LoadConfigurationFile(); }
            if (!IsReady) { return false; }

            return true;
        }
    }
}
