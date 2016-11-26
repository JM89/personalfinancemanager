using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils
{
    public class Config
    {
        public static List<string> BankIconAllowedExtensions
        {
            get { return GetConfig().Split(';').ToList(); }
        }

        public static long BankIconMaxSize
        {
            get { return Convert.ToInt64(GetConfig()); }
        }

        public static string BankIconBasePath
        {
            get { return GetConfig(); }
        } 

        private static string GetConfig(string settingName = null, [CallerMemberName] string callerProperty = "")
        {
            settingName = settingName ?? callerProperty;
            return ConfigurationManager.AppSettings[settingName] ?? string.Empty;
        }
    }
}
