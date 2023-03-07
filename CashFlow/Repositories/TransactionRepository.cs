using Microsoft.Extensions.Configuration;
using CashFlow.Models;
using CashFlow.Utils;
using System.Collections.Generic;

namespace CashFlow.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
       public TransactionRepository(IConfiguration configuration) : base(configuration) { }

       
    }
}

