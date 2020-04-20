using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.MVVM.Commands;
using TodoApp.MVVM.Converter;
using TodoApp.MVVM.Services;

namespace TodoApp.MVVM.Converter
{
    class TodoItemFileService : ITodoItemService //implementiert das Interface: erbt vom Interface, Methoden bekommen in der abgeleiteten Klasse eine Funktionalität
    {
        private readonly string _path;

        public IEnumerable<TodoItemModel> ReadTodoItems() //Liest die Datei aus und gibt die Liste mit den Lines zurück 
        {
            if (File.Exists(_path)) //Wenn Datei existiert
            {
                var FileContent = File.ReadAllText(_path); //Text soll aus der Datei gelesen werden und FileContent zugewiesen werden 
                var list = JsonConvert.DeserializeObject<List<TodoItemModel>>(FileContent); //deserialisieren: string in TodoItemModel konvertieren

                return list; //Beendet die Methode und gibt die gelesene Datei/Liste als Typ TodoItemModel zurück
            } 
            else
            {
                return new List<TodoItemModel>(); //Ansonsten wird eine leere Liste zurückgegeben
            }
        }

        public void WriteTodoItems(IEnumerable<TodoItemModel> Items) //Übergibt die Liste mit den TodoItems, die in die Datei geschrieben werden sollen
        {
            string s = JsonConvert.SerializeObject(Items, Formatting.Indented); //serialisieren: TodoItemModel in string konvertieren
                                                                                //Formatting: gibt an, wie der Output formatiert werden soll: eingerückt
            File.WriteAllText(_path, s); //Der Text soll der Datei hinzugefügt werden: path = welcher Datei, string = was soll geschrieben werden
        
        }

        public TodoItemFileService(IAppConfiguration appConfiguration) //man übergibt die Abhängigkeit dem Konstruktor
        {
            _path = appConfiguration.TodoItemFilePath; //greift auf das property zu
        }
    }
}

