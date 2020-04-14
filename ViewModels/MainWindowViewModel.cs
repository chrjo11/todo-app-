using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq; //Abfrage/Zugriff von/auf Daten aus verschiedenen Datenquellen wird ermöglicht
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TodoApp.MVVM.Commands;
using TodoApp.MVVM.Converter;
using TodoApp.MVVM.Services;

namespace TodoApp.MVVM.Converter
{
    class MainWindowViewModel : ViewModelBase //ViewModelBase: benachtrichtigt, wenn sich ein Property/Eigenschaftwert:z.B string geändert hat 
    {
        
        public ActionCommand AddNewTodoCommand { get; } //property mit dem Datentyp ICommand (Interface)/ Eigenschaft / Hinzufügen Button mit der ListBox
        public ObservableCollection<TodoItemModel> Items { get; } //property für die Items der ListBox, die mit dem Hinzufügen Button hinzugefügt werden/Typ der Elemente in der Liste: string

        public ActionCommand RemoveTodoCommand { get; }  //property mit dem Datentyp ICommand (Interface)/Eigenschaft/Erledigt-Button (Löschen von Item) der ListBox
        public ActionCommand RemoveAllTodoCommand { get; } //property für Alles-Erledigt-Button

        public ActionCommand NewCommandDownload { get; } //property für neuen Button 

        private readonly ITodoItemService _todoItemService; //im Konstruktor initialisiert und nicht mehr verändert
        
        public IEnumerable<string> Tags { get; } //property für ComboBox

        private string _todoItemText; //Feld für die TextBox
        public string TodoItemText //property für Text der TextBox
        {
            get { return _todoItemText; } //Methode zum Auslesen: gibt den Wert des privaten Felds zurück
            set                           //Methode zum Setzen: kann die Validierung einiger Daten ausführen, bevor er dem privaten Feld einen Wert zuweist
            {
                _todoItemText = value;
                if (TodoItemText.Length > 0) //Wenn die Textlänge größer 0 ist
                {

                    AddNewTodoCommand.IsEnabled = true; //Methodenaufruf: AddNewTodoCommand (Hinzufügen-Button) soll klickbar sein
                }
                else //Ansonsten
                {
                    AddNewTodoCommand.IsEnabled = false; //Methodenaufruf: AddNewTodoCommand (Hinzufügen-Button) soll nicht klickbar sein, wenn kein Text in die TextBox eingegeben ist
                }
            }
        }


