using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace am_client.Common
{
    public static class AppSettings
    {
        public static T Get<T>(string key,T defaultValue=default(T))
        {
            try
            {
                return (T)Windows.Storage.ApplicationData.Current.RoamingSettings.Values[key];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        public static void Set(string key, object val)
        {
            Windows.Storage.ApplicationData.Current.RoamingSettings.Values[key] = val;
        }
    }
}
