using System.Runtime.InteropServices.JavaScript;
using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetController : ControllerBase
{
    private readonly DataContext _context;

    public BudgetController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllBudgets")]
    public async Task<ActionResult<List<Budget>>> GetAllBudgets()
    {
        var budgets = await _context.Budgets.ToListAsync();
        
        if (!budgets.Any())
            return NotFound("No Budget found.");
        
        var result = budgets.Select(e => new
        {
            e.Id,
            e.Amount,
            e.CategoryId,
            e.Period,
            e.StartDate,
            e.EndDate,
            e.CreatedAt,
            e.UpdatedAt,
            e.UserId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(int id)
    {
        var budget = await _context.Budgets
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.CategoryId,
                e.Period,
                e.CreatedAt,
                e.UpdatedAt,
                e.StartDate,
                e.EndDate,
                e.UserId
            })
            .FirstOrDefaultAsync();

        if (budget == null)
        {
            return NotFound("Budget not found.");
        }

        return Ok(budget);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBudget(Budget budget)
    {
        var categoryExists = await _context.Categories.AnyAsync(e => e.Id == budget.CategoryId);
        if (!categoryExists)
        {
            return BadRequest("Category with the provided CategoryId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == budget.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        budget.CreatedAt = DateTime.UtcNow;
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(int id, [FromBody] Budget budgetResult)
    {
        if (id != budgetResult.Id)
        {
            return BadRequest("Budget ID mismatch.");
        }

        var existingBudget = await _context.Budgets.FindAsync(id);
        if (existingBudget == null)
        {
            return NotFound("Budget not found.");
        }
        
        var categoryExists = await _context.Categories.AnyAsync(e => e.Id == budgetResult.CategoryId);
        if (!categoryExists)
        {
            return BadRequest("Category with the provided CategoryId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == budgetResult.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        existingBudget.Amount = budgetResult.Amount;
        existingBudget.Period = budgetResult.Period;
        existingBudget.CategoryId = budgetResult.CategoryId;
        existingBudget.UpdatedAt = DateTime.Now;
        existingBudget.StartDate = budgetResult.StartDate;
        existingBudget.EndDate = budgetResult.EndDate;
        existingBudget.UserId = budgetResult.UserId;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Budget updated successfully.", Budget = existingBudget });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        var budgetResult = await _context.Budgets.FindAsync(id);
        if (budgetResult == null)
        {
            return NotFound("Budget not found.");
        }

        _context.Budgets.Remove(budgetResult);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Budget deleted successfully." });
    }

    
    [HttpGet("GetBudgetByUser/{userId}")]
    public async Task<IActionResult> GetBudgetByUser(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return BadRequest("Invalid UserId.");
        }

        var budgets = await _context.Budgets
            .Where(e => e.UserId == userId)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.CategoryId,
                e.Period,
                e.CreatedAt,
                e.UpdatedAt,
                e.StartDate,
                e.EndDate,
                e.UserId
            })
            .ToListAsync();

        if (!budgets.Any())
        {
            return NotFound("No Budget found for the provided UserId.");
        }

        return Ok(budgets);
    }

    [HttpGet("GetBudgetCountByUserId/{userId}")]
    public async Task<ActionResult<int>> GetExamCountByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid UserId.");

        var budgetCount = await _context.Budgets
            .Where(e => e.UserId == userId)
            .CountAsync();
        return Ok(budgetCount);
    }

}