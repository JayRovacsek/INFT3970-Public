﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INFT3970Project.Helpers;
using INFT3970Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace INFT3970Project.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IConfiguration configuration, DatabaseHelper databaseHelper, IHttpContextAccessor httpContextAccessor) : base(configuration, databaseHelper, httpContextAccessor)
        {
        }

        /// <summary>
        /// Synchronous method to return index view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Asynchronous method to update user password
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdatePassword()
        {
            var userId = Convert.ToInt32(Request.Cookies["UserId"]);
            var username = await _databaseHelper.QueryUserEmailAsync(userId);
            var userAndPasswordModel = new UserAndPasswordModel
            {
                User = new UserModel
                {
                    Email = username
                }
            };
            return View(userAndPasswordModel);
        }

        /// <summary>
        /// Asynchronous method to update user details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateDetails()
        {
            var userId = Convert.ToInt32(Request.Cookies["UserId"]);
            var models = await _databaseHelper.QueryUserDetailsAsync(userId);
            var model = models.FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// Synchronous method to update user details given valid model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateDetails(UpdateUserDetailsModel model)
        {
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.fName) && !string.IsNullOrEmpty(model.lName))
                {
                    using (var _databaseHelper = new DatabaseHelper(configuration))
                    {
                        var valid = _databaseHelper.UpdateUserDetails(model);

                        if (valid)
                        {
                            ViewData["Message"] = "Details Changed";
                            return View("Index");
                        }
                        else { ViewData["Message"] = "FAIL";
                            return View("Index");
                        }
                    }
                }
            }
            ViewData["Message"] = "Didn't work";
            return View();
        }

        /// <summary>
        /// Synchronous method to update user password given valid model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdatePassword(UserAndPasswordModel model)
        {
            if(model != null)
            {
                if (!string.IsNullOrEmpty(model.User.Email) && !string.IsNullOrEmpty(model.User.Email))
                {
                    using (var _databaseHelper = new DatabaseHelper(configuration))
                    {
                        var valid = _databaseHelper.UpdatePassword(model);

                        if (valid)
                        {
                            ViewData["Message"] = "Password Changed";
                            return View("Index");
                        }
                    }
                }
            }
            ViewData["Message"] = "Didn't work";
            return View();
        }
    }
}