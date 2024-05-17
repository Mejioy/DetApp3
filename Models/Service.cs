using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DetailingCenterApp.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        [DisplayName("Название")]
        public string Servicename { get; set; }
        [DisplayName("Длительность")]
        public int Duration { get; set; }
        [DisplayName("Стоимость")]
        public int Price { get; set; }
        [DisplayName("Описание")]
        public string? Description { get; set; }


    }
}
