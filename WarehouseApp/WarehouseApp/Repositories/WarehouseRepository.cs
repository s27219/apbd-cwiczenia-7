using WarehouseApp.DTOs;

namespace WarehouseApp.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public Task<bool> DoesProductExist(int idProduct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesWarehouseExist(int idWarehouse)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCorrectOrderId(int idProduct, int amount, DateTime createdAt)
    {
        throw new NotImplementedException();
    }

    /*public Task<bool> DoesCorrectOrderExist(int idProduct, int amount, DateTime createdAt)
    {
        throw new NotImplementedException();
    }*/

    public Task<bool> DoesIdOrderExistInProductWarehouse(int idOrder)
    {
        throw new NotImplementedException();
    }

    public Task UpdateFullfilledAt(int idOrder)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetProductPrice(int idProduct)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertProductWarehouse(ProductWarehouseDTO productWarehouseDto)
    {
        throw new NotImplementedException();
    }
}