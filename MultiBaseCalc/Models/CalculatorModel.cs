using System.ComponentModel.DataAnnotations;

namespace MultiBaseCalc.Models
{
    public class CalculatorModel
    {
        [Required(ErrorMessage = "Please enter the first number")]
        public string FirstNumber { get; set; }

        [Required(ErrorMessage = "Please enter the second number")]
        public string SecondNumber { get; set; }

        public int BaseSystem { get; set; }
        public string Operation { get; set; }
        public string Result { get; set; }
    }


}
