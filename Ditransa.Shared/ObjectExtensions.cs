using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Shared
{
    public static class ObjectExtensions
    {
        public static object ToObject(this IDictionary<string, object> source)
        {
            var expandoObj = new ExpandoObject();
            
            foreach (var keyValuePair in source)
            {
                
                ((IDictionary<string, object>)expandoObj).Add(keyValuePair.Key, keyValuePair.Value);
            }
            
            return expandoObj;
        }

        public static string ToShortFormat(this DateTime? source)
        {
            if (source.HasValue)
            {
                return source.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            }
            return string.Empty;
        }

        public static string ToDateFormat(this DateTime source)
        {   
            return source.ToLocalTime().ToString("yyyy-MM-dd");
        }

    }
}
