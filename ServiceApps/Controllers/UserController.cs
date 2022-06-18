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
    public class UserController : Controller
    {


        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly AppDBContext _context;

        public UserController(AppDBContext context,UserManager<AppUsers> userManager,
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

      


        public async Task<IActionResult> Index()
        {
            //var applicationUser = await _userManager.GetUserAsync(User);
            //var Id = applicationUser.Id;


            var allUsers = await _userManager.Users.ToListAsync();


            return View(allUsers);
        }

       





    }
}
