using System.Xml.Linq;
using DiagramDesigner;

namespace ActyvityDiagram_Pal
{
    /// <summary>
    /// Interaction logic for Arrow45_Element.xaml
    /// </summary>
    public partial class AD_Merge2_Element : IXMLSaveable
    {
        public const string ElementName = "AD_Merge2_Element";
        public AD_Merge2_Element()
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
