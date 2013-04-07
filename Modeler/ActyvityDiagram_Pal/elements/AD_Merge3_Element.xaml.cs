using System.Xml.Linq;
using DiagramDesigner;

namespace ActyvityDiagram_Pal
{
    public partial class AD_Merge3_Element : IXMLSaveable
    {
        public const string ElementName = "AD_Merge3_Element";
        public AD_Merge3_Element()
        {
            InitializeComponent();
            DataContext = this;
        }
        public XElement getData()
        {
            var res = new XElement("Content_UserXML");
            res.Add(new XElement("Name", ElementName));
            return res;
        }

        public void loadData(XElement data)
        {
        }
    }
}
