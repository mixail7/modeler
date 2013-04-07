using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using Application = System.Windows.Forms.Application;

namespace DiagramDesigner {
  /// <summary>
  /// Interaction logic for UserPalleter.xaml
  /// </summary>
  public partial class UserPalleter : Window {
    public UserPalleter() {
      InitializeComponent();
    }

    public UserPalleter(PalleterInfo[] palleters) {
      InitializeComponent();
      MainTree.ItemsSource = palleters;
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      Close();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e) {
      var saveDialog = new SaveFileDialog {
                                            InitialDirectory = Application.StartupPath,
                                            Filter = "user palleters (*.udp)|*.udp",
                                            Title = "Имя палитры совпадает с именем файла"
                                          };
      var openRes = saveDialog.ShowDialog();
      if (openRes != System.Windows.Forms.DialogResult.OK) {
        return;
      }
      var fileName = saveDialog.FileName;
      var palleters = (PalleterInfo[]) MainTree.ItemsSource;
      var res = new XDocument(new XDeclaration("1.0", "windows-1251", "yes"));
      res.Add(new XElement("UserPalleter"));
      foreach (var palleterInfo in palleters) {
        var selectedElements = (from p in palleterInfo.elements where p.IsSelected select p).ToArray();
        if (selectedElements.Length == 0) {
          continue;
        }
        var palleterNode = new XElement("Palleter");
        palleterNode.Add(new XAttribute("PalleterName", palleterInfo.Name));
        foreach (var selectedElement in selectedElements) {
          palleterNode.Add(new XElement("Element", selectedElement.Name));
        }
        res.Root.Add(palleterNode);
      }
      res.Save(fileName);
      DialogResult = true;
      Close();
    }
  }
}
