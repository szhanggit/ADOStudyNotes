using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ADOAccess
{
    public static class Helper
    {
        public static string Append(params object[] objs)
        {
            var stringBuilder = new StringBuilder();

            foreach (var obj in objs)
            {
                stringBuilder.Append(obj);
            }

            return stringBuilder.ToString();
        }

        public static IEnumerable<T> Convert<T>(this DataTable table) where T : new()
        {
            foreach (DataRow row in table.Rows)
            {
                T t = new T();
                Type tType = t.GetType();

                foreach (var property in tType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (table.Columns.Contains(property.Name) && !row.IsNull(property.Name))
                    {
                        property.SetValue(t, row[property.Name]);
                    }
                }

                yield return t;
            }
        }

        public static T Convert<T>(this object source) where T : new()
        {
            if (source == null)
            {
                return default(T);
            }

            T t = new T();
            Type tType = t.GetType();
            PropertyInfo property = null;

            foreach (var sourceProperty in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                property = tType.GetProperty(sourceProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (property != null && property.CanWrite)
                {
                    property.SetValue(t, sourceProperty.GetValue(source));
                }
            }

            return t;
        }

        public static void RemoveDuplicate<T>(this List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                List<T> t = new List<T>();

                foreach (var item in list)
                {
                    if (!t.Contains(item))
                    {
                        t.Add(item);
                    }
                }

                list = t;
            }
        }

        public static void CopyTo(this object source, object target)
        {
            if (source != null && target != null)
            {
                Type tType = target.GetType();
                PropertyInfo property = null;

                foreach (var sourceProperty in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    property = tType.GetProperty(sourceProperty.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (property != null && property.CanWrite && !Equals(property.GetValue(target), sourceProperty.GetValue(source)))
                    {
                        property.SetValue(target, sourceProperty.GetValue(source));
                    }
                }
            }
        }

        public new static bool Equals(object objA, object objB)
        {
            if (objA == null && objB == null)
            {
                return true;
            }

            if (objA == null)
            {
                return false;
            }

            if (objB == null)
            {
                return false;
            }

            return objA.Equals(objB);
        }

        public static DateTime SqlMinValue(this DateTime sqlDateTime)
        {
            return new DateTime(1900, 01, 01, 00, 00, 00);
        }

        public static DateTime SqlMaxValue(this DateTime sqlDateTime)
        {
            return new DateTime(3999, 01, 01, 00, 00, 00);
        }


        public static DateTime GetSqlValidDateTime(this DateTime sqlDateTime, bool MinValue)
        {
            return MinValue ? (sqlDateTime < SqlMinValue(sqlDateTime) || sqlDateTime > SqlMaxValue(sqlDateTime) ? SqlMinValue(sqlDateTime) : sqlDateTime)
                : (sqlDateTime > SqlMaxValue(sqlDateTime) || sqlDateTime < SqlMinValue(sqlDateTime) ? SqlMaxValue(sqlDateTime) : sqlDateTime);
        }

        public static string Serialize<T>(T t)
        {
            string result = string.Empty;
            if (t != null)
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                using (var writer = new StringWriter())
                {
                    xmlSerializer.Serialize(writer, t);
                    result = writer.ToString();
                }
            }

            return result;
        }



        public static void WriteFile(string configLocation, string content)
        {
            using (StreamWriter outfile = new StreamWriter(configLocation))
            {
                outfile.Write(content);
            }
        }

        public static string ReadFile(string configLocation)
        {
            string content = null;

            using (StreamReader sr = new StreamReader(configLocation))
            {
                content = sr.ReadToEnd();
            }

            return content;
        }
    }
}
