using System;
namespace Fantasy.BusinessEngine
{
    public interface IConnectionFactory
    {
        void CloseConnection(System.Data.IDbConnection connection);
        System.Data.IDbConnection GetConnection();
    }
}
