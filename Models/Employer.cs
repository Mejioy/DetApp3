using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DetailingCenterApp.Models
{
    public class Employer
    {
        public int EmployerID { get; set; }
        [DisplayName("Фамилия")]
        public string Surname { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Отчество")]
        public string Patronym { get; set; }
        [DisplayName("Номер телефона")]
        [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$", ErrorMessage = "Неверно введён номер телефона. Необходимо ввести в следующем формате 8(888)888-88-88")]
        public string Phone { get; set; }
        [DisplayName("Город")]
        public string City{ get; set; }
        [DisplayName("Улица")]
        public string Street { get; set; }
        [DisplayName("Номер дома")]
        public int Housenumber { get; set; }
        [DisplayName("Номер квартиры")]
        public int? Appartmentnumber { get; set; }

        //Add photo
        public string? Photo { get; set; }

    }
}
