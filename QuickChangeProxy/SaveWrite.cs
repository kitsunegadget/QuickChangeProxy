using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickChangeProxy
{
    static class SaveWrite
    {
        private static string fileName = AppDomain.CurrentDomain.BaseDirectory + "settings.config";

        public static LanValue[] ReadXML()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LanValue[]));
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName, new UTF8Encoding(false));
            LanValue[] lan = (LanValue[])serializer.Deserialize(sr);
            sr.Close();

            return lan;
        }

        public static void WriteXML(LanValue[] lanvalue)
        {
            System.Xml.Serialization.XmlSerializer serializerWrite = new System.Xml.Serialization.XmlSerializer(typeof(LanValue[]));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false, new UTF8Encoding(false));
            serializerWrite.Serialize(sw, lanvalue);
            sw.Close();
        }
    }

    public class LanValue
    {
        public string server;
        public string port;
        public bool over;
    }
}
