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

namespace TodoApp.MVVM.ViewModels
{
    class MainWindowViewModel : ViewModelBase, IDataErrorInfo //ViewModelBase: benachtrichtigt, wenn sich ein Property/Eigenschaftwert:z.B string geändert hat 
    {
        private readonly ITodoItemService _todoItemService; //im Konstruktor initialisiert und nicht mehr verändert

        public ActionCommand AddNewTodoCommand { get; } //property mit dem Datentyp ICommand (Interface)/ Eigenschaft / Hinzufügen Button mit der ListBox
        public ObservableCollection<TodoItemModel> Items { get; } //property für die Items der ListBox, die mit dem Hinzufügen Button hinzugefügt werden/Typ der Elemente in der Liste: string

        public ActionCommand RemoveTodoCommand { get; }  //property mit dem Datentyp ICommand (Interface)/Eigenschaft/Erledigt-Button (Löschen von Item) der ListBox
        public ActionCommand RemoveAllTodoCommand { get; } //property für Alles-Erledigt-Button

        public ActionCommand NewCommandDownload { get; } //property für neuen Button 

        public string PathPicture { get; set; } //property für den Pfad vom Bild

        public IEnumerable<string> Tags { get; } //property für ComboBox

        private int _priority; //Feld für Ratingbar
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

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
                    //ResetError(TodoItemText);
                    SetError(nameof(TodoItemText), "");
                }
                else 
                {
                    AddNewTodoCommand.IsEnabled = false; //Methodenaufruf: AddNewTodoCommand (Hinzufügen-Button) soll nicht klickbar sein, wenn kein Text in die TextBox eingegeben ist
                    SetError(nameof(TodoItemText), "TodoItem - text kann nicht leer sein.");
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

        private string _selectedTag;
        public string SelectedTag //property für ComboBox: ausgewählter Tag soll dem TodoItemModel Tag zugewiesen bekommen
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
            }
        }


        // Binding an TextBlock, der un-/sichtbar geschaltet werden kann (HasErrors)
        public string Error
        {
            get
            {
                string joinedErrors = null;

                // läuft durch das Dictionary Errors durch und fügt Einträge zu einem Error zusammen
                foreach (string key in Errors.Keys)
                {
                    if (!String.IsNullOrEmpty(joinedErrors))
                    {
                        joinedErrors += "\n";
                    }

                    joinedErrors += Errors[key];
                }

                return joinedErrors;
            }
        }

        // speichert property und errormessage
        public Dictionary<string, string> Errors { get; } 

        // gets errormessage für das übergebene Property
        public string this[string columnName] => ValidateProperty(columnName);

        // überprüft, ob es einen Error gibt -> bool
        public bool HasErrors
        {
            get { return !String.IsNullOrEmpty(Error); }
        }

        // Methode überprüft, ob das Property im Errors Dictionary vorhanden ist
        // Wenn ja: gets propertyName und gibts zurück
        // Wenn nein: wird null zurückgegeben
        private string ValidateProperty(string propertyName)
        {
            return Errors.ContainsKey(propertyName) ? Errors[propertyName] : null;
        }


        public MainWindowViewModel(
            ITodoItemService todoItemService,
            ITagService tagService) //Konstruktor mit Parameterübergabe
                                    //in MainWindow.xaml.cs wird neues Object von MWVM angelegt und Parameter übergeben: neues Object vom Typ TodoItemFileService
        {
            Errors = new Dictionary<string, string>();
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


        // Methode, die den Error rückgängig macht bzw. aus dem Dictionary löscht
        public void ResetError(string propertyName)
        {
            SetError(propertyName, default(string));
        }
        
        // Methode, die unter bestimmten Bedingungen Errors Dictionary erweitert/kürzt
        protected virtual void SetError(string propertyName, string errorMessage)
        {
            if (String.IsNullOrEmpty(errorMessage))
            {
                Errors.Remove(propertyName);
            }
            else if (Errors.ContainsKey(propertyName) && Errors.Count != 0)
            {
                Errors[propertyName] = errorMessage;
            }
            else
            {
                Errors.Add(propertyName, errorMessage);
            }

            RaisePropertyChanged("Error");
            RaisePropertyChanged("HasErrors");
        }

            private async void NewDownloadAsync()
        {
            string url = "http://hintergrundbilder-pc.de/hintergrundbilder-fruehling-06-bilder/bilder-1920x1080/fruehling-107.jpg";
            //string url2 = "http://www.hintergrundbilder-pc.de/hintergrundbilder-fruehling-02-bilder/bilder-1920x1080/fruehling-025.jpg";
            //string url1 = "http://www.hintergrundbilder-pc.de/hintergrundbilder-sonnenuntergang-4k-04-bilder/bilder-3840x2160/sonnenuntergang-059.jpg";
            
            var imageData = await DownloadImage(url);
            string imagePath = "C:\\01_Data\\Prj\\TodoApp.MVVM\\Hintergrundbild2.png";
            await SaveImage(imageData, imagePath); //liefert einen string zurück, da await den Task "auspackt"; man wartet bis die Methode ausgeführt wurde
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
                await responseMessage.Content.CopyToAsync(fileStream); //content ist ein Bild, das durch FileStream erstellt wurde
            }

        } 

        private void AddNewTodoItem() //Methode, um neues Item der ListBox anzuheften durch Klicken des Hinzufügen-Buttons
        {
            //Der Text, der im TextBox eingegeben wurde, soll dem Items=ListBox hinzugefügt werden
            Items.Add(new TodoItemModel(TodoItemText, DateTime.Now, SelectedTag, Priority)); 
            _todoItemService.WriteTodoItems(Items.ToList()); //Methodenaufruf mit Übergabe einer Liste
            
            TodoItemText = ""; //Der Text soll nach dem Hinzufügen gelöscht werden, Kästchen soll wieder leer sein 
            RaisePropertyChanged(nameof(TodoItemText));   //RaisePropertyChanged-Methode soll ausgeführt werden mit der Instanzübergabe des strings TodoItemText ->""
                                                          //Methodenaufruf immer ohne Datentyp: wird ausgeführt, weil sich das property geändert hat
                                                          //(wurde ausgelagert) falls sich was ändert, muss nur noch RaisePropertyChanged-Methode aufgerufen werden
            Priority = 0;
            RaisePropertyChanged(nameof(Priority));
            ResetError(nameof(TodoItemText));
            SetError(nameof(TodoItemText), "TodoItem - text kann nicht leer sein.");
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