using System;
using System.Collections.Generic;
using System.Xml;

namespace Wpf_XMLEditor.Model
{
    public class Thread
    {
        public int Id { get; set; }
        public ulong Time { get; set; }
        public List<Method> Methods { get; }


        public static Thread GetThreadFromElement(XmlElement element)
        {
            if (element.Name != "thread")
                throw new XmlException();

            int id;
            ulong time;

            id = Convert.ToInt32(element.Attributes["id"].Value);
            time = Convert.ToUInt64(element.Attributes["time"].Value);

            Thread currentThread = new Thread()
            {
                Id = id,
                Time = time
            };

            foreach (XmlElement childElement in element.ChildNodes)
            {
                Method method = Method.GetMethodFromElement(childElement, currentThread);
                currentThread.Methods.Add(method);
            }

            return currentThread;
        }

        public XmlElement GetElement(XmlDocument document)
        {
            XmlElement element = document.CreateElement("thread");

            element.SetAttribute("id", Id.ToString());
            element.SetAttribute("time", Time.ToString());

            foreach (var item in Methods)
            {
                element.AppendChild(item.GetElement(document));
            }
            return element;
        }


        private Thread()
        {
            Methods = new List<Method>();
        }
    }
}
