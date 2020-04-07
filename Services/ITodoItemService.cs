using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.MVVM.Converter;

namespace TodoApp.MVVM.Services
{
    interface ITodoItemService //Interface mit 2 Methoden ohne Funktionalität->keine geschweifte Klammern
    {
        void WriteTodoItems(IEnumerable<TodoItemModel> todoItemModel);

        IEnumerable<TodoItemModel> ReadTodoItems();
    }
}
