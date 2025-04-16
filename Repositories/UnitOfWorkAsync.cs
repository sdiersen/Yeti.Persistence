using Microsoft.Data.SqlClient;

using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWorkAsync : IAsyncDisposable
    {
        private static string? s_defaultConnectionString;
        public SqlConnection Connection { get; private set; } = null!; // used to supress CS8618 as compiler can't infer Initialize() is called in the constructor
        public SqlTransaction? Transaction { get; private set; } = null!; // used to supress CS8618 as compiler can't infer Initialize() is called in the constructor
        private bool _disposed = false;

        public UnitOfWorkAsync(bool useTransaction = false)
        {
            if (string.IsNullOrEmpty(s_defaultConnectionString))
            {
                throw new InvalidOperationException("Default connection string is not set.");
            }
            InitializeAsync(s_defaultConnectionString, useTransaction).GetAwaiter().GetResult();
        }

        public UnitOfWorkAsync(string connectionString, bool useTransaction = false)
        {
            InitializeAsync(connectionString, useTransaction).GetAwaiter().GetResult();
        }

        private async Task InitializeAsync(string connectionString, bool useTransaction)
        {
            Connection = new SqlConnection(connectionString);
            await Connection.OpenAsync();
            if (useTransaction)
            {
                Transaction = (SqlTransaction)(await Connection.BeginTransactionAsync());
            }
        }

        public static void SetDefaultConnectionString(string connectionString)
        {
            s_defaultConnectionString = connectionString;
        }

        public static async Task<UnitOfWorkAsync> CreateAsync(bool useTransaction = false)
        {
            var unitOfWork = new UnitOfWorkAsync();
            await unitOfWork.InitializeAsync(s_defaultConnectionString!, useTransaction);
            return unitOfWork;
        }

        public static async Task<UnitOfWorkAsync> CreateAsync(string connectionString, bool useTransaction = false)
        {
            var unitOfWork = new UnitOfWorkAsync();
            await unitOfWork.InitializeAsync(connectionString, useTransaction);
            return unitOfWork;
        }

        public async Task CommitAsync()
        {
            if (Transaction != null)
            {
                await Transaction.CommitAsync();
            }
            await DisposeAsync();
        }

        public async Task RollbackAsync()
        {
            if (Transaction != null)
            {
                await Transaction.RollbackAsync();
            }
            await DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (Transaction != null)
                {
                    await Transaction.DisposeAsync();
                }
                if (Connection != null)
                {
                    await Connection.CloseAsync();
                    await Connection.DisposeAsync();
                }
                _disposed = true;
            }
        }
    }
}
