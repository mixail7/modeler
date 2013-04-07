using System;
using System.IO;
using System.Windows;
using DiagramDesigner;

namespace Modeler.modeler
{
    public class FunctionCode
    {
        private const string Endl = "\r\n";
        private readonly string _functionName;
        private readonly string _functionParamsIn;
        private readonly string _functionParamsOut;
        private readonly string _blockName;
        private readonly string _functionBody;
        
        public string FunctionDefenition
        {
            get
            {
                return "def " + _functionName + "(" + _functionParamsIn + ")" + Endl
                       + (_functionBody.Trim().Length == 0 ? "#" + _blockName : _functionBody) + Endl + "return " +
                       _functionParamsOut + Endl + "end" + Endl;
            }
        }

        public string FunctionCall
        {
            get
            {
                return _functionParamsOut + (_functionParamsOut.Trim().Length != 0 ? "=" : "") + _functionName + "(" +
                       _functionParamsIn + ")" + Endl;
            }
        }

        public FunctionCode(string name, string inParams, string outParams, string blName, string body)
        {
            _functionName = name;
            _functionParamsIn = inParams;
            _functionParamsOut = outParams;
            _blockName = blName;
            _functionBody = body;
        }
    }

    public class RubyCodeGenerator : ICodeGenerator
    {
        private Project _project;
        private ModelCode _modelCode = new ModelCode();

        public RubyCodeGenerator(Project project)
        {
            _project = project;
        }
        private string GetValidFileName(string baseFileName)
        {
            if (baseFileName == "" || baseFileName == _project.FileName)
                baseFileName = _project.FileModelPath + "\\Общая схема.model";
            if (Directory.Exists(baseFileName))
                baseFileName += "\\Общая схема.model";
            return baseFileName;
        }
        
        public string GetCodeFileName(string modelFileName)
        {
            modelFileName = GetValidFileName(modelFileName);
            return modelFileName.Replace(_project.FileModelPath, _project.FileCodePath);
        }
        private DesignerItem getFirst(DesignerCanvas canva)
        {
            foreach (var elem in canva.Children)
            {
                var item = elem as DesignerItem;
                if (item != null)
                {
                    if (item.ParentID == Guid.Empty)
                        return item;
                }
            }
            return null;
        }
        
        private DesignerItem getNext(DesignerItem item, DesignerCanvas canva)
        {
            string id = item.ID.ToString();
            foreach (var elem in canva.Children)
            {
                var conn = elem as Connection;
                if (conn != null)
                {
                    if (conn.Source.ParentDesignerItem.ID.ToString() == id || conn.ID.ToString() == id)
                    {
                        return conn.Sink.ParentDesignerItem;
                    }
                }
            }
            return null;
        }

        private CodeElement getCode(DesignerItem item, string fileName)
        {
            return _modelCode.GetCodeForItem(item.ID.ToString(), fileName);
        }

        private string getNameForElement(DesignerItem item)
        {
            var block = item.Content as IBlockElementInterface;
            return block != null ? block.TextValue : "";
        }

        private string getOptNameForElement(DesignerItem item)
        {
            return getNameForElement(item).Replace(" ", "_");
        }

        private string GetInputParamForElement(string id, DesignerCanvas canva, string fileName)
        {
            foreach (var elem in canva.Children)
            {
                var conn = elem as Connection;
                if (conn != null)
                {
                    if (conn.Sink.ParentDesignerItem.ID.ToString() == id || conn.ID.ToString() == id)
                    {
                        var item = _modelCode.GetCodeForItem(conn.Source.ParentDesignerItem.ID.ToString(), fileName);
                        return item.OutputParam;
                    }
                }
            }
            return "";
        }

        private FunctionCode getFunctionCode(CodeElement elem, DesignerItem item, string fileName, DesignerCanvas canva)
        {
            return new FunctionCode(getOptNameForElement(item),
                                    GetInputParamForElement(item.ID.ToString(), canva, fileName), elem.OutputParam, getNameForElement(item),
                                    elem.Code);
        }

        public void Generate(string baseFileName)
        {
            string fileName = GetValidFileName(baseFileName);
            try
            {
                DesignerCanvas canva = new DesignerCanvas();
                canva.Open_Executed(fileName);
                DesignerItem item = getFirst(canva);
                string bufferFunctions = "", bufferFunctionsSource = "";
                while (item != null)
                {
                    CodeElement elem = getCode(item, fileName);
                    FunctionCode code = getFunctionCode(elem, item, fileName, canva);
                    bufferFunctions += code.FunctionCall;
                    bufferFunctionsSource += code.FunctionDefenition;
                    item = getNext(item, canva);
                }
                
                File.WriteAllText(GetCodeFileName(fileName), (bufferFunctionsSource + "\r\n#function exec\r\n\r\n" + bufferFunctions).Trim());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
