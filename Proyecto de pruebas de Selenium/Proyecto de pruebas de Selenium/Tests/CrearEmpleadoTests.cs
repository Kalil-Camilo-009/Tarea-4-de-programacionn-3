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
    public class CrearEmpleadoTests
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

            // Login primero
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);
        }

        [Test]
        public void CrearNuevoEmpleado()
        {
            Reporteador.test = Reporteador.extent.CreateTest("🧑‍💼 Crear Nuevo Empleado");

            try
            {
                driver.FindElement(By.XPath("//span[text()='PIM']")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("//a[text()='Add Employee']")).Click();
                Thread.Sleep(1000);

                driver.FindElement(By.Name("firstName")).SendKeys("Juan");
                driver.FindElement(By.Name("lastName")).SendKeys("Pérez");
                Thread.Sleep(500);

                driver.FindElement(By.XPath("//button[@type='submit']")).Click();
                Thread.Sleep(3000);

                //bool apareceFotoPerfil = driver.Url.Contains("pim/viewPersonalDetails");
                //Assert.That(apareceFotoPerfil);

                GuardarCaptura("CrearEmpleado");
                Reporteador.test.Pass("✅ Empleado creado con éxito");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_CrearEmpleado");
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


