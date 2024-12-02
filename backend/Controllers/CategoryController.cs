using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly DataContext _context;

    public CategoryController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllCategories")]
    public async Task<ActionResult<List<Category>>> GetAllCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        
        if (!categories.Any())
            return NotFound("No exams found.");
        
        var result = categories.Select(e => new
        {
            e.Id,
            e.Name,
            e.Type,
            e.UserId,
            e.IsCustom,
            e.CreatedAt,
            e.UpdatedAt
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _context.Categories
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Name,
                e.Type,
                e.UserId,
                e.IsCustom,
                e.CreatedAt,
                e.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return NotFound("Category not found.");
        }

        return Ok(category);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCategory(Category category)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == category.UserId);
        if (!userExists)
            return BadRequest("Invalid UserId.");
        
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateCategory(Category category)
    {
        var dbCategory = await _context.Categories.FindAsync(category.Id);
        if (dbCategory is null)
            return NotFound("Category not found.");
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == category.UserId);
        if (!userExists)
            return BadRequest("Invalid UserId.");
        
        dbCategory.Name = category.Name;
        dbCategory.Type = category.Type;
        dbCategory.IsCustom = category.IsCustom;
        dbCategory.UserId = category.UserId;
        dbCategory.UpdatedAt = DateTime.Now;
        
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "Category updated successfully.",
            Exam = new
            {
                dbCategory.Id,
                dbCategory.Name,
                dbCategory.Type,
                dbCategory.IsCustom,
                dbCategory.UserId,
                dbCategory.CreatedAt,
                dbCategory.UpdatedAt
            }
        });
    }

    [HttpDelete]
    public async Task<ActionResult<List<Category>>> DeleteCategory(int id)
    {
        var dbCategory = await _context.Categories.FindAsync(id);
        if (dbCategory is null)
            return NotFound("Exam not found.");

        _context.Categories.Remove(dbCategory);
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            Message = "Category deleted successfully.",
            Id = id
        });
    }


    [HttpGet("GetCategoriesByUserId/{userId}")]
    public async Task<ActionResult<List<Category>>> GetCategoriesByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid UserId.");

        var categories = await _context.Categories
            .Where(e => e.UserId == userId)
            .ToListAsync();

        if (!categories.Any())
            return NotFound("No Categories found.");

        return Ok(categories);
    }
}