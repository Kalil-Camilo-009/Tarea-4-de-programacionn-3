using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Proyecto_de_pruebas_de_Selenium.Utils
{
    public class DriverFactory
    {
        public static IWebDriver CrearDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            return new ChromeDriver(options);
        }
    }
}

