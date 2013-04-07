using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using DiagramDesigner;
using Modeler;

namespace BlockLibrary_Pal {
  /// <summary>
  /// Interaction logic for Arrow45_Element.xaml
  /// </summary>
  public partial class ArrowLeft_Element : UserControl, INotifyPropertyChanged, IBlockElementInterface {
    public ArrowLeft_Element() {
      InitializeComponent();
      this.DataContext = this;
      //IBlockElementInterface
      BorderBrush = new SolidColorBrush(BorderColor);
      TextValue = "Стрелка";
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
          MainBorder.Fill = Block_Extentions.getBrush(0);
        } else if (value == HatchType.Наклон_45) {
          MainBorder.Fill = Block_Extentions.getBrush(45);
        } else if (value == HatchType.Наклон_90) {
          MainBorder.Fill = Block_Extentions.getBrush(90);
        } else if (value == HatchType.Наклон_135) {
          MainBorder.Fill = Block_Extentions.getBrush(135);
        } else {
          MainBorder.Fill = null;
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

    private string _textValue = "Стрелка135";

    public string TextValue {
      get {
        return _textValue;
      }
      set {
        _textValue = value;

        //transtateTransform.X = -formatterText.Width/2;
        //transtateTransform.Y = formatterText.Height ;
        OnPropertyChanged("TextValue");
      }
    }

    protected void OnPropertyChanged(string name) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

    public const string ElementName = "ArrowLeft";

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
