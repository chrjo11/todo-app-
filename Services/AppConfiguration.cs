using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.MVVM.Services
{
    class AppConfiguration : IAppConfiguration
    {
      
        public string Folder { get; set; }
        public string TempPath { get; set; }

        public string TodoItemFilePath => Path.Combine(TempPath, "test1.txt"); //=> entspricht get

        public string TagFilePath => Path.Combine(TempPath, "tags.txt");

        public string GetOrCreateTempFolder(string FolderPath)
        {
            string combined = FolderPath + @"\Temp"; //path für Temp-Ordner

            if (Directory.Exists(combined)) //schaut ob es diesen Ordner gibt
            {
                return combined;
            }
            else //wenn der Temp-Ordner nicht existiert: legt er einen an 
            {
                var pathFolder = Directory.CreateDirectory(combined); //erstellt Temp-Ordner
                return pathFolder.FullName;
            }
        }

        public string GetProjectPath()
        {
            // Ergebnis: \TodoApp.MVVM\bin\Debug-Ordner
            // gibt den Ordner zurück, in der sich die .exe-Datei befindet
            string projectPath = Environment.CurrentDirectory; 

            // Ziel: TodoApp.MVVM Ordner erreichen, um dort einen Temp-Ordner zu erstellen/ finden
            // Mit jedem Durchlauf geht es im Verzeichnisbaum eine Stufe höher: GetDirectoryName-Methode
            for (int i = 0; i < 2; i++)
            {
                // 1. Durchlauf: \TodoApp.MVVM\bin
                // 2. Durchlauf: \TodoApp.MVVM
                projectPath = Path.GetDirectoryName(projectPath);
            }
            return projectPath;
        }

        public AppConfiguration()
        {
            Folder = GetProjectPath(); //ermittelt den Ordner TodoApp.MVVM
            TempPath = GetOrCreateTempFolder(Folder); //legt Temp-Ordner an oder findet ihn, wenn vorhanden 
        }
    }
}
