using System;

namespace Domain.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        void Commit();
    }
}