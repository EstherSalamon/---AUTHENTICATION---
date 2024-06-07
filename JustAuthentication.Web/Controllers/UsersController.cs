﻿using JustAuthentication.Data;
using JustAuthentication.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JustAuthentication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("signup")]
        public void SignUp(SignUpVM sivm)
        {
            if(sivm.User.Name == null || sivm.User.Email == null || sivm.User.Password == null)
            {
                //either this also doesn't work or it's still my button not clicking issue
                return;
            }

            UserRepository repository = new UserRepository(_connectionString);
            repository.AddUser(sivm.User, sivm.User.Password);
        }

        [HttpGet]
        [Route("emailexists")]
        public EmailCheck EmailExists(string email)
        {
            UserRepository repository = new UserRepository(_connectionString);
            EmailCheck check = new EmailCheck
            {
                EmailExists = repository.EmailExists(email)
            };

            return check;
        }

        [HttpPost]
        [Route("login")]
        public User LogIn(LogInVM livm)
        {
            UserRepository repository = new UserRepository(_connectionString);
            User user = repository.LogIn(livm.Email, livm.Password);

            if(user == null)
            {
                return null;
            }

            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Email, user.Email) 
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "role"))).Wait();

            return user;
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return null;
            }

            UserRepository repository = new UserRepository(_connectionString);
            return repository.GetByEmail(User.Identity.Name);
        }

        [HttpPost]
        [Route("logout")]
        public void LogOut()
        {
            HttpContext.SignOutAsync().Wait();
        }
    }
}
