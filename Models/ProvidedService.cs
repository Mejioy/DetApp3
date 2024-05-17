using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DetailingCenterApp.Models
{
    public class ProvidedService
    {
        public int ProvidedServiceID { get; set; }
        public int? ServiceId { get; set; }
        public int? EmployerId { get; set; }
        public int? AutomobileId { get; set; }
        [DisplayName("Дата и время оказания услуги")]
        public DateTime dateTime { get; set; }

        public Service? Service { get; set; }
        public Employer? Employer { get; set; }
        public Automobile? Automobile { get; set; }

    }
}
