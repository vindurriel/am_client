using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using am_client.Common;

namespace am_client.Data
{
    public class MessageItem : SampleDataCommon
    {
        private MessageGroup group;

        public MessageGroup Group
        {
            get { return group; }
            set { group = value; }
        }
        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }
        private ObservableCollection<string> links=new ObservableCollection<string>();
        public ObservableCollection<string> Links
        {
            get { return links; }
        }
        private ObservableCollection<string> pictures = new ObservableCollection<string>();

        public ObservableCollection<string> Pictures
        {
            get { return pictures; }
        } 
        private ObservableCollection<string> tags = new ObservableCollection<string>();

        public ObservableCollection<string> Tags
        {
            get { return tags; }
        }
        private DateTime arrivalDate=DateTime.Now;
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { SetProperty(ref arrivalDate, value); }
        }
        public void SetArrivalDate(string raw)
        {
            arrivalDate = raw.ParseUnixTime();
        }
        

    }
}
