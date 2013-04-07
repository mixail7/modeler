using System.Windows;

namespace Modeler
{
    /// <summary>
    /// Логика взаимодействия для RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        public RenameWindow()
        {
            InitializeComponent();
        }
        
        public RenameWindow(string oldName)
        {
            InitializeComponent();
            NewNameTextBox.Text = oldName;
        }

        public string _newName = null;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _newName = NewNameTextBox.Text;
            this.Close();
        }
    }
}
