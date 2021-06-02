using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Networking
{
    public class MessageProcessing
    {
        public static void SendMessage<T>(T message) where T : class
        {
            var obj = JObject.FromObject(message);
            obj.Add("_type", JToken.FromObject(typeof(T).Name));
        }

        public static bool TryParseMessage<T>(string message, out T? result) where T : class
        {
            var jobj = JsonConvert.DeserializeObject<JObject>(message);
            if (jobj.Value<string>("_type") == typeof(T).Name) {
                result = jobj.ToObject<T>();
                return true;
            }
            result = null;
            return false;
        }
    }
}