        private TodoItemModel _selectedItem; //Feld für ListBox
        public TodoItemModel SelectedItem //property für die Items der ListBox/Ziel: ausgewähltes Item mit Erledigt Button löschen
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (SelectedItem != null)
                {
                    RemoveTodoCommand.IsEnabled = true;
                   
                }
                else
                {
                    RemoveTodoCommand.IsEnabled = false;
                    
                }
            }
        }
        //einfach so

        private string _selectedTag; 
        public string SelectedTag //property für ComboBox: ausgewählter Tag soll dem TodoItemModel Tag zugewiesen bekommen
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
            }
        }

        public string PathPicture { get; set; } //property für den Pfad vom Bild
        
        public MainWindowViewModel(ITodoItemService todoItemService, ITagService tagService) //Konstruktor mit Parameterübergabe
                                                                     //in MainWindow.xaml.cs wird neues Object von MWVM angelegt und Parameter übergeben: neues Object vom Typ TodoItemFileService
        {
            AddNewTodoCommand = new ActionCommand(AddNewTodoItem); //man erzeugt ein Object und übergibt die Binding-Quelle/Methode
                                                                   //Ziel: Wenn der Hinzufügen Button gedrückt wird, soll der Text in der TextBox der ListBox hinzugefügt werden

            RemoveTodoCommand = new ActionCommand(RemoveTodoItem);//Ziel: Wenn Erledigt-Button gedrückt wird, soll das ausgewählte Item gelöscht werden
                                                                  //ButtonFreigebenCommand = new ActionCommand(ButtonFreigeben);
            RemoveAllTodoCommand = new ActionCommand(RemoveAllTodoItems);
            RemoveAllTodoCommand.IsEnabled = true;

            NewCommandDownload = new ActionCommand(NewDownloadAsync);
            NewCommandDownload.IsEnabled = true;

            _todoItemService = todoItemService; //Initialisierung
            Items = new ObservableCollection<TodoItemModel>(); //und leere Liste erstellt, sodass später bei den UnitTests die Abhängigkeit der TextDatei ignoriert werden kann
            var items = todoItemService.ReadTodoItems(); //Methodenaufruf, gibt eine Liste von Items zurück
            Items = new ObservableCollection<TodoItemModel>(items); //leere ObservableCollection/Liste wird angelegt und übergibt Benachrichtigung, wenn Element hinzugefügt/gelöscht wird
            Tags = tagService.ReadTags(); //Methodenaufruf, gibt eine Liste von Tags zurück //Binding an ComboBox
                                           //vorher: Tags = new List<string>(){"Haushalt", "Einkauf"}; //Items der ComboBox
        }

        private async void NewDownloadAsync()
        {
            string url = "http://hintergrundbilder-pc.de/hintergrundbilder-fruehling-06-bilder/bilder-1920x1080/fruehling-107.jpg";
            //string url2 = "http://www.hintergrundbilder-pc.de/hintergrundbilder-fruehling-02-bilder/bilder-1920x1080/fruehling-025.jpg";
            //string url1 = "http://www.hintergrundbilder-pc.de/hintergrundbilder-sonnenuntergang-4k-04-bilder/bilder-3840x2160/sonnenuntergang-059.jpg";
            //await SaveImage(url); //liefert einen string zurück, da await den Task "auspackt" //man wartet bis die Methode ausgeführt wurde
            //RaisePropertyChanged(nameof(PathPicture));
            
            var imageData = await DownloadImage(url);
            string imagePath = "C:\\01_Data\\Prj\\TodoApp.MVVM\\Hintergrundbild2.png";
            await SaveImage(imageData, imagePath);
            PathPicture = imagePath;
            RaisePropertyChanged(nameof(PathPicture));
        }
        
        private async Task<HttpResponseMessage> DownloadImage(string url)
        {
            HttpClient httpClient = new HttpClient(); //wartet auf eine Antwort: sendet GET-Anforderung an den angegebenen URI als asynchronen Vorgang
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); //wirft Exception, wenn StatusCode false ist //true, dh. Anfrage wurde erfolgreich bearbeitet
            return response; 
            
        }
        private async Task SaveImage(HttpResponseMessage responseMessage, string pathPic)
        {
            using (FileStream fileStream = new FileStream(pathPic, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //copy the content from response to filestream
                await responseMessage.Content.CopyToAsync(fileStream); //content ist ein Bild, as durch FileStream erstellt wurde
            }
        } 

        /*
        private async Task<string> SaveImage(string url) //Methoden, die mit async gekennzeichnet sind, geben gen. Par. aus
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url); //enthält viele Infos: status code, RequestMessage
            response.EnsureSuccessStatusCode(); //wirft Exception, wenn StatusCode false ist //true, dh. Anfrage wurde erfolgreich bearbeitet
            //Initialisiert eine neue Instanz der FileStream-Klasse für das angegebene Dateihandle mit den Angaben für die Lese-/Schreibberechtigung, den Besitz der FileStream-Instanz und die Puffergröße.
            using (FileStream fileStream = new FileStream("C:\\01_Data\\Prj\\TodoApp.MVVM\\Hintergrundbild.png", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //copy the content from response to filestream
                await response.Content.CopyToAsync(fileStream); //content ist ein Bild, as durch FileStream erstellt wurde
                return PathPicture = fileStream.Name;
            }
        } */

        private void AddNewTodoItem() //Methode, um neues Item der ListBox anzuheften durch Klicken des Hinzufügen-Buttons
        {
            //Der Text, der im TextBox eingegeben wurde, soll dem Items=ListBox hinzugefügt werden
            Items.Add(new TodoItemModel(TodoItemText, DateTime.Now, SelectedTag)); 
            _todoItemService.WriteTodoItems(Items.ToList()); //Methodenaufruf mit Übergabe einer Liste
            

            TodoItemText = ""; //Der Text soll nach dem Hinzufügen gelöscht werden, Kästchen soll wieder leer sein 
            RaisePropertyChanged(nameof(TodoItemText));   //RaisePropertyChanged-Methode soll ausgeführt werden mit der Instanzübergabe des strings TodoItemText ->""
                                                    //Methodenaufruf immer ohne Datentyp: wird ausgeführt, weil sich das property geändert hat
                                                    //(wurde ausgelagert) falls sich was ändert, muss nur noch RaisePropertyChanged-Methode aufgerufen werden

        }

        private void RemoveTodoItem() //Methode, um ausgewähltes Item aus der ListBox zu entfernen durch Klicken des Erledigt-Button
        {
            Items.Remove(SelectedItem); //Konstruktoraufruf 
            _todoItemService.WriteTodoItems(Items.ToList());
        }

        private void RemoveAllTodoItems()
        {
            Items.Clear(); //löscht alle Einträge der Liste
            _todoItemService.WriteTodoItems(Items.ToList());
        }

    }
}