using System;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;
using System.Linq;
namespace am_client.Common
{
    /// <summary>
    /// 从 true 转换为 false 以及进行相反转换的值转换器。
    /// </summary>
    public sealed class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var list = value as System.Collections.Generic.IEnumerable<string>;
            return string.Join(" ", list);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }
}
