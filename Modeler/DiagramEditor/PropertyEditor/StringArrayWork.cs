using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DiagramDesigner.PropertyEditor;

namespace DiagramDesigner {

  //этот аттрибут заставляет конвертор использовать MyColorConverter вместо редактора по умолчанию
  [TypeConverter(typeof(MyStringArrayConverter))]
  public class MyStringArray {
    public StringList data { get; set; }

    public MyStringArray(StringList _data) {
      data = _data;
    }

    public override string ToString() {
      return String.Format("" + data.Count + " элементов");
    }

  }

  public class MyStringArrayConverter : TypeConverter {
    //используется при преобразовании из строки в MyColor(например если задано DefaultValueAttribute)
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      if (value.GetType() == typeof(StringList))
        return new MyStringArray((StringList)value);
      return base.ConvertFrom(context, culture, value);
    }
    //это используется при преобрзавании MyColor в строку
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) {
      if ((destType == typeof(string)) && (value is MyStringArray)) {
        var color = (MyStringArray)value;
        return color.ToString();
      }
      return base.ConvertTo(context, culture, value, destType);
    }
  }

  
}
