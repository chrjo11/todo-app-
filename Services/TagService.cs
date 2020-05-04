using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.MVVM.Services;

namespace TodoApp.MVVM
{
    class TagService : ITagService
    {
        private readonly string _path;
        
        public IEnumerable<string> ReadTags()
        {
            if (File.Exists(_path)) //Wenn Datei existiert
            {
                var FileContent = File.ReadAllText(_path); //Text soll aus der Datei gelesen werden und FileContent zugewiesen werden 
                var tagsList = JsonConvert.DeserializeObject<List<string>>(FileContent); //deserialisieren: string in Object konvertieren

                return tagsList; //Beendet die Methode und gibt die gelesene Datei/Liste als Typ string zurück
            }
            else
            {
                File.Create(_path); //Wenn die Datei nicht existiert, soll eine neue Datei erstellt werden
                return new List<string>(); //und es wird eine leere Liste zurückgegeben
            }
        }

        public TagService(IAppConfiguration appConfiguration)
        {
            _path = appConfiguration.TagFilePath;
        }
    }
}
