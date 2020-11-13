using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Utility
{
    public class XmlUtility
    {
        /// <summary>
        /// 将自定义对象序列化为XML字符串
        /// </summary>
        /// <param name="myObject">自定义对象实体</param>
        /// <returns>序列化后的XML字符串</returns>
        public static string SerializeToXml<T>(T myObject)
        {
            if (myObject != null)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (MemoryStream stream = new MemoryStream())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8) { Formatting = Formatting.None })
                    {
                        xs.Serialize(writer, myObject);
                        stream.Position = 0;
                        StringBuilder sb = new StringBuilder();
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                sb.Append(line);
                            }
                        }
                        writer.Close();
                        return sb.ToString();
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tempString">XML字符或者xml文件路径</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string tempString)
        {
            try
            {
                string xmlString = tempString;
                if (File.Exists(tempString))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    using (FileStream filestream = File.OpenRead(tempString))
                    {
                        xmlDoc.Load(filestream);
                        xmlString = xmlDoc.InnerXml;
                        filestream.Dispose();
                    }
                    GC.Collect();
                }
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 保存对象为xml文件到指定路径
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myObject"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool SaveObjectToXmlFile<T>(T myObject, string fullPath)
        {
            string xmlString = SerializeToXml(myObject);
            try
            {
                XmlDocument doc = new XmlDocument
                {
                    InnerXml = xmlString
                };
                doc.Save(fullPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
