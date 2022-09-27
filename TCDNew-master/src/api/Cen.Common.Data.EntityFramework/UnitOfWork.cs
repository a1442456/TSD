using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Cen.Common.Data.EntityFramework
{
    public class UnitOfWork<T>: IDisposable where T: DbContext
    {
        public DbConnection Connection => _dbConnection;
        public DbTransaction Transaction => _dbTransaction;
        public T Context => _dbContext;

        private readonly DbConnection _dbConnection;
        private DbTransaction _dbTransaction;
        private readonly T _dbContext;

        public UnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.AutoTransactionsEnabled = false;
            
            _dbConnection = dbContext.Database.GetDbConnection();
            
            Start();
        }

        public void Start()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            _dbTransaction = _dbConnection.BeginTransaction();
        }
        
        public void Commit()
        {
            _dbTransaction.Commit();
            _dbTransaction.Dispose();
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
            _dbTransaction.Dispose();
        }
        
        private void ReleaseUnmanagedResources()
        {
            if (_dbConnection != null)
            {
                _dbTransaction?.Dispose();

                if (_dbConnection.State != ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
                
                _dbConnection.Dispose();
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            ReleaseUnmanagedResources();
        }
    }
}