using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "AdminRole")]
        [HttpGet("modifyroles")]
        public async Task<ActionResult> ModifyUserRoles()
        {
            var users = await _userManager.Users
                .Include(ur => ur.UserRoles)
                .ThenInclude(ur => ur.Role)
                .OrderBy(user => user.UserName)
                .Select(user => new
                {
                    user.Id,
                    Username = user.UserName,
                    Roles = user.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpPost("editroles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var activeRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User does not exist");

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, activeRoles.Except(userRoles));
            if (!result.Succeeded) return BadRequest("Failed to add role");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(activeRoles));
            if (!result.Succeeded) return BadRequest("Failed to remove role");

            return Ok(await _userManager.GetRolesAsync(user));
        }
    }
}