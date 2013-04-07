using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner {
  public class Connection : Control, ISelectable, INotifyPropertyChanged {
    private Adorner connectionAdorner;

    #region Properties

    public Guid ID { get; set; }
    /// <summary>
    /// текст, который приписывается стрелке
    /// </summary>
    public string Text {
      get { return (string)GetValue(TextProperty); }
      set {
        SetValue(TextProperty, value);
      }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(Connection), new PropertyMetadata(""));
    /// <summary>
    /// Направлен текст всегда горизонтально (true) или по направлению стрелки (false)
    /// </summary>
    public bool HorizontalText {
      get { return (bool)GetValue(HorizontalTextProperty); }
      set {
        SetValue(HorizontalTextProperty, value);
        OnPropertyChanged("LabelAngle");
        OnPropertyChanged("HorizontalText");
      }
    }

    public static readonly DependencyProperty StartTextProperty =
      DependencyProperty.Register("StartText", typeof(string), typeof(Connection), new PropertyMetadata(""));
    /// <summary>
    /// Надпись у начала стрелки
    /// </summary>
    public string StartText {
      get { return (string)GetValue(StartTextProperty); }
      set {
        SetValue(StartTextProperty, value);
        OnPropertyChanged("LabelAngle");
        OnPropertyChanged("StartTextProperty");
        OnPropertyChanged("StartLabelPosition");
      }
    }

    public static readonly DependencyProperty EndTextProperty =
     DependencyProperty.Register("EndText", typeof(string), typeof(Connection), new PropertyMetadata(""));
    /// <summary>
    /// Надпись у начала стрелки
    /// </summary>
    public string EndText {
      get { return (string)GetValue(EndTextProperty); }
      set {
        SetValue(EndTextProperty, value);
        OnPropertyChanged("LabelAngle");
        OnPropertyChanged("EndTextProperty");
        OnPropertyChanged("EndLabelPosition");
      }
    }

    public static readonly DependencyProperty HorizontalTextProperty =
        DependencyProperty.Register("HorizontalText", typeof(bool), typeof(Connection), new PropertyMetadata(false));

    public Color Color {
      get { return (Color)GetValue(ColorProperty); }
      set {
        SetValue(ColorProperty, value);
        ColorBrush = new SolidColorBrush(value);
      }
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register("Color", typeof(Color), typeof(Connection), new PropertyMetadata(Colors.Black));

    public Brush ColorBrush {
      get { return (Brush)GetValue(ColorBrushProperty); }
      set {
        SetValue(ColorBrushProperty, value);
      }
    }

    public static readonly DependencyProperty ColorBrushProperty =
        DependencyProperty.Register("ColorBrush", typeof(Brush), typeof(Connection), new PropertyMetadata(Brushes.Black));

    // source connector
    private Connector _source;
    /// <summary>
    /// коннектор, из которого выходит соединение
    /// </summary>
    public Connector Source {
      get {
        return _source;
      }
      set {
        if (_source == value) {
          return;
        }
        if (_source != null) {
          _source.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
          _source.Connections.Remove(this);
        }

        _source = value;

        if (_source != null) {
          _source.Connections.Add(this);
          _source.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
        }

        UpdatePathGeometry();
      }
    }

    // sink connector
    private Connector _sink;
    /// <summary>
    /// коннектор, в который заходит соединение
    /// </summary>
    public Connector Sink {
      get { return _sink; }
      set {
        if (_sink == value) {
          return;
        }
        if (_sink != null) {
          _sink.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
          _sink.Connections.Remove(this);
        }

        _sink = value;

        if (_sink != null) {
          _sink.Connections.Add(this);
          _sink.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
        }
        UpdatePathGeometry();
      }
    }

    // connection path geometry
    private PathGeometry pathGeometry;
    public PathGeometry PathGeometry {
      get { return pathGeometry; }
      set {
        if (pathGeometry != value) {
          pathGeometry = value;
          UpdateAnchorPosition();
          OnPropertyChanged("PathGeometry");
        }
      }
    }

    // between source connector position and the beginning 
    // of the path geometry we leave some space for visual reasons; 
    // so the anchor position source really marks the beginning 
    // of the path geometry on the source side
    private Point anchorPositionSource;
    public Point AnchorPositionSource {
      get { return anchorPositionSource; }
      set {
        if (anchorPositionSource != value) {
          anchorPositionSource = value;
          OnPropertyChanged("AnchorPositionSource");
        }
      }
    }

    // slope of the path at the anchor position
    // needed for the rotation angle of the arrow
    private double anchorAngleSource;// = 0; - инициализация по умолчанию
    public double AnchorAngleSource {
      get { return anchorAngleSource; }
      set {
        if (anchorAngleSource != value) {
          anchorAngleSource = value;
          OnPropertyChanged("AnchorAngleSource");
        }
      }
    }

    // analogue to source side
    private Point anchorPositionSink;
    public Point AnchorPositionSink {
      get { return anchorPositionSink; }
      set {
        if (anchorPositionSink != value) {
          anchorPositionSink = value;
          OnPropertyChanged("AnchorPositionSink");
        }
      }
    }
    // analogue to source side
    private double anchorAngleSink = 0;
    public double AnchorAngleSink {
      get { return anchorAngleSink; }
      set {
        if (anchorAngleSink != value) {
          anchorAngleSink = value;
          OnPropertyChanged("AnchorAngleSink");
        }
      }
    }

    private ArrowSymbol _sourceArrowSymbol = ArrowSymbol.None;
    public ArrowSymbol SourceArrowSymbol {
      get { return _sourceArrowSymbol; }
      set {
        if (_sourceArrowSymbol != value) {
          _sourceArrowSymbol = value;
          OnPropertyChanged("SourceArrowSymbol");
          UpdatePathGeometry();
        }
      }
    }

    public ArrowSymbol _sinkArrowSymbol = ArrowSymbol.Arrow;
    public ArrowSymbol SinkArrowSymbol {
      get { return _sinkArrowSymbol; }
      set {
        if (_sinkArrowSymbol != value) {
          _sinkArrowSymbol = value;
          OnPropertyChanged("SinkArrowSymbol");
          UpdatePathGeometry();
        }
      }
    }

    private double _labelAngle;
    /// <summary>
    /// угол, под которым наклонена надпись
    /// </summary>
    public double LabelAngle {
      get {
        if (HorizontalText) {
          return 0;
        }
        return _labelAngle;
      }
      set {
        if (_labelAngle != value) {
          _labelAngle = value;
          OnPropertyChanged("LabelAngle");
        }
      }
    }
    /// <summary>
    /// получение позиции текста около коннектора
    /// </summary>
    /// <param name="connector"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private static Point getTextPosition(Connector connector, string text) {
      var x = connector.Position.X;
      var y = connector.Position.Y;
      if (connector.Orientation == ConnectorOrientation.Right) {
        x += 10;
      } else if (connector.Orientation == ConnectorOrientation.Bottom) {
        y += 10;
        x += 5;
      } else if (connector.Orientation == ConnectorOrientation.Top) {
        y -= 25;
        x += 5;
      } else if (connector.Orientation == ConnectorOrientation.Left) {
        var formatterText = new FormattedText(text, System.Threading.Thread.CurrentThread.CurrentUICulture,
          FlowDirection.LeftToRight, new Typeface("Segoe UI"), 12, Brushes.Black);
        x -= formatterText.Width;
        x -= 10;
      }
      return new Point(x, y);
    }


    /// <summary>
    /// координаты начала стрелки
    /// </summary>
    public Point StartLabelPosition {
      get {
        return getTextPosition(this.Source, StartText);
      }
      set {
        OnPropertyChanged("StartLabelPosition");
      }
    }

    /// <summary>
    /// координаты начала стредки
    /// </summary>
    public Point EndLabelPosition {
      get {
        return getTextPosition(this.Sink, EndText);
      }
      set {
        OnPropertyChanged("EndLabelPosition");
      }
    }


    // specifies a point at half path length
    private Point _labelPosition;
    /// <summary>
    /// координаты середины стрелки
    /// </summary>
    public Point LabelPosition {
      get { return _labelPosition; }
      set {
        if (_labelPosition != value) {
          _labelPosition = value;
          OnPropertyChanged("LabelPosition");
          OnPropertyChanged("StartLabelPosition");
          OnPropertyChanged("EndLabelPosition");
        }
      }
    }

    // pattern of dashes and gaps that is used to outline the connection path
    private DoubleCollection strokeDashArray;
    public DoubleCollection StrokeDashArray {
      get {
        return strokeDashArray;
      }
      set {
        if (strokeDashArray != value) {
          strokeDashArray = value;
          OnPropertyChanged("StrokeDashArray");
        }
      }
    }
    // if connected, the ConnectionAdorner becomes visible
    private bool isSelected;
    public bool IsSelected {
      get { return isSelected; }
      set {
        if (isSelected != value) {
          isSelected = value;
          OnPropertyChanged("IsSelected");
          if (isSelected)
            ShowAdorner();
          else
            HideAdorner();
        }
      }
    }

    #endregion
    /// <summary>
    /// true, если линия вляется прямой
    /// <para>false, если линия ломаная</para>
    /// </summary>
    internal bool isSimple { get; set; }

    public Connection(Connector source, Connector sink, bool _isSimple) {
      isSimple = _isSimple;
      this.ID = Guid.NewGuid();
      this.Source = source;
      this.Sink = sink;
      base.Unloaded += new RoutedEventHandler(Connection_Unloaded);
    }


    protected override void OnMouseDown(MouseButtonEventArgs e) {
      base.OnMouseDown(e);

      // usual selection business
      var designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
      if (designer != null) {
        if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
          if (this.IsSelected) {
            designer.SelectionService.RemoveFromSelection(this);
          } else {
            designer.SelectionService.AddToSelection(this);
          } else if (!this.IsSelected) {
          designer.SelectionService.SelectItem(this);
        }

        Focus();
      }
      e.Handled = false;
    }

    void OnConnectorPositionChanged(object sender, PropertyChangedEventArgs e) {
      // whenever the 'Position' property of the source or sink Connector 
      // changes we must update the connection path geometry
      if (e.PropertyName.Equals("Position")) {
        UpdatePathGeometry();
      }
    }

    private void UpdatePathGeometry() {
      if (Source != null && Sink != null) {
        var geometry = new PathGeometry();

        List<Point> linePoints = PathFinder.GetConnectionLine(Source.GetInfo(), Sink.GetInfo(), true, isSimple, this.SinkArrowSymbol, this.SourceArrowSymbol);
        if (linePoints.Count > 0) {
          var figure = new PathFigure();
          figure.StartPoint = linePoints[0];
          linePoints.Remove(linePoints[0]);
          figure.Segments.Add(new PolyLineSegment(linePoints, true));
          geometry.Figures.Add(figure);

          this.PathGeometry = geometry;
        }
      }
    }

    private void UpdateAnchorPosition() {
      Point pathStartPoint, pathTangentAtStartPoint;
      Point pathEndPoint, pathTangentAtEndPoint;
      Point pathMidPoint, pathTangentAtMidPoint;

      // the PathGeometry.GetPointAtFractionLength method gets the point and a tangent vector 
      // on PathGeometry at the specified fraction of its length
      this.PathGeometry.GetPointAtFractionLength(0, out pathStartPoint, out pathTangentAtStartPoint);
      this.PathGeometry.GetPointAtFractionLength(1, out pathEndPoint, out pathTangentAtEndPoint);
      this.PathGeometry.GetPointAtFractionLength(0.5, out pathMidPoint, out pathTangentAtMidPoint);

      // get angle from tangent vector
      this.AnchorAngleSource = Math.Atan2(-pathTangentAtStartPoint.Y, -pathTangentAtStartPoint.X) * (180 / Math.PI);
      this.AnchorAngleSink = Math.Atan2(pathTangentAtEndPoint.Y, pathTangentAtEndPoint.X) * (180 / Math.PI);

      // add some margin on source and sink side for visual reasons only
      pathStartPoint.Offset(-pathTangentAtStartPoint.X * 5, -pathTangentAtStartPoint.Y * 5);
      pathEndPoint.Offset(pathTangentAtEndPoint.X * 5, pathTangentAtEndPoint.Y * 5);

      this.AnchorPositionSource = pathStartPoint;
      this.AnchorPositionSink = pathEndPoint;
      this.LabelPosition = pathMidPoint;

      if (isSimple) {
        var radianAngle = Math.Atan2(pathEndPoint.Y - pathStartPoint.Y, pathEndPoint.X - pathStartPoint.X);
        var angle = radianAngle * 360 / (2 * Math.PI);
        LabelAngle = angle;
      } else {
        LabelAngle = 0;
      }
      //это нужно просто что бы вызвать обновление, новое значение само посчитается
      StartLabelPosition = new Point(0, 0);
    }

    private void ShowAdorner() {
      // the ConnectionAdorner is created once for each Connection
      if (this.connectionAdorner == null) {
        var designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (adornerLayer != null) {
          this.connectionAdorner = new ConnectionAdorner(designer, this);
          adornerLayer.Add(this.connectionAdorner);
        }
      }
      this.connectionAdorner.Visibility = Visibility.Visible;
    }

    internal void HideAdorner() {
      if (this.connectionAdorner != null)
        this.connectionAdorner.Visibility = Visibility.Collapsed;
    }

    void Connection_Unloaded(object sender, RoutedEventArgs e) {
      // do some housekeeping when Connection is unloaded

      // remove event handler
      this.Source = null;
      this.Sink = null;


      if (this.connectionAdorner == null) {
        return;
      }
      // remove adorner
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
      if (adornerLayer != null) {
        adornerLayer.Remove(this.connectionAdorner);
        this.connectionAdorner = null;
      }
    }

    #region INotifyPropertyChanged Members

    // we could use DependencyProperties as well to inform others of property changes
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }

    #endregion
  }

  public enum ArrowSymbol {
    None,
    Arrow,
    ArrowEmpty,
    Diamond,
    Circle

  }


  [DefaultPropertyAttribute("Text")]
  public class ConnectionCoverForPropertyEditor {
    protected readonly Connection c;

    public ConnectionCoverForPropertyEditor(Connection _c) {
      c = _c;
    }


    [CategoryAttribute("Основные данные"), DescriptionAttribute("Id")]
    [DisplayName("Id")]
    public Guid Id {
      get {
        return c.ID;
      }
      set {
        c.ID = value;
      }
    }
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Текст")]
    [DisplayName("Текст")]
    public string Text {
      get {
        return c.Text;
      }
      set {
        c.Text = value;
      }
    }

    [EditorAttribute(typeof(MyColorEditor), typeof(UITypeEditor))]
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Цвет")]
    [DisplayName("Цвет")]
    public MyColor Color2 {
      get { return new MyColor(c.Color.R, c.Color.G, c.Color.B); }
      set { c.Color = Color.FromRgb(value.Red, value.Green, value.Blue); }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("Окончание")]
    [DisplayName("Окончание")]
    public ArrowSymbol EndType {
      get {
        return c.SinkArrowSymbol;
      }
      set {
        c.SinkArrowSymbol = value;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("Начало")]
    [DisplayName("Начало")]
    public ArrowSymbol StartType {
      get {
        return c.SourceArrowSymbol;
      }
      set {
        c.SourceArrowSymbol = value;
      }
    }
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Тип линии")]
    [DisplayName("Тип линии")]
    public LineType LineType {
      get {
        return LineTypeConvertor.getLineType(c.StrokeDashArray);
      }
      set {
        c.StrokeDashArray = LineTypeConvertor.getStrokeDashArray(value);
      }
    }


  }

  public class PolygonConnectionCoverForPropertyEditor : ConnectionCoverForPropertyEditor {

    public PolygonConnectionCoverForPropertyEditor(Connection _c)
      : base(_c) {
    }
    [CategoryAttribute("Основные данные"), DescriptionAttribute("начальная надпись")]
    [DisplayName("Начальная надпись")]
    public string StartText {
      get {
        return c.StartText;
      }
      set {
        c.StartText = value;
      }
    }

    [CategoryAttribute("Основные данные"), DescriptionAttribute("конечная надпись")]
    [DisplayName("Конечная надпись")]
    public string EndText {
      get {
        return c.EndText;
      }
      set {
        c.EndText = value;
      }
    }
  }

  /// <summary>
  /// класс для отображения данных прямой стрелки
  /// </summary>
  public class SimpleConnectionCoverForPropertyEditor : ConnectionCoverForPropertyEditor {

    public SimpleConnectionCoverForPropertyEditor(Connection _c)
      : base(_c) {
    }
    [CategoryAttribute("Основные данные"), DescriptionAttribute("Горизонтальная надпись")]
    [DisplayName("Горизонтальная надпись")]
    public bool HorizontalText {
      get {
        return c.HorizontalText;
      }
      set {
        c.HorizontalText = value;
      }
    }
  }
    
  public enum LineType {
    Сплошная,
    Пунктирная,
    ШтрихПунктирная
  }

  public static class LineTypeConvertor {
    public static LineType getLineType(DoubleCollection strokeDashArray) {
      if (strokeDashArray == null) {
        return LineType.Сплошная;
      }
      if (strokeDashArray.Count == 1) {
        return LineType.Сплошная;
      }
      if (strokeDashArray.Count != 2) {
        Debugger.Break();
        throw new ApplicationException("Непредусмотренный LineType");
      }
      if (strokeDashArray[0] == 2) {
        return LineType.ШтрихПунктирная;
      }
      return LineType.Пунктирная;
    }

    public static DoubleCollection getStrokeDashArray(LineType type) {
      var res = new DoubleCollection(0);
      if (type == LineType.Пунктирная) {
        res.Add(1);
        res.Add(1);
        return res;
      }
      if (type == LineType.Сплошная) {
        //res.Add(1);
        return null;
      }
      if (type == LineType.ШтрихПунктирная) {
        res.Add(2);
        res.Add(1);
        return res;
      }
      Debugger.Break();
      throw new ApplicationException("Непредусмотренный LineType");
    }
  }
}
