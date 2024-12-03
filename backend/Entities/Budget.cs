using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Budget
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
    public string Period { get; set; } // daily, monthly, etc...
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}