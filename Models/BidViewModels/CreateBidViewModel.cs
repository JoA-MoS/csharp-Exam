using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Exam.CustomAttributes;

namespace Exam.Models.AuctionViewModels
{
    public class CreateBidViewModel
    {


        // TODO: Need to validate greater than next highest
        [Required]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "The {0} must be entered in the format #.##")]
        [Range(0, 9999999999999999.99)]
        [Display(Name = "Bid")]
        public Decimal Amount { get; set; }


    }
}
