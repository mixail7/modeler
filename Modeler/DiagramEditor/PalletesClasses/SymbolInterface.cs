using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DiagramDesigner {
  /// <summary>
  /// интерфейс палитры
  /// </summary>
  public interface IToolBoxPalleter {
    /// <summary>
    /// получение имен всех компонентов, которые есть в палитре
    /// </summary>
    /// <returns></returns>
    string[] getNames();
    /// <summary>
    /// получение компонентов палитры по их именам
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    UserControl[] getControls(params string[] names);
    /// <summary>
    /// получение имени палитры
    /// </summary>
    /// <returns></returns>
    string getPalleterName();
    /// <summary>
    /// получение Expander, на котором находятся все компоненты палитры
    /// </summary>
    /// <returns></returns>
    Expander getPanel();
    /// <summary>
    /// проверка того, что эта палитра отвечает за обработку DragData, в котором formatName==formatName
    /// </summary>
    /// <param name="formatName"></param>
    /// <returns></returns>
    bool isFormatOwner(string formatName);
    /// <summary>
    /// получение компонента, который будет добавлен на диаграмму по информации в буфере обмена
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    DesignerItem getElement(DragObject data);
  }

  /// <summary>
  /// интерфейс элемента, свойства которого можно редактировать через палитру свойств
  /// </summary>
  public interface IPropertyEditable {
    object getCover(DesignerItem item);
  }
}
