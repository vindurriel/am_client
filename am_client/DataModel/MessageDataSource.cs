using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;
namespace am_client.Data
{
    public class MessageDataSource
    {
        static MessageDataSource instance = new MessageDataSource();
        ObservableCollection<MessageGroup> _allgroups = new ObservableCollection<MessageGroup>();
        public static  ObservableCollection<MessageGroup> AllGroups
        {
            get { return instance._allgroups; }
        }
        ObservableCollection<MessageItem> _allitems = new ObservableCollection<MessageItem>();
        public static ObservableCollection<MessageItem> AllItems
        {
            get { return instance._allitems; }
        }
        public MessageDataSource()
        {
        }

        public static async Task<string> Load()
        {
            var url = Common.AppSettings.Get<string>("server");
            if (string.IsNullOrEmpty(url))
            {
                await new MessageDialog("no server settings found, please setting urls first", "error").ShowAsync();
                return "no server settings found, please setting urls first";
            }
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await new MessageDialog("invalid server url settings", "error").ShowAsync();
                return "invalid server url settings";
            }
            var error = string.Empty;
            url += "/messages";
            var queries = new List<string>();
            var user = Common.AppSettings.Get<string>("username", null);
            if (!string.IsNullOrEmpty(user))
                queries.Add("user=" + user);
            queries.Add("limit=150");
            url += "?" + string.Join("&", queries);
            var client = new HttpClient();
            int timeout = Common.AppSettings.Get<int>("connection_timeout", 10);
            client.Timeout = TimeSpan.FromSeconds(timeout);
            try
            {
                var res = await client.GetStringAsync(new Uri(url));
                // Parse the JSON recipe data
                var json = JsonObject.Parse(res);
                LoadFromJson(res);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return error;
        }
        public static MessageGroup GetGroup(string id)
        {
            return instance._allgroups.FirstOrDefault(x => x.UniqueId == id);
        }
        public static MessageItem GetItem(string id)
        {
            return instance._allitems.FirstOrDefault(x => x.UniqueId == id);
        }
        private static void LoadFromJson(string input)
        {
            input=input.Trim();
            IJsonValue raw=null;
            if (input.StartsWith("{"))
                raw = JsonObject.Parse(input);
            else if (input.StartsWith("["))
                raw = JsonArray.Parse(input);
            else
                throw new Exception("invalid json format");
            instance._allgroups.Clear();
            instance._allitems.Clear();
            var kind = raw.ValueType;
            if (kind == JsonValueType.Object)
                raw = raw.GetObject()["messages"];
            var i = 3;
            var index = 0;
            foreach (var t in raw.GetArray())
            {
                index += 1;
                var item = new MessageItem();
                var pic = (i % 3).ToString();
                i += 1;
                item.Tags.Add("Item " + index.ToString());
                pic = "Assets/pic" + pic + ".jpg";
                item.SetImage(pic);
                instance._allitems.Add(item);
                var obj = t.GetObject();
                foreach (var k in obj.Keys)
                {
                    IJsonValue val;
                    if (!obj.TryGetValue(k, out val))
                            continue;
                    switch (k.ToLower())
                    {
                        case "user":
                            var user = val.GetString();
                            var group = instance._allgroups.FirstOrDefault(x => x.Title == user);
                            if (group==null)
                            {
                                group = new MessageGroup { Title = user };
                                group.SetImage("Assets/user.png");
                                instance._allgroups.Add(group);
                            }
                            group.Add(item);
                            item.Group = group;
                            break;
                        case "_id":
                            item.UniqueId = val.GetString();
                            break;
                        case "title":
                            item.Title = val.GetString();
                            break;
                        case "description":
                            item.Description = val.GetString();
                            break;
                        case "tags":
                            foreach (var child in val.GetArray())
                                item.Tags.Add(child.GetString());
                            break;
                        case "links":
                            foreach (var child in val.GetArray())
                                item.Links.Add(child.GetString());
                            break;
                        case "pictures":
                            foreach (var child in val.GetArray())
                                item.Pictures.Add(child.GetString());
                            break;
                        case "created_on":
                            item.SetArrivalDate(val.GetString());
                            break;
                        case "content":
                            item.Content += val.GetString();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
