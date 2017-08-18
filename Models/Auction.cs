using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exam.Models
{
    public class Auction : AuditEntity, IHaveAnOwner
    {
        public int AuctionId { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public string ProductName { get; set; }
        public decimal StartingBid { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public int? WinningBidId { get; set; }
        // public Bid WinningBid { get; set; }

        [NotMapped]
        public bool Closed
        {
            get
            {
                return EndDate < DateTime.Now;
            }
        }

        [NotMapped]
        public Bid TopBid
        {
            get
            {
                try
                {
                    return Bids.OrderByDescending(b => b.Amount).Take(1).First();
                }
                catch
                {
                    return null;
                }
            }
        }

        [NotMapped]
        public decimal TopBidAmount
        {
            get
            {
                try
                {
                    Bid topBid = Bids.OrderByDescending(b => b.Amount).Take(1).First();
                    return topBid.Amount;
                }
                catch
                {
                    return 0;
                }
            }
        }


        [NotMapped]
        public ApplicationUser TopBidder
        {
            get
            {
                try
                {
                    Bid topBidder = Bids.OrderByDescending(b => b.Amount).Take(1).First();
                    return topBidder.Owner;
                }
                catch
                {
                    return null;
                }
            }
        }

        [NotMapped]
        public int RemainingDays
        {
            get { return (int)EndDate.Subtract(DateTime.Now).TotalDays; }
        }

        [NotMapped]
        public int RemainingHours
        {
            get { return (int)EndDate.Subtract(DateTime.Now).TotalHours; }
        }


        [NotMapped]
        public int RemainingMinutes
        {
            get { return (int)EndDate.Subtract(DateTime.Now).TotalMinutes; }
        }



        [NotMapped]
        public int RemainingSeconds
        {
            get { return (int)EndDate.Subtract(DateTime.Now).TotalSeconds; }
        }

        [NotMapped]
        public TimeSpan RemainingTimeSpan
        {
            get { return EndDate.Subtract(DateTime.Now); }
        }

        public string RemainingTimeString
        {
            get
            {
                if (RemainingDays >= 1)
                {
                    return RemainingDays.ToString() + " Days";
                }
                if (RemainingHours >= 1)
                {
                    return RemainingHours.ToString() + " Hours";
                }
                if (RemainingMinutes >= 1)
                {
                    return RemainingMinutes.ToString() + " Minutes";
                }
                if (RemainingSeconds >= 1)
                {
                    return RemainingSeconds.ToString() + " Seconds";
                }
                return "Ended";
            }
        }
        public virtual List<Bid> Bids { get; set; } = new List<Bid>();

    }
}
