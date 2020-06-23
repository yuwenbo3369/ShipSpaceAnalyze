using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ProConfiguration_IntelShipSpaceAnalys
{
    public class ProConfigHelper
    {
        //初始化自定义配置文件
        private static string ConfigFile = System.Environment.CurrentDirectory + @"\ProConfig.xml";
        private static ProConfig InitInfoConfig;
        public static ProConfig GetInitInfoConfig()
        {
            if (InitInfoConfig == null)
            {
                InitInfoConfig = OpenFile(ConfigFile);
            }
            return InitInfoConfig;
        }

        private static ProConfig OpenFile(string xmlfile)
        {
            ProConfig config = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ProConfig));
            try
            {
                using (XmlTextReader reader = new XmlTextReader(xmlfile))
                {
                    config = (ProConfig)serializer.Deserialize(reader);
                }
                return config;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    [XmlRoot(ElementName = "doc")]
    public class ProConfig
    {
        [XmlElement(ElementName = "DefaultProject")]
        public string DefaultProject { get; set; }
        [XmlElement(ElementName = "DefaultSDEConnection")]
        public string DefaultSDEConnection { get; set; }
        [XmlElement(ElementName = "HistoryData")]
        public HistoryData HistoryPath { get; set; }

        [XmlElement(ElementName = "PreviewOpenTable")]
        public bool PreviewOpenTable { get; set; }

    }

    public class HistoryData
    {
        [XmlAttribute(AttributeName = "ConnFile")]
        public string ConnFile { get; set; }

        [XmlAttribute(AttributeName = "DataSet")]
        public string DataSet { get; set; }

        [XmlAttribute(AttributeName = "UserName")]
        public string UserName { get; set; }
    }


    }
