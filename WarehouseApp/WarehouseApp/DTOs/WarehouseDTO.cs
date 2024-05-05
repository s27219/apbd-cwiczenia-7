using System.ComponentModel.DataAnnotations;

namespace WarehouseApp.DTOs;

public class WarehouseDTO
{
    
}

public class OrderDTO
{
    
}

public class ProductDTO
{
    
}
public class ProductWarehouseDTO
{
    public int IdWarehouse { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class ProductWarehouseInputDTO
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}