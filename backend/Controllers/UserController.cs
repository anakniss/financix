using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            return NotFound("User not found.");

        return Ok(user);
    }
  
    [HttpPost]
    public async Task<ActionResult<object>> CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "User created successfully.",
            User = user
        });
    }

    [HttpPut]
    public async Task<ActionResult<List<User>>> UpdateUser(User user)
    {
        var dbUser = await _context.Users.FindAsync(user.Id);
        if (dbUser is null)
            return NotFound("User not found.");

        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.Username = user.Username;
        dbUser.Email = user.Email;
        dbUser.Password = user.Password;
        dbUser.Role = user.Role;

        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "User updated successfully.",
            User = user
        });
    }
  
    [HttpDelete]
    public async Task<ActionResult<List<User>>> DeleteUser(int id)
    {
        var dbUser = await _context.Users.FindAsync(id);
        if (dbUser is null)
            return NotFound("User not found.");

        _context.Users.Remove(dbUser);
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "User deleted successfully.",
            Id = id
        });
    }
}