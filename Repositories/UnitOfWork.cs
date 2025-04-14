using Microsoft.Data.SqlClient;

namespace Persistence.Repositories;
public class UnitOfWork : IDisposable
{
    public SqlConnection Connection { get; private set; }
    public SqlTransaction Transaction { get; private set; }
    private bool _disposed = false;

    public UnitOfWork(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
        Connection.Open();
        Transaction = Connection.BeginTransaction();
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
            Connection?.Close();
            Connection?.Dispose();
            _disposed = true;
        }
    }
}
