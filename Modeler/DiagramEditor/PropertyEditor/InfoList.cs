using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;


namespace DiagramDesigner.PropertyEditor {
  
    /// <summary>
    /// класс для поддержки списковой структуры, когда в AdditionalInfo хранится данное
    /// <para>Поддерживает редактирование списка, если тот стиле WPFDEditableList</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>  
    [Serializable]
    public abstract class InfoList<T> : BindingList<T> {
      /// <summary>
      /// процедура добавления данных (вызывается из ListBox, у которого Style=WPFEditableList
      /// </summary>
      public abstract void addValue();

      /// <summary>
      /// процедура изменения данных
      /// </summary>
      /// <param name="record"></param>
      public abstract void updateValue(T record);

      /// <summary>
      /// процедура удаления данных 
      /// </summary>
      /// <param name="record"></param>
      public abstract void deleteValue(T record);


      private readonly string addName;
      private readonly string updateName;
      private readonly string deleteName;



      /// <summary>
      /// название, которое отображается на элементе добавления
      /// <para>readonly</para>
      /// </summary>
      public string AddCommandName {
        get {
          return addName;
        }
      }
      /// <summary>
      /// название, которое отображается на элементе обновления
      /// <para>readonly</para>
      /// </summary>
      public string UpdateCommandName {
        get {
          return updateName;
        }
      }
      /// <summary>
      /// название, которое отображается на элементе удаления
      /// <para>readonly</para>
      /// </summary>    
      public string DeleteCommandName {
        get {
          return deleteName;
        }
      }


      public ICommand AddCommand { get; private set; }
      public ICommand UpdateCommand { get; private set; }
      public ICommand DeleteCommand { get; private set; }

      /// <summary>
      /// функция сравнения данных, которую обязательно перегрузить
      /// </summary>
      /// <param name="data1"></param>
      /// <param name="data2"></param>
      /// <returns></returns>
      protected abstract int compareData(T data1, T data2);


      protected void setStartData(T[] startData) {
        foreach (var conceptInfo in startData) {
          this.Add(conceptInfo);
        }
      }

      public void setListBox(ListBox listBox) {
        listBox.DataContext = this;
        listBox.ItemsSource = this;
      }


      protected InfoList() {

        addName = "добавить";
        updateName = "изменить";
        deleteName = "удалить";


        AddCommand = new SimpleCommand<T>(addValue);
        UpdateCommand = new SimpleCommand<T>(updateValue);
        DeleteCommand = new SimpleCommand<T>(deleteValue);        
      }

      public virtual CommandInfo[] commands {
        get {
          return new CommandInfo[] {
                                   new CommandInfo {command = DeleteCommand, commandName = deleteName}/*,
                                   new CommandInfo {command = UpdateCommand, commandName = updateName}*/};
        }
      }
    }
  [Serializable]
    public class StringList : InfoList<string> {
      public override void addValue() {
        this.Add(DateTime.Now.ToString());
      }

      public override void updateValue(string record) {
        throw new NotImplementedException();
      }

      public override void deleteValue(string record) {
        for (int i=0;i<this.Count;i++) {
          if (this[i] == record) {
            this.RemoveAt(i);
            break;
          }
        }
      }

      protected override int compareData(string data1, string data2) {
        return string.Compare(data1, data2);
      }
    }
  [Serializable]
    public class CommandInfo {
      public ICommand command { get; set; }
      public string commandName { get; set; }
    }

  [Serializable]
    public class SimpleCommand<Z> : ICommand {
      public bool CanExecute(object parameter) {
        return true;
      }

      public event EventHandler CanExecuteChanged;

      private readonly Action del_NoParam;
      private readonly Action<Z> del_WithParam;

      public SimpleCommand(Action _del) {
        Debug.Assert(CanExecuteChanged == null);
        del_NoParam = _del;
      }

      public SimpleCommand(Action<Z> _del) {
        Debug.Assert(CanExecuteChanged == null);
        del_WithParam = _del;
      }

      public void Execute(object parameter) {
        if (del_NoParam != null) {
          del_NoParam();
        } else {
          var tempValue = (Z)parameter;
          del_WithParam(tempValue);
        }
      }
    }

    public delegate void RecordAddedOrUpdatedEvent<T>(T data);

}
