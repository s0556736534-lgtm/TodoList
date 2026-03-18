
namespace TodoApi.Models;

public partial class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public bool IsComplete { get; set; } = false;    
    public int UserId { get; set; }
}