using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using DiagramDesigner.Controls;

namespace DiagramDesigner {
  //These attributes identify the types of the named parts that are used for templating
  [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
  [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
  [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
  [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
  public class DesignerItem : ContentControl, ISelectable, IGroupable, INotifyPropertyChanged {

    //public event EventHandler SelectedChanged;
    private RelativePositionPanel getPanel() {
      return getPanelRec(this);
    }


    private Connector[] _connectors;
    /// <summary>
    /// коннекторы, которые есть у фигуры
    /// </summary>
    public Connector[] Connectors {
      get {
        if (_connectors == null) {
          _connectors = new Connector[0];
        }
        return _connectors;
      }
      set {
        _connectors = value;
        //получаем панель, на которой все размещается
        var panel = getPanel();
        if (panel == null) {
          return;
        }
        Debug.Assert(panel != null, "Можно получать только после Loaded");
        //в массиве новых значений не должно быть повторяющихся имен
        Debug.Assert(value.Count(p => value.Count(z => z.Name == p.Name) > 1) == 0, "У коннекторов не должно быть повторяющихся имен");

        //т.е. в коннектторе хрантяся соединения, которые с ним связаны, то заменять нельзя. Необходимо обновить свойства.
        //поэтому из пары новые коннекторы + старые коннекторы нужно вытащить удаленные коннекторы, обновленные коннекторы и новые коннекторы
        //а для этого новые коннекторы надо будет редактировать, поэтому список предпочтительнее
        var newConnectorsList = value.ToList();
        //для каждого элемента в текущих коннекторах
        for (int i = 0; i < panel.Children.Count; i++) {
          //если что-то получили, что не является коннектором, пропускаем
          if ((panel.Children[i] is Connector) == false) {
            continue;
          }
          //преобразуем в коннектор (один из тех, которые были до установки нового значения)
          var con = (Connector)panel.Children[i];
          //ищем его среди новых по имени
          var newValue = value.FirstOrDefault(p => p.Name == con.Name);
          //если его нет, то это значит, что он был удален
          if (newValue == null) {
            //поэтому удаляем его с панели
            panel.Children.RemoveAt(i--);
            continue;
          }
          //обновляем свойства
          con.Orientation = newValue.Orientation;
          var newPoint = (Point)newValue.GetValue(RelativePositionPanel.RelativePositionProperty);
          con.SetValue(RelativePositionPanel.RelativePositionProperty, newPoint);
          //т.к. новый коннектор мы оработали, то он нам в списке новых коннекторов не нужен.
          newConnectorsList.Remove(newValue);
        }
        //когда дошли до суда, в списке останутся только добавленные коннекторы, которых до этого не было на панели
        //поэтому их надо добавить на панель
        for (int i = 0; i < newConnectorsList.Count; i++) {
          panel.Children.Add(newConnectorsList[i]);
        }
        this.NotifyPropertyChanged(PropertyChanged);

      }
    }

    private static RelativePositionPanel getPanelRec(DependencyObject sel) {
      var childrenCount = VisualTreeHelper.GetChildrenCount(sel);
      for (int i = 0; i < childrenCount; i++) {
        var child = VisualTreeHelper.GetChild(sel, i);
        if (child is RelativePositionPanel) {
          var resDebug = (child as RelativePositionPanel);
          return resDebug;
        }
        var res = getPanelRec(child);
        if (res == null) {
          continue;
        }
        return res;
      }
      return null;
    }


    public bool saveXMLData(out XElement xmlData) {
      if (Content is IXMLSaveable == false) {
        xmlData = null;
        return false;
      }
      var c = (IXMLSaveable)Content;
      xmlData = c.getData();
      var typeName = Content.GetType().FullName;
      xmlData.Add(new XAttribute("ObjectType", typeName));
      return true;
    }

    public bool saveBinaryData(out string typeName, out object dataObject) {
      if (Content is IBinarySaveable == false) {
        dataObject = null;
        typeName = null;
        return false;
      }
      var c = (IBinarySaveable)Content;
      typeName = Content.GetType().FullName;
      dataObject = c.getData();
      return true;
    }

    public Connector getConnectorByName(string name) {
      var cd = this.Template.FindName("PART_ConnectorDecorator", this) as Control;
      Debug.Assert(cd != null, "не найден шаблон");

      var connectors = new List<Connector>();
      DesignerCanvas.GetConnectors(cd, connectors);
      var res = connectors.FirstOrDefault(p => p.Name == name);
      return res;
    }

    #region ID
    public Guid ID {
      get;
      private set;
    }
    #endregion

    #region ParentID
    public Guid ParentID {
      get { return (Guid)GetValue(ParentIDProperty); }
      set { SetValue(ParentIDProperty, value); }
    }
    public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DesignerItem));
    #endregion

    #region CanResize
    /// <summary>
    /// можно или нет изменять размеры фигуры
    /// </summary>
    public bool CanResize {
      get { return (bool)GetValue(CanResizeProperty); }
      set { SetValue(CanResizeProperty, value); }
    }

    public static readonly DependencyProperty CanResizeProperty =
        DependencyProperty.Register("CanResize", typeof(bool), typeof(DesignerItem), new PropertyMetadata(true));
    /// <summary>
    /// можно или нет вращать фигуру
    /// </summary>
    public bool CanRotate {
      get { return (bool)GetValue(CanRotateProperty); }
      set { SetValue(CanRotateProperty, value); }
    }

    public static readonly DependencyProperty CanRotateProperty =
        DependencyProperty.Register("CanRotate", typeof(bool), typeof(DesignerItem), new PropertyMetadata(true));
    #endregion

    private void setArrowOnDragThumbs(Cursor c) {
      var dragThumbs = new List<DragThumb>();
      DesignerCanvas.GetDragThumbs(this, dragThumbs);

      foreach (var dragThumb in dragThumbs) {
        dragThumb.Cursor = c;
      }
    }

    #region CanMove
    /// <summary>
    /// можно таскать фигуру мышкой или нет
    /// </summary>
    public bool CanMove {
      get { return (bool)GetValue(CanMoveProperty); }
      set {
        SetValue(CanMoveProperty, value);
        if (value) {
          setArrowOnDragThumbs(Cursors.SizeAll);
        } else {
          setArrowOnDragThumbs(Cursors.Arrow);
        }
      }
    }
    public static readonly DependencyProperty CanMoveProperty =
        DependencyProperty.Register("CanMove", typeof(bool), typeof(DesignerItem), new PropertyMetadata(true));
    #endregion


    #region IsGroup
    /// <summary>
    /// Является фигура группой или нет
    /// </summary>
    public bool IsGroup {
      get { return (bool)GetValue(IsGroupProperty); }
      set { SetValue(IsGroupProperty, value); }
    }
    public static readonly DependencyProperty IsGroupProperty =
        DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));
    #endregion

    #region IsSelected Property

    public bool IsSelected {
      get { return (bool)GetValue(IsSelectedProperty); }
      set { SetValue(IsSelectedProperty, value); }
    }
    public static readonly DependencyProperty IsSelectedProperty =
      DependencyProperty.Register("IsSelected",
                                   typeof(bool),
                                   typeof(DesignerItem),
                                   new FrameworkPropertyMetadata(false));

    #endregion

    #region DragThumbTemplate Property

    // can be used to replace the default template for the DragThumb
    public static readonly DependencyProperty DragThumbTemplateProperty =
        DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

    private static ControlTemplate GetDragThumbTemplate(UIElement element) {
      return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
    }


    #endregion

    #region ConnectorDecoratorTemplate Property

    // can be used to replace the default template for the ConnectorDecorator
    public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
        DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

    public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element) {
      return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
    }


