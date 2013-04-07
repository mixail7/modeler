using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Modeler.modeler
{
    /// <summary>
    /// Производит отображение файловой системы в дерево моделей
    /// </summary>
    public partial class ModelerTreeView
    {
        private Project _project;
        public event EventHandler OpenModel;
        public event EventHandler RenameElement;
        public event EventHandler GenerateCodeForElement;


        public ModelerTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// создает элемент для отображение в дереве элементов
        /// </summary>
        /// <param name="fileName">путь к файлу который необходимо добавить в дерево. имя нужно только для определения с какой иконкой его рисовать(по расширению определяется)</param>
        /// <returns>созданный элемент дерева. в параметре TAG храниться путь к файлу на диске</returns>
        private TreeViewItem CreateItem(string fileName)
        {
            var menu = (ContextMenu)Resources["TreeMenu"];
            var rootItem = new TreeViewItem
            {
                Tag = fileName,
                ContextMenu = menu
            };

            var stack = new StackPanel { Orientation = Orientation.Horizontal };

            var fileType = "";

            if (File.Exists(fileName))
            {
                var ext = Path.GetExtension(fileName);
                switch (ext)
                {
                    case ".model":
                        fileType = "model.ico";
                        rootItem.IsExpanded = false;
                        break;
                    case ".mdd":
                        fileType = "project.ico";
                        rootItem.IsExpanded = true;
                        rootItem.Tag = _project.FileModelPath;
                        break;
                    default:
                        return null;
                }
            }
            else if (Directory.Exists(fileName))
            {
                fileType = "folder.ico";
                rootItem.IsExpanded = true;
            }
            var image = new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,/Resources/img/" + fileType)),
                Height = 16,
                Width = 16
            };

            var lbl = new TextBlock
            {
                Text = Path.GetFileNameWithoutExtension(fileName),
                Width = 200,
                TextWrapping = TextWrapping.Wrap
            };

            stack.Children.Add(image);
            stack.Children.Add(lbl);

            rootItem.Header = stack;
            return rootItem;
        }

        /// <summary>
        /// строет стрпуктуру 1 уровня для папки
        /// </summary>
        /// <param name="item">элементфайла с которого будем строить дерево</param>
        public void ScanDir(TreeViewItem item)
        {
            if (item.Tag == null)
                return;
            if (!Directory.Exists(item.Tag.ToString()))
            {
                return;
            }
            item.Items.Clear();
            var entries = Directory.GetFileSystemEntries(item.Tag.ToString());
            foreach (var entrie in entries)
            {
                var res = CreateItem(entrie);
                if (Directory.Exists(entrie))
                {
                    var it = new TreeViewItem();
                    res.IsExpanded = false;
                    res.Items.Add(it);
                }
                if (res != null)
                    item.Items.Add(res);
            }
        }

        /// <summary>
        /// визуализирует представление файловой системы в дерево моделей
        /// </summary>
        /// <param name="project"></param>
        public void LoadProject(Project project)
        {
            _project = project;
            MyTree.Items.Clear();
            MyTree.Items.Add(CreateItem(_project.FileName));
            ScanDir(MyTree.Items[0] as TreeViewItem);
        }
        /// <summary>
        /// событие открытия новой папки - производит достройку дерева
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MyTree_OnExpanded(object sender, RoutedEventArgs e)
        {
            var tvi = e.OriginalSource as TreeViewItem;
            if (tvi == null) return;
            if (Directory.Exists(tvi.Tag.ToString()))
            {
                ScanDir(tvi);
            }
        }

        /// <summary>
        /// получает имя для конкретной модели или подсистемы
        /// </summary>
        /// <param name="item">элемент дерева для которой нужно получить имя</param>
        /// <returns></returns>
        private string getTitleForTreeviewItem(TreeViewItem item)
        {
            if (item != null)
            {
                var st = item.Header as StackPanel;
                if (st != null)
                {
                    var tb = st.Children[1] as TextBlock;
                    if (tb != null)
                    {
                        return tb.Text;
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// переименовывает элемент проекта
        /// </summary>
        /// <param name="newName">новое имя</param>
        /// <param name="tvi">элемент дерева, который необходио переименовать</param>
        private void RenameNode(string newName, TreeViewItem tvi)
        {
            try
            {
                if (tvi == null) return;
                var st = tvi.Header as StackPanel;
                if (st == null) return;
                var tb = st.Children[1] as TextBlock;
                if (tb == null) return;
                tb.Text = newName;
                var path = tvi.Tag as string;
                if (path == null) return;
                if (File.Exists(path))
                {
                    var newPath = Path.GetDirectoryName(path) + "\\" + newName + Path.GetExtension(path);
                    File.Move(path, newPath);
                    tvi.Tag = newPath;
                    RenameElement(tvi.Tag, EventArgs.Empty);
                    newPath = Path.GetDirectoryName(path) + "\\" + newName + ".code";
                    path = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".code";
                    if(File.Exists(path))
                        File.Move(path, newPath);
                }
                else
                {
                    //Это под проект (папка)
                    if (Directory.Exists(path))
                    {
                        string str = Directory.GetParent(path).ToString();
                        str += "\\" + newName;
                        Directory.Move(path, str);
                        tvi.Tag = str;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// переименование элемента проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemRename_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var tvi = (TreeViewItem)MyTree.SelectedItem;
                if (tvi != null)
                    tvi.IsSelected = true;
                var title = getTitleForTreeviewItem(tvi);
                var rn = new RenameWindow(title);
                rn.ShowDialog();
                RenameNode(rn._newName, tvi);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }
        /// <summary>
        /// удаление модели или подсистемы из проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Хотите удалить выбранный элемент?", "", MessageBoxButton.OKCancel,
                                                  MessageBoxImage.Question);
            if (res != MessageBoxResult.OK) return;
            var tvi = (TreeViewItem)MyTree.SelectedItem;
            var s = tvi.Tag.ToString();
            var parent = (TreeViewItem)tvi.Parent;
            parent.Items.Remove(tvi);
            if (File.Exists(s))
                File.Delete(s);
            else if (Directory.Exists(s))
                Directory.Delete(s, true);
        }
        /// <summary>
        /// событие добавление новой модели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var tvi = (TreeViewItem)MyTree.SelectedItem;
            var rn = new RenameWindow("");
            rn.ShowDialog();
            if (rn._newName == null)
            {
                return;
            }
            string path = tvi.Tag.ToString();
            path += "\\" + rn._newName + ".model";
            try
            {
                File.Create(path);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
            TreeViewItem res = CreateItem(path);
            tvi.Items.Add(res);
        }
        /// <summary>
        /// скрывает значение добавить модель если выбрана модель
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            var tvi = ((ContextMenu)sender).PlacementTarget as TreeViewItem;
            if (tvi == null) return;
            tvi.IsSelected = true;
            var menu = (ContextMenu)sender;
            var item = menu.Items[2] as MenuItem;
            if (item == null) return;
            item.IsEnabled = Directory.Exists(tvi.Tag.ToString());
            /*if (tvi.Tag.ToString() == _project.FileModelPath)
            {
                item = menu.Items[0] as MenuItem;
                if (item == null) return;
                item.IsEnabled = false;
                item = menu.Items[1] as MenuItem;
                if (item == null) return;
                item.IsEnabled = false;
            }*/
        }

        /// <summary>
        /// событие по которому происходит открытие модели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTree_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tvi = MyTree.SelectedItem as TreeViewItem;
            if (tvi == null) return;
            if (Directory.Exists(tvi.Tag.ToString())) return;
            OpenModel(tvi.Tag, EventArgs.Empty);
        }

        private void MenuGenerateCode_OnClick(object sender, RoutedEventArgs e)
        {
            var tvi = MyTree.SelectedItem as TreeViewItem;
            if (tvi == null) return;
            GenerateCodeForElement(tvi.Tag, EventArgs.Empty);
        }
    }
}
