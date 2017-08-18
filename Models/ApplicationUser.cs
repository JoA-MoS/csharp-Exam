using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exam.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [InverseProperty("Owner")]
        public Wallet Wallet { get; set; } = new Wallet();

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        [InverseProperty("Owner")]
        public virtual List<Auction> Auctions { get; set; } = new List<Auction>();

        [InverseProperty("Owner")]
        public virtual List<Bid> Bids { get; set; } = new List<Bid>();


    }
}
