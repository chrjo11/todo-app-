using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.MVVM.Converter;
using TodoApp.MVVM.Services;
using TodoApp.MVVM.ViewModels;

namespace TodoApp.MVVM
{
    class IoCConfiguration
    {
        private static IContainer _container;

        public static void Initialize() //soll vor dem Window initialisiert werden -> App.xaml.cs
        {
            var builder = new ContainerBuilder();
            RegisterDependencies(builder);
            _container = builder.Build();
        }

         
        public static T Resolve<T>() //generische Methode
        {
            return _container.Resolve<T>(); ;
        }

        public static object Resolve(Type type) //Methode für das ViewModel
        {
            return _container.Resolve(type);
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            // Services als Singleton registrieren
            builder.RegisterType<TodoItemFileService>().As<ITodoItemService>().SingleInstance();
            builder.RegisterType<TagService>().As<ITagService>().SingleInstance();
            builder.RegisterType<AppConfiguration>().As<IAppConfiguration>().SingleInstance();

            // ViewModels als Singleton registrieren
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
        }

    }
}
