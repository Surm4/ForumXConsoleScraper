using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ForumXScrap {
    public class Scraper {
        private HtmlWeb web;
        private HtmlDocument doc;
        private HtmlNodeCollection posts;
        private HtmlNode[] postids;
        private HtmlNode[] messages;
        private HtmlNode[] ids;
        private HtmlNode[] dates;
        private HtmlNode[] picturesData;
        private HtmlNode[] picturesUri;
        private List<string> pictureIds = new List<string>();
        private List<string> filesToDl = new List<string>();
        private List<string> filesNames = new List<string>();
        private StringBuilder scrappedThread;
        private int pictureIndex = 0;
        public Scraper(string url) {
            web = new HtmlWeb();
            doc = web.Load(url);
            scrappedThread = new StringBuilder($"Thread: {url}");
            scrappedThread.AppendLine();
            scrappedThread.AppendLine();

            GetThreadData();
            SetFilesToDlList();
            SetPicturesList();
            BuildString();
        }
        private void GetThreadData() {
            posts = doc.DocumentNode.SelectNodes(@"//div[@class='postContainer opContainer' or @class='postContainer replyContainer']");

            postids = posts.Descendants("div").Where(d => d.Attributes["class"].Value.Contains("post op") || d.Attributes["class"].Value.Contains("post reply")).ToArray();

            messages = posts.Descendants("blockquote")
                .Where(d => d.Attributes["class"].Value.Contains("postMessage") || d.Attributes["class"].Value.Contains("postMessage postwImage"))
                .ToArray();

            ids = posts.Descendants("span").Where(d => d.GetClasses().Contains("posteruid"))
                .ToArray();

            dates = posts.Descendants("span").Where(d => d.GetClasses().Contains("dateTime"))
                .ToArray();

            picturesData = posts.Descendants("span").Where(d => d.GetClasses().Contains("fileText"))
                .ToArray();

            picturesUri = posts.Descendants("a").Where(d => d.GetClasses().Contains("fileThumb"))
                .ToArray();
        }
        private void SetFilesToDlList() {
            foreach(var pic in picturesUri) {
                var file = pic.GetAttributeValue("href", String.Empty);
                if (!String.IsNullOrEmpty(file)) filesToDl.Add(file);
            }
        }
        private void SetPicturesList() {
            foreach(var pic in picturesData) {
                pictureIds.Add(StringUtilities.GetPictureId(pic.Id));
            }
        }
        private void BuildString() {
            for(var i = 0; i < posts.Count; i++) {
                var pictureName = GetPictureName(i);
                scrappedThread.Append($"{StringUtilities.GetId(postids[i].Id)} {ids[i].InnerHtml} {dates[i].InnerHtml}");
                scrappedThread.AppendLine();
                scrappedThread.Append(pictureName);
                scrappedThread.AppendLine();
                scrappedThread.Append(StringUtilities.EscapeCharacters(messages[i].InnerHtml));
                scrappedThread.AppendLine();
        
                if(pictureName != "Jakiś embed..." && pictureName != "Bobrazek: żaden") filesNames.Add(pictureName.Substring(11)); //relatywny: 
            }
        }
        private string GetPictureName(int i) => pictureIds.Exists(d => d == StringUtilities.GetId(postids[i].Id)) ? 
            $"Relatywny: {StringUtilities.EscapeCharacters(StringUtilities.GetPictureName(picturesData[pictureIndex++].InnerHtml))}" : "Bobrazek: żaden";
        public string GetThread() => scrappedThread.ToString();
        public List<string> GetFilesToDl() => filesToDl;
        public List<string> GetFilesNames() => filesNames;
    }
}