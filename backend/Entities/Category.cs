using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backend.Entities;

public class Category
{
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsCustom { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}