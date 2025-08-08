using NUnit.Framework;
using OpenQA.Selenium;
using Proyecto_de_pruebas_de_Selenium.Utils;
using System;
using System.IO;
using System.Threading;
using AventStack.ExtentReports;
using OpenQA.Selenium.Chrome;

namespace Proyecto_de_pruebas_de_Selenium.Tests
{
    [TestFixture]
    public class LoginCredencialesIncorrectasTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void IniciarReporte() => Reporteador.IniciarReporte();

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");
            Thread.Sleep(2000);
        }

        [Test]
        public void CredencialesIncorrectas()
        {
            Reporteador.test = Reporteador.extent.CreateTest("❌ Credenciales Incorrectas");

            try
            {
                driver.FindElement(By.Name("username")).SendKeys("AdminFake");
                driver.FindElement(By.Name("password")).SendKeys("WrongPassword123");
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                Thread.Sleep(2000);

                var error = driver.FindElement(By.ClassName("oxd-alert-content-text")).Text;
                Assert.That(error.Contains("Invalid credentials"));

                GuardarCaptura("CredencialesIncorrectas");
                Reporteador.test.Pass("✅ Error mostrado correctamente");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_CredencialesIncorrectas");
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

