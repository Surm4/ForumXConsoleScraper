using System;
namespace ForumXScrap {
    public class StringUtilities {
        public static string ReplaceLinks(string msg) {
            if (!msg.Contains("<a href") && !msg.Contains("class=\"quotelink\">")) return msg;
            int start = msg.IndexOf("<a href");
            int end = msg.IndexOf("class=\"quotelink\">") - start + "class=\"quotelink\">".Length;
            string unnecessaryValue = msg.Substring(start, end);
            string neededValue = msg.Replace(unnecessaryValue, "");
            return StringUtilities.ReplaceLinks(neededValue);
        }

        public static string GetPictureName(string msg) {
            int start = msg.IndexOf("download=\"");
            if (start < 0) return "Jakiś embed...";
            start = msg.IndexOf("download=\"") + "download=\"".Length;
            int end = msg.IndexOf("\">");
            return msg.Substring(start, end - start);
        }
        
        public static string GetPictureId(string msg) {
            msg = msg.Substring(2);
            msg = msg.Substring(0, msg.Length - 2);
            return msg;
        }
        public static string GetId(string msg) => msg.Substring(1); 
        public static string EscapeCharacters(string msg) {
            msg = msg.Replace(@"&amp;", @"g")
                .Replace(@"&lt;", @"<")
                .Replace(@"&gt;", @">")
                .Replace(@"&quot;", "\"")
                .Replace(@"&#039;", @"'")
                .Replace("<span class=\"quote\">", "")
                .Replace("</span>", "")
                .Replace("<br>", "\n")
                .Replace(@"&#263;", "ć")
                .Replace(@"&#347;", "ś")
                .Replace(@"&oacute;", "ó")
                .Replace(@"&#281;", "ę")
                .Replace(@"&#261;", "ą")
                .Replace(@"&#378;", "ź")
                .Replace(@"&#322;", "ł")
                .Replace(@"&#380;", "ż")
                .Replace(@"</s>", "")
                .Replace(@"<s>", "")
                .Replace(@"</a>", "")
                .Replace(@"&#1072;", "a")
                .Replace(@"<style>@keyframes pulse { 0% {background-color: #EA6045;} 25% {background-color: #F8CA4D;} 50% {background-color: #F5E5C0;} 75% {background-color: #3F5666;} 100% {background-color: #2F3440;} }</style>", "")
                .Replace("<span style=\"animation: pulse 1s infinite alternate; \">KOMPROMITACJA CWELA", "KOMPROMITACJA CWELA");
            return StringUtilities.ReplaceLinks(msg);
        }
    }
}