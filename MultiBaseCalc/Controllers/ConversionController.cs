using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MultiBaseCalc.Controllers
{
    public class ConversionController : Controller
    {
        // Форма для введення числа та систем числення
        public ActionResult Index()
        {
            // Передаємо у ViewBag доступні системи числення
            ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
            // Отримуємо попередні результати з сесії
            ViewBag.PreviousResults = HttpContext.Session.GetString("PreviousResults")?.Split(';') ?? new string[] { };
            return View();
        }

        // Логіка переведення числа з однієї системи числення в іншу
        [HttpPost]
        public ActionResult ConvertNumber(string number, int fromBase, int toBase)
        {
            try
            {
                // Переведення з вхідної системи числення в десяткову
                int decimalNumber = Convert.ToInt32(number, fromBase);

                // Переведення з десяткової системи в іншу систему числення
                string result = ConvertToBase(decimalNumber, toBase);

                // Додаємо новий результат до сесії
                var previousResults = HttpContext.Session.GetString("PreviousResults")?.Split(';') ?? new string[] { };
                var newResults = new List<string>(previousResults) { $"{number} ({fromBase}) -> {result} ({toBase})" };
                HttpContext.Session.SetString("PreviousResults", string.Join(";", newResults));

                ViewBag.Result = result;
                ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
                ViewBag.PreviousResults = newResults;

                return View("Index");
            }
            catch (Exception)
            {
                ViewBag.Error = "Невірний формат числа або систем числення.";
                ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
                return View("Index");
            }
        }

        // Допоміжний метод для переведення числа з десяткової системи в іншу
        private string ConvertToBase(int decimalNumber, int toBase)
        {
            if (toBase == 10)
            {
                return decimalNumber.ToString();
            }

            string result = "";
            char[] digits = "0123456789ABCDEF".ToCharArray();
            while (decimalNumber > 0)
            {
                result = digits[decimalNumber % toBase] + result;
                decimalNumber /= toBase;
            }

            return result.Length == 0 ? "0" : result;
        }
    }
}
