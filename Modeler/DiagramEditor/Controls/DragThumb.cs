using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner.Controls {
  public class DragThumb : Thumb {
    public DragThumb() {
      base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
        DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
      //DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
    }

    private RotateTransform rotateTransform;
    private ContentControl designerItem;


    private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e) {
      this.designerItem = DataContext as ContentControl;

      if (this.designerItem != null) {
        this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
      }
    }

    //private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e) {
    //  if (this.designerItem == null) {
    //    return;
    //  }
    //  var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

    //  if (this.rotateTransform != null) {
    //    dragDelta = this.rotateTransform.Transform(dragDelta);
    //  }

    //  Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
    //  Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);
    //}

    //public bool AllowDrag {
    //  get { return (bool)GetValue(AllowDragProperty); }
    //  set {
    //    SetValue(AllowDragProperty, value);
    //  }
    //}

    //public static readonly DependencyProperty AllowDragProperty =
    //    DependencyProperty.Register("AllowDrag", typeof(bool), typeof(DragThumb), new PropertyMetadata(true));

    private static DesignerItem getControlFromPart(UIElement element) {
      var curElement = element;
      while (true) {
        if (curElement == null) {
          break;
        }
        if (curElement is DesignerItem) {
          break;
        }
        curElement = VisualTreeHelper.GetParent(curElement) as UIElement;
      }
      return (DesignerItem)curElement;
    }

    void DragThumb_DragDelta(object sender, DragDeltaEventArgs e) {
      //if (AllowDrag == false) {
      //  return;
      //}
      DesignerItem designerItem = this.DataContext as DesignerItem;
      if (designerItem == null) {

        var elem = (UIElement)e.OriginalSource;
        designerItem = getControlFromPart(elem);
      }
      DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;
      if (designerItem == null || designer == null || !designerItem.IsSelected) {
        return;
      }
      double minLeft = double.MaxValue;
      double minTop = double.MaxValue;

      // we only move DesignerItems
      var designerItems = designer.SelectionService.CurrentSelection.OfType<DesignerItem>();

      foreach (DesignerItem item in designerItems) {
        double left = Canvas.GetLeft(item);
        double top = Canvas.GetTop(item);

        minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
        minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
      }

      double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
      double deltaVertical = Math.Max(-minTop, e.VerticalChange);

      foreach (DesignerItem item in designerItems) {
          var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
        if (this.rotateTransform != null) {
          dragDelta = this.rotateTransform.Transform(dragDelta);
        }

        if (item.CanMove == false) {
          continue;
        }

        double left = Canvas.GetLeft(item);
        double top = Canvas.GetTop(item);

        if (double.IsNaN(left)) left = 0;
        if (double.IsNaN(top)) top = 0;
       
        

        Canvas.SetLeft(item, left + deltaHorizontal);
        Canvas.SetTop(item, top + deltaVertical);
      }
      //if (this.designerItem == null) {
    //    return;
    //  }
    //  var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

    //  if (this.rotateTransform != null) {
    //    dragDelta = this.rotateTransform.Transform(dragDelta);
    //  }

    //  Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
    //  Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);

      designer.InvalidateMeasure();
      e.Handled = true;
    }
  }

}
