using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using DiagramDesigner;

namespace Modeler {
  public interface IBlockElementInterface : IXMLSaveable, IPropertyEditable {
    bool IsBorderVisible { get; set; }
    Color BorderColor { get; set; }
    string TextValue { get; set; }
    HatchType Hatch { get; set; }
  }

  public class BlockElement_ControlCoverForPropertyEditor : DesignerItemCoverForPropertyEditor {
    private readonly IBlockElementInterface contentControl;

    public BlockElement_ControlCoverForPropertyEditor(DesignerItem item)
      : base(item) {
      var content = item.Content;
      Debug.Assert(content is IBlockElementInterface);
      contentControl = (IBlockElementInterface)content;
    }

    [Editor(typeof(MyColorEditor), typeof(UITypeEditor))]
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Цвет границы")]
    [DisplayName("Цвет границы")]
    public MyColor Color2 {
      get { return new MyColor(contentControl.BorderColor.R, contentControl.BorderColor.G, contentControl.BorderColor.B); }
      set { contentControl.BorderColor = Color.FromRgb(value.Red, value.Green, value.Blue); }
    }


    [CategoryAttribute("Основные данные"), DescriptionAttribute("Граница видна")]
    [DisplayName("Граница видна")]
    public bool BorderVisible {
      get {
        return contentControl.IsBorderVisible;
      }
      set {
        contentControl.IsBorderVisible = value;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("штриховка")]
    [DisplayName("штриховка")]
    public HatchType HasHatch {
      get {
        return contentControl.Hatch;
      }
      set {
        contentControl.Hatch = value;
      }
    }
  
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Надпись")]
    [DisplayName("Надпись")]
    public string Name {
      get {
        return contentControl.TextValue;
      }
      set {
        contentControl.TextValue = value;
      }
    }
  }

  public enum HatchType {
    НетШтриховки,
    Наклон_0,
    Наклон_45,
    Наклон_90,
    Наклон_135,
  }

  public static class Block_Extentions {
    public static XElement getXmlDescription(this IBlockElementInterface element) {
      var res = new XElement("Content_UserXML");
      res.Add(new XElement("Text", element.TextValue));
      res.Add(new XElement("Hatch", element.Hatch));
      res.Add(new XElement("BorderVisible", element.IsBorderVisible));
      res.Add(new XElement("BorderColor", element.BorderColor.R + " " + element.BorderColor.G + " " + element.BorderColor.B));
      return res;
    }

    public static void loadFromXmlDescription(this IBlockElementInterface element, XElement data) {
      element.TextValue = data.Element("Text").Value;
      element.IsBorderVisible = bool.Parse(data.Element("BorderVisible").Value);
      var tempStr = data.Element("BorderColor").Value;
      var parts = tempStr.Split(' ');
      element.BorderColor = Color.FromRgb(Byte.Parse(parts[0]), Byte.Parse(parts[1]), Byte.Parse(parts[2]));

      var hatchStr = data.Element("Hatch").Value;
      var hatchTypes = Enum.GetValues(typeof (HatchType));
      foreach (var hatchType in hatchTypes) {
        if (hatchType.ToString() == hatchStr) {
          element.Hatch = (HatchType) hatchType;
          break;
        }
      }
    }

    public static VisualBrush getBrush(int angle) {
      var radianAngle = Math.PI / 2 + Math.PI * 2 * angle / 360;

      VisualBrush vb = new VisualBrush();

      vb.TileMode = TileMode.Tile;

      vb.Viewport = new Rect(0, 0, 10, 10);
      vb.ViewportUnits = BrushMappingMode.Absolute;

      vb.Viewbox = new Rect(1, 1, 9, 9);
      vb.ViewboxUnits = BrushMappingMode.Absolute;
      const int r = 15;

      Line temp = new Line {
        X1 = 5 + r * Math.Sin(radianAngle), Y1 = 5 + r * Math.Cos(radianAngle),
        X2 = 5 + r * Math.Sin(radianAngle + Math.PI), Y2 = 5 + r * Math.Cos(radianAngle + Math.PI)
      };
      temp.Stroke = Brushes.Black;
      vb.Visual = temp;

      return vb;
    }
  }
}
