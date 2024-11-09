using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MultiBaseCalc.Controllers
{
    public class ConversionController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
            ViewBag.PreviousResults = HttpContext.Session.GetString("PreviousResults")?.Split(';').ToList() ?? new List<string>();
            return View();
        }

        [HttpPost]
        public ActionResult ConvertNumber(string number, int fromBase, int toBase)
        {
            try
            {
                // First convert to decimal
                int decimalNumber = CustomBaseToDecimal(number.ToUpper(), fromBase);

                // Then convert from decimal to target base
                string result = CustomDecimalToBase(decimalNumber, toBase);

                // Store in session
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

        private int CustomBaseToDecimal(string number, int fromBase)
        {
            int result = 0;
            int power = 0;

            // Process from right to left
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digitValue = GetDigitValue(number[i]);

                if (digitValue >= fromBase)
                    throw new Exception("Invalid digit for given base");

                result += digitValue * (int)Math.Pow(fromBase, power);
                power++;
            }

            return result;
        }

        private string CustomDecimalToBase(int decimalNumber, int toBase)
        {
            if (decimalNumber == 0)
                return "0";

            string result = "";
            int number = decimalNumber;

            while (number > 0)
            {
                int remainder = number % toBase;
                result = GetDigitChar(remainder) + result;
                number /= toBase;
            }

            return result;
        }

        private int GetDigitValue(char digit)
        {
            if (digit >= '0' && digit <= '9')
                return digit - '0';
            if (digit >= 'A' && digit <= 'F')
                return 10 + (digit - 'A');
            throw new Exception("Invalid digit");
        }

        private char GetDigitChar(int value)
        {
            if (value >= 0 && value <= 9)
                return (char)('0' + value);
            if (value >= 10 && value <= 15)
                return (char)('A' + (value - 10));
            throw new Exception("Invalid value");
        }
    }
}
