using NUnit.Framework;
using OpenQA.Selenium;
using Proyecto_de_pruebas_de_Selenium.Utils;
using System;
using System.IO;
using AventStack.ExtentReports;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Proyecto_de_pruebas_de_Selenium.Tests
{
    [TestFixture]
    public class EliminarEmpleadoTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [OneTimeSetUp]
        public void IniciarReporte() => Reporteador.IniciarReporte();

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("username"))).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.UrlContains("dashboard"));
        }

        [Test]
        public void EliminarEmpleado()
        {
            Reporteador.test = Reporteador.extent.CreateTest("🗑️ Eliminar Empleado");

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='PIM']"))).Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[text()='Employee List']"))).Click();

                
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='oxd-table-body']/div[1]")));

                
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='oxd-table-body']/div[1]//i[contains(@class,'bi-trash')]"))).Click();

                
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[normalize-space()='Yes, Delete']"))).Click();

                
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@role='dialog']")));

                GuardarCaptura("EliminarEmpleado");
                Reporteador.test.Pass("✅ Empleado eliminado con éxito");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_EliminarEmpleado");
                Reporteador.test.Fail($"❌ Excepción: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

        [OneTimeTearDown]
        public void FinalizarReporte() => Reporteador.FinalizarReporte();

        private void GuardarCaptura(string nombre)
        {
            Screenshot captura = ((ITakesScreenshot)driver).GetScreenshot();
            string carpeta = @"C:\Users\Kalil Camilo\Documents\Tarea-4-de-programacionn-3\Proyecto de pruebas de Selenium\Screenshots";
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta, $"{nombre}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            File.WriteAllBytes(ruta, captura.AsByteArray);
            Reporteador.test.AddScreenCaptureFromPath(ruta);
        }
    }
}






