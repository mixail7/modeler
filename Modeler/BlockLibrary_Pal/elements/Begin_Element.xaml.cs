using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using DiagramDesigner;
using Modeler;

namespace BlockLibrary_Pal {
  /// <summary>
  /// Interaction logic for Begin_Element.xaml
  /// </summary>
  public partial class Begin_Element : UserControl, INotifyPropertyChanged, IBlockElementInterface {
    public Begin_Element() {
      InitializeComponent();
      DataContext = this;
      //IBlockElementInterface
        BorderBrush = new SolidColorBrush(BorderColor);
    }

    private HatchType _hatchAngle = HatchType.НетШтриховки;
    /// <summary>
    /// угол, под которым направлена штриховка
    /// </summary>
    public HatchType Hatch {
      get {
        return _hatchAngle;
      }
      set {
        _hatchAngle = value;
        if (value == HatchType.Наклон_0) {
          MainBorder.Background = Block_Extentions.getBrush(0);
        } else if (value == HatchType.Наклон_45) {
          MainBorder.Background = Block_Extentions.getBrush(45);
        } else if (value == HatchType.Наклон_90) {
          MainBorder.Background = Block_Extentions.getBrush(90);
        } else if (value == HatchType.Наклон_135) {
          MainBorder.Background = Block_Extentions.getBrush(135);
        } else {
          MainBorder.Background = null;
        }
        OnPropertyChanged("HatchAngle");
      }
    }

    private bool _isBorderVisible = true;

    public bool IsBorderVisible {
      get {
        return _isBorderVisible;
      }
      set {
        _isBorderVisible = value;
        if (IsBorderVisible == false) {
          BorderBrush = new SolidColorBrush(Colors.Transparent);
        } else {
          BorderBrush = new SolidColorBrush(BorderColor);
        }
      }
    }

    private Color _colorBrush = Colors.Black;
    public Color BorderColor {
      get {
        if (IsBorderVisible == false) {
          return Colors.Transparent;
        }
        return _colorBrush;
      }
      set {
        _colorBrush = value;
        if (IsBorderVisible == false) {
          return;
        }
        BorderBrush = new SolidColorBrush(value);
      }
    } 

    private string _textValue = "Начало";

    public string TextValue {
      get {
        return _textValue;
      }
      set {
        _textValue = value;
        OnPropertyChanged("TextValue");
      }
    }

    protected void OnPropertyChanged(string name) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

    public const string ElementName = "Begin";

    public event PropertyChangedEventHandler PropertyChanged;

    public XElement getData() {
      return this.getXmlDescription();
    }

    public void loadData(System.Xml.Linq.XElement data) {
      this.loadFromXmlDescription(data);
    }


    public object getCover(DesignerItem item) {
      return new BlockElement_ControlCoverForPropertyEditor(item);
    }
  }

}
