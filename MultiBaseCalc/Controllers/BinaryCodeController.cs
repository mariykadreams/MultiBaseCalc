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
        public ActionResult Calculate(int number, int bits = 8)
        {
            try
            {
                if (bits < 2)
                    throw new Exception("Number of bits must be at least 2");

                if (!IsNumberInRange(number, bits))
                    throw new Exception($"Number is too large for {bits}-bit representation");

                var result = new
                {
                    Number = number,
                    DirectCode = GetDirectCode(number, bits),
                    InverseCode = GetInverseCode(number, bits),
                    ComplementCode = GetComplementCode(number, bits)
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        private bool IsNumberInRange(int number, int bits)
        {
            int maxValue = (1 << (bits - 1)) - 1;
            int minValue = -(1 << (bits - 1));
            return number >= minValue && number <= maxValue;
        }

        private string GetDirectCode(int number, int bits)
        {
            // Sign bit (0 for positive, 1 for negative)
            char signBit = number >= 0 ? '0' : '1';

            // Convert absolute value to binary, padding with zeros
            string binaryValue = Convert.ToString(Math.Abs(number), 2).PadLeft(bits - 1, '0');

            // Return sign bit and binary value
            return signBit + "." + binaryValue;
        }

        private string GetInverseCode(int number, int bits)
        {
            if (number == 0)
                return "0." + new string('0', bits - 1);

            if (number > 0)
                return GetDirectCode(number, bits);

            string directCode = GetDirectCode(number, bits);
            char[] inverseCode = directCode.ToCharArray();

            // Invert all bits except sign bit and dot
            for (int i = 2; i < inverseCode.Length; i++)
            {
                inverseCode[i] = inverseCode[i] == '0' ? '1' : '0';
            }

            return new string(inverseCode);
        }

        private string GetComplementCode(int number, int bits)
        {
            if (number == 0)
                return "0." + new string('0', bits - 1);

            if (number > 0)
                return GetDirectCode(number, bits);

            // Get inverse code first
            string inverseCode = GetInverseCode(number, bits);

            // Split the code into parts
            string[] parts = inverseCode.Split('.');
            string binaryPart = parts[1];

            // Convert binary part to integer and add 1
            int inverseValue = Convert.ToInt32(binaryPart, 2);
            int complementValue = inverseValue + 1;

            // Convert back to binary and ensure proper length
            string complementBinary = Convert.ToString(complementValue, 2)
                .PadLeft(bits - 1, '0');

            return "1." + complementBinary;
        }

    }

}
