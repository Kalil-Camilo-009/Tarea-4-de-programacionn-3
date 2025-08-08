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
    public class LoginTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void IniciarReporte()
        {
            Reporteador.IniciarReporte();
        }

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument("--start-maximized");

            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");
            Thread.Sleep(2000);
        }

        [Test]
        public void LoginCorrecto()
        {
            Reporteador.test = Reporteador.extent.CreateTest("Login Correcto");

            try
            {
                driver.FindElement(By.Name("username")).SendKeys("Admin");
                driver.FindElement(By.Name("password")).SendKeys("admin123");
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                Thread.Sleep(3000);

                string url = driver.Url;
                Assert.That(url.Contains("dashboard"), "❌ No se redireccionó al dashboard");

                GuardarCaptura("LoginCorrecto");
                Reporteador.test.Pass("✅ Login exitoso");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_LoginCorrecto");
                Reporteador.test.Fail($"❌ Excepción: {ex.Message}");
                throw;
            }
        }

        [Test]
        public void LoginIncorrecto()
        {
            Reporteador.test = Reporteador.extent.CreateTest("❌ Login Incorrecto");

            try
            {
                driver.FindElement(By.Name("username")).SendKeys("adminFake");
                driver.FindElement(By.Name("password")).SendKeys("passFake");
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                Thread.Sleep(2000);

                string error = driver.FindElement(By.ClassName("oxd-alert-content-text")).Text;
                Assert.That(error.Contains("Invalid credentials"), "❌ No se mostró el mensaje de error esperado");

                GuardarCaptura("LoginIncorrecto");
                Reporteador.test.Pass("✅ Error mostrado correctamente con credenciales inválidas");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_LoginIncorrecto");
                Reporteador.test.Fail($"❌ Excepción: {ex.Message}");
                throw;
            }
        }

        [Test]
        public void LoginCamposVacios()
        {
            Reporteador.test = Reporteador.extent.CreateTest("❌ Login con Campos Vacíos");

            try
            {
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();
                Thread.Sleep(2000);

                var camposRequeridos = driver.FindElements(By.XPath("//span[text()='Required']"));
                Assert.That(camposRequeridos.Count >= 1, "❌ No se mostraron los mensajes de campos requeridos");

                GuardarCaptura("LoginCamposVacios");
                Reporteador.test.Pass("Validación de campos vacíos exitosa");
            }
            catch (Exception ex)
            {
                GuardarCaptura("Error_LoginCamposVacios");
                Reporteador.test.Fail($"❌ Excepción: {ex.Message}");
                throw;
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        [OneTimeTearDown]
        public void FinalizarReporte()
        {
            Reporteador.FinalizarReporte();
        }

        private void GuardarCaptura(string nombre)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                byte[] imagenBytes = screenshot.AsByteArray;

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string carpeta = @"C:\Users\Kalil Camilo\Documents\Tarea-4-de-programacionn-3\Proyecto de pruebas de Selenium\Screenshots";
                Directory.CreateDirectory(carpeta);

                string rutaArchivo = Path.Combine(carpeta, $"{nombre}_{timestamp}.png");
                File.WriteAllBytes(rutaArchivo, imagenBytes);

                Reporteador.test.AddScreenCaptureFromPath(rutaArchivo);
                TestContext.WriteLine($"Captura guardada en: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error al tomar la captura: {ex.Message}");
            }
        }
    }
}



