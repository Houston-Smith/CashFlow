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

       public Transaction GetTransactionById(int transactionId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT t.Id AS TransactionId, t.Ammount, t.Note, t.Date, t.UserProfileId, t.Categories
                    tc.Id AS TransactionCategoryId,
                    c.Id AS CategoryId, c.Name, C.Type
                    FROM Transaction t
                    LEFT JOIN TransactionCategory tc ON t.Id = tc.TransactionId
                    LEFT JOIN Category c ON tc.CategoryId = c.Id
                    WHERE t.Id = @Id
                    ";
                    
                    cmd.Parameters.AddWithValue("@Id", transactionId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Transaction transaction = null;
                        if (reader.Read())
                        {
                            transaction = new Transaction()
                            {
                                Id = DbUtils.GetInt(reader, "TransactionId"),
                                Ammount = DbUtils.GetInt(reader, "Ammount"),
                                Note = DbUtils.GetString(reader, "Note"),
                                Date = DbUtils.GetDateTime(reader, "Date"),
                                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                Categories = new List<Category>()
                            };

                            do
                            {
                                if (DbUtils.IsNotDbNull(reader, "TransactionId"))
                                {
                                    transaction.Categories.Add(new Category()
                                    {
                                        Id = DbUtils.GetInt(reader, "CategoryId"),
                                        Name = DbUtils.GetString(reader, "Name"),
                                        Type = DbUtils.GetString(reader, "Type"),
                                    });
                                }
                            } while (reader.Read());

                            return transaction;
                        }

                        else { return null; }
                    }
                }
            }
        }

       public List<Transaction> GetTransactionsByUserId(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT t.Id AS TransactionId, t.Ammount, t.Note, t.Date, t.UserProfileId, t.Category,
                        tc.Id AS TransactionCategoryId,
                        c.Id AS CategoryId, c.Name, c.Type

                        FROM Transaction t
                        LEFT JOIN TransactionCategory tc ON t.Id = tc.TransactionId
                        LEFT JOIN Category c ON tc.CategoryId = c.Id
                        WHERE UserProfileId = @UserProfileId
                        ";

                    cmd.Parameters.AddWithValue("@UserProfileId", userId);
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
                            if (DbUtils.IsNotDbNull(reader, "TransactionId"))
                            {
                                existingTransaction.Categories.Add(new Category()
                                {
                                    Id = DbUtils.GetInt(reader, "CategoryId"),
                                    Name = DbUtils.GetString(reader, "Name"),
                                    Type = DbUtils.GetString(reader, "Type"),
                                });
                            }
                        }

                        return transactions;
                    }
                }
            }
        }

        public void Add(Transaction transaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSTERT INTO Transaction (Ammount, Note, Date, UserProfileId)
                    OUTPUT INSERTED.ID
                    VALUES (@Ammount, @Note, @Date, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Ammount", transaction.Ammount);
                    DbUtils.AddParameter(cmd, "@Note", transaction.Note);
                    DbUtils.AddParameter(cmd, "@Date", transaction.Date);
                    DbUtils.AddParameter(cmd, "@UserProfileId", transaction.UserProfileId);

                    transaction.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}

