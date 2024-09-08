using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Context;
using MyProject.Dtos;
using MyProject.Interfaces;
using MyProject.Models;

namespace MyProject.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly AppDbContext _context;
        private readonly IAuthentication _Auth;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> usermanager, IAuthentication auth, SignInManager<AppUser> appuser, AppDbContext context, IMapper mapper)
        {
            _usermanager = usermanager;
            _Auth = auth;
            _signInManager = appuser;
            _context = context;
            _mapper = mapper;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto rg)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var appuser = new AppUser
                    {
                        UserName = rg.UserName,
                        Email = rg.Email,
                    };
                    var createUser = await _usermanager.CreateAsync(appuser, rg.Password);

                    if (createUser.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                        return Ok(new { message = "User Created" });
                    }
                    else return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(ModelState);
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usermanager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("invalid username");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("username or password is incorrect");

            return Ok(new ConfirmUserDto
            {
                Email = loginDto.Email,
                Token = _Auth.CreateToken(user, user.Email)
            }) ;
        }

    }
}
