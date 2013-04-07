using System;
using System.Xml.Linq;

namespace DiagramDesigner {
  public interface IGroupable {
    Guid ID { get; }
    Guid ParentID { get; set; }
    bool IsGroup { get; set; }
  }

  // Common interface for items that can be selected
  // on the DesignerCanvas; used by DesignerItem and Connection
  public interface ISelectable {
    bool IsSelected { get; set; }
  }

  public interface IBinarySaveable {
    object getData();
    void loadData(object data);
  }

  public interface IXMLSaveable {
    XElement getData();
    void loadData(XElement data);
  }
}
