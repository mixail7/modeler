using System.Collections.Generic;
using System.Diagnostics;

namespace DiagramDesigner {
  /// <summary>
  /// информация об элементе палитры
  /// </summary>
  public class ElementInfo {
    /// <summary>
    /// название элемента
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// выбрал его пользователь или нет
    /// </summary>   
    public bool IsSelected { get; set; }
    /// <summary>
    /// информация о родительской палитре
    /// </summary>
    public PalleterInfo parentPalleter { get; private set; }
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="_parentPalleter">палитра, на которой находится компонент</param>
    /// <param name="_name">имя компонента</param>
    public ElementInfo(PalleterInfo _parentPalleter, string _name) {
      parentPalleter = _parentPalleter;
      IsSelected = false;
      Name = _name;
    }
  }
  /// <summary>
  /// информация о палитре
  /// </summary>
  public class PalleterInfo {
    /// <summary>
    /// название палитры
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// информация об элементах, которые расположены на палитре
    /// </summary>
    public List<ElementInfo> elements { get; set; }
        
    /// <summary>
    /// конструктор
    /// </summary>
    /// <param name="palleter">палитра</param>
    public PalleterInfo(IToolBoxPalleter palleter) {
      Debug.Assert(palleter != null, "нет палитры");
      elements = new List<ElementInfo>();
      Name = palleter.getPalleterName();
      var tempElements = palleter.getNames();
      foreach (var tempElement in tempElements) {
        elements.Add(new ElementInfo(this, tempElement));
      }
    }
  }

}
