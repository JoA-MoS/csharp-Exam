using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data;
using Exam.Models;
using Exam.Models.AuctionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{

    [Authorize]
    [Route("auctions")]
    public class AuctionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public UserManager<ApplicationUser> UserManager => _userManager;

        public AuctionsController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            CheckforWinners();
        }

        private void CheckforWinners()
        {
            // This would be a great candate for some sort of event that fires when the remaining time == 0 
            // but no time for that
            List<Auction> auctions = _context.Auctions.Where(a => a.Closed && a.WinningBidId == null || a.WinningBidId == 0)
                                        .Include(a => a.Bids)
                                            .ThenInclude(b => b.Owner)
                                                .ThenInclude(o => o.Wallet)
                                        .Include(a => a.Owner)
                                            .ThenInclude(ao => ao.Wallet)
                                        .ToList();

            foreach (Auction a in auctions)
            {
                if (a.Bids.Count > 0)
                {
                    a.WinningBidId = a.TopBid.BidId;
                    a.TopBid.Owner.Wallet.Amount -= a.TopBidAmount;
                    a.Owner.Wallet.Amount += a.TopBidAmount;
                }
                _context.SaveChanges();
            }



        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var userId = _userManager.GetUserId(User);
            List<Auction> auctions = _context.Auctions.Where(a => !a.Closed)
                                        .Include(a => a.Bids)
                                        .Include(a => a.Owner)
                                        .OrderBy(a => a.RemainingDays).ToList();

            ApplicationUser user = _context.Users.Where(u => u.Id == userId)
                                        .Include(u => u.Wallet)
                                        .First();
            ViewBag.User = user;
            return View("Dashboard", auctions);
        }

        [HttpGet("{auctionId:int}")]
        public IActionResult GetAuction(int auctionId)
        {
            Auction auction = _context.Auctions.Where(a => a.AuctionId == auctionId)
                                                            .Include(a => a.Owner)
                                                            .Include(a => a.Bids)
                                                                .ThenInclude(b => b.Owner)
                                                            .First();

            ViewBag.Auction = auction;

            return View("AuctionDetails");

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateAuction");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAuctionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                Auction auction = new Auction
                {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    EndDate = model.EndDate,
                    StartingBid = model.StartingBid,
                    OwnerId = userId,
                    CreatedById = userId,
                    ModifiedById = userId
                };
                _context.Auctions.Add(auction);
                _context.SaveChanges();
                System.Console.WriteLine("++++++++++++++++++++VALID+++++++++++++++++++");
                return RedirectToAction("Dashboard");
            }
            else
            {
                System.Console.WriteLine("--------------------NOT VALID--------------------");
                return View("CreateAuction", model);
            }
        }


        [HttpGet("{auctionId:int}/delete")]
        // [Authorize(Policy = "Over21")]
        public IActionResult DeleteAuction(int auctionId)
        {
            // quick validatin check 
            try
            {
                var userId = _userManager.GetUserId(User);
                Auction auction = _context.Auctions.Where(w => w.AuctionId == auctionId && w.OwnerId == userId).First();

                if (auction != null)
                {
                    _context.Auctions.Remove(auction);
                    _context.SaveChanges();
                }
                else
                {
                    // this if probably isnt needed
                    return View("AccessDenied");
                }


                return RedirectToAction("Dashboard");
            }
            catch
            {
                return View("AccessDenied");
            }


        }

        [HttpPost("{auctionId:int}/bids")]
        public IActionResult CreateBid(CreateBidViewModel model, int auctionId)
        {
            var userId = _userManager.GetUserId(User);
            Auction auction = _context.Auctions.Where(a => a.AuctionId == auctionId)
                                                    .Include(a => a.Owner)
                                                    .Include(a => a.Bids)
                                                        .ThenInclude(b => b.Owner)
                                                    .First();

            ApplicationUser user = _context.Users.Where(u => u.Id == userId)
                                        .Include(u => u.Wallet)
                                        .First();

            // Validate bid greater then other bids
            if (auction != null)
            {
                if (model.Amount <= auction.TopBidAmount)
                {
                    ModelState.AddModelError("Amount", "Amount must be greater than the current top bid");
                }
                if (model.Amount > user.Wallet.Amount)
                {
                    ModelState.AddModelError("Amount", "You can't bid more than you have");
                }
                if (model.Amount <= auction.StartingBid)
                {
                    ModelState.AddModelError("Amount", "You must bid more than the starting bid");
                }
                if (auction.Closed)
                {
                    ModelState.AddModelError("Amount", "You can only bid on auctions that have not ended");
                }
                if (ModelState.IsValid)
                {
                    auction.Bids.Add(new Bid
                    {
                        OwnerId = userId,
                        CreatedById = userId,
                        ModifiedById = userId,
                        Amount = model.Amount
                    }
                    );
                    _context.SaveChanges();
                }
                else
                {
                    ViewBag.Auction = auction;
                    // dont like this because of the address change but
                    return View("AuctionDetails", model);
                }
            }


            return RedirectToAction("Dashboard");

        }


    }

}
