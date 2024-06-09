
using System.Collections.Generic;
using System.Linq;
using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;

namespace DenemeConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                var path1 = "https://www.google.com/";
                var path2 = "https://www.google.com/";
                var path3 = "https://www.google.com/";
                string[] newstrarray = { path1,path2,path3};
                args = newstrarray; 
            }
            foreach(var i in args)
            {
                Console.WriteLine(i);
            }
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(args[0]);
        }
    }
}