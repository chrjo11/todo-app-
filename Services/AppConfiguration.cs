using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM.Services
{
    class AppConfiguration : IAppConfiguration
    {
        public string TodoItemFilePath => @"C:\01_Data\Prj\TodoApp.MVVM\test1.txt"; //=> entspricht get

        public string TagFilePath => @"C:\01_Data\Prj\TodoApp.MVVM\tags.txt";
    }
}
