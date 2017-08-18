using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exam.Models
{
    public class Bid : AuditEntity, IHaveAnOwner
    {
        public int BidId { get; set; }

        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }
        public decimal Amount { get; set; }


    }
}
