using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace TodoApp.MVVM.Commands
{
    public class Items : ObservableCollection<string> //<Typ> wird deklariert, erbt von der ObserableCollection: dynamische Datenauflistung,
                                                      //die Benachrichtigungen bereitstellt, wenn ein Element hinzugefügt/gelöscht wurde
    {

    }
}
