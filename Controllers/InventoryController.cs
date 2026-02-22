using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;   
using System.Text.Json.Serialization;
using TechAPI.Models;

namespace TechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public ActionResult SaveInventoryData(Inventory InventoryDto)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=techDb;Trusted_Connection=True;"
            };
            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_SaveInventoryData",
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = connection
            };

            command.Parameters.AddWithValue("@ProductId", InventoryDto.ProductId);
            command.Parameters.AddWithValue("@ProductName", InventoryDto.ProductName);
            command.Parameters.AddWithValue("@StockAvailable", InventoryDto.StockAvailable);
            command.Parameters.AddWithValue("@ReorderStock", InventoryDto.ReorderStock);

            connection.Open();
            command.ExecuteNonQuery();

            return Ok();
        }

        [HttpDelete("{productId}")]
        public ActionResult DeleteInventoryDetails(int productId)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=techDb;Trusted_Connection=True;"
            };
            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_DeleteInventoryDetails",
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = connection
            };

            connection.Open();
            command.Parameters.AddWithValue("@ProductId", productId);
            command.ExecuteNonQuery();
            connection.Close();
            return Ok();
        }

        [HttpGet]
        public ActionResult GetInventoryData()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=techDb;Trusted_Connection=True;"
            };
            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_GetInventoryData",
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = connection
            };
            connection.Open();

            List<InventoryDto> responce = new List<InventoryDto>();

            using (SqlDataReader sqlDataReader = command.ExecuteReader())
            {
                while (sqlDataReader.Read()) 
                { 
                    InventoryDto inventoryDto = new InventoryDto();
                    inventoryDto.ProductId = Convert.ToInt32(sqlDataReader["ProductId"]);
                    inventoryDto.ProductName = Convert.ToString(sqlDataReader["ProductName"]);
                    inventoryDto.StockAvailable = Convert.ToInt32(sqlDataReader["StockAvailable"]);
                    inventoryDto.ReorderStock = Convert.ToInt32(sqlDataReader["ReorderStock"]);

                    responce.Add(inventoryDto);
                }
            }
            connection.Close();

            return Ok((responce));
        }
    }
}
