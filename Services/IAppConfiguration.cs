using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM.Services
{
    interface IAppConfiguration
    {
        string TodoItemFilePath { get; } //property für die TodoItem Datei

        string TagFilePath { get; }
    }
}
