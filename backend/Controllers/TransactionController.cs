using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly DataContext _context;

    public TransactionController(DataContext context)
    {
        _context = context;
    }
  
    [HttpGet("GetAllTransactions")]
    public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
    {
        var transactions = await _context.Transactions.ToListAsync();
        
        if (!transactions.Any())
            return NotFound("No Transaction found.");
        
        var result = transactions.Select(e => new
        {
            e.Id,
            e.Amount,
            e.Description,
            e.CategoryId,
            e.Date,
            e.Type,
            e.CreatedAt,
            e.UpdatedAt,
            e.UserId
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var transaction = await _context.Transactions
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.Description,
                e.CategoryId,
                e.Date,
                e.Type,
                e.CreatedAt,
                e.UpdatedAt,
                e.UserId
            })
            .FirstOrDefaultAsync();

        if (transaction == null)
        {
            return NotFound("Transaction not found.");
        }

        return Ok(transaction);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction(Transaction transaction)
    {
        var categoryExists = await _context.Categories.AnyAsync(e => e.Id == transaction.CategoryId);
        if (!categoryExists)
        {
            return BadRequest("Category with the provided CategoryId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == transaction.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        transaction.CreatedAt = DateTime.UtcNow;
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return BadRequest("Transaction ID mismatch.");
        }

        var existingTransaction = await _context.Transactions.FindAsync(id);
        if (existingTransaction == null)
        {
            return NotFound("Transaction not found.");
        }
        
        var categoryExists = await _context.Categories.AnyAsync(e => e.Id == transaction.CategoryId);
        if (!categoryExists)
        {
            return BadRequest("Category with the provided CategoryId does not exist.");
        }
        
        var userExists = await _context.Users.AnyAsync(u => u.Id == transaction.UserId);
        if (!userExists)
        {
            return BadRequest("User with the provided UserId does not exist.");
        }
        
        existingTransaction.Amount = transaction.Amount;
        existingTransaction.Type = transaction.Type;
        existingTransaction.CategoryId = transaction.CategoryId;
        existingTransaction.UpdatedAt = DateTime.Now;
        existingTransaction.Description = transaction.Description;
        existingTransaction.Date = transaction.Date;
        existingTransaction.UserId = transaction.UserId;

        await _context.SaveChangesAsync();

        return Ok(new { Message = "Transaction updated successfully.", Transaction = existingTransaction });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound("Transaction not found.");
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Transaction deleted successfully." });
    }

    
    [HttpGet("GetTransactionByUser/{userId}")]
    public async Task<IActionResult> GetTransactionByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return BadRequest("Invalid UserId.");
        }

        var transactions = await _context.Transactions
            .Where(e => e.UserId == userId)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.Description,
                e.CategoryId,
                e.Date,
                e.Type,
                e.CreatedAt,
                e.UpdatedAt,
                e.UserId
            })
            .ToListAsync();

        if (!transactions.Any())
        {
            return NotFound("No Transaction found for the provided UserId.");
        }

        return Ok(transactions);
    }

    [HttpGet("GetTransactionCountByUserId/{userId}")]
    public async Task<ActionResult<int>> GetTransactionCountByUserId(int userId)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return BadRequest("Invalid UserId.");

        var transactionCount = await _context.Transactions
            .Where(e => e.UserId == userId)
            .CountAsync();
        return Ok(transactionCount);
    }
    
    [HttpGet("GetTransactionByCategoryId/{categoryId}")]
    public async Task<IActionResult> GetTransactionByCategoryId(int categoryId)
    {
        var categoryExists = await _context.Categories.AnyAsync(u => u.Id == categoryId);
        if (!categoryExists)
        {
            return BadRequest("Invalid CategoryId.");
        }

        var transactions = await _context.Transactions
            .Where(e => e.CategoryId == categoryId)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.Description,
                e.CategoryId,
                e.Date,
                e.Type,
                e.CreatedAt,
                e.UpdatedAt,
                e.UserId
            })
            .ToListAsync();

        if (!transactions.Any())
        {
            return NotFound("No Transaction found for the provided CategoryId.");
        }

        return Ok(transactions);
    }

    [HttpGet("GetTransactionCountByCategoryId/{categoryId}")]
    public async Task<ActionResult<int>> GetTransactionCountByCategoryId(int categoryId)
    {
        var userExists = await _context.Categories.AnyAsync(u => u.Id == categoryId);
        if (!userExists)
            return BadRequest("Invalid CategoryId.");

        var transactionCount = await _context.Transactions
            .Where(e => e.CategoryId == categoryId)
            .CountAsync();
        return Ok(transactionCount);
    }
}