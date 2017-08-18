using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public abstract class AuditEntity : BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedById { get; set; }
        // // [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { get; set; }


        public DateTime Modified { get; set; } = DateTime.Now;
        public string ModifiedById { get; set; }
        // // [ForeignKey("ModifiedById")]
        public ApplicationUser ModifiedBy { get; set; }
    }
}
