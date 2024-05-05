using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Data.SqlClient;
using WarehouseApp.DTOs;

namespace WarehouseApp.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesProductExist(int idProduct)
    {
        var query = "SELECT 1 FROM Product WHERE IdProduct = @IdProduct";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idProduct);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }

    public async Task<bool> DoesWarehouseExist(int idWarehouse)
    {
        var query = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }

    public async Task<int> GetCorrectOrderId(int idProduct, int amount, DateTime createdAt)
    {
        var query = "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt";
        //var query = "SELECT IdOrder FROM [Order] WHERE IdProduct = @IdProduct AND Amount = @Amount";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idProduct);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);
        //command.Parameters.AddWithValue("@CreatedAt", createdAt.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        if (res is null)
        {
            return -1;
        }
        return (int)res;
    }

    public async Task<bool> DoesIdOrderExistInProductWarehouse(int idOrder)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdOrder", idOrder);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();
        
        return res is not null;
    }

    public async Task UpdateFullfilledAt(int idOrder)
    {
        var query = "UPDATE [Order] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
        command.Parameters.AddWithValue("@IdOrder", idOrder);
        
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<decimal> GetProductPrice(int idProduct)
    {
        var query = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
        
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idProduct);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return (decimal)res;
    }
    
    public async Task<int> InsertProductWarehouse(ProductWarehouseDTO productWarehouseDto)
    {
        var query =
            @"
        INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
        OUTPUT INSERTED.IdProductWarehouse
        VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = (SqlTransaction)transaction;
        try
        {
            command.Parameters.Clear();
            command.CommandText = query;
            command.Parameters.AddWithValue("@IdWarehouse", productWarehouseDto.IdWarehouse);
            command.Parameters.AddWithValue("@IdProduct", productWarehouseDto.IdProduct);
            command.Parameters.AddWithValue("@IdOrder", productWarehouseDto.IdOrder);
            command.Parameters.AddWithValue("@Amount", productWarehouseDto.Amount);
            command.Parameters.AddWithValue("@Price", productWarehouseDto.Price);
            command.Parameters.AddWithValue("@CreatedAt", productWarehouseDto.CreatedAt);
            
            int createdId = (int)await command.ExecuteScalarAsync();
            await transaction.CommitAsync();
            return createdId;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to insert product into warehouse: ", e);
        }
    }
}