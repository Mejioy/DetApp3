using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DetailingCenterApp.Models
{
    public class Automobile
    {
        public int AutomobileID { get; set; }
        [DisplayName("Марка")]
        public string Mark { get; set; }
        [DisplayName("Модель")]
        public string Model { get; set; }
        [DisplayName("Гос. номер")]
        [RegularExpression(@"^[ABEKMHOPCTYX]\d{3}(?<!000)[ABEKMHOPCTYX]{2}\d{1,3}rus$", ErrorMessage = "Неверно введён гос. номер. Необходимо ввести в следующем формате A111AA222rus")]
        public string Gosnumber { get; set; }
        public int? ClientId { get; set; }

        public Client? Client { get; set; }

    }
}
