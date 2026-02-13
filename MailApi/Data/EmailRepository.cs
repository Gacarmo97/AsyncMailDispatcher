using Dapper;
using MailApi.Domain.Models;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MailApi.Repositories
{
    public class EmailRepository
    {
        private readonly string _connectionString;

        public EmailRepository(IConfiguration configuration)
        {
            // Pega a string de conexão do appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            // Tenta criar a tabela toda vez que a classe é instanciada (se não existir)
            CriarTabelaSeNaoExistir();
        }

        // --- AQUI ESTÁ A CRIAÇÃO DA TABELA ---
        private void CriarTabelaSeNaoExistir()
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                    CREATE TABLE IF NOT EXISTS Emails (
                        Id TEXT PRIMARY KEY,
                        EnderecoEmail TEXT NOT NULL,
                        Assunto TEXT,
                        Conteudo TEXT,
                        StatusEnviado INTEGER DEFAULT 0
                    )";

                connection.Execute(sql);
            }
        }

        // Método auxiliar para abrir conexão
        private IDbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        // 1. SALVAR
        public async Task SalvarEmailAsync(Email email)
        {
            using (var connection = GetConnection())
            {
                var sql = @"
                    INSERT INTO Emails (Id, EnderecoEmail, Assunto, Conteudo, StatusEnviado) 
                    VALUES (@Id, @EnderecoEmail, @Assunto, @Conteudo, @StatusEnviado)";

                await connection.ExecuteAsync(sql, email);
            }
        }

        // 2. BUSCAR TODOS
        public async Task<IEnumerable<Email>> BuscarTodosAsync()
        {
            using (var connection = GetConnection())
            {
                var sql = "SELECT * FROM Emails";
                return await connection.QueryAsync<Email>(sql);
            }
        }

        // 3. ATUALIZAR STATUS
        public async Task<bool> AtualizarStatusAsync(string id, bool status)
        {
            using (var connection = GetConnection())
            {
                var sql = "UPDATE Emails SET StatusEnviado = @Status WHERE Id = @Id";
                var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id, Status = status ? 1 : 0 });
                return linhasAfetadas > 0;
            }
        }

        // 4. DELETAR
        public async Task<bool> DeletarAsync(string id)
        {
            using (var connection = GetConnection())
            {
                var sql = "DELETE FROM Emails WHERE Id = @Id";
                var linhasAfetadas = await connection.ExecuteAsync(sql, new { Id = id });
                return linhasAfetadas > 0;
            }
        }
    }
}