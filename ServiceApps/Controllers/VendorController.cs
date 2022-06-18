using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;
using SLE_System.Models;

namespace Core_3._1.Controllers
{
    public class VendorController : Controller
    {


        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly AppDBContext _context;

        public VendorController(AppDBContext context,UserManager<AppUsers> userManager,
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
            var vendors = await _context.Vendors.ToListAsync();

            return View(vendors);
        }

        public async Task<IActionResult> AddOrEdit(int? vendorId)
        {
            ViewBag.PageName = vendorId == null ? "Create Vendor" : "Edit Vendor";
            ViewBag.IsEdit = vendorId == null ? false : true;
            if (vendorId == null)
            {
                return View();
            }
            else
            {
                var vendo = await _context.Vendors.FindAsync(vendorId);

                if (vendo == null)
                {
                    return NotFound();
                }
                return View(vendo);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int vendorId, 
            [Bind("Id,VendorName,Emailaddress,VendorDescription")]
        Vendors vendorData)
        {
            bool IsVendorExist = false;

            Vendors vend = await _context.Vendors.FindAsync(vendorId);

            if (vend != null)
            {
                IsVendorExist = true;
            }
            else
            {
                vend = new Vendors();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vend.CreatedAt = DateTime.Now;
                    vend.Emailaddress = vendorData.Emailaddress;
                    vend.VendorDescription = vendorData.VendorDescription;
                    vend.VendorName = vendorData.VendorName;
                

                    if (IsVendorExist)
                    {
                        _context.Update(vend);
                    }
                    else
                    {
                        _context.Add(vend);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vendorData);
        }


        public async Task<IActionResult> Details(int? vendorId)
        {
            if (vendorId == null)
            {
                return NotFound();
            }
            var vend = await _context.Vendors.FirstOrDefaultAsync(m => m.Id == vendorId);
            if (vend == null)
            {
                return NotFound();
            }
            return View(vend);

        }

        // GET: 
        public async Task<IActionResult> Delete(int? vendorId)
        {
            if (vendorId == null)
            {
                return NotFound();
            }
            var vend = await _context.Vendors.FirstOrDefaultAsync(m => m.Id == vendorId);

            if (vend == null)
            {
                return NotFound();
            }

            return View(vend);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int vendorId)
        {
            var vend = await _context.Vendors.FindAsync(vendorId);
            _context.Vendors.Remove(vend);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       





    }
}
