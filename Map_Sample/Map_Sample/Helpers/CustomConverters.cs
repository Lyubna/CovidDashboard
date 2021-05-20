using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Map_Sample.Helpers
{
    public class CustomConverters
    {

    }
    public static class JObjectExtensions
    {
        public static T GetValue<T>(this JObject jobj, string propertyName)
        {
            if (jobj != null)
                return jobj.GetValue(propertyName).Value<T>();

            return default;
        }

        public static T TryGetValue<T>(this JObject jobj, string propertyName)
        {
            try
            {
                return jobj.GetValue<T>(propertyName);
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static IEnumerable<T> GetValues<T>(this JObject jobj, string propertyName)
        {
            return jobj.GetValue<JArray>(propertyName).Values<T>();
        }
        public static IEnumerable<T> TryGetValues<T>(this JObject jobj, string propertyName)
        {
            try
            {
                return jobj.GetValue<JArray>(propertyName).Values<T>();
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
