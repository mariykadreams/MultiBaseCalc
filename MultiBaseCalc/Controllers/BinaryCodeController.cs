using Microsoft.AspNetCore.Mvc;

namespace MultiBaseCalc.Controllers
{
    public class BinaryCodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateCodes(int number)
        {
            int positiveNumber = Math.Abs(number);
            int negativeNumber = -positiveNumber;

            // Codes for the positive equivalent of the input number
            var directCodePositive = BinaryCodeCalculator.DirectCode(positiveNumber);
            var reverseCodePositive = BinaryCodeCalculator.ReverseCode(positiveNumber);
            var additionalCodePositive = BinaryCodeCalculator.AdditionalCode(positiveNumber);

            // Codes for the negative equivalent of the input number
            var directCodeNegative = BinaryCodeCalculator.DirectCode(negativeNumber);
            var reverseCodeNegative = BinaryCodeCalculator.ReverseCode(negativeNumber);
            var additionalCodeNegative = BinaryCodeCalculator.AdditionalCode(negativeNumber);

            // Pass results to the view
            ViewBag.Number = number;
            ViewBag.DirectCodePositive = directCodePositive;
            ViewBag.ReverseCodePositive = reverseCodePositive;
            ViewBag.AdditionalCodePositive = additionalCodePositive;
            ViewBag.DirectCodeNegative = directCodeNegative;
            ViewBag.ReverseCodeNegative = reverseCodeNegative;
            ViewBag.AdditionalCodeNegative = additionalCodeNegative;

            return View("Index");
        }
    }

    public static class BinaryCodeCalculator
    {
        public static string DirectCode(int number)
        {
            string binary = Convert.ToString(Math.Abs(number), 2).PadLeft(8, '0');
            return (number >= 0 ? "0" : "1") + "." + binary;
        }

        public static string ReverseCode(int number)
        {
            if (number >= 0)
                return DirectCode(number);

            string direct = DirectCode(number).Split('.')[1];
            char[] reversed = new char[direct.Length];
            for (int i = 0; i < direct.Length; i++)
            {
                reversed[i] = direct[i] == '0' ? '1' : '0';
            }

            return "1." + new string(reversed);
        }

        public static string AdditionalCode(int number)
        {
            if (number >= 0)
                return DirectCode(number);

            char[] reversed = ReverseCode(number).Split('.')[1].ToCharArray();
            for (int i = reversed.Length - 1; i >= 0; i--)
            {
                if (reversed[i] == '0')
                {
                    reversed[i] = '1';
                    break;
                }
                else
                {
                    reversed[i] = '0';
                }
            }

            return "1." + new string(reversed);
        }
    }

}