    #endregion

    #region IsDragConnectionOver

    // while drag connection procedure is ongoing and the mouse moves over 
    // this item this value is true; if true the ConnectorDecorator is triggered
    // to be visible, see template
    public bool IsDragConnectionOver {
      get { return (bool)GetValue(IsDragConnectionOverProperty); }
      set { SetValue(IsDragConnectionOverProperty, value); }
    }
    public static readonly DependencyProperty IsDragConnectionOverProperty =
        DependencyProperty.Register("IsDragConnectionOver",
                                     typeof(bool),
                                     typeof(DesignerItem),
                                     new FrameworkPropertyMetadata(false));

    #endregion

    static DesignerItem() {
      // set the key to reference the style for this control
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
          typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
    }

    public DesignerItem(Guid id) {
      this.ID = id;
      this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
    }

    public DesignerItem()
      : this(Guid.NewGuid()) {
    }

    public DesignerItem(bool _canResize)
      : this(Guid.NewGuid()) {
      CanResize = _canResize;
    }

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
      base.OnPreviewMouseDown(e);
      DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

      // update selection
      if (designer != null) {
        if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None) {
          if (this.IsSelected) {
            designer.SelectionService.RemoveFromSelection(this);
          } else {
            designer.SelectionService.AddToSelection(this);
          }
        } else if (!this.IsSelected) {
          designer.SelectionService.SelectItem(this);
        }
        Focus();

      }

      e.Handled = false;
    }

    void DesignerItem_Loaded(object sender, RoutedEventArgs e) {
      if (Connectors.Length == 0) {
        Connectors = new[] {
                                       new Connector(ConnectorOrientation.Left, this, 0, 0.5, "Left"),
                                       new Connector(ConnectorOrientation.Right, this, 1, 0.5, "Right"),
                                       new Connector(ConnectorOrientation.Top, this, 0.5, 0, "Top"),
                                       new Connector(ConnectorOrientation.Bottom, this, 0.5, 1, "Bottom"),
                                       new Connector(ConnectorOrientation.None, this, 0.5, 0.5, "Center")
                                     };
      } else {
        Connectors = Connectors;
      }


      //albug работа с коннекторами
      //new Connector(ConnectorOrientation.None, this, 0.7, 0.7, "Center2"),
      //new Connector(ConnectorOrientation.None, this, 0.3, 0.3, "Center3")};

      if (Template == null) {
        return;
      }
      var contentPresenter = this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
      if (contentPresenter == null) {
        return;
      }
      var contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
      if (contentVisual == null) {
        return;
      }
      var thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
      if (thumb == null) {
        return;
      }
      var template = GetDragThumbTemplate(contentVisual) as ControlTemplate;
      if (template != null)
        thumb.Template = template;
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }

  public class DesignerItemCoverForPropertyEditor {
    private readonly DesignerItem d;

    public DesignerItemCoverForPropertyEditor(DesignerItem _d) {
      d = _d;
    }


    [CategoryAttribute("Основные данные"), DescriptionAttribute("Id")]
    [DisplayName("Id")]
    public Guid Id {
      get {
        return d.ID;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("Можно двигать")]
    [DisplayName("Можно двигать")]
    public bool CanMove {
      get {
        return d.CanMove;
      }
      set {
        d.CanMove = value;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("Можно вращать")]
    [DisplayName("Можно вращать")]
    public bool CanRotate {
      get {
        return d.CanRotate;
      }
      set {
        d.CanRotate = value;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("Можно масштабировать")]
    [DisplayName("Можно масштабировать")]
    public bool CanResize {
      get {
        return d.CanResize;
      }
      set {
        d.CanResize = value;
      }
    }
  }


}
