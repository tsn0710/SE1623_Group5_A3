using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estore2API.Models
{
    public class Report
    {
        public Report()
        {
            Reports = new HashSet<Report>();
        }
        [Key]
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
