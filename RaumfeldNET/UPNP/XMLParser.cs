using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace RaumfeldNET.UPNP
{
    public class XMLParser
    {
        public string getNodeValue(string _xmlString, string _nodeId)
        {
            XmlReader reader = XmlReader.Create(new StringReader(_xmlString));

            if (!reader.ReadToFollowing(_nodeId))
                return null;
            return reader.ReadElementContentAsString();            
        }

        public string getNodeAttributeValue(string _xmlString, string _nodeId, string _attributeId)
        {
            XmlReader reader = XmlReader.Create(new StringReader(_xmlString));

            if (!reader.ReadToFollowing(_nodeId))
                return null;
            if (!reader.MoveToAttribute(_attributeId))
                return null;
            return reader.Value;
        }  
    }
}

