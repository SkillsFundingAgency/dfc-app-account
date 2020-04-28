using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFC.App.Account.Application.Common.Extensions
{
    public static class CollectionsExtensions
    {
        public static string ToConcatenatedString<T>(this IEnumerable<T> source, Func<T, string> toString, string separator)
        {
            if (source == null || source.Count() == 0) return string.Empty;

            var sb = new StringBuilder(toString(source.First()));
            foreach (var t in source.Skip(1))
            {
                if (t != null)
                {
                    sb.Append(separator);
                    sb.Append(toString(t));
                }
            }
            return sb.ToString();
        }

        public static string ToConcatenatedString<T>(this IEnumerable<T> list, string delimiter)
        {
            return list.ToConcatenatedString(t => t?.ToString(), delimiter);
        }

        public static string ToConcatenatedString<T>(this IEnumerable<T> list)
        {
            return list.ToConcatenatedString(t => t?.ToString(), ", ");
        }
    }
}
