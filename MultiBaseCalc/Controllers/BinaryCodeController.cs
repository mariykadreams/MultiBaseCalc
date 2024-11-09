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

        private string GetDirectCode(int number, int bits)
        {
            char signBit = number >= 0 ? '0' : '1';
            string binaryValue = Convert.ToString(Math.Abs(number), 2).PadLeft(bits - 1, '0');

            if (binaryValue.Length > bits - 1)
                throw new Exception($"Number is too large for {bits}-bit representation");

            // Concatenate sign bit with binary representation
            return signBit + binaryValue;
        }


        private string GetInverseCode(int number, int bits)
        {
            string directCode = GetDirectCode(number, bits);

            if (number >= 0)
                return directCode;

            // For negative numbers, invert all bits except sign bit
            char[] inverseCode = directCode.ToCharArray();
            for (int i = 2; i < inverseCode.Length; i++) // Start from index 2 to skip sign bit and dot
            {
                inverseCode[i] = inverseCode[i] == '0' ? '1' : '0';
            }

            return new string(inverseCode);
        }

        private string GetComplementCode(int number, int bits)
        {
            if (number >= 0)
                return GetDirectCode(number, bits);

            string inverseCode = GetInverseCode(number, bits);

            // Convert the binary part (after dot) to integer
            string binaryPart = inverseCode.Substring(2);
            int inverseValue = Convert.ToInt32(binaryPart, 2);

            // Add 1 to get complement code
            int complementValue = inverseValue + 1;

            // Convert back to binary and ensure proper length
            string complementBinary = Convert.ToString(complementValue, 2).PadLeft(bits - 1, '0');

            return "1." + complementBinary;
        }
    }
}
