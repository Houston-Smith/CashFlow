using Microsoft.Extensions.Configuration;
using CashFlow.Models;
using CashFlow.Utils;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace CashFlow.Repositories
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
       public TransactionRepository(IConfiguration configuration) : base(configuration) { }

       public List<Transaction> GetTransactions()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT t.Id AS TransactionId, r.Ammount, r.Note, r.Date, r.UserProfileId, r.Category,

                    tc.Id AS TransactionCategoryId,
                    
                    c.Id AS CategoryId, c.Name, c.Type

                    FROM Transaction t
                    LEFT JOIN TransactionCategory tc ON t.Id = tc.TransactionId
                    LEFT JOIN Category c ON tc.CategoryId = c.Id
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var transactions = new List<Transaction>();
                        while (reader.Read())
                        {
                            var transactionId = DbUtils.GetInt(reader, "TransactionId");

                            var existingTransaction = transactions.FirstOrDefault(p => p.Id == transactionId);
                            if (existingTransaction == null)
                            {
                                existingTransaction = new Transaction()
                                {
                                    Id = transactionId,
                                    Ammount = DbUtils.GetInt(reader, "Ammount"),
                                    Note = DbUtils.GetString(reader, "Note"),
                                    Date = DbUtils.GetDateTime(reader, "Date"),
                                    UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                    Categories = new List<Category>()
                                };

                                transactions.Add(existingTransaction);
                            }

                            if (DbUtils.IsNotDbNull(reader, "CategoryId"))
                            {
                                existingTransaction.Categories.Add(new Category()
                                {
                                    Id = DbUtils.GetInt(reader, "CategoryId"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Type = DbUtils.GetString(reader, "Type")
                                });
                            }
                        }
                        return transactions;
                    }
                }
            }
        }
    }
}

