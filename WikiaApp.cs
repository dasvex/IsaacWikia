using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace IsaacWikia {

    class WikiaApp {
        private string XmlFile = @"content.xml";
        public WikiaApp() {
        }
        public string WaitUserInput() {
            return Console.ReadLine();
        }
        public void Run() {
            string input;
            while(true) {
                Console.WriteLine("What r u find ?");
                input = WaitUserInput();
                Console.Clear();
                var list = FindResult(input);
                foreach(var item in list) {
                    Console.WriteLine(item.Element("name").Value );
                    Console.WriteLine(item.Element("translate").Value);
                    Console.WriteLine(item.Element("description").Value);
                    try {
                        Console.WriteLine("reload : "+item.Element("reload").Value);
                    } catch(NullReferenceException) {
                    }
                    try {
                        Console.WriteLine(item.Element("info").Value);
                    } catch(NullReferenceException) {
                    }
                    Console.WriteLine("============================");
                }
                Console.WriteLine("#######################################3");
            }
        }
        public List<XElement> FindResult(string pattern) {
            XDocument doc = XDocument.Load(XmlFile,LoadOptions.None);
            var a = (from t in doc.Root.Elements("type").Elements("item")
                     where Regex.IsMatch(t.Element("name").Value,pattern,RegexOptions.IgnoreCase) || Regex.IsMatch(t.Element("translate").Value,pattern,RegexOptions.IgnoreCase)
                     select t)
                     .OrderBy(t => t.Element("name").Value,new MyComp(pattern));
            //foreach(var item in a)
            //    Console.WriteLine(item.Element("name").Value);
            //Console.ReadLine();
            return a.ToList();
        }
    }
    class MyComp : IComparer<string> {
        private readonly string pattern;
        public MyComp(string pattern) {
            this.pattern = pattern;
        }
        public int Compare(string x,string y) {
            Match MatchX = Regex.Match(x,pattern,RegexOptions.IgnoreCase);
            Match MatchY = Regex.Match(y,pattern,RegexOptions.IgnoreCase);
            if(MatchX.Index == MatchY.Index)
                return 0;
            return MatchX.Index > MatchY.Index ? 1 : -1;
        }
    }
}
