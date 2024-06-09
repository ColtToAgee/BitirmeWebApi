
using System.Collections.Generic;
using System.Linq;
using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace DenemeConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(args[1]);
        }
    }
}