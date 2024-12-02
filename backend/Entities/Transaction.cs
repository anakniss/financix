using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Transaction
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Category? Category { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
}