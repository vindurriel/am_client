using am_client.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “分组项页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234231 上提供

namespace am_client
{
    /// <summary>
    /// 显示分组的项集合的页。
    /// </summary>
    public sealed partial class GroupedItemsPage : am_client.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: 创建适用于问题域的合适数据模型以替换示例数据
            this.DefaultViewModel["Groups"] = MessageDataSource.AllGroups;
        }

        /// <summary>
        /// 在单击组标题时进行调用。
        /// </summary>
        /// <param name="sender">用作选定组的组标题的 Button。</param>
        /// <param name="e">描述如何启动单击的事件数据。</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // 确定 Button 实例表示的组
            var group = (sender as FrameworkElement).DataContext;

            // 导航至相应的目标页，并
            // 通过将所需信息作为导航参数传入来配置新页
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataCommon)group).UniqueId);
        }

        /// <summary>
        /// 在单击组内的项时进行调用。
        /// </summary>
        /// <param name="sender">显示所单击项的 GridView (在应用程序处于对齐状态时
        /// 为 ListView)。</param>
        /// <param name="e">描述所单击项的事件数据。</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 导航至相应的目标页，并
            // 通过将所需信息作为导航参数传入来配置新页
            var itemId = ((SampleDataCommon)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }
        private void btn_show_settings_click(object sender, RoutedEventArgs e) 
        {
            this.BottomAppBar.IsOpen = false;
            (App.Current as App).ShowSettings();
        }
        private void btn_show_toast_click(object sender, RoutedEventArgs e)
        {
            //使用string创建一个XML文档作为通知的模板
            string toastStr = "<toast duration='short' launch='Page1'>"//通过给Toast节点添加一个duration属性来设置Toast通知的显示时间长(long)短(short)
                               + "<visual version='1'>"
                               + "<binding template='ToastText03'>"
                               + "<text id='1'>Heading text</text>"
                               + "<text id='2'>我来试一试</text>"
                               + "</binding>"
                               + "</visual>"
                               + "<audio src='ms-winsoundevent:Notification.IM' loop='true'/>"//可以在toast的节点下面设置声音节点,当声音设置循环的时候，duration必须设置为long，不播放声音的时候需要设置audio元素的属性silent=true
                               + "</toast>";
            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastStr);
            ToastNotification toastNotification = new ToastNotification(toastXml);
            //在调试前必须先在程序清单中Package.appxmanifest文件中将支持Toast通知设置为'是'
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
        private async void btn_refresh_click(object sender, RoutedEventArgs e) 
        {
            bushIndicator.IsIndeterminate = true;
            var error = await Data.MessageDataSource.Load();
           if (!string.IsNullOrEmpty(error))
               await new Windows.UI.Popups.MessageDialog(error).ShowAsync();
           bushIndicator.IsIndeterminate = false;

        }

        private void Grid_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            var grid = (sender as Grid);
            var storyboard = grid.Resources["fadeIn"] as Windows.UI.Xaml.Media.Animation.Storyboard;
            storyboard.Begin();
        }
        private void Grid_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            var grid = (sender as Grid);
            var storyboard = grid.Resources["fadeOut"] as Windows.UI.Xaml.Media.Animation.Storyboard;
            storyboard.Begin();
        }
    }
    public class MainItemStyleSelector : StyleSelector
    {
        protected override Style SelectStyleCore(object i, DependencyObject container)
        {
            var a = i as SampleDataItem;
            var b = i as MessageItem;
            int index = 0;
            var period = 5;
            if (a != null)
                index = (a.Group.TopItems.IndexOf(a));
            if (b != null)
                index = (b.Group.TopItems.IndexOf(b));
            index = index % 2;
            if (index==0)
            {
                return App.Current.Resources["LargeGridViewItemStyle"] as Style;
            }

            return App.Current.Resources["DefaultGridViewItemStyle"] as Style;
        }
    }
}
