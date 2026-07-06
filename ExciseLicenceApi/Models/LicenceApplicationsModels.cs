using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExciseLicenceApi.Models
{
    [Table("barfirstregistration")]
    public class BarFirstRegistration
    {
        [Key] // Tells EF which column is the Primary Key
        public int Id { get; set; }

        [Column("financialyear")]
        public string FinancialYear { get; set; }

        public string BarName { get; set; }
        // Add your other columns here...
    }
}