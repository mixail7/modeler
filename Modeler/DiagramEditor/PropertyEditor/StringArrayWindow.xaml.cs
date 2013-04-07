using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiagramDesigner.PropertyEditor {
  /// <summary>
  /// Interaction logic for StringArrayWindow.xaml
  /// </summary>
  public partial class StringArrayWindow : Window {


    public StringArrayWindow(MyStringArray _stringArray) {
      InitializeComponent();


      _stringArray.data.setListBox(MainListBox);
    }
  }
}
