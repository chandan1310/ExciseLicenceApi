using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExciseLicenceApi.Models
{

    [Table("tblChallanCheck")]
    public class ChallanCheck
    {
        [Key]
        [Column("Cid")]
        public int Cid { get; set; }

        [Column("ChallanNo")]
        public string ChallanNo { get; set; }

        [Column("TblName")]
        public string TblName { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("LastscheduleTime")] // Matches exact case from SQL Server
        public DateTime? LastScheduleTime { get; set; }
    }
}