using System.IO;
using System;
using System.Net;
using System.Collections.Generic;

namespace ForumXScrap {
    public class SaveThread {
        private string baseUri = ""; //adress of forum x
         private string docPath;
        public SaveThread(string text, List<string> filesUri, List<string> names) {
            docPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ForumXThreads\{DateTime.Now.ToFileTime()}";
            CreateFileIfNotExists();
            SaveText(text);
            DownloadFiles(filesUri, names);
        } 
        private void CreateFileIfNotExists() {
             if (!Directory.Exists(docPath)) {
                Directory.CreateDirectory(docPath);
            }
            if (!Directory.Exists(docPath + @"\obrazki")) {
                Directory.CreateDirectory(docPath + @"\obrazki");
            }
        }
        private void SaveText(string text) {
            File.WriteAllText($@"{docPath}\thread.txt", text);
        }
        private void DownloadFiles(List<string> filesUri, List<string> names) {
            var count = filesUri.Count;
            for(var i = 0; i < count; i++) {
                DownloadFile(filesUri[i].Substring(5), names[i]);
            }
        }
        private void DownloadFile(string file, string fileName) {
            using (WebClient myWebClient = new WebClient())
            {
                string picturesResource = baseUri + file;
                myWebClient.DownloadFile(picturesResource, $@"{docPath}\obrazki\{fileName}");        
            }
        }
    }
}