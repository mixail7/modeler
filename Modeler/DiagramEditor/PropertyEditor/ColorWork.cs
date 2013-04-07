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
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace DiagramDesigner {
  //этот аттрибут заставляет конвертор использовать MyColorConverter вместо редактора по умолчанию
  [TypeConverter(typeof(MyColorConverter))]
  public class MyColor {
    public byte Red { get; set; }

    public byte Green { get; set; }

    public byte Blue { get; set; }

    public MyColor(byte red, byte green, byte blue) {
      Red = red;
      Green = green;
      Blue = blue;
    }

    public MyColor(int argb) {
      byte[] bytes = BitConverter.GetBytes(argb);
      Red = bytes[2];
      Green = bytes[1];
      Blue = bytes[0];
    }

    public MyColor(string rgb) {
      string[] parts = rgb.Split(' ');
      if (parts.Length != 3)
        throw new Exception("Array must have a length of 3.");
      Red = Convert.ToByte(parts[0]);
      Green = Convert.ToByte(parts[1]);
      Blue = Convert.ToByte(parts[2]);
    }

    public new string ToString() {
      return String.Format("{0} {1} {2}", Red, Green, Blue); ;
    }

    public int GetARGB() {
      byte[] temp = new byte[] { Blue, Green, Red, 255 };
      return BitConverter.ToInt32(temp, 0);
    }
  }

  public class MyColorConverter : TypeConverter {
    //используется при преобразовании из строки в MyColor(например если задано DefaultValueAttribute)
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
      if (value.GetType() == typeof(string))
        return new MyColor((string)value);
      return base.ConvertFrom(context, culture, value);
    }
    //это используется при преобрзавании MyColor в строку
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) {
      if ((destType == typeof(string)) && (value is MyColor)) {
        MyColor color = (MyColor)value;
        return color.ToString();
      }
      return base.ConvertTo(context, culture, value, destType);
    }
  }

  public class MyColorEditor : UITypeEditor {
    private IWindowsFormsEditorService service;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
      //это заставляет показать [...] в редакторе свойств, на которую можно нажать, если разрешено изменение
      return UITypeEditorEditStyle.Modal;
    }
    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
      if (provider != null)
        service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

      if (service != null) {
        //этот код будет запущен после клика по [...] и после всех проверок        

        //получаем текущий цвет
        var color = (MyColor)value;

        var selectionControl = new ColorDialog { Color = Color.FromArgb(color.GetARGB()) };
        selectionControl.ShowDialog();
        value = new MyColor(selectionControl.Color.ToArgb());
      }

      return value;
    }
  }
}
