using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;

namespace Proyecto_de_pruebas_de_Selenium.Utils
{
    public static class Reporteador
    {
        public static ExtentReports extent;
        public static ExtentTest test;

        public static void IniciarReporte()
        {
            string ruta = @"C:\Users\Kalil Camilo\Documents\Tarea-4-de-programacionn-3\Proyecto de pruebas de Selenium\Reports\ReporteTest.html";

            // ✅ Reemplaza ExtentHtmlReporter por ExtentSparkReporter
            var sparkReporter = new ExtentSparkReporter(ruta);

            extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);
        }

        public static void FinalizarReporte()
        {
            extent.Flush();
        }
    }
}



