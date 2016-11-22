using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Wpf_XMLEditor.Model
{
    public class FileInformation
    {
        public string FilePath { get; set; }
        public bool IsSave { get; set; }
        public List<Thread> Threads { get; }
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.FilePath);
            }
        }



        public static FileInformation GetFileInformation(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            FileInformation fileInfo = new FileInformation { FilePath = filePath };
            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(filePath);
            }
            catch (XmlException exception)
            {
                throw new XmlException("Ошибка загрузки XML-файла", exception);
            }

            fileInfo.GetThreadsFromDocument(document);
            return fileInfo;
        }

        public void SaveFile()
        {
            XmlDocument document = new XmlDocument();
            XmlNode rootElement = document.AppendChild(document.CreateElement("root"));

            foreach (var thread in Threads)
            {
                rootElement.AppendChild(thread.GetElement(document));
            }

            document.Save(FilePath);
            IsSave = true;
        }


        private void GetThreadsFromDocument(XmlNode document)
        {
            XmlNode element = document.FirstChild;
            if (element == null || element.Name != "root")
            {
                throw new XmlException();
            }

            foreach (XmlElement childElement in element.ChildNodes)
            {
                Thread thread = Thread.GetThreadFromElement(childElement);
                Threads.Add(thread);
            }

            IsSave = true;
        }

        private FileInformation()
        {
            Threads = new List<Thread>();
        }

    }
}

