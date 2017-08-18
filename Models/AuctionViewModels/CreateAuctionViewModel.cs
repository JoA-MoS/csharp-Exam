using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exam.CustomAttributes;

namespace Exam.Models.AuctionViewModels
{
    public class CreateAuctionViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(int.MaxValue, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
        [Display(Name = "Description")]
        public string Description { get; set; }



        [Required]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "The {0} must be entered in the format #.##")]
        [Range(0, 9999999999999999.99)]
        [Display(Name = "Starting Bid")]
        public Decimal StartingBid { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "The {0} must be in the future.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
