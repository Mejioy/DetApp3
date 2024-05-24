using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DetailingCenterApp.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        [DisplayName("Фамилия")]
        public string Surname { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Отчество")]
        public string Patronym { get; set; }
        [DisplayName("Номер телефона")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Неверно введён номер телефона. Необходимо ввести в следующем формате 8(888)888-88-88")]
        public string Phone { get; set; }        
        
    }
}
