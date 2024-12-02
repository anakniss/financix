using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Goal
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    public string Name { get; set; } 
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
}