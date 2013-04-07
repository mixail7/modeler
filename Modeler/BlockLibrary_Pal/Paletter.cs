using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DiagramDesigner;

namespace BlockLibrary_Pal {
  /// <summary>
  /// класс, который отвечает за палитру компонентов
  /// </summary>
  [Serializable]
  public class BlockToolBoxPalleter : IToolBoxPalleter {

    public const string blockFormatName = "Block";

    public string[] getNames() {
      return new string[]{Begin_Element.ElementName, End_Element.ElementName, Action_Element.ElementName,
        If_Element.ElementName,Input_Element.ElementName,Output_Element.ElementName, ArrowRight_Element.ElementName, 
        ArrowLeft_Element.ElementName};
    }

    public UserControl[] getControls(params string[] names) {
      var res = new List<UserControl>();
      if (names.Contains(Begin_Element.ElementName)) {
        res.Add(new Begin_ToolBox());
      }
      if (names.Contains(End_Element.ElementName)) {
        res.Add(new End_ToolBox());
      }
      if (names.Contains(Action_Element.ElementName)) {
        res.Add(new Action_ToolBox());
      }
      if (names.Contains(If_Element.ElementName)) {
        res.Add(new If_ToolBox());
      }
      if (names.Contains(Input_Element.ElementName)) {
        res.Add(new Input_ToolBox());
      }
      if (names.Contains(Output_Element.ElementName)) {
        res.Add(new Output_ToolBox());
      }
      if (names.Contains(ArrowRight_Element.ElementName)) {
        res.Add(new ArrowRight_ToolBox());
      }
      return res.ToArray();
    }

    public string getPalleterName() {
      return "Блок-схема";
    }

    public Expander getPanel() {
      var toolBox = new WrapPanel();

      //toolBox.Background = Brushes.Azure;
      toolBox.Orientation = Orientation.Horizontal;

      toolBox.ItemHeight = 80;
      toolBox.ItemWidth = 80;

      toolBox.HorizontalAlignment = HorizontalAlignment.Stretch;
      toolBox.VerticalAlignment = VerticalAlignment.Stretch;


      toolBox.Children.Add(new Begin_ToolBox());
      toolBox.Children.Add(new End_ToolBox());
      toolBox.Children.Add(new Action_ToolBox());
      toolBox.Children.Add(new If_ToolBox());
      toolBox.Children.Add(new Input_ToolBox());
      toolBox.Children.Add(new Output_ToolBox());
      toolBox.Children.Add(new ArrowRight_ToolBox());
      toolBox.Children.Add(new ArrowLeft_ToolBox());

      var res = new Expander { Header = getPalleterName(), IsExpanded = true, Content = toolBox };

      return res;
    }

    public bool isFormatOwner(string formatName) {
      return (formatName == blockFormatName);
    }

    public DesignerItem getElement(DragObject data) {
      var str = (string)data.data;
      if (str == ArrowRight_Element.ElementName) {
        var res = new DesignerItem { Content = new ArrowRight_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.None, res, 0.5, 0.5, "Center") };
        return res;
      }
      if (str == ArrowLeft_Element.ElementName) {
        var res = new DesignerItem { Content = new ArrowLeft_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.None, res, 0.5, 0.5, "Center") };
        return res;
      }
      if (str == Begin_Element.ElementName) {
        var res = new DesignerItem { Content = new Begin_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom") };
        return res;
      }
      if (str == End_Element.ElementName) {
        var res = new DesignerItem { Content = new End_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top") };
        return res;
      }

      if (str == Action_Element.ElementName) {
        var res = new DesignerItem { Content = new Action_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
         new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
        return res;
      }

      if (str == Input_Element.ElementName) {
        var res = new DesignerItem { Content = new Input_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
         new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
        return res;
      }

      if (str == Output_Element.ElementName) {
        var res = new DesignerItem { Content = new Output_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
         new Connector(ConnectorOrientation.Bottom, res, 0.5, 1, "Bottom")};
        return res;
      }

      if (str == If_Element.ElementName) {
        var res = new DesignerItem { Content = new If_Element() };
        res.Connectors = new Connector[] { new Connector(ConnectorOrientation.Top, res, 0.5, 0, "Top"),
          new Connector(ConnectorOrientation.Left, res, 0, 0.5, "Left"),
          new Connector(ConnectorOrientation.Right, res,1,  0.5, "Right")};
        return res;
      }
      Debugger.Break();
      throw new ApplicationException("Непредусмотеное название фигуры");
    }
  }
}
