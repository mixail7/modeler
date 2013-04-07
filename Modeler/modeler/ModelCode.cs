using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using DiagramDesigner;

namespace Modeler.modeler
{
    class CodeElement
    {
        public string Id;
        public string Code;
        public string File;
        public string OutputParam;

        public CodeElement(string id, string code, string file, string output)
        {
            Id = id;
            Code = code;
            File = file;
            OutputParam = output;
        }
    }

    class ModelCode
    {
        private Dictionary<string, CodeElement> _dictionary = new Dictionary<string, CodeElement>();
        private readonly Dictionary<string, int> _filesName = new Dictionary<string, int>();

        private string getIdForElement(object elem)
        {
            var designElem = elem as DesignerItem;
            if (designElem != null)
                return designElem.ID.ToString();
            var connect = elem as Connection;
            if (connect != null)
                return connect.ID.ToString();
            return "";
        }

        private void FilteDic(UIElementCollection collection)
        {
            var filter = new Dictionary<string, int>();
            foreach (var coll in collection)
            {
                string id = getIdForElement(coll);
                if (!filter.ContainsKey(id))
                    filter.Add(id, 1);
            }
            var newDictionary = new Dictionary<string, CodeElement>();
            foreach (var element in _dictionary)
            {
                if (filter.ContainsKey(element.Key))
                {
                    newDictionary.Add(element.Key, element.Value);
                }
            }
            _dictionary = newDictionary;

        }

        private void ExtendDic(string fileName)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(fileName);
                var items = doc.GetElementsByTagName("node");
                foreach (XmlNode node in items)
                {
                    string id = "", cd = "", output = "";
                    foreach (XmlNode xc in node.ChildNodes)
                    {
                        if (xc.Name == "id")
                            id = xc.InnerText;
                        if (xc.Name == "code")
                            cd = xc.InnerText;
                        if (xc.Name == "output")
                            output = xc.InnerText;
                    }
                    if (!_dictionary.ContainsKey(id))
                    {
                        _dictionary.Add(id, new CodeElement(id, cd, fileName, output));
                    }
                }
            }
            catch (Exception e)
            {
                string code = e.Message;
            }
        }

        public void SaveDicForFile(string fileName)
        {
            var writer = XmlWriter.Create(fileName);
            writer.WriteStartDocument();
            writer.WriteStartElement("nodes");
            foreach (var dic in _dictionary)
            {
                if (dic.Value.File == fileName)
                {
                    writer.WriteStartElement("node");
                    writer.WriteElementString("id", dic.Key);
                    writer.WriteElementString("code", dic.Value.Code);
                    writer.WriteElementString("output", dic.Value.OutputParam);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }

        private string getCodePath(string modelPath)
        {
            return Path.GetDirectoryName(modelPath) + "\\" + Path.GetFileNameWithoutExtension(modelPath) + ".code";
        }

        public void FlushAll(UIElementCollection collection)
        {
            try
            {
                foreach (var fileName in _filesName)
                {
                    string codeFile = getCodePath(fileName.Key);
                    ExtendDic(codeFile);
                    FilteDic(collection);
                    SaveDicForFile(codeFile);
                }
                _dictionary.Clear();
                _filesName.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void AddCode(string fileName, string id, string code, string output)
        {
            fileName = getCodePath(fileName);
            if (!_filesName.ContainsKey(fileName))
            {
                _filesName.Add(fileName, 1);
            }
            if (_dictionary.ContainsKey(id))
            {
                _dictionary.Remove(id);
            }
            _dictionary.Add(id, new CodeElement(id, code, fileName, output));
        }

        public CodeElement GetCodeForItem(string id, string fileName)
        {
            var elem = new CodeElement(id, "", fileName, "");
            if (_dictionary.ContainsKey(id))
            {
                elem = _dictionary[id];
                return elem;
            }
            ExtendDic(getCodePath(fileName));
            elem = _dictionary.ContainsKey(id) ? _dictionary[id] : elem;
            return elem;
        }
    }
}
