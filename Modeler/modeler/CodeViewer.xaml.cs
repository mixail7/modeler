using System;
using System.IO;
using System.Windows;
using AurelienRibon.Ui.SyntaxHighlightBox;

namespace Modeler.modeler
{
    /// <summary>
    /// Логика взаимодействия для CodeViewer.xaml
    /// </summary>
    public partial class CodeViewer 
    {
        public CodeViewer(string fileName)
        {
            InitializeComponent();
            Code.CurrentHighlighter = HighlighterManager.Instance.Highlighters["RUBY"];
            Title = fileName;
            try
            {
                Code.Text = File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
