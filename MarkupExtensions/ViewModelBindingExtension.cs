using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace TodoApp.MVVM.MarkupExtensions
{
    //eigene Markup-Extension für das ViewModel: braucht man, wenn man mehrere ViewModels hat und zwischen den switchen möchte
    class ViewModelBindingExtension : MarkupExtension
    {
        public Type ViewModelType { get; set; }

        //ProvideValue: Wenn in einer abgeleiteten Klasse implementiert, wird ein Objekt zurückgegeben, 
        //das als Wert der Zieleigenschaft für diese Markuperweiterung bereitgestellt wird.
        public override object ProvideValue(IServiceProvider _) // _-Operator/Discard-Operator: wird nicht verwendet
        {
            //soll Objekt vom Typ MainWindowViewModel zurückgeben 
            var viewModel = IoCConfiguration.Resolve(ViewModelType); 
            return viewModel;
        }
    }
}
