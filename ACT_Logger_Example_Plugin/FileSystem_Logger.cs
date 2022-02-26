using ACT.Core.Enums;
using ACT.Core.Interfaces.Common;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ACT.Applications.ACT_Logger.Plugins
{
    public class FileSystem_Logger : I_ErrorLoggable
    {
        public FileSystem_Logger()
        {
           if (loadConfiguration() == false) { throw new Exception("Error Loading and Starting Example Plugin"); }
        }

        private string CurrentFileName = "logFileExample.txt";
        private string _baseConfigFilePath = "";
        private ActExamplePackageConfig _configData = null;
        /// <summary>
        /// Returns Package Name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ACT_Example_Package";
        }

        internal bool loadConfiguration()
        {
            try
            {
                string _ConfigurationFileName = "ACT_Example_Package".GetHashCode().ToString();
                _baseConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\Plugins\\" + this.ToString() + "\\" + _ConfigurationFileName + ".json";
                _configData = ActExamplePackageConfig.FromJson(File.ReadAllText(_baseConfigFilePath));
            }
            catch
            {
                _configData = null;
                return false;
            }

            var _AllFilesInLogPath = System.IO.Directory.GetFiles(_configData.Filesystem.Path, "*." + _configData.Filesystem.Fileformat.Split('.', StringSplitOptions.RemoveEmptyEntries).Last());
            CurrentFileName = _configData.Filesystem.Fileformat.Replace("MM-YYYY", DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString());

            if (_AllFilesInLogPath.Contains(_configData.Filesystem.Path + CurrentFileName)==false)
            {
                File.WriteAllText(_configData.Filesystem.Path + CurrentFileName, "File Created ON: " + DateTime.Now.ToString() + Environment.NewLine + Environment.NewLine);
            }

            return true;
        }

        public void DLogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            #if DEBUG
                LogError(className, summary, ex, additionInformation, errorType);
            #endif

            if (errorType == ErrorLevel.Critical)
            {
                LogError(className, summary, ex, additionInformation, errorType);
            }
        }

        public void LogError(string className, string summary, Exception ex, string additionInformation, ErrorLevel errorType)
        {
            if (errorType== ErrorLevel.Critical)
            {
                if (_configData.Emailcriticalerrors.ToString().ToLower() == "true")
                {

                }
            }
        }

        public bool OverRidePlugin(I_Plugin pluginData)
        {
            throw new NotImplementedException();
        }

        public void QuickLog(string Information)
        {
            throw new NotImplementedException();
        }

        private void SendEmail(string Address, string Subject, string Body)
        {
            using (var client = new SmtpClient())
            {
                // Note: don't set a timeout unless you REALLY know what you are doing.
                //client.Timeout = 1000 * 20;

            //    client.Connect("smtp.mail.me.com", 587, SecureSocketOptions.StartTls);
               // client.Authenticate(_configData., password);
             //   client.Send(message);
            }
        }
    }
}