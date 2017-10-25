using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace BizTalk.PipelineComponents
{
    public static class Utils
    {
        private static Dictionary<string, string> defaultNamespaces = null;

        
        public static Dictionary<string, string> DefaultNamespaces
        {
            get
            {
                if (defaultNamespaces == null)
                { 
                    defaultNamespaces = new Dictionary<string, string>();
                    defaultNamespaces.Add("XMLNORM", "http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties");
                    defaultNamespaces.Add("SBMessaging", "http://schemas.microsoft.com/BizTalk/2012/Adapter/BrokeredMessage-properties");
                    defaultNamespaces.Add("POP3", "http://schemas.microsoft.com/BizTalk/2003/pop3-properties");
                    defaultNamespaces.Add("MSMQT", "http://schemas.microsoft.com/BizTalk/2003/msmqt-properties");
                    defaultNamespaces.Add("ErrorReport", "http://schemas.microsoft.com/BizTalk/2005/error-report");
                    defaultNamespaces.Add("EdiOverride", "http://schemas.microsoft.com/BizTalk/2006/edi-properties");
                    defaultNamespaces.Add("EDI", "http://schemas.microsoft.com/Edi/PropertySchema");
                    defaultNamespaces.Add("EdiIntAS", "http://schemas.microsoft.com/BizTalk/2006/as2-properties");
                    defaultNamespaces.Add("BTF2", "http://schemas.microsoft.com/BizTalk/2003/btf2-properties");
                    defaultNamespaces.Add("BTS", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
                    defaultNamespaces.Add("FILE", "http://schemas.microsoft.com/BizTalk/2003/file-properties");
                    defaultNamespaces.Add("MessageTracking", "http://schemas.microsoft.com/BizTalk/2003/messagetracking-properties");
                    defaultNamespaces.Add("FTP", "http://schemas.microsoft.com/BizTalk/2003/ftp-properties");
                    defaultNamespaces.Add("HTTP", "http://schemas.microsoft.com/BizTalk/2003/http-properties");
                    defaultNamespaces.Add("LEGACY ", "http://schemas.microsoft.com/BizTalk/2003/legacy-properties");
                    defaultNamespaces.Add("MIME", "http://schemas.microsoft.com/BizTalk/2003/mime-properties");
                   // defaultNamespaces.Add("CST", "http://BizTalk.PipelineComponents.PromoteContext/propertys/notfound");
                }

                return defaultNamespaces;
            }
        }
        public static string DecodeSpecialCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                });
        }

        public static string EncodeSpecialCharacters(string value)
        {
            string result = value.Replace("=", @"\u003D");

            result = result.Replace("#", @"\u0023");

            result = result.Replace(";", @"\u003B");

            return result;
        }

    }
}
