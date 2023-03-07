using CashFlow.Models;
using System.Collections.Generic;

namespace CashFlow.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        void Delete(int id);
        List<Transaction> GetAllTransactions();
        Transaction GetTransactionById(int transactionId);
        List<Transaction> GetAllTransactionsByUserId(int userId);
        void Update (Transaction transaction);
    }
}
