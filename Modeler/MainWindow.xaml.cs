using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AurelienRibon.Ui.SyntaxHighlightBox;
using AvalonDock.Layout;
using DiagramDesigner;
using DiagramDesigner.Controls;
using Modeler.modeler;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using FlowDirection = System.Windows.FlowDirection;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;
using TreeView = System.Windows.Controls.TreeView;

namespace Modeler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _showPropertyPanel = true;
        public MainWindow()
        {
            InitializeComponent();
            InitModeler();
            
            LoadPalletesFiles();
            MyDesigner.initPalleters(Palleters);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// пример программного управления компонентом
        /// </summary>
        public void programAdd_Sample1()
        {

            //устанавливаем программно размеры области диаграммы
            //DesignerGroupBox.Width = 600;
            //DesignerGroupBox.Height = 600;
            //создаем текст, который будет выводится
            var textBlock = new TextBlock { Text = "Родитель (можно двигать)" };
            //т.к. первая фигура может таскаться, то надо создать контейнер для DragThumb и TextBlock
            var tempGrid = new Grid();
            //добавляем в него textBlock
            tempGrid.Children.Add(textBlock);
            //добавляем в него DragThumb (важно, что после добавления TextBlock), или TextBlock его перекроет и DragThumb работать не будет
            tempGrid.Children.Add(new DragThumb());
            //создаем элемент, который будет добавляться в диаграмму
            var item1 = new DesignerItem { Content = tempGrid };
            //запрещаем его изменения
            item1.CanResize = false;
            //создаем коннекторы для последующих связей
            item1.Connectors = new[] { new Connector(ConnectorOrientation.Bottom, item1, 0.5, 1, "Bottom") };
            //создаем объект для измерения размеров текста
            var formatterText = new FormattedText(textBlock.Text, System.Threading.Thread.CurrentThread.CurrentUICulture,
                  FlowDirection.LeftToRight, new Typeface(textBlock.FontFamily.ToString()), textBlock.FontSize, Brushes.Black);
            //зам. MyDesigner вложен в DesignerGroupBox, поэтому его размер чуть меньше DesignerGroupBox, поэтому 
            //не 300, а 298
            MyDesigner.addDesignerItem(item1, (int)(298 - formatterText.Width / 2), 50);


            var textBlock2 = new TextBlock { Text = "Потомок (нельзя двигать)" };
            var tempGrid2 = new Grid();
            tempGrid2.Children.Add(textBlock2);
            var item2 = new DesignerItem { Content = tempGrid2 };
            item2.CanResize = false;
            item2.Connectors = new[] { new Connector(ConnectorOrientation.Top, item2, 0.5, 0, "Top") };
            MyDesigner.addDesignerItem(item2, 100, 150);

            var textBlock3 = new TextBlock { Text = "Потомок (Можно двигать)" };
            var tempGrid3 = new Grid();
            tempGrid3.Children.Add(textBlock3);
            tempGrid3.Children.Add(new DragThumb());
            var item3 = new DesignerItem { Content = tempGrid3, CanResize = false };
            item3.Connectors = new[] { new Connector(ConnectorOrientation.Top, item3, 0.5, 0, "Top") };
            MyDesigner.addDesignerItem(item3, 400, 100);

            //собственно создание двух связей
            var startConnector = item1.Connectors[0];
            var endConnector = item2.Connectors[0];
            var connection = new Connection(startConnector, endConnector, false);
            connection.Color = Color.FromRgb(0, 255, 0);
            connection.StrokeDashArray = new DoubleCollection { 2, 1 };
            MyDesigner.addConnection(connection);


            var startConnector2 = item1.Connectors[0];
            var endConnector2 = item3.Connectors[0];
            var connection2 = new Connection(startConnector2, endConnector2, true);
            MyDesigner.addConnection(connection2);
        }

        public IToolBoxPalleter[] Palleters { get; private set; }

        internal static IEnumerable<IToolBoxPalleter> GetPalletesFromFile(string file)
        {
            var res = new List<IToolBoxPalleter>();
            try
            {
                var interfaceName = typeof(IToolBoxPalleter).Name;
                var assembly = Assembly.LoadFile(file);
                //показывает, есть ли в файле клиентские плагины
                res.AddRange(from type in assembly.GetTypes()
                             where !type.IsAbstract && !type.IsInterface
                             where type.GetInterface(interfaceName) != null
                             select (IToolBoxPalleter)Activator.CreateInstance(type));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки палитры из файла " + file + "\n" + ex.Message);
            }


            return res.ToArray();
        }

        private void LoadPalletesFiles()
        {
            try
            {
                var appDir = System.Windows.Forms.Application.StartupPath;
                var files = Directory.GetFiles(appDir, "*_pal.dll");
                var localPalleters = new List<IToolBoxPalleter>();
                foreach (var file in files)
                {
                    localPalleters.AddRange(GetPalletesFromFile(file));
                }
                Palleters = localPalleters.ToArray();

                foreach (var palleter in Palleters)
                {
                    ToolBoxStackPanel.Children.Add(palleter.getPanel());
                }
            }
            catch
            {
                Debugger.Break();
            }
        }

        /// <summary>
        /// показывать или нет палитру свойств
        /// </summary>
        public bool ShowPropertyPanel
        {
            get
            {
                return _showPropertyPanel;
            }
            set
            {
                if (_showPropertyPanel == value)
                {
                    return;
                }
                _showPropertyPanel = value;
            }
        }


        private bool SelectConnection()
        {
            var selected =
               MyDesigner.SelectionService.CurrentSelection.OfType<Connection>().ToList();
            if (selected.Count == 0)
            {
                return false;
            }
            var covers = new List<ConnectionCoverForPropertyEditor>();
            foreach (var connection in selected)
            {
                if (connection.isSimple)
                {
                    covers.Add(new SimpleConnectionCoverForPropertyEditor(connection));
                }
                else
                {
                    covers.Add(new PolygonConnectionCoverForPropertyEditor(connection));
                }
            }
            SelectedObjectProperyGrid.SelectedObjects = covers.ToArray();
            PropertyStackPanel.Visibility = Visibility.Visible;
            if (selected.Count == 1)
            {
                SelectDiagramElement(selected[0]);
            }
            return true;
        }

        private bool SelectDesignerItem()
        {
            var selected =
               MyDesigner.SelectionService.CurrentSelection.OfType<DesignerItem>().ToList();
            if (selected.Count == 0)
            {
                ElementCode.Text = "";
                return false;
            }
            var covers =
              (from p in selected where p.Content is IPropertyEditable select ((IPropertyEditable)p.Content).getCover(p)).
                ToArray();
            SelectedObjectProperyGrid.SelectedObjects = covers;
            PropertyStackPanel.Visibility = Visibility.Visible;
            if (selected.Count == 1)
            {
                SelectDiagramElement(selected[0]);
            }
            return true;
        }

        private void MyDesigner_SelectedChanged(object sender, EventArgs e)
        {
            PropertyStackPanel.Visibility = Visibility.Collapsed;
            if (ShowPropertyPanel == false)
            {
                SelectedObjectProperyGrid.Enabled = false;
                return;
            }
            SelectedObjectProperyGrid.Enabled = true;
            SelectedObjectProperyGrid.SelectedObject = null;

            if (SelectConnection())
            {
                return;
            }
            if (SelectDesignerItem())
            {
                return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _modelCode.FlushAll(MyDesigner.Children);
            /*if (!_is_changed)
            {
                e.Cancel = true;
                Application.Current.Shutdown();
                return;

            }
            var res = MessageBox.Show("Сохранить файл?", "Вопрос", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            if (res == MessageBoxResult.Yes)
            {
                ApplicationCommands.Save.Execute(null, null);
            }*/
        }
    }
}
