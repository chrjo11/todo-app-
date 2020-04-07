using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM.Converter
{
    class TodoItemModel
    {
        public string Text
        {
            get;
        }

        public DateTime CreatedAt //vorher:  Uhrzeit + Konstruktor verbessert
        {
            get;
        }
        
        
        public string Tag
        {
            get;
           
        }

        public TodoItemModel(string text, DateTime createdAt, string tag) //Konstruktor: leere Zeile mit Werkzeug (links) angelegt
        {
            Text = text;
            CreatedAt = createdAt;
            Tag = tag;
        }

        public override string ToString()
        {
            return Text;

        }
    }
}
