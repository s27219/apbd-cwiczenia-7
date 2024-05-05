using Microsoft.AspNetCore.Mvc;
using WarehouseApp.DTOs;
using WarehouseApp.Repositories;

namespace WarehouseApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _warehouseRepository;
    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToWarehouse([FromBody] ProductWarehouseInputDTO productWarehouseInputDto)
    {
        if (productWarehouseInputDto.Amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }

        if (!await _warehouseRepository.DoesProductExist(productWarehouseInputDto.IdProduct) || !await _warehouseRepository.DoesWarehouseExist(productWarehouseInputDto.IdWarehouse))
        {
            return NotFound("Product or warehouse not found.");
        }

        int orderId = await _warehouseRepository.GetCorrectOrderId(productWarehouseInputDto.IdProduct, productWarehouseInputDto.Amount, productWarehouseInputDto.CreatedAt);
        if (orderId == -1)
        {
            return BadRequest("No valid order found for this product.");
        }
        //Console.WriteLine(orderId);

        if (await _warehouseRepository.DoesIdOrderExistInProductWarehouse(orderId))
        {
            return BadRequest("This order has already been fulfilled.");
        }

        await _warehouseRepository.UpdateFullfilledAt(orderId);

        Decimal price = await _warehouseRepository.GetProductPrice(productWarehouseInputDto.IdProduct);
        Console.WriteLine(price);
        
        var productWarehouseEntry = new ProductWarehouseDTO()
        {
            IdWarehouse = productWarehouseInputDto.IdWarehouse,
            IdProduct = productWarehouseInputDto.IdProduct,
            IdOrder = orderId,
            Amount = productWarehouseInputDto.Amount,
            Price = productWarehouseInputDto.Amount * price,
            CreatedAt = productWarehouseInputDto.CreatedAt
        };

        int productWarehouseId = await _warehouseRepository.InsertProductWarehouse(productWarehouseEntry);
        
        return Ok(productWarehouseId);
    }
}