using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Database.SQLServer.ADO
{
    public class Veiculo : IRepository<Models.Veiculo>
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;
        private readonly string keyCache;

        public Veiculo(string connectionString)
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            keyCache = "Veículos";
        }

        public List<Models.Veiculo> get()
        {
            List<Models.Veiculo> veiculos = (List<Models.Veiculo>)Cache.get(keyCache);
            if (veiculos != null) return veiculos;

            veiculos = new List<Models.Veiculo>();

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT Id, Marca, Nome, AnoModelo, DataFabricacao, Valor, Opcionais FROM Veiculos;";
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Models.Veiculo veiculo = new Models.Veiculo();
                        veiculo.Id = (int)dr["Id"];
                        veiculo.Marca = dr["Marca"].ToString();
                        veiculo.Nome = dr["Nome"].ToString();
                        veiculo.AnoModelo = (int)dr["AnoModelo"];
                        veiculo.DataFabricacao = dr["DataFabricacao"] == DBNull.Value ? null : (DateTime?)dr["DataFabricacao"];
                        veiculo.Valor = (decimal)dr["Valor"];
                        veiculo.Opcionais = dr["Opcionais"].ToString();

                        veiculos.Add(veiculo);
                    }
                }
            }
            Cache.add(keyCache, veiculos, 60);
            return veiculos;
        }

        public Models.Veiculo getById(int id)
        {
            List<Models.Veiculo> veiculos = (List<Models.Veiculo>)Cache.get(keyCache);

            if (veiculos != null) return veiculos.Find(veiculoCache => veiculoCache.Id == id);

            Models.Veiculo veiculo = null;

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT Id, Marca, Nome, AnoModelo, DataFabricacao, Valor, Opcionais FROM Veiculos WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        veiculo = new Models.Veiculo();
                        veiculo.Id = id;
                        veiculo.Marca = dr["Marca"].ToString();
                        veiculo.Nome = dr["Nome"].ToString();
                        veiculo.AnoModelo = (int)dr["AnoModelo"];
                        veiculo.DataFabricacao = dr["DataFabricacao"] == DBNull.Value ? null : (DateTime?)dr["DataFabricacao"];
                        veiculo.Valor = (decimal)dr["Valor"];
                        veiculo.Opcionais = dr["Opcionais"].ToString();

                    }
                }
            }
            return veiculo;
        }

        public void add(Models.Veiculo veiculo)
        {
            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO Veiculos (Marca, Nome, AnoModelo, DataFabricacao, Valor, Opcionais) VALUES (@Marca, @Nome, @AnoModelo, @DataFabricacao, @Valor, @Opcionais); SELECT CONVERT (INT, @@IDENTITY) AS Id;";

                    cmd.Parameters.Add(new SqlParameter("@Marca", System.Data.SqlDbType.VarChar)).Value = veiculo.Marca;
                    cmd.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = veiculo.Nome;
                    cmd.Parameters.Add(new SqlParameter("@AnoModelo", System.Data.SqlDbType.Int)).Value = veiculo.AnoModelo;

                    if (veiculo.DataFabricacao != null)
                        cmd.Parameters.Add(new SqlParameter("@DataFabricacao", System.Data.SqlDbType.Date)).Value = veiculo.DataFabricacao;
                    else
                        cmd.Parameters.Add(new SqlParameter("@DataFabricacao", System.Data.SqlDbType.Date)).Value = DBNull.Value;

                    cmd.Parameters.Add(new SqlParameter("@Valor", System.Data.SqlDbType.Decimal)).Value = veiculo.Valor;
                    cmd.Parameters.Add(new SqlParameter("@Opcionais", System.Data.SqlDbType.VarChar)).Value = veiculo.Opcionais;

                    veiculo.Id = (int)cmd.ExecuteScalar();
                }
            }
            Cache.delete(keyCache);
        }

        public int update(int id, Models.Veiculo veiculo)
        {
            int rowsAffected = 0;

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Veiculos SET Marca = @Marca, Nome = @Nome, AnoModelo = @AnoModelo, DataFabricacao = @DataFabricacao, Valor = @Valor, Opcionais = @Opcionais WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter("@Marca", System.Data.SqlDbType.VarChar)).Value = veiculo.Marca;
                    cmd.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar)).Value = veiculo.Nome;
                    cmd.Parameters.Add(new SqlParameter("@AnoModelo", System.Data.SqlDbType.Int)).Value = veiculo.AnoModelo;

                    if (veiculo.DataFabricacao != null)
                        cmd.Parameters.Add(new SqlParameter("@DataFabricacao", System.Data.SqlDbType.Date)).Value = veiculo.DataFabricacao;
                    else
                        cmd.Parameters.Add(new SqlParameter("@DataFabricacao", System.Data.SqlDbType.Date)).Value = DBNull.Value;

                    cmd.Parameters.Add(new SqlParameter("@Valor", System.Data.SqlDbType.Decimal)).Value = veiculo.Valor;
                    cmd.Parameters.Add(new SqlParameter("@Opcionais", System.Data.SqlDbType.VarChar)).Value = veiculo.Opcionais;
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = id;

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            Cache.delete(keyCache);

            return rowsAffected;
        }

        public int delete(int id)
        {
            int rowsAffected = 0;

            using (conn)
            {
                conn.Open();

                using (cmd)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM Veiculos WHERE Id = @Id";
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = id;
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            Cache.delete(keyCache);

            return rowsAffected;
        }
    }
}