using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoApp.MVVM.Commands
{
    class ActionCommand : ICommand //Erbt von Interface, sie sind abstract, dh. Variablen und Konstruktoren sind nicht erlaubt
                                   //ICommand besitzt den EventHandler=Ereignis und zwei Methoden: CanExecute bool, Execute void
    {
        public event EventHandler CanExecuteChanged; //Ereignis wird ausgelöst, wenn sich die Ausführungsmöglichkeit eines Kommandos etwas geändert hat


        private bool _isEnabled; //Feld angelegt

        public bool IsEnabled //property: Für den Hinzufügen-Button
        {
            get { return _isEnabled; } //wird immer ausgeführt, wenn das property gelesen wird
            set
            {
                _isEnabled = value; //wird immer ausgeführt, wenn das property sich ändert
                if(CanExecuteChanged != null) 
                { 
                    CanExecuteChanged.Invoke(this, EventArgs.Empty); //Methode wird übergeben und Invoke führt diese aus (delegate)
                                                                 //Invoke erwartet: object sender (Auslöser des events: Klicken des Buttons Hinzufügen), und das event
                }
            }
        }

        public bool CanExecute(object parameter)  //über diese Methode wird abgefragt, ob ein Kommando auf einem bestimmten Ziel ausgeführt werden kann
                                                  //Wenn true ausgegeben wird, dann kann der Execute Befehl ausgeführt werden
        {
            return IsEnabled; 
        }

        public void Execute(object parameter)   //enthält Anweisungen, die beim Auslösen des Kommandos (hier: Klicken des Buttons Hinzufügen) abgearbeitet werden
                                                //Immer wenn auf Button geklickt wird/wird nur ausgeführt, wenn CanExecute true zurückgibt
        {
            _action.Invoke(); //Methode wird übergeben und Invoke führt diese aus/Führt den angegebenen Delegaten(=Methodenzeiger) auf dem Thread aus, der das zugrunde liegende Fensterhandle des Steuerelements besitzt
        }

        private Action _action; //Felder mit Unterstrich vom Typ delegate

        public ActionCommand(Action action) // Das ist ein delegate-Typ, dh. er ist ein Referenztyp, der eine Methode kapselt 
        //die über keine Parameter verfügt und keinen Wert zurückgibt/ ähnelt einer Methodensignatur
        //Der Delegat verfügt über einen Methodenverweis, der ausgeführt werden soll, wenn der Befehl ausgelöst wird.
        {
            _action = action; //action hat als Returntyp void, dh. es wird nichts zurückgegeben und hat keine Parameter: es soll nichts zurückgegeben werden, wenn Hinzufügen gedrückt wird
                              //in diesem Aktionsdelegat können wir nur die Methoden speichern, die keine Parameter und keinen void-Rückgabetyp haben.
        }
    }
}
