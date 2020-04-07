using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM
{
    interface ITagService //Interface mit einer Methode ohne Funktionalität->keine geschweifte Klammern
    {
        IEnumerable<string> ReadTags();
    }
}
