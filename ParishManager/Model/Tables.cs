using System.ComponentModel.DataAnnotations;

namespace ParishManager.Model;

public class Parish
{
    public int Id { get; set; }
    
    [MaxLength(200)]
    public required string City { get; set; }
    [MaxLength(200)]
    public required string Address { get; set; }
    [MaxLength(200)]
    public required string Patronage { get; set; }
    public int Parishioners { get; set; }
    
}