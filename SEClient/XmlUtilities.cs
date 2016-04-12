using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace XMLSerializationCustomization
{
    public class XmlUtilities
    {
        //public static string SerializeToString(object obj)
        //{
        //    if (obj != null)
        //    {
        //        XmlSerializer serializer = new XmlSerializer(obj.GetType());

        //        using (StringWriter writer = new StringWriter())
        //        {
        //            try
        //            {
        //                serializer.Serialize(writer, obj);

        //                return writer.ToString();
        //            }
        //            catch
        //            {
        //                return string.Empty;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        //public static T SerializeFromString<T>(string xml)
        //{
        //    if (!string.IsNullOrEmpty(xml))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(T));

        //        using (StringReader reader = new StringReader(xml))
        //        {
        //            try
        //            {
        //                return (T)serializer.Deserialize(reader);
        //            }
        //            catch
        //            {
        //                throw new Exception("XML document is wrong.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return default(T);
        //    }
        //}

        public static void GetTypePropertyDic(Type type, ref Dictionary<string, Type> propertyTypeDic, ref Dictionary<string, PropertyInfo> propertyInfoDic)
        {
            PropertyInfo[] propertyArray = type.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.SetProperty);
            if (propertyArray != null)
            {
                foreach (PropertyInfo property in propertyArray)
                {
                    propertyTypeDic[property.Name] = property.PropertyType;
                    propertyInfoDic[property.Name] = property;
                }
            }
        }
    }


 
    public class PdkXmlTextReader : XmlTextReader
    {
        public PdkXmlTextReader(System.IO.TextReader reader) : base(reader) { }

        public override string NamespaceURI
        {
            get 
            {
                if (base.NamespaceURI == "LancePlatform")
                {
                    return "tf";
                }
                else
                {
                    return base.NamespaceURI;
                }
            }
        }
    }
}
