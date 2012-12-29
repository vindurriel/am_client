using System;
using Windows.UI.Xaml.Data;
using System.Linq;
namespace am_client.Common
{
    public static class DateTimeHelper
    {
        public static DateTime ParseUnixTime(this string input)
        {
            var res= DateTime.Parse(input);
            return res;
        }
    }
    
}