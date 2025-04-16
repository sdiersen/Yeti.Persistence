using Microsoft.Data.SqlClient;

namespace Persistence.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private static string? s_defaultConnectionString;
        public SqlConnection Connection { get; private set; } = null!; // used to supress CS8618 as compiler can't infer Initialize() is called in the constructor
        public SqlTransaction? Transaction { get; private set; } = null!; // used to supress CS8618 as compiler can't infer Initialize() is called in the constructor
        private bool _disposed = false;

        public UnitOfWork(bool useTransaction = false)
        {
            if (string.IsNullOrEmpty(s_defaultConnectionString))
            {
                throw new InvalidOperationException("Default connection string is not set.");
            }
            Initialize(s_defaultConnectionString, useTransaction);
        }

        public UnitOfWork(string connectionString, bool useTransaction = false)
        {
            Initialize(connectionString, useTransaction);
        }

        private void Initialize(string connectionString, bool useTransaction)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            if (useTransaction)
            {
                Transaction = Connection.BeginTransaction();
            }
        }

        public static void SetDefaultConnectionString(string connectionString)
        {
            s_defaultConnectionString = connectionString;
        }

        public static UnitOfWork Create(bool useTransaction = false)
        {
            return new UnitOfWork(useTransaction);
        }

        public static UnitOfWork Create(string connectionString, bool useTransaction = false)
        {
            return new UnitOfWork(connectionString, useTransaction);
        }

        public void Commit()
        {
            Transaction?.Commit();
            Dispose();
        }

        public void Rollback()
        {
            Transaction?.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Transaction?.Dispose();
                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
