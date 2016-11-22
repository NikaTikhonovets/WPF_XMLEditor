using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Wpf_XMLEditor.Model
{
    public class Method
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public int ParamsCount { get; set; }
        public int Time { get; set; }
        public object Parent { get; private set; }
        public List<Method> ChildMethods { get; }

        private Method()
        {
            ChildMethods = new List<Method>();
            Parent = null;
        }
        
        public static Method GetMethodFromElement(XmlElement element, object parent = null)
        {
            if (element.Name != "method")
                throw new XmlException();

            string name, package;
            int paramsCount, time;

            name = element.Attributes["name"].Value;
            package = element.Attributes["package"].Value;
            paramsCount = Convert.ToInt32(element.Attributes["params"].Value);
            time = Convert.ToInt32(element.Attributes["time"].Value);

            Method currentMethod = new Method()
            {
                Name = name,
                Package = package,
                ParamsCount = paramsCount,
                Time = time
            };

            foreach (XmlElement childElement in element.ChildNodes)
            {
                Method child = GetMethodFromElement(childElement, currentMethod);
                currentMethod.ChildMethods.Add(child);
            }

            currentMethod.Parent = parent;
            return currentMethod;
        }


        public XmlElement GetElement(XmlDocument document)
        {
            XmlElement element = document.CreateElement("method");

            element.SetAttribute("name", Name);
            element.SetAttribute("time", Time.ToString());
            element.SetAttribute("package", Package);
            element.SetAttribute("params", ParamsCount.ToString());

            foreach (var child in ChildMethods)
            {
                element.AppendChild(child.GetElement(document));
            }

            return element;
        }

       
    }
}
