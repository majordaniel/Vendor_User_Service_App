using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;
using SLE_System.Models;

namespace Core_3._1.Controllers
{
    public class BookingController : Controller
    {


        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly AppDBContext _context;

        public BookingController(AppDBContext context,UserManager<AppUsers> userManager,
                              SignInManager<AppUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;

        //public AccountController(UserManager<IdentityUser> userManager,
        //                      SignInManager<IdentityUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        //public IActionResult Register()
        //{
        //    return View();
        //}


        public async Task<IActionResult> Index()
        {
         

           // string userEmail = applicationUser?.Email; // will give the user's Email
            var bookings = await _context.Bookings
                .Include(x=>x.Vendors)
                //.Include(x=>x.User)
                .ToListAsync();

            //foreach (var item in bookings)
            //{
            //    item.UserId
            //}


            return View(bookings);
        }

        public async Task<IActionResult> AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Create Booking" : "Edit Booking";
            ViewBag.IsEdit = id == null ? false : true;

            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "VendorName");

            if (id == null)
            {
                return View();
            }
            else
            {
                var bk = await _context.Bookings.FindAsync(id);

                if (bk == null)
                {
                    return NotFound();
                }
                return View(bk);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, 
            [Bind("Id,VendorId,CreatedAt")]
        Bookings bkData)
        {
            bool IsVendorExist = false;

            Bookings book = await _context.Bookings.FindAsync(id);

            if (book != null)
            {
                IsVendorExist = true;
            }
            else
            {
                book = new Bookings();
            }

            //if (ModelState.IsValid)
            //{
                try
                {

                var applicationUser = await _userManager.GetUserAsync(User);
                var UId = applicationUser.Id;
                //book.User.Id = UId;
                //book.UserId= Guid.Parse( UId);
                book.UserId = applicationUser.Email;
                book.CreatedAt = bkData.CreatedAt;
                book.VendorId = bkData.VendorId;
                  
                

                    if (IsVendorExist)
                    {
                        _context.Update(book);
                    }
                    else
                    {
                        _context.Add(book);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            //}
            //return View(vendorData);
        }


        public async Task<IActionResult> Details(int? bookingId)
        {
            if (bookingId == null)
            {
                return NotFound();
            }
            var bookings = await _context.Bookings
                .Include(x=>x.Vendors)
                .FirstOrDefaultAsync(m => m.Id == bookingId);
            if (bookings == null)
            {
                return NotFound();
            }
            return View(bookings);

        }

        // GET: 
        public async Task<IActionResult> Delete(int? bookingId)
        {
            if (bookingId == null)
            {
                return NotFound();
            }
            var book = await _context.Bookings
                .Include(x=>x.Vendors)
                .FirstOrDefaultAsync(m => m.Id == bookingId);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var book = await _context.Bookings.FindAsync(Id);
            _context.Bookings.Remove(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       





    }
}
