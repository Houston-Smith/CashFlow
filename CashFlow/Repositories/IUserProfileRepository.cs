using GravyTrain.Models;
using System.Collections.Generic;

namespace GravyTrain.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile userProfile);
        void UpdateUserProfile(UserProfile userProfile);
        UserProfile GetByFirebaseUserId(string firebaseUserId);
        List<UserProfile> GetAllUsers();
        UserProfile GetByUserId(int userId);
    }
}
