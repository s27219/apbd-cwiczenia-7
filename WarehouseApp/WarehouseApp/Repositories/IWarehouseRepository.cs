using Microsoft.VisualBasic;
using WarehouseApp.DTOs;

namespace WarehouseApp.Repositories;

public interface IWarehouseRepository
{
    Task<bool> DoesProductExist(int idProduct);
    Task<bool> DoesWarehouseExist(int idWarehouse);
    Task<int> GetCorrectOrderId(int idProduct, int amount, DateTime createdAt);
    //Task<bool> DoesCorrectOrderExist(int idProduct, int amount, DateTime createdAt);
    Task<bool> DoesIdOrderExistInProductWarehouse(int idOrder);
    Task UpdateFullfilledAt(int idOrder);
    Task<Decimal> GetProductPrice(int idProduct);
    Task<int> InsertProductWarehouse(ProductWarehouseDTO productWarehouseDto);
}