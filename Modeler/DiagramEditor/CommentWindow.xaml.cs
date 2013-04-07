using System.Windows;
using System.Windows.Input;

namespace DiagramDesigner {
  /// <summary>
  /// Interaction logic for CommentWindow.xaml
  /// </summary>
  public partial class CommentWindow : Window {
    public CommentWindow() {
      InitializeComponent();
      textBox.Focus();
    }

    public string comment {
      get {
        return textBox.Text;
      }
      set {
        textBox.Text = value;
      }
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e) {
      DialogResult = true;
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e) {
      DialogResult = false;
    }

    private void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
      if ((e.Key == Key.Enter) && (Keyboard.Modifiers == ModifierKeys.Control)) {
        DialogResult = true;
      }
    }
  }
}
