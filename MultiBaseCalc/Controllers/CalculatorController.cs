using Microsoft.AspNetCore.Mvc;
using MultiBaseCalc.Models;

namespace MultiBaseCalc.Controllers
{
    public class CalculatorController : Controller
    {
        private static readonly char[] HexChars = "0123456789ABCDEF".ToCharArray();

        public ActionResult Index()
        {
            ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
            ViewBag.Operations = new List<string> { "+", "-", "*", "/" };
            return View(new CalculatorModel());
        }

        [HttpPost]
        public ActionResult Calculate(CalculatorModel model)
        {
            ViewBag.Bases = new List<int> { 2, 8, 10, 16 };
            ViewBag.Operations = new List<string> { "+", "-", "*", "/" };

            if (string.IsNullOrEmpty(model.FirstNumber) || string.IsNullOrEmpty(model.SecondNumber))
            {
                ModelState.AddModelError("", "Both numbers are required!");
                return View("Index", model);
            }

            if (!IsValidForBase(model.FirstNumber, model.BaseSystem))
            {
                ModelState.AddModelError("FirstNumber", $"First number contains invalid digits for base-{model.BaseSystem}");
                return View("Index", model); // Нужно добавить возврат здесь
            }

            if (!IsValidForBase(model.SecondNumber, model.BaseSystem))
            {
                ModelState.AddModelError("SecondNumber", $"Second number contains invalid digits for base-{model.BaseSystem}");
                return View("Index", model); // Нужно добавить возврат здесь
            }


            try
            {
                // Convert from input base to decimal
                double num1 = ToDecimal(model.FirstNumber, model.BaseSystem);
                double num2 = ToDecimal(model.SecondNumber, model.BaseSystem);
                double result = 0;

                switch (model.Operation)
                {
                    case "+": result = num1 + num2; break;
                    case "-": result = num1 - num2; break;
                    case "*": result = num1 * num2; break;
                    case "/":
                        if (num2 == 0) throw new DivideByZeroException();
                        result = num1 / num2;
                        break;
                }

                if (model.Operation == "/")
                {
                    string intPart = FromDecimal((long)result, model.BaseSystem);
                    double fractionalPart = result - Math.Floor(result);

                    // Convert fractional part
                    string fractional = "";
                    int precision = 8;

                    while (fractionalPart > 0 && fractional.Length < precision)
                    {
                        fractionalPart *= model.BaseSystem;
                        long digit = (long)fractionalPart;
                        fractional += ToChar(digit);
                        fractionalPart -= digit;
                    }

                    model.Result = intPart + (fractional.Length > 0 ? "." + fractional : "");
                }
                else
                {
                    model.Result = FromDecimal((long)result, model.BaseSystem);
                }

                model.Result = model.Result.ToUpper();
            }
            catch (DivideByZeroException)
            {
                ModelState.AddModelError("", "Cannot divide by zero!");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid input for selected number system!");
                return View("Index", model); // Нужно добавить возврат здесь
            }


            return View("Index", model);
        }

        private double ToDecimal(string number, int baseSystem)
        {
            if (string.IsNullOrEmpty(number)) return 0;

            double result = 0;
            number = number.ToUpper();

            for (int i = 0; i < number.Length; i++)
            {
                result = result * baseSystem + FromChar(number[i]);
            }

            return result;
        }

        private string FromDecimal(long number, int baseSystem)
        {
            if (number == 0) return "0";

            string result = "";
            while (number > 0)
            {
                result = ToChar(number % baseSystem) + result;
                number /= baseSystem;
            }

            return result;
        }

        private int FromChar(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            throw new ArgumentException("Invalid character");
        }

        private char ToChar(long digit)
        {
            if (digit < 0 || digit >= HexChars.Length)
                throw new ArgumentException("Invalid digit");
            return HexChars[digit];
        }

        private bool IsValidForBase(string number, int baseSystem)
        {
            // Разрешенные символы для всех систем счисления
            string validChars = "0123456789ABCDEF".Substring(0, baseSystem);
            return number.ToUpper().All(c => validChars.Contains(c));
        }

    }
}
