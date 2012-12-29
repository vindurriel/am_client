using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace am_client
{
    public sealed partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            this.InitializeComponent();
            Load();
        }
        public void Load()
        {
            foreach (var x in settings.Children)
            {
                if (x is TextBox || x is ToggleSwitch)
                {
                    _load(x as FrameworkElement);
                }
            }
        }

        void _load(FrameworkElement s)
        {
            var key = parseControl(s);
            if (string.IsNullOrEmpty(key))
                return;
            if (s is TextBox)
            {
                var obj = s as TextBox;
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
                    obj.Text = (string)ApplicationData.Current.RoamingSettings.Values[key];
                obj.TextChanged -= _save;
                obj.TextChanged += _save;
            }
            //todo:add new control types
            else
            {
                return;
            }
        }
        void _save(object obj, RoutedEventArgs e)
        {
            var s = obj as FrameworkElement;
            var key = parseControl(s);
            if (string.IsNullOrEmpty(key))
                return;
            object val = null;
            if (s is TextBox)
            {
                var x = s as TextBox;
                val = x.Text;
            }
            //todo:add new control types
            else
            {
                return;
            }

            ApplicationData.Current.RoamingSettings.Values[key] = val;
        }
        string parseControl(FrameworkElement s)
        {
            var name = s.Name;
            if (string.IsNullOrEmpty(name) || !name.Contains("_"))
                return null;
            var storage = (s as FrameworkElement).Name.Split("_".ToCharArray(), 2);
            return storage[1];
        }
    }
}
