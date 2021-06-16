using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Repositories
{
    public class ProductMaterialRepository : IRepository<ProductsMaterial>
    {
        private string _connectionString;

        public ProductMaterialRepository(string connection)
        {
            _connectionString = connection;
        }

        public void Create(ProductsMaterial item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Insert into ProductsMaterials values(@materialId, @productId, @quantity)";
                SqlCommand command = new(sql, connection);
                SqlParameter materialId = new("@materialId", item.MaterialId);
                command.Parameters.Add(materialId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter quanitity = new("@quantity", item.Quantity);
                command.Parameters.Add(quanitity);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(ProductsMaterial item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Delete ProductsMaterials where productMaterial = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductMaterial);
                command.Parameters.Add(idParam);
                command.ExecuteNonQuery();

            }
        }

        public ProductsMaterial Get(int id)
        {
            ProductsMaterial productsMaterial = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productMaterial, ProductsMaterials.materialId, ProductsMaterials.productId, quantity, productName, materialName from ProductsMaterials " +
                    "inner join Products on Products.productId = ProductsMaterials.ProductId " +
                    "inner join Materials on Materials.materialId = ProductsMaterials.materialId " +
                    "where ProductsMaterials.productMaterial = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", id);
                command.Parameters.Add(idParam);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        productsMaterial.ProductMaterial = reader.GetInt32(0);
                        productsMaterial.MaterialId = reader.GetInt32(1);
                        productsMaterial.ProductId = reader.GetInt32(2);
                        productsMaterial.Quantity = reader.GetDouble(3);
                        productsMaterial.Product = new Product();
                        productsMaterial.Product.ProductId = (int)productsMaterial.ProductId;
                        productsMaterial.Product.ProductName = reader.GetString(4);
                        productsMaterial.Material = new Material();
                        productsMaterial.Material.MaterialId = (int)productsMaterial.MaterialId;
                        productsMaterial.Material.MaterialName = reader.GetString(5);
                    }
                }
            }
            return productsMaterial;
        }

        public IEnumerable<ProductsMaterial> GetAll()
        {
            List<ProductsMaterial> productsMaterials = new();
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "select productMaterial, ProductsMaterials.materialId, ProductsMaterials.productId, quantity, productName, materialName from ProductsMaterials " +
                   "inner join Products on Products.productId = ProductsMaterials.ProductId " +
                   "inner join Materials on Materials.materialId = ProductsMaterials.materialId";
                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductsMaterial productsMaterial = new ProductsMaterial();
                        productsMaterial.ProductMaterial = reader.GetInt32(0);
                        productsMaterial.MaterialId = reader.GetInt32(1);
                        productsMaterial.ProductId = reader.GetInt32(2);
                        productsMaterial.Quantity = reader.GetDouble(3);
                        productsMaterial.Product = new Product();
                        productsMaterial.Product.ProductId = (int)productsMaterial.ProductId;
                        productsMaterial.Product.ProductName = reader.GetString(4);
                        productsMaterial.Material = new Material();
                        productsMaterial.Material.MaterialId = (int)productsMaterial.MaterialId;
                        productsMaterial.Material.MaterialName = reader.GetString(5);
                        productsMaterials.Add(productsMaterial);
                    }
                }
            }
            return productsMaterials;
        }

        public void Update(ProductsMaterial item)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                connection.Open();
                string sql = "Update ProductsMaterials set materialId = @materialId, productId = @productId, quantity = @quantity where productMaterial = @id";
                SqlCommand command = new(sql, connection);
                SqlParameter idParam = new("@id", item.ProductMaterial);
                command.Parameters.Add(idParam);
                SqlParameter rawId = new("@materialId", item.MaterialId);
                command.Parameters.Add(rawId);
                SqlParameter productId = new("@productId", item.ProductId);
                command.Parameters.Add(productId);
                SqlParameter quanitity = new("@quantity", item.Quantity);
                command.Parameters.Add(quanitity);
                command.ExecuteNonQuery();
            }
        }
    }
}
