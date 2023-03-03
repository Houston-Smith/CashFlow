using Microsoft.Extensions.Configuration;
using CashFlow.Models;
using CashFlow.Utils;
using System.Collections.Generic;

namespace CashFlow.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
       public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

       public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirebaseUserId, Username, FirstName, LastName, Email, CreateDate
                         FROM UserProfile
                         WHERE FirebaseUserId = @FirebaseUserId";

                    DbUtils.AddParameter(cmd, "@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            FirebaseUserId = DbUtils.GetString(reader, "FirebaseUserId"),
                            Username = DbUtils.GetString(reader, "Username"),
                            FirstName = DbUtils.GetString(reader,"FirstName"),
                            LastName = DbUtils.GetString(reader, "LastName"),
                            Email = DbUtils.GetString(reader, "Email"),
                            CreateDate = DbUtils.GetDateTime(reader, "CreateDate"),

                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }
    }
}

