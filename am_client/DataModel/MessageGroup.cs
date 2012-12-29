using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace am_client.Data
{
    public class MessageGroup : SampleDataCommon
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private ObservableCollection<MessageItem> items = new ObservableCollection<MessageItem>();

        public ObservableCollection<MessageItem> Items
        {
            get { return items; }
        }
        private ObservableCollection<MessageItem> topitems = new ObservableCollection<MessageItem>();
        public ObservableCollection<MessageItem> TopItems
        {
            get { return topitems; }
        }
        public void Add(MessageItem item)
        {
            items.Add(item);
            if (topitems.Count < 20)
                topitems.Add(item);
            else 
            {
                var earliest = findEarliest();
                if (earliest.ArrivalDate < item.ArrivalDate)
                {
                    topitems.Remove(earliest);
                    topitems.Add(item);
                }
            }
        }
        public MessageItem findEarliest()
        {
            var date = DateTime.Now;
            MessageItem res = null;
            foreach (var item in topitems)
            {
                if (date >= item.ArrivalDate)
                {
                    date = item.ArrivalDate;
                    res = item;
                }
            }
            return res;
        }

    }
}
