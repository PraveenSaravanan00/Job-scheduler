using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSchedular.Model
{
    public class Employees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int EmpId { get; set; }
        public required string JobName { get; set; }
        public required string EmpName { get; set; }
        public required int EmpAge { get; set; }
        public required string Gender { get; set; }
        public required string EmpEmail { get; set; }
        public required string EmpPhone { get; set; }
        public required string EmpAddress { get; set; }

    }
}
