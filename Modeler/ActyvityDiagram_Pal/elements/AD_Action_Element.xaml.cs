using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Linq;
using DiagramDesigner;
using Modeler;

namespace ActyvityDiagram_Pal
{
    internal partial class AD_Action_Element : INotifyPropertyChanged, IBlockElementInterface
    {
        public AD_Action_Element()
        {
            InitializeComponent();
            DataContext = this;
            BorderBrush = new SolidColorBrush(BorderColor);
        }

        private HatchType _hatchAngle = HatchType.НетШтриховки;
 
        public HatchType Hatch
        {
            get
            {
                return _hatchAngle;
            }
            set
            {
                _hatchAngle = value;
                if (value == HatchType.Наклон_0)
                {
                    MainBorder.Background = Block_Extentions.getBrush(0);
                }
                else if (value == HatchType.Наклон_45)
                {
                    MainBorder.Background = Block_Extentions.getBrush(45);
                }
                else if (value == HatchType.Наклон_90)
                {
                    MainBorder.Background = Block_Extentions.getBrush(90);
                }
                else if (value == HatchType.Наклон_135)
                {
                    MainBorder.Background = Block_Extentions.getBrush(135);
                }
                else
                {
                    MainBorder.Background = null;
                }
                OnPropertyChanged("HatchAngle");
            }
        }
        
        private bool _isBorderVisible = true;

        public bool IsBorderVisible
        {
            get
            {
                return _isBorderVisible;
            }
            set
            {
                _isBorderVisible = value;
                if (IsBorderVisible == false)
                {
                    BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    BorderBrush = new SolidColorBrush(BorderColor);
                }
            }
        }

        private Color _colorBrush = Colors.Black;
        public Color BorderColor
        {
            get
            {
                if (IsBorderVisible == false)
                {
                    return Colors.Transparent;
                }
                return _colorBrush;
            }
            set
            {
                _colorBrush = value;
                if (IsBorderVisible == false)
                {
                    return;
                }
                BorderBrush = new SolidColorBrush(value);
            }
        }

        private string _textValue = "Действие";

        public string TextValue
        {
            get
            {
                return _textValue;
            }
            set
            {
                _textValue = value;
                OnPropertyChanged("TextValue");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public const string ElementName = "AD_Action_Element";

        public event PropertyChangedEventHandler PropertyChanged;

        public XElement getData()
        {
            return this.getXmlDescription();
        }

        public void loadData(XElement data)
        {
            this.loadFromXmlDescription(data);
        }

        public object getCover(DesignerItem item)
        {
            return new BlockElement_ControlCoverForPropertyEditor(item);
        }
    }


}
