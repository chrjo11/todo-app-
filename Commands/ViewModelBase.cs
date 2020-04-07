using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoApp.MVVM.Commands
{
    class ViewModelBase : INotifyPropertyChanged //erbt von einem Interface: benachrichtigt, wenn sich was geändert hat

    {
        public event PropertyChangedEventHandler PropertyChanged; //hier wird ein event/Ereignis deklariert es heißt PropertyChanged
                                                                  //PropertyChangedEventHandler: wird ausgelöst, wenn sich ein property/Eigenschaft ändert
        public void RaisePropertyChanged(string PropertyName) //Methode anlegen mit Übergabe des PropertyName vom Typ string
                                                              //Methode wird immer angelegt mit dem übergebenen Datentyp und Namen
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); //TodoItemText hat sich geändert (Text in TextBox), schaut nach einem Binding: TextBox
                                                                                      //meldet mit dem Event dem View, dass sich was geändert hat und damit wird TodoItem der ListBox hinzugefügt
                                                                                      //mit this wird der PropertyName übergeben, dh. es wird dem View die Info übergeben, dass sich der Inhalt der TextBox geändert hat
                                                                                      //PropertyChangedEventArgs-Objekt gibt den Namen des geänderten Eigenschaftswert an
        }
    }
}
