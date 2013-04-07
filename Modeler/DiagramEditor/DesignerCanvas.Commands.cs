using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using DiagramDesigner.Controls;
using Microsoft.Win32;
using Modeler;

namespace DiagramDesigner
{
    public partial class DesignerCanvas
    {
        public static RoutedCommand Group = new RoutedCommand();
        public static RoutedCommand Ungroup = new RoutedCommand();
        public static RoutedCommand BringForward = new RoutedCommand();
        public static RoutedCommand BringToFront = new RoutedCommand();
        public static RoutedCommand SendBackward = new RoutedCommand();
        public static RoutedCommand SendToBack = new RoutedCommand();
        public static RoutedCommand AlignTop = new RoutedCommand();
        public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        public static RoutedCommand AlignBottom = new RoutedCommand();
        public static RoutedCommand AlignLeft = new RoutedCommand();
        public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        public static RoutedCommand AlignRight = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand DeleteSelection = new RoutedCommand();
        public static RoutedCommand Check = new RoutedCommand();
        public static RoutedCommand Convert = new RoutedCommand();
        public static RoutedCommand CreateNewAD = new RoutedCommand();


        public static RoutedCommand ArrowLink = new RoutedCommand();
        public static RoutedCommand DiamondLink = new RoutedCommand();
        public static RoutedCommand NoneLink = new RoutedCommand();

        public static RoutedCommand LineConnection = new RoutedCommand();
        public static RoutedCommand PolyLineConnection = new RoutedCommand();
        /*
        public static RoutedCommand AllowResize = new RoutedCommand();
        public static RoutedCommand BanResize = new RoutedCommand();

        public static RoutedCommand AllowMove = new RoutedCommand();
        public static RoutedCommand BanMove = new RoutedCommand();
        */
        public static RoutedCommand TestCommand = new RoutedCommand();

        public static RoutedCommand IncreaseScaleCommand = new RoutedCommand();
        public static RoutedCommand DecreaseScaleCommand = new RoutedCommand();
        public static RoutedCommand NormScaleCommand = new RoutedCommand();

        public static RoutedCommand CreateAbsoluteIndexCommand = new RoutedCommand();
        public static RoutedCommand CreateRelativeIndexCommand = new RoutedCommand();
        public static RoutedCommand ShowIndexCommand = new RoutedCommand();

        public static RoutedCommand CreateCommentCommand = new RoutedCommand();
        public static RoutedCommand ShowCommentCommand = new RoutedCommand();

        public static RoutedCommand CreateUserPalleterCommand = new RoutedCommand();
        public static RoutedCommand SaveAsImageCommand = new RoutedCommand();


        public DesignerCanvas()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(DeleteSelection, Delete_Executed, Delete_Enabled));
            this.CommandBindings.Add(new CommandBinding(Convert, Convert_Executed, Convert_Enabled));
            this.CommandBindings.Add(new CommandBinding(CreateNewAD, CreateNewAD_Executed, CreateNewAD_Enabled));
            this.CommandBindings.Add(new CommandBinding(Check, Check_Executed, Check_Enabled));

            this.CommandBindings.Add(new CommandBinding(Group, Group_Executed, Group_Enabled));
            this.CommandBindings.Add(new CommandBinding(Ungroup, Ungroup_Executed, Ungroup_Enabled));
            this.CommandBindings.Add(new CommandBinding(BringForward, BringForward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(BringToFront, BringToFront_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(SendBackward, SendBackward_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(SendToBack, SendToBack_Executed, Order_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignTop, AlignTop_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignBottom, AlignBottom_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignLeft, AlignLeft_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(AlignRight, AlignRight_Executed, Align_Enabled));
            this.CommandBindings.Add(new CommandBinding(SelectAll, SelectAll_Executed));
            this.CommandBindings.Add(new CommandBinding(ArrowLink, ArrowLink_Executed));
            this.CommandBindings.Add(new CommandBinding(NoneLink, NoneLink_Executed));
            this.CommandBindings.Add(new CommandBinding(DiamondLink, DiamondLink_Executed));

            this.CommandBindings.Add(new CommandBinding(LineConnection, LineConnection_Executed));
            this.CommandBindings.Add(new CommandBinding(PolyLineConnection, PolyLineConnection_Executed));

            //this.CommandBindings.Add(new CommandBinding(AllowResize, AllowResize_Executed));
            //this.CommandBindings.Add(new CommandBinding(BanResize, BanResize_Executed));
            ////
            //this.CommandBindings.Add(new CommandBinding(AllowMove, AllowMove_Executed));
            //this.CommandBindings.Add(new CommandBinding(BanMove, BanMove_Executed));


            this.CommandBindings.Add(new CommandBinding(TestCommand, TestCommand_Executed));
            this.CommandBindings.Add(new CommandBinding(IncreaseScaleCommand, IncreaseScaleCommand_Executed));
            this.CommandBindings.Add(new CommandBinding(DecreaseScaleCommand, DecreaseScaleCommand_Executed));
            this.CommandBindings.Add(new CommandBinding(NormScaleCommand, NormScaleCommand_Executed));


            this.CommandBindings.Add(new CommandBinding(CreateAbsoluteIndexCommand, CreateAbsoluteIndexCommand_Executed, CreateAbsoluteIndexCommand_Enabled));
            this.CommandBindings.Add(new CommandBinding(CreateRelativeIndexCommand, CreateRelativeIndexCommand_Executed, CreateRelativeIndexCommand_Enabled));
            this.CommandBindings.Add(new CommandBinding(ShowIndexCommand, ShowIndexCommand_Executed, ShowIndexCommand_Enabled));

            this.CommandBindings.Add(new CommandBinding(CreateCommentCommand, CreateCommentCommand_Executed, CreateCommentCommand_Enabled));
            this.CommandBindings.Add(new CommandBinding(ShowCommentCommand, ShowCommentCommand_Executed, ShowCommentCommand_Enabled));
            this.CommandBindings.Add(new CommandBinding(CreateUserPalleterCommand, CreateUserPalleterCommand_Executed));

            this.CommandBindings.Add(new CommandBinding(SaveAsImageCommand, SaveAsImage_Executed));



            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            DeleteSelection.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Control));

            this.AllowDrop = true;
            Clipboard.Clear();
        }

        #region New Command

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Children.Clear();
            this.SelectionService.ClearSelection();
            fileName = "";
        }

