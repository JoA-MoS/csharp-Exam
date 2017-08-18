using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exam.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Wallet : AuditEntity, IHaveAnOwner
    {
        public int WalletId { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public decimal Amount { get; set; } = 1000;
    }
}
