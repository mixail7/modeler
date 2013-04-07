using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DiagramDesigner;

namespace ActyvityDiagram_Pal
{
    /// <summary>
    /// класс, который отвечает за палитру компонентов
    /// </summary>
    [Serializable]
    public class BlockToolBoxPalleter : IToolBoxPalleter
    {

        public const string blockFormatName = "ActyvityDiagram";

        public string[] getNames()
        {
            return new string[]{AD_Begin_Element.ElementName, AD_Action_Element.ElementName,
                                AD_Condition_Element.ElementName,AD_EndCondition_Element.ElementName, 
                                AD_Division2_Element.ElementName,AD_Merge2_Element.ElementName, 
                                AD_Division3_Element.ElementName,AD_Merge3_Element.ElementName, 
                                AD_End_Element.ElementName, AD_Precondition_Element.ElementName, 
                                AD_SendSignal_Element.ElementName,AD_Reception_Element.ElementName,
                                AD_ActionTime_Element.ElementName, AD_EndThread_Element.ElementName,
                                AD_Object_Element.ElementName};
        }

        public UserControl[] getControls(params string[] names)
        {
            var res = new List<UserControl>();
            if (names.Contains(AD_Begin_Element.ElementName))
                res.Add(new AD_Begin_ToolBox());
            if (names.Contains(AD_Action_Element.ElementName))
                res.Add(new AD_Action_ToolBox());
            if (names.Contains(AD_Condition_Element.ElementName))
                res.Add(new AD_Condition_ToolBox());
            if (names.Contains(AD_EndCondition_Element.ElementName))            
                res.Add(new AD_EndCondition_ToolBox());
            if (names.Contains(AD_Division2_Element.ElementName))
                res.Add(new AD_Division2_ToolBox());   
            if (names.Contains(AD_Merge2_Element.ElementName))
                res.Add(new AD_Merge2_ToolBox());  
            if (names.Contains(AD_Division3_Element.ElementName))            
                res.Add(new AD_Division3_ToolBox());            
            if (names.Contains(AD_Merge3_Element.ElementName))            
                res.Add(new AD_Merge3_ToolBox());                                    
            if (names.Contains(AD_End_Element.ElementName))            
               res.Add(new AD_End_ToolBox());
            if (names.Contains(AD_Precondition_Element.ElementName))
                res.Add(new AD_Precondition_ToolBox());
            if (names.Contains(AD_SendSignal_Element.ElementName))
                res.Add(new AD_SendSignal_ToolBox());
            if (names.Contains(AD_Reception_Element.ElementName))
                res.Add(new AD_Reception_ToolBox());
            if (names.Contains(AD_ActionTime_Element.ElementName))
                res.Add(new AD_ActionTime_ToolBox());
            if (names.Contains(AD_EndThread_Element.ElementName))
                res.Add(new AD_EndThread_ToolBox());
            if (names.Contains(AD_Object_Element.ElementName))
                res.Add(new AD_Object_ToolBox());
            return res.ToArray();
        }

        public string getPalleterName()
        {
            return "Actyvity diagram";
        }

        public Expander getPanel()
        {
            var toolBox = new WrapPanel();
            
            toolBox.Orientation = Orientation.Horizontal;

            toolBox.ItemHeight = 80;
            toolBox.ItemWidth = 80;
            toolBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            toolBox.VerticalAlignment = VerticalAlignment.Stretch;
            toolBox.Children.Add(new AD_Begin_ToolBox());            
            toolBox.Children.Add(new AD_Action_ToolBox());
            toolBox.Children.Add(new AD_Condition_ToolBox());
            toolBox.Children.Add(new AD_EndCondition_ToolBox());
            toolBox.Children.Add(new AD_Division2_ToolBox());
            toolBox.Children.Add(new AD_Merge2_ToolBox());
            toolBox.Children.Add(new AD_Division3_ToolBox());
            toolBox.Children.Add(new AD_Merge3_ToolBox());
            toolBox.Children.Add(new AD_End_ToolBox());
            toolBox.Children.Add(new AD_Precondition_ToolBox());
            toolBox.Children.Add(new AD_SendSignal_ToolBox());
            toolBox.Children.Add(new AD_Reception_ToolBox());
            toolBox.Children.Add(new AD_ActionTime_ToolBox());
            toolBox.Children.Add(new AD_EndThread_ToolBox());
            toolBox.Children.Add(new AD_Object_ToolBox());
            var res = new Expander { Header = getPalleterName(), IsExpanded = true, Content = toolBox };
            return res;
        }

        public bool isFormatOwner(string formatName)
        {
            return (formatName == blockFormatName);
        }

        public DesignerItem getElement(DragObject data)
        {
            string element_name = (string)data.data;
            if (element_name == AD_Begin_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Begin_Element() };
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom") };
                return res;
            }
            if (element_name == AD_Action_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Action_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }
            if (element_name == AD_Condition_Element.ElementName)
            {                
                var res = new DesignerItem { Content = new AD_Condition_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Left, res, 0, 0.5, "Left"),
                                                  new Connector(ConnectorOrientation.Right, res,1,  0.5, "Right")};
                return res;
            }
            if (element_name == AD_EndCondition_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_EndCondition_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Left, res, 0, 0.5, "Left"),
                                                  new Connector(ConnectorOrientation.Right, res,1,  0.5, "Right"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }
            if (element_name == AD_Division2_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Division2_Element() };
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.None, res, 0.5, 0, "Center"), 
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.75, 1, "Bottom"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.25, 1, "Bottom1")};
                return res;
            }
            if (element_name == AD_Merge2_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Merge2_Element() };
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Center"),
                                                  new Connector(ConnectorOrientation.Top, res, 0.75, 0, "Bottom"),
                                                  new Connector(ConnectorOrientation.Top, res, 0.25, 0, "Bottom1")};
                return res;
            }
            if (element_name == AD_Division3_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Division3_Element() };
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Center"), 
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.75, 1, "Bottom"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.25, 1, "Bottom1"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom2")};
                return res;
            }

            if (element_name == AD_Merge3_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Merge3_Element() };
                res.CanRotate = false;
                res.CanResize = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Center"),
                                                  new Connector(ConnectorOrientation.Top, res, 0.75, 0, "Bottom"),
                                                  new Connector(ConnectorOrientation.Top, res, 0.25, 0, "Bottom1"),
                                                  new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Bottom2")};
                return res;
            }
            
            if (element_name == AD_End_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_End_Element() };                
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top") };
                return res;
            }

            if (element_name == AD_Precondition_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Precondition_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }
            if (element_name == AD_SendSignal_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_SendSignal_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }

            if (element_name == AD_Reception_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Reception_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }
            if (element_name == AD_ActionTime_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_ActionTime_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Left, res, 0.5, 0.5, "Left")};
                return res;
            }
            if (element_name == AD_EndThread_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_EndThread_Element() };
                res.CanResize = false;
                res.CanRotate = false;
                res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top") };
                return res;
            }
            if (element_name == AD_Object_Element.ElementName)
            {
                var res = new DesignerItem { Content = new AD_Object_Element() };
                res.CanRotate = false;
                res.Connectors = new Connector[] {new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
                                                  new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
                return res;
            }
            Debugger.Break();
            throw new ApplicationException("Непредусмотеное название фигуры");
        }
    }
}
