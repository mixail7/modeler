using System.ComponentModel;
using System.Diagnostics;

namespace DiagramDesigner {
  public static class ExtentionsClass {
    public static void NotifyPropertyChanged(this INotifyPropertyChanged npc,
      PropertyChangedEventHandler PropertyChanged, string fieldName = "") {
      //использовать 
      // this.NotifyPropertyChanged(PropertyChanged);

      if (PropertyChanged != null) {
        string propertyName = (fieldName == "") ?
                              new StackTrace().GetFrame(1).GetMethod().Name.Substring(4)
                              : fieldName;

        PropertyChanged(npc, new PropertyChangedEventArgs(propertyName));
      }
    }

  }
}