        #endregion

        #region Open Command
        /// <summary>
        /// получение типа начала/окончания стрелки из XML узла-описания
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static ArrowSymbol getArrowSymbolFromNode(XElement node)
        {
            var values = Enum.GetValues(typeof(ArrowSymbol));
            foreach (var value in values)
            {
                if (value.ToString() == node.Value)
                {
                    return (ArrowSymbol)value;
                }
            }
            throw new ApplicationException("Неизвестный тип начала/окончания стрелки");
        }

        public void Save_Executed(string fileName)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            Dictionary<string, string> typesDict;
            Dictionary<string, object> binaryDataDict;
            XElement designerItemsXML = SerializeDesignerItems(designerItems, out typesDict, out binaryDataDict);
            XElement connectionsXML = SerializeConnections(connections);

            var root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            SaveFile(root, typesDict, binaryDataDict, fileName);
        }

        public void Open_Executed(string f_name)
        {
            Dictionary<string, string> typesDict;
            Dictionary<string, object> binaryDataDict;
            XElement root;
            using (var fs = File.OpenRead(f_name))
            {
                var formatter = new BinaryFormatter();
                var xmlStr = (string)formatter.Deserialize(fs);
                root = XElement.Parse(xmlStr);
                typesDict = (Dictionary<string, string>)formatter.Deserialize(fs);
                binaryDataDict = (Dictionary<string, object>)formatter.Deserialize(fs);
                fs.Close();
            }

            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (var itemXML in itemsXML)
            {
                var id = new Guid(itemXML.Element("ID").Value);
                var item = DeserializeDesignerItem(itemXML, id, 0, 0, typesDict, binaryDataDict);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                var sourceID = new Guid(connectionXML.Element("SourceID").Value);
                var sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);
                var connection = new Connection(sourceConnector, sinkConnector, false);
                connection.ID = new Guid(connectionXML.Element("ID").Value);
                SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                loadAdditionalConnectionInfo(connection, connectionXML);

                this.Children.Add(connection);
            }

        }

        public void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dictionary<string, string> typesDict;
            Dictionary<string, object> binaryDataDict;
            var root = LoadSerializedDataFromFile(out typesDict, out binaryDataDict);

            if (root == null)
            {
                return;
            }

            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (var itemXML in itemsXML)
            {
                var id = new Guid(itemXML.Element("ID").Value);
                var item = DeserializeDesignerItem(itemXML, id, 0, 0, typesDict, binaryDataDict);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                var sourceID = new Guid(connectionXML.Element("SourceID").Value);
                var sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);
                var connection = new Connection(sourceConnector, sinkConnector, false);
                SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                loadAdditionalConnectionInfo(connection, connectionXML);

                this.Children.Add(connection);
            }
        }

        #endregion

        #region Save Command

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            Dictionary<string, string> typesDict;
            Dictionary<string, object> binaryDataDict;
            XElement designerItemsXML = SerializeDesignerItems(designerItems, out typesDict, out binaryDataDict);
            XElement connectionsXML = SerializeConnections(connections);

            var root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            SaveFile(root, typesDict, binaryDataDict, "");
        }

        #endregion

        #region Print Command

        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.ClearSelection();

            var printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "WPF Diagram");
            }
        }

        #endregion

        #region Copy Command

        private void SaveAsImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            const string imageFilter = "JPG files (*.jpg)|*.jpg|GIF files (*.gif)|*.gif|PNG Files (*.png)|*.png";
            var saveFileDialog = new SaveFileDialog { Filter = imageFilter };
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }
            string file = saveFileDialog.FileName;
            try
            {
                var bmp = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(this);

                string Extension = Path.GetExtension(file).ToLower();
                BitmapEncoder encoder;
                if (Extension == ".gif")
                    encoder = new GifBitmapEncoder();
                else if (Extension == ".png")
                    encoder = new PngBitmapEncoder();
                else if (Extension == ".jpg")
                    encoder = new JpegBitmapEncoder();
                else
                    return;
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                using (Stream stm = File.Create(file))
                {
                    encoder.Save(stm);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ошибка : " + exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
        }

        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion


        #region CreateNewAD
        private void CreateNewAD_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("354 DesignCanvas.Commands.cs");
            /*Window2 wn = new Window2();            
            wn.ShowDialog();            */
        }

        private static void CreateNewAD_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = Clipboard.ContainsData(DiagramCopyData.formatName);
            e.CanExecute = true;
        }
        #endregion
        #region Convert
        private void Convert_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("368 DesignCanvas.Commands.cs");
            /*Window2 wn = new Window2();
            wn.Show();*/
            //DiagramDesigner.
            //Window1
            MessageBox.Show("ok");
        }

        private static void Convert_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = Clipboard.ContainsData(DiagramCopyData.formatName);
            e.CanExecute = true;
        }

        #endregion
        #region Check
        private void Check_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var designerItems = this.Children.OfType<DesignerItem>();
            var connections = this.Children.OfType<Connection>();

            MessageBox.Show("sd" + designerItems.Count().ToString());
        }

        private static void Check_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion
        #region Paste Command

        public static void loadAdditionalConnectionInfo(Connection connection, XElement xmlDescription)
        {
            connection.SourceArrowSymbol = getArrowSymbolFromNode(xmlDescription.Element("SourceArrowSymbol"));
            connection.SinkArrowSymbol = getArrowSymbolFromNode(xmlDescription.Element("SinkArrowSymbol"));
            var tempStr = xmlDescription.Element("Color").Value.Split(' ');
            connection.Color = Color.FromRgb(Byte.Parse(tempStr[0]), Byte.Parse(tempStr[1]), Byte.Parse(tempStr[2]));
            connection.Text = xmlDescription.Element("Text").Value;
            connection.StartText = xmlDescription.Element("StartText").Value;
            connection.EndText = xmlDescription.Element("EndText").Value;
            connection.isSimple = Boolean.Parse(xmlDescription.Element("IsSimple").Value);
            connection.HorizontalText = Boolean.Parse(xmlDescription.Element("HorizontalText").Value);
            connection.StrokeDashArray = getStrokeDashArrayFromXmlDescription(xmlDescription.Element("DashArray"));
        }


        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var diagramData = getDiagramDataFromClipboard();
            XElement root = diagramData.xmlData;

            if (root == null)
            {
                return;
            }

            //соответствие старого и нового GUID
            var mappingOldToNewIDs = new Dictionary<Guid, Guid>();
            var newItems = new List<ISelectable>();
            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");

            double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
            double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);

            foreach (XElement itemXML in itemsXML)
            {
                var oldID = new Guid(itemXML.Element("ID").Value);
                var newID = Guid.NewGuid();
                mappingOldToNewIDs.Add(oldID, newID);
                DesignerItem item = DeserializeDesignerItem(itemXML, newID, offsetX, offsetY, diagramData.typesNamesDict, diagramData.binaryDataDict);

                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
                newItems.Add(item);
                item.Connectors = new[] {
                                     new Connector(ConnectorOrientation.Left, item, 0, 0.5, "Left"),
                                     new Connector(ConnectorOrientation.Right, item, 1, 0.5, "Right"),
                                     new Connector(ConnectorOrientation.Top, item, 0.5, 0, "Top"),
                                     new Connector(ConnectorOrientation.Bottom, item, 0.5, 1, "Bottom"),
                                     new Connector(ConnectorOrientation.None, item, 0.5, 0.5, "Center")
                                   };

            }

            // update group hierarchy
            SelectionService.ClearSelection();
            foreach (DesignerItem el in newItems)
            {
                if (el.ParentID != Guid.Empty)
                    el.ParentID = mappingOldToNewIDs[el.ParentID];
            }


            foreach (DesignerItem item in newItems)
            {
                if (item.ParentID == Guid.Empty)
                {
                    SelectionService.AddToSelection(item);
                }
            }

            // create Connections
            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                var oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                var oldSinkID = new Guid(connectionXML.Element("SinkID").Value);
                //по-хорошему, если это условие не выполняется, то повреждена структура данных
                if (!mappingOldToNewIDs.ContainsKey(oldSourceID) || !mappingOldToNewIDs.ContainsKey(oldSinkID))
                {
                    continue;
                }
                Guid newSourceID = mappingOldToNewIDs[oldSourceID];
                Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                var sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                var sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                var sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                var sinkConnector = GetConnector(newSinkID, sinkConnectorName);
                var connection = new Connection(sourceConnector, sinkConnector, false);
                SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));

                loadAdditionalConnectionInfo(connection, connectionXML);
                this.Children.Add(connection);

                SelectionService.AddToSelection(connection);
            }

            BringToFront.Execute(null, this);

            // update paste offset
            root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
            root.Attribute("OffsetY").Value = (offsetY + 10).ToString();
            Clipboard.Clear();
            copyDiagramDataToClipboard(diagramData);
        }

        private static void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsData(DiagramCopyData.formatName);
        }

        #endregion

        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurrentSelection();
        }

        private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var temp = this.SelectionService.CurrentSelection.Count() > 0;
            e.CanExecute = temp;
        }

        #endregion

        #region Cut Command

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
            DeleteCurrentSelection();
        }

        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region Group Command

        private void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var items = getSelectedItems();

            var rect = GetBoundingRectangle(items);

            var groupItem = new DesignerItem { IsGroup = true, Width = rect.Width, Height = rect.Height };
            SetLeft(groupItem, rect.Left);
            SetTop(groupItem, rect.Top);

            var groupCanvas = new Canvas();
            groupItem.Content = groupCanvas;
            SetZIndex(groupItem, this.Children.Count);
            this.Children.Add(groupItem);

            foreach (var item in items)
            {
                item.ParentID = groupItem.ID;
            }

            this.SelectionService.SelectItem(groupItem);
        }

        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int count = getSelectedItems().Count();

            e.CanExecute = count > 1;
        }

        #endregion

        #region Ungroup Command

        private void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentID == Guid.Empty
                          select item).ToArray();

            foreach (DesignerItem groupRoot in groups)
            {
                var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                               where child.ParentID == groupRoot.ID
                               select child;

                foreach (DesignerItem child in children)
                    child.ParentID = Guid.Empty;

                this.SelectionService.RemoveFromSelection(groupRoot);
                this.Children.Remove(groupRoot);
                UpdateZIndex();
            }
        }

        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 0;
        }

        #endregion

        #region BringForward Command


        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ordered = (from item in SelectionService.CurrentSelection
                           orderby GetZIndex(item as UIElement) descending
                           select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                var currentIndex = GetZIndex(ordered[i]);
                var newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex == newIndex)
                {
                    continue;
                }

                SetZIndex(ordered[i], newIndex);
                var it = this.Children.OfType<UIElement>().Where(item => GetZIndex(item) == newIndex);

                foreach (UIElement elm in it)
                {
                    if (elm != ordered[i])
                    {
                        SetZIndex(elm, currentIndex);
                        break;
                    }
                }
            }
        }

        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
        }

        #endregion

        #region BringToFront Command

        private void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectionSorted = (from item in SelectionService.CurrentSelection
                                   orderby GetZIndex(item as UIElement) ascending
                                   select item as UIElement).ToList();

            var childrenSorted = (from UIElement item in this.Children
                                  orderby GetZIndex(item) ascending
                                  select item).ToList();

            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    SetZIndex(item, i++);
                }
            }
        }

        #endregion

        #region SendBackward Command

        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();


            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = GetZIndex(ordered[i]);
                int newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex == newIndex)
                {
                    continue;
                }
                SetZIndex(ordered[i], newIndex);
                IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => GetZIndex(item) == newIndex);
                foreach (UIElement elm in it)
                {
                    if (elm != ordered[i])
                    {
                        SetZIndex(elm, currentIndex);
                        break;
                    }
                }
            }
        }

        #endregion

        #region SendToBack Command

        private void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectionSorted = (from item in SelectionService.CurrentSelection
                                   orderby GetZIndex(item as UIElement) ascending
                                   select item as UIElement).ToList();

            var childrenSorted = (from UIElement item in this.Children
                                  orderby GetZIndex(item) ascending
                                  select item).ToList();
            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    SetZIndex(item, j++);
                }
                else
                {
                    SetZIndex(item, selectionSorted.Count + i++);
                }
            }
        }

        #endregion

        #region AlignTop Command

        private void setTopDeltaInGroupMembers(DesignerItem item, double delta)
        {
            foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
            {
                SetTop(di, GetTop(di) + delta);
            }
        }

        private void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();

            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double top = GetTop(selectedItems.First());

            foreach (DesignerItem item in selectedItems)
            {
                double delta = top - GetTop(item);
                setTopDeltaInGroupMembers(item, delta);
            }
        }

        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = getSelectedItems();
            e.CanExecute = groupedItem.Count() > 1;
        }

        #endregion

        #region AlignVerticalCenters Command

        private void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();

            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double bottom = GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

            foreach (DesignerItem item in selectedItems)
            {
                double delta = bottom - (GetTop(item) + item.Height / 2);
                setTopDeltaInGroupMembers(item, delta);
            }
        }

        #endregion

        #region AlignBottom Command

        private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();

            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double bottom = GetTop(selectedItems.First()) + selectedItems.First().Height;

            foreach (DesignerItem item in selectedItems)
            {
                double delta = bottom - (GetTop(item) + item.Height);
                setTopDeltaInGroupMembers(item, delta);
            }
        }

        #endregion

        #region AlignLeft Command
        /// <summary>
        /// получает "Корневые" выделенные элементы диаграммы
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DesignerItem> getSelectedItems()
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;
            return selectedItems;
        }

        private void setLeftDeltaInGroupMembers(DesignerItem item, double delta)
        {
            foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
            {
                SetLeft(di, GetLeft(di) + delta);
            }
        }

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();
            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double left = GetLeft(selectedItems.First());

            foreach (DesignerItem item in selectedItems)
            {
                double delta = left - GetLeft(item);
                setLeftDeltaInGroupMembers(item, delta);
            }
        }

        #endregion

        #region AlignHorizontalCenters Command

        private void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();

            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double center = GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

            foreach (DesignerItem item in selectedItems)
            {
                double delta = center - (GetLeft(item) + item.Width / 2);
                setLeftDeltaInGroupMembers(item, delta);
            }
        }

        #endregion

        #region AlignRight Command

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = getSelectedItems();

            if (selectedItems.Count() <= 1)
            {
                return;
            }
            double right = GetLeft(selectedItems.First()) + selectedItems.First().Width;

            foreach (DesignerItem item in selectedItems)
            {
                double delta = right - (GetLeft(item) + item.Width);
                setLeftDeltaInGroupMembers(item, delta);
            }
        }



        #endregion

        #region SelectAll Command

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }

        #endregion

        #region ArrowCommands

        private void NoneLink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentArrowSymbol = ArrowSymbol.None;
        }

        private void ArrowLink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentArrowSymbol = ArrowSymbol.Arrow;
        }

        private void DiamondLink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            currentArrowSymbol = ArrowSymbol.Diamond;
        }
        #endregion

        #region connectionTypes

        private void PolyLineConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            simpleConnections = false;
        }

        private void LineConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            simpleConnections = true;
        }

        #endregion
        /*
    #region allow/ban Resize, Move

    private void AllowResize_Executed(object sender, ExecutedRoutedEventArgs e) {
      var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          select item;

      foreach (DesignerItem item in selectedItems) {

        item.SetValue(DesignerItem.CanResizeProperty, true);
        //item.CanResize = true;
      }
      MessageBox.Show("разрешено изменение размеров");
    }

    private void BanResize_Executed(object sender, ExecutedRoutedEventArgs e) {
      var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          select item;

      foreach (DesignerItem item in selectedItems) {
        item.SetValue(DesignerItem.CanResizeProperty, false);
        //item.CanResize = false;
      }
      MessageBox.Show("запрещено изменение размеров");
    }

    private void AllowMove_Executed(object sender, ExecutedRoutedEventArgs e) {
      var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          select item;

      foreach (DesignerItem item in selectedItems) {
        item.SetValue(DesignerItem.CanMoveProperty, true);
        //item.CanMove = true;
      }
      //MessageBox.Show("разрешено перетаскивание");
    }

    private void BanMove_Executed(object sender, ExecutedRoutedEventArgs e) {
      var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          select item;

      foreach (DesignerItem item in selectedItems) {
        //item.SetValue(DesignerItem.CanMoveProperty, false);
        //item.CanMove = false;
      }
      //MessageBox.Show("запрещено перетаскивание");
    }


    #endregion
    */
        #region testCommand

        private void IncreaseScaleCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.scaleSize *= 1.1;
            this.LayoutTransform = new ScaleTransform(scaleSize, scaleSize);
        }

        private void DecreaseScaleCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.scaleSize /= 1.1;
            this.LayoutTransform = new ScaleTransform(scaleSize, scaleSize);
        }

        private void NormScaleCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.scaleSize = 1;
            this.LayoutTransform = new ScaleTransform(1, 1);
        }

        private void CreateAbsoluteIndexCommand_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                 select item).ToArray();
            e.CanExecute = (selectedItems.Length == 1);
        }

        private void CreateRelativeIndexCommand_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                e.CanExecute = false;
                return;
            }
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                 select item).ToArray();
            e.CanExecute = (selectedItems.Length == 1);
        }


        private void ShowIndexCommand_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                 select item).ToArray();
            if (selectedItems.Length != 1)
            {
                e.CanExecute = false;
                return;
            }
            var selItem = selectedItems[0];
            e.CanExecute = (indexs.ContainsKey(selItem.ID));
        }

        private void CreateRelativeIndexCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Укажите файл";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            //получаем относительный путь относительно файла с диаграмммой
            // .. означает необходимость подъема вверх по директориям на 1 уровень
            var fileParts = Path.GetDirectoryName(fileName).Split(Path.DirectorySeparatorChar).ToList();
            var valueParts = openFileDialog.FileName.Split(Path.DirectorySeparatorChar).ToList();
            while (true)
            {
                if (fileParts.Count == 0)
                {
                    break;
                }
                if (valueParts.Count == 0)
                {
                    break;
                }
                if (valueParts[0].ToLower() == fileParts[0].ToLower())
                {
                    valueParts.RemoveAt(0);
                    fileParts.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            var indexType = IndexTypesEnum.RelativeFileLink;
            var value = "";
            //сколько других частей осталось - на столько и надо подниматься вверх
            for (int i = 0; i < fileParts.Count; i++)
            {
                value = Path.Combine(value, "..");
            }
            //а потом добавляем ту часть, которая указывает на другой файл
            if (Path.GetPathRoot(fileName) != Path.GetPathRoot(openFileDialog.FileName))
            {
                //но если файл расположен на другом диске, то необходимо прописать полный путь
                //потому что относительная ссылка в данном случае - бред
                value = openFileDialog.FileName;
                //поэтому её переводим в абсолютную
                indexType = IndexTypesEnum.FileLink;
            }
            else
            {

                for (int i = 0; i < valueParts.Count; i++)
                {
                    value = Path.Combine(value, valueParts[i]);
                }
            }


            var newIndex = new IndexClass { indexType = indexType, value = value };
            var selItem = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                           select item).First();

            if (indexs.ContainsKey(selItem.ID))
            {
                indexs.Remove(selItem.ID);
            }
            indexs.Add(selItem.ID, newIndex);
        }


        private void CreateAbsoluteIndexCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selItem = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                           select item).First();
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Укажите файл";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            var newIndex = new IndexClass { indexType = IndexTypesEnum.FileLink, value = openFileDialog.FileName };

            if (indexs.ContainsKey(selItem.ID))
            {
                indexs.Remove(selItem.ID);
            }
            indexs.Add(selItem.ID, newIndex);
        }

        private void ShowIndexCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selItem = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                           select item).First();
            Debug.Assert(indexs.ContainsKey(selItem.ID), "Просмотр индекса у элемента, у которого нет индексной ссылки");

            string linkFileName = "";
            if (indexs[selItem.ID].indexType == IndexTypesEnum.FileLink)
            {
                linkFileName = indexs[selItem.ID].value;
            }
            else if (indexs[selItem.ID].indexType == IndexTypesEnum.RelativeFileLink)
            {
                linkFileName = Path.GetDirectoryName(fileName);
                var fileParts = indexs[selItem.ID].value.Split(Path.DirectorySeparatorChar).ToList();
                while (true)
                {
                    if (fileParts.Count == 0)
                    {
                        break;
                    }
                    if (fileParts[0] == "..")
                    {
                        linkFileName = Path.GetDirectoryName(linkFileName);
                        fileParts.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }
                var addStr = string.Join("" + Path.DirectorySeparatorChar, fileParts.ToArray());
                linkFileName = Path.Combine(linkFileName, addStr);
            }
            else
            {
                Debugger.Break();//не предусмотренный тип ссылки
            }

            string startMessage = "К данной фигуре прилагается файл " + linkFileName + ".\n";
            if (File.Exists(linkFileName))
            {
                MessageBox.Show(startMessage + "Но его нет на диске", "Внимание", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var res = MessageBox.Show(startMessage + "Запустить его?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes)
            {
                return;
            }
            Process.Start(linkFileName);
        }

        private void CreateCommentCommand_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                 select item).ToArray();
            var count = selectedItems.Count(p => p.ParentID == Guid.Empty);
            e.CanExecute = (count == 1);
        }


        private void ShowCommentCommand_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                 select item).ToArray();
            var count = selectedItems.Count(p => p.ParentID == Guid.Empty);
            if (count != 1)
            {
                e.CanExecute = false;
                return;
            }
            var parentItem = selectedItems.Single(p => p.ParentID == Guid.Empty);

            e.CanExecute = (comments.ContainsKey(parentItem.ID));
        }


        private void CreateCommentCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selItem = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                           where item.ParentID == Guid.Empty
                           select item).Single();

            var commentWindow = new CommentWindow();
            if (comments.ContainsKey(selItem.ID))
            {
                commentWindow.comment = comments[selItem.ID];
            }
            if (commentWindow.ShowDialog() != true)
            {
                return;
            }
            if (comments.ContainsKey(selItem.ID))
            {
                comments.Remove(selItem.ID);
            }
            comments.Add(selItem.ID, commentWindow.comment);
        }

        private void ShowCommentCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selItem = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                           where item.ParentID == Guid.Empty
                           select item).Single();
            Debug.Assert(comments.ContainsKey(selItem.ID), "Просмотр комментария у элемента, у которого нет комментария");
            MessageBox.Show(comments[selItem.ID], "Комментарий", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateUserPalleterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var appDir = System.Windows.Forms.Application.StartupPath;
                var files = Directory.GetFiles(appDir, "*_pal.dll");
                var localPalleters = new List<IToolBoxPalleter>();
                foreach (var file in files)
                {
                    localPalleters.AddRange(MainWindow.GetPalletesFromFile(file));
                }
                palleters = localPalleters.ToArray();
                var tempForm = new UserPalleter((from p in palleters select new PalleterInfo(p)).ToArray());
                var res = tempForm.ShowDialog();
                if (res == true)
                {
                    MessageBox.Show("Для того, что бы появилась палитра, необходимо перезапустить программу", "Внимание",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            catch
            {
                Debugger.Break();
            }
        }

        private void Rotate(double angle)
        {
            var SelectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()

                                 select item).ToArray();
            foreach (DesignerItem item in SelectedItems)
            {
                var element = item.Content as FrameworkElement;
                if (element != null)
                {
                    var rotateTransform = element.LayoutTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        rotateTransform = new RotateTransform();
                        element.LayoutTransform = rotateTransform;
                    }

                    rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;
                    rotateTransform.CenterX = 0.5;
                    rotateTransform.CenterY = 0.5;
                    SetLeft(item, GetLeft(item) - (item.Height - item.Width) / 2);
                    SetTop(item, GetTop(item) - (item.Width - item.Height) / 2);
                    double width = item.Width;
                    item.Width = item.Height;
                    item.Height = width;
                }
            }
        }
        private void TestCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Rotate(45);
            //Debugger.Break();
            //var selectedConnections = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                           select item).ToArray();
            //string tempStr;


            /*var temp = sel.Template.FindName();
            var child = temp.VisualTree.FirstChild;

            Control cd = this.Template.FindName("PART_ConnectorDecorator", this) as Control;
            Debug.Assert(cd != null, "не найден шаблон");

            var connectors = new List<Connector>();
            DesignerCanvas.GetConnectors(cd, connectors);
            */
            /*
             *  <s:DesignerItem.ConnectorDecoratorTemplate>
                <ControlTemplate>
                  <c:RelativePositionPanel Margin="-4">
                    <s:Connector Orientation="Top" c:RelativePositionPanel.RelativePosition="0.5,0"/>
                    <s:Connector Orientation="Left" c:RelativePositionPanel.RelativePosition="0,0.5"/>
                    <s:Connector Orientation="Right" c:RelativePositionPanel.RelativePosition="1,0.5"/>
                    <s:Connector Orientation="Bottom" c:RelativePositionPanel.RelativePosition="0.5,0.93"/>
                  </c:RelativePositionPanel>
                </ControlTemplate>
              </s:DesignerItem.ConnectorDecoratorTemplate>
             * */

            //foreach (var item in selectedConnections) {
            //  item.Text = "большой длинный текст";
            //}


            //var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                    select item).ToArray();
            //if (selectedItems.Length < 2) {
            //  return;
            //}
            //var firstItem = selectedItems[0];
            //var secondItem = selectedItems[1];
            //var connector1 = GetConnector(firstItem.ID, "Center");
            //var connector2 = GetConnector(secondItem.ID, "Center");


            //Connection newConnection = new Connection(connector1, connector2, true);

            //Canvas.SetZIndex(newConnection, this.Children.Count);

            //newConnection.sinkArrowSymbol = this.currentArrowSymbol;
            //this.Children.Add(newConnection);

        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// фильтр, который необходим для выделения файлов диаграмм
        /// </summary>
        private const string diagramFileFilter = "Designer Files (*.ddf)|*.ddf|All Files (*.*)|*.*";

        private XElement LoadSerializedDataFromFile(out Dictionary<string, string> typesDict, out Dictionary<string, object> binaryDataDict)
        {
            var openFile = new OpenFileDialog { Filter = diagramFileFilter };

            if (openFile.ShowDialog() == false)
            {
                typesDict = null;
                binaryDataDict = null;
                return null;
            }
            try
            {
                fileName = openFile.FileName;
                using (var fs = File.OpenRead(openFile.FileName))
                {
                    var formatter = new BinaryFormatter();
                    var xmlStr = (string)formatter.Deserialize(fs);
                    var res = XElement.Parse(xmlStr);
                    typesDict = (Dictionary<string, string>)formatter.Deserialize(fs);
                    binaryDataDict = (Dictionary<string, object>)formatter.Deserialize(fs);
                    return res;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузку данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                typesDict = null;
                binaryDataDict = null;
                return null;
            }
        }

        private void SaveFile(XElement xElement, Dictionary<string, string> typesDict, Dictionary<string, object> binaryDataDict, string fileName)
        {
            if(fileName == null)
                return;
            if (fileName == "")
            {
                var saveFile = new SaveFileDialog { Filter = diagramFileFilter };
                if (saveFile.ShowDialog() == false)
                {
                    return;
                }
                fileName = saveFile.FileName;
            }
            var formatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, xElement.ToString());
            formatter.Serialize(memoryStream, typesDict);
            formatter.Serialize(memoryStream, binaryDataDict);

            var memoryBytes = new byte[memoryStream.Position];
            memoryStream.Position = 0;
            memoryStream.Read(memoryBytes, 0, memoryBytes.Length);
            File.WriteAllBytes(fileName, memoryBytes);
        }


        private static XElement getConnectorsInfo(DesignerItem item)
        {
            var res = new XElement("Connectors");
            foreach (var connector in item.Connectors)
            {
                var nameElement = new XElement("Name", connector.Name);
                var orientationElement = new XElement("Orientation", connector.Orientation);
                var connectorPos = (Point)connector.GetValue(RelativePositionPanel.RelativePositionProperty);
                var xElement = new XElement("X", connectorPos.X);
                var yElement = new XElement("Y", connectorPos.Y);
                var connectorNode = new XElement("Connector", nameElement, orientationElement, xElement, yElement);
                res.Add(connectorNode);
            }
            return res;
        }

        private XElement SerializeDesignerItems(IEnumerable<DesignerItem> designerItems, out Dictionary<string, string> types,
                                                out Dictionary<string, object> binaryData)
        {
            binaryData = new Dictionary<string, object>();
            types = new Dictionary<string, string>();
            var resXElement = new XElement("DesignerItems");
            foreach (var item in designerItems)
            {
                var xmlData = new XElement("DesignerItem",
                                                    new XElement("Left", GetLeft(item)),
                                                    new XElement("Top", GetTop(item)),
                                                    new XElement("Width", item.Width),
                                                    new XElement("Height", item.Height),
                                                    new XElement("ID", item.ID),
                                                    new XElement("zIndex", GetZIndex(item)),
                                                    new XElement("IsGroup", item.IsGroup),
                                                    new XElement("ParentID", item.ParentID),
                                                    getConnectorsInfo(item));
                if (indexs.ContainsKey(item.ID) && (indexs[item.ID] != null))
                {
                    xmlData.Add(new XElement("IndexType", IndexTypesEnumConverter.getIntValue(indexs[item.ID].indexType)));
                    xmlData.Add(new XElement("IndexValue", indexs[item.ID].value));
                }

                if ((item.RenderTransform != null) && (item.RenderTransform is RotateTransform))
                {
                    var trans = item.RenderTransform as RotateTransform;
                    xmlData.Add(new XElement("Angle", trans.Angle));
                }

                if (comments.ContainsKey(item.ID))
                {
                    xmlData.Add(new XElement("Comment", comments[item.ID]));
                }

                var curGiud = item.ID;

                XElement curXmlData;
                if (item.saveXMLData(out curXmlData))
                {
                    Debug.Assert(curXmlData.Name == "Content_UserXML", "имя узла, описывающего содержимое должно быть Content");
                    xmlData.Add(curXmlData);
                    resXElement.Add(xmlData);
                    continue;
                }
                string typeName;
                object dataObject;
                if (item.saveBinaryData(out typeName, out dataObject))
                {
                    types.Add(curGiud.ToString(), typeName);
                    binaryData.Add(curGiud.ToString(), dataObject);
                    resXElement.Add(xmlData);
                    continue;
                }
                try
                {
                    var contentXaml = XamlWriter.Save(item.Content);
                    xmlData.Add(new XElement("Content", contentXaml));
                    resXElement.Add(xmlData);
                }
                catch
                {
                    Debugger.Break();
                }
            }
            return resXElement;
        }

        private static XElement getStrokeDashArrayXmlDescription(Connection c)
        {
            if (c.StrokeDashArray == null)
            {
                return new XElement("DashArray", "1");
            }
            var helpStr = string.Join(" ", (from p in c.StrokeDashArray select p.ToString()).ToArray());
            return new XElement("DashArray", helpStr);
        }

        private static DoubleCollection getStrokeDashArrayFromXmlDescription(XElement c)
        {
            var tempStr = c.Value;
            var res = new DoubleCollection();
            foreach (var re in tempStr.Split(' '))
            {
                res.Add(Int32.Parse(re));
            }
            if (res.Count == 1)
            {
                return null;
            }
            return res;
        }

        private static XElement SerializeConnections(IEnumerable<Connection> connections)
        {

            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("ID", connection.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", GetZIndex(connection)),
                                      new XElement("HorizontalText", connection.HorizontalText),
                                      new XElement("IsSimple", connection.isSimple),
                                      new XElement("Color", connection.Color.R + " " + connection.Color.G + " " + connection.Color.B),
                                      new XElement("Text", connection.Text),
                                      new XElement("StartText", connection.StartText),
                                      new XElement("EndText", connection.EndText),
                                      getStrokeDashArrayXmlDescription(connection))

                                  );

            return serializedConnections;
        }

        private static Type getTypeByName(string typeName)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblys)
            {
                var res = assembly.GetType(typeName);
                if (res == null)
                {
                    continue;
                }
                return res;
            }
            Debugger.Break();
            throw new ApplicationException("В загруженных сборках не найден тип: " + typeName);
        }

        private static Connector[] getConnectors(XElement itemElement, DesignerItem connectorOwner)
        {
            var orientatrions = Enum.GetValues(typeof(ConnectorOrientation));
            var root = itemElement.Element("Connectors");
            var res = new List<Connector>();
            foreach (var connectorElement in root.Elements())
            {
                var name = connectorElement.Element("Name").Value;
                var posXStr = connectorElement.Element("X").Value;
                posXStr = posXStr.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                posXStr = posXStr.Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                var posX = Double.Parse(posXStr);
                var posYStr = connectorElement.Element("Y").Value;
                posYStr = posYStr.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                posYStr = posYStr.Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                var posY = Double.Parse(posYStr);
                var orientationStr = connectorElement.Element("Orientation").Value;
                foreach (var orientatrion in orientatrions)
                {
                    if (orientatrion.ToString() == orientationStr)
                    {
                        res.Add(new Connector((ConnectorOrientation)orientatrion, connectorOwner, posX, posY, name));
                    }
                }
            }
            return res.ToArray();
        }

        public DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY,
                                                            Dictionary<string, string> typesDict,
                                                            Dictionary<string, object> binaryDataDict)
        {

            var item = new DesignerItem(id);
            var oldId = itemXML.Element("ID").Value;
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);
            SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
            SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));

            if (itemXML.Element("IndexType") != null)
            {
                var intValue = Int32.Parse(itemXML.Element("IndexType").Value);
                var indexType = IndexTypesEnumConverter.getEnumValue(intValue);
                var indexValue = itemXML.Element("IndexValue").Value;
                indexs.Add(id, new IndexClass { indexType = indexType, value = indexValue });
            }

            if (itemXML.Element("Angle") != null)
            {
                var angleValue = Double.Parse(itemXML.Element("Angle").Value);
                item.RenderTransform = new RotateTransform(angleValue);
            }

            if (itemXML.Element("Comment") != null)
            {
                var commentValue = itemXML.Element("Comment").Value;
                if (comments.ContainsKey(id))
                {
                    comments.Remove(id);
                }
                comments.Add(id, commentValue);
            }
            if (binaryDataDict.ContainsKey(oldId))
            {
                var typeName = typesDict[oldId];
                var type = getTypeByName(typeName);
                var typeObject = Activator.CreateInstance(type);
                var binaryLoadableObject = (IBinarySaveable)typeObject;
                binaryLoadableObject.loadData(binaryDataDict[oldId]);
                item.Content = typeObject;
            }
            else if (itemXML.Element("Content_UserXML") != null)
            {
                var element = itemXML.Element("Content_UserXML");
                var typeName = element.Attribute("ObjectType").Value;
                var type = getTypeByName(typeName);
                var typeObject = Activator.CreateInstance(type);
                var xmlLoadableObject = (IXMLSaveable)typeObject;
                xmlLoadableObject.loadData(element);
                item.Content = typeObject;
            }
            else
            {
                var content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
                item.Content = content;
            }

            item.Connectors = getConnectors(itemXML, item);

            return item;
        }

        private void CopyCurrentSelection()
        {
            var selectedDesignerItems = this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            var selectedConnections = this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }

            Dictionary<string, string> typesDict;
            Dictionary<string, object> binaryDataDict;
            XElement designerItemsXML = SerializeDesignerItems(selectedDesignerItems, out typesDict, out binaryDataDict);
            XElement connectionsXML = SerializeConnections(selectedConnections);

            var root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            var data = new DiagramCopyData { xmlData = root, typesNamesDict = typesDict, binaryDataDict = binaryDataDict };
            copyDiagramDataToClipboard(data);
        }

        private static void copyDiagramDataToClipboard(DiagramCopyData data)
        {
            Clipboard.Clear();

            var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            var xmlStr = data.xmlData.ToString();
            formatter.Serialize(ms, xmlStr);
            formatter.Serialize(ms, data.typesNamesDict);
            formatter.Serialize(ms, data.binaryDataDict);

            var byteArray = new byte[ms.Position];
            ms.Position = 0;
            ms.Read(byteArray, 0, byteArray.Length);
            ms.Close();
            Clipboard.SetData(DiagramCopyData.formatName, byteArray);
        }

        private static DiagramCopyData getDiagramDataFromClipboard()
        {
            var byteArray = (byte[])Clipboard.GetData(DiagramCopyData.formatName);
            var data = new DiagramCopyData();
            var ms = new MemoryStream(byteArray);
            var formatter = new BinaryFormatter();

            var xmlStr = (string)formatter.Deserialize(ms);
            data.xmlData = XElement.Parse(xmlStr);
            data.typesNamesDict = (Dictionary<string, string>)formatter.Deserialize(ms);
            data.binaryDataDict = (Dictionary<string, object>)formatter.Deserialize(ms);
            ms.Close();

            Clipboard.SetData(DiagramCopyData.formatName, byteArray);

            return data;
        }

        private void DeleteCurrentSelection()
        {
            foreach (var connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
            }

            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                var cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                var connectors = new List<Connector>();
                GetConnectors(cd, connectors);

                foreach (Connector connector in connectors)
                {
                    foreach (Connection connection in connector.Connections)
                    {
                        this.Children.Remove(connection);
                    }
                }
                this.Children.Remove(item);
            }

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby GetZIndex(item)
                                       select item).ToList();

            for (var i = 0; i < ordered.Count; i++)
            {
                SetZIndex(ordered[i], i);
            }
        }

        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            var x1 = Double.MaxValue;
            var y1 = Double.MaxValue;
            var x2 = Double.MinValue;
            var y2 = Double.MinValue;

            foreach (var item in items)
            {
                x1 = Math.Min(GetLeft(item), x1);
                y1 = Math.Min(GetTop(item), y1);

                x2 = Math.Max(GetLeft(item) + item.Width, x2);
                y2 = Math.Max(GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        internal static void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        internal static void GetDragThumbs(DependencyObject parent, List<DragThumb> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is DragThumb)
                {
                    connectors.Add(child as DragThumb);
                }
                else
                    GetDragThumbs(child, connectors);
            }
        }

        public Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();
            return designerItem.Connectors.Single(p => p.Name == connectorName);
        }

        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        #endregion
    }
    /// <summary>
    /// данные, которые используются при копировании элементов диаграммы
    /// </summary>
    [Serializable]
    public class DiagramCopyData
    {
        public XElement xmlData { get; set; }
        /// <summary>
        /// соответствие строка GUID - строка, в которой хранится название типа
        /// </summary>
        public Dictionary<string, string> typesNamesDict { get; set; }
        public Dictionary<string, object> binaryDataDict { get; set; }

        public const string formatName = "DiagramCopyData_Clipboard";
    }
}
