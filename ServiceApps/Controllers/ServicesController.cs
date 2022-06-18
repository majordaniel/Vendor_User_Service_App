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
    public class ServicesController : Controller
    {


        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly AppDBContext _context;

        public ServicesController(AppDBContext context,UserManager<AppUsers> userManager,
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
            var applicationUser = await _userManager.GetUserAsync(User);
            var Id = applicationUser.Id;

            string userEmail = applicationUser?.Email; // will give the user's Email
            var serv = await _context.Services
                .Include(X=>X.Vendors).ToListAsync();

            return View(serv);
        }

        public async Task<IActionResult> AddOrEdit(int? id)
        {
            ViewBag.PageName = id == null ? "Create Service" : "Edit Service";
            ViewBag.IsEdit = id == null ? false : true;


            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "VendorName");
      

            if (id == null)
            {
                return View();
            }
            else
            {
                var service = await _context.Services.FindAsync(id);

                if (service == null)
                {
                    return NotFound();
                }
                return View(service);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, 
            [Bind("Id,VendorId,ServiceName,Fee")]
        Services serviceData)
        {
            bool IsVendorExist = false;

            Services servi = await _context.Services.FindAsync(id);

            if (servi != null)
            {
                IsVendorExist = true;
            }
            else
            {
                servi = new Services();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    servi.CreatedAt = DateTime.Now;
                    servi.VendorId = serviceData.VendorId;
                    servi.ServiceName = serviceData.ServiceName;
                    servi.Fee = serviceData.Fee;
                

                    if (IsVendorExist)
                    {
                        _context.Update(servi);
                    }
                    else
                    {
                        _context.Add(servi);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceData);
        }


        public async Task<IActionResult> Details(int? serviceId)
        {
            if (serviceId == null)
            {
                return NotFound();
            }
            var serv = await _context.Services.FirstOrDefaultAsync(m => m.Id == serviceId);
            if (serv == null)
            {
                return NotFound();
            }
            return View(serv);

        }

        // GET: 
        public async Task<IActionResult> Delete(int? serviceId)
        {
            if (serviceId == null)
            {
                return NotFound();
            }
            var serv = await _context.Services.FirstOrDefaultAsync(m => m.Id == serviceId);

            if (serv == null)
            {
                return NotFound();
            }

            return View(serv);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int serviceId)
        {
            var servv = await _context.Services.FindAsync(serviceId);
            _context.Services.Remove(servv);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       





    }
}
