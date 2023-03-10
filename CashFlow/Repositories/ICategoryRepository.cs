using CashFlow.Models;
using System.Collections.Generic;

namespace CashFlow.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        List<Category> GetCategoriesByTransactionId();
        void AddTransactionCategories(List<TransactionCategory> transactionCategories);
        void DeleteTransactionCategoriesByTransactionId(int transactionId);
    }
}
