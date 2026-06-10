using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ReadScriptFromFile.Data
{
    public sealed class Repository
    {
        private string sqlBaseDir = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Scripts");
        private string connectionString;
        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Execute(Model model)
        {
            string file = Path.Combine(sqlBaseDir, model.ScriptFile);
            string script = File.ReadAllText(file);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = script,
                    CommandType = CommandType.Text
                })
                {
                    if (model.Parameters != null)
                    {
                        model.Parameters.ForEach(fe => cmd.Parameters.Add(
                                                new SqlParameter
                                                {
                                                    ParameterName = fe.ParameterName,
                                                    Value = fe.Value
                                                }));
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                };

                connection.Close();
            }
        }
    }
}
