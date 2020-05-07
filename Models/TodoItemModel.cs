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

        public DateTime CreatedAt
        {
            get;
        }
        
        
        public string Tag
        {
            get;
        }

        public int Priority
        {
            get;
        }

        public TodoItemModel(string text, DateTime createdAt, string tag, int priority) //Konstruktor: leere Zeile mit Werkzeug (links) angelegt
        {
            Text = text;
            CreatedAt = createdAt;
            Tag = tag;
            Priority = priority;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
