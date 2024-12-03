using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalController : ControllerBase
{
    private readonly DataContext _context;

    public GoalController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllGoals")]
    public async Task<ActionResult<List<Category>>> GetAllGoals()
    {
        var goals = await _context.Goals.ToListAsync();
        
        if (!goals.Any())
            return NotFound("No Goals found.");
        
        var result = goals.Select(e => new
        {
            e.Id,
            e.Deadline,
            e.Name,
            e.CreatedAt,
            e.UpdatedAt,
            e.CurrentAmount,
            e.TargetAmount,
            e.UserId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoal(int id)
    {
        var goal = await _context.Goals
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Deadline,
                e.Name,
                e.CreatedAt,
                e.UpdatedAt,
                e.CurrentAmount,
                e.TargetAmount,
                e.UserId
            })
            .FirstOrDefaultAsync();

        if (goal == null)
        {
            return NotFound("Goal not found.");
        }

        return Ok(goal);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateGoal(Goal goal)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == goal.UserId);
        if (!userExists)
            return BadRequest("Invalid UserId.");
        
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        return Ok(goal);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateGoal(Goal goal)
    {
        var dbGoal = await _context.Goals.FindAsync(goal.Id);
        if (dbGoal is null)
            return NotFound("Goal not found.");
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == goal.UserId);
        if (!userExists)
            return BadRequest("Invalid UserId.");
        
        dbGoal.Name = goal.Name;
        dbGoal.Deadline = goal.Deadline;
        dbGoal.UserId = goal.UserId;
        dbGoal.UpdatedAt = DateTime.Now;
        dbGoal.CurrentAmount = goal.CurrentAmount;
        dbGoal.TargetAmount = goal.TargetAmount;
        dbGoal.UserId = goal.UserId;
        
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "Goal updated successfully.",
            Goal = new
            {
                dbGoal.Id,
                dbGoal.Deadline,
                dbGoal.Name,
                dbGoal.CreatedAt,
                dbGoal.UpdatedAt,
                dbGoal.CurrentAmount,
                dbGoal.TargetAmount,
                dbGoal.UserId
            }
        });
    }

    [HttpDelete]
    public async Task<ActionResult<List<Goal>>> DeleteGoal(int id)
    {
        var dbGoal = await _context.Goals.FindAsync(id);
        if (dbGoal is null)
            return NotFound("Goal not found.");

        _context.Goals.Remove(dbGoal);
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "Goal deleted successfully.",
            Id = id
        });
    }


    [HttpGet("GetGoalsByUserId/{userId}")]
    public async Task<ActionResult<List<Goal>>> GetGoalsByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid UserId.");

        var goals = await _context.Goals
            .Where(e => e.UserId == userId)
            .ToListAsync();

        if (!goals.Any())
            return NotFound("No Goals found.");

        return Ok(goals);
    }
    
    [HttpGet("GetGoalCountByUserId/{userId}")]
    public async Task<ActionResult<int>> GetGoalCountByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid UserId.");

        var goalCount = await _context.Goals
            .Where(e => e.UserId == userId)
            .CountAsync();
        return Ok(goalCount);
    }
}