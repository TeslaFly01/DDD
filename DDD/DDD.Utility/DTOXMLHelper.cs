using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DDD.Utility
{
   public static class DTOXMLHelper
    {
       public static string ToXML(Object oObject)
       {
           XmlDocument xmlDoc = new XmlDocument();
           XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
           using (MemoryStream xmlStream = new MemoryStream())
           {
               var ns = new XmlSerializerNamespaces();
               ns.Add(string.Empty,string.Empty); 
               xmlSerializer.Serialize(xmlStream, oObject,ns);
               xmlStream.Position = 0;
               xmlDoc.Load(xmlStream);
               return xmlDoc.InnerXml;
           }
       }

       public static Object XMLToObject(string XMLString, Object oObject)
       {
           using (var rdr = new StringReader(XMLString))
           {
               XmlSerializer oXmlSerializer = new XmlSerializer(oObject.GetType());
               oObject = oXmlSerializer.Deserialize(rdr);
               return oObject;
           }
       }

       public static string ToXml<T>(T oObject)
       {
           var xmlDoc = new XmlDocument();
           var xmlSerializer = new XmlSerializer(typeof(T));
           using (var xmlStream = new MemoryStream())
           {
               var ns = new XmlSerializerNamespaces();
               ns.Add(string.Empty, string.Empty);
               xmlSerializer.Serialize(xmlStream, oObject, ns);
               xmlStream.Position = 0;
               xmlDoc.Load(xmlStream);
               return xmlDoc.InnerXml;
           }
       }

       public static XDocument ToXmlDoc<T>(T oObject)
       {
           var xmlSerializer = new XmlSerializer(typeof(T));
           using (var xmlStream = new MemoryStream())
           {
               var ns = new XmlSerializerNamespaces();
               ns.Add(string.Empty, string.Empty);
               xmlSerializer.Serialize(xmlStream, oObject, ns);
               xmlStream.Position = 0;
               var xmlDoc = XDocument.Load(xmlStream);
               return xmlDoc;
           }
       }

       public static XDocument ToXmlDoc<T>(T oObject, Type[] knownTypes)
       {
           var xmlSerializer = new XmlSerializer(typeof(T), knownTypes);
           using (var xmlStream = new MemoryStream())
           {
               var ns = new XmlSerializerNamespaces();
               ns.Add(string.Empty, string.Empty);
               xmlSerializer.Serialize(xmlStream, oObject, ns);
               xmlStream.Position = 0;
               var xmlDoc = XDocument.Load(xmlStream);
               return xmlDoc;
           }
       }


       public static T XmlToObject<T>(string xmlString)
       {
           var oXmlSerializer = new XmlSerializer(typeof(T));
           using (var rdr = new StringReader(xmlString))
           {
               var oObject = (T)oXmlSerializer.Deserialize(rdr);
               return oObject;
           }
       }

       public static T XmlToObject<T>(XDocument doc)
       {
           if (doc == null || doc.Root == null)
           {
               throw  new ArgumentNullException("doc");
           }

           var oXmlSerializer = new XmlSerializer(typeof(T));           
           using (var rdr =doc.Root.CreateReader())
           {
               var oObject = (T)oXmlSerializer.Deserialize(rdr);
               return oObject;
           }
       }

        #region demo

//       public class UserInfoDTO
//    {
 
//        public string UserId { get; set; }
//        public string UserName { get; set; }
//        public string UserAddress { get; set; }
//        public string UserPhone { get; set; }
//    }

//Now we will fill it with data

//UserInfoDTO oUserInfoDTO
//                = new UserInfoDTO { UserId = "12", UserName = "Hasibul Haque",
//                                    UserAddress = " Dhaka, Bangladesh", UserPhone = "+8801912104674" }; 

//       Using ToXML method.

//string strXML = ToXML(oUserInfoDTO); 

//We will then find the following XML after converting our UserInfoDTO object.

//<?xml version="1.0"?>
//<UserInfoDTO xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
//<UserId>12</UserId>
//<UserName>Hasibul Haque</UserName>
//<UserAddress> Dhaka, Bangladesh</UserAddress>
//<UserPhone>+8801912104674</UserPhone>
//</UserInfoDTO> 

//       Using XMLToObject method:

//UserInfoDTO oUserInfoDTO = new UserInfoDTO();
//            oUserInfoDTO = (UserInfoDTO)XMLToObject(richTextBox1.Text, oUserInfoDTO); 

        #endregion

    }
}
