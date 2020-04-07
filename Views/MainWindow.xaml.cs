using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TodoApp.MVVM.Converter;

namespace TodoApp.MVVM
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new TodoItemFileService(), new TagService()); //DataContext in Code-Behind Datei festgelegt, vorher in MainWindow.xaml
                                                                              //legt neues Object an von MainWindowViewModel 
                                                                              //und man übergibt Parameter: neues Object angelegt TodoItemFileService
        }
    }
}
