namespace RailwayProgrammingExample.Common
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class Database
    {
        private static int id = 0;
        private readonly static IDictionary<int, User> fakeUsersDb = new Dictionary<int, User>();

        public static int CreateUser(string userName, int age, int membership)
        {
            id++;

            fakeUsersDb.Add(id, new User 
            { 
                Id = id, 
                Age = age,
                Name = userName, 
                Membership = (Membership)membership 
            });

            return id;
        }

        public static IEnumerable<Maybe<User>> GetAllUsers()
            => fakeUsersDb.Select(kvp => (Maybe<User>)kvp.Value);

        public static Maybe<User> GetUser(int id)
            => GetAllUsers().FirstOrDefault(u => u.HasValue && u.Value.Id == id);

        public static Result DeleteUser(int id)
        {
            if (!fakeUsersDb.ContainsKey(id))
            {
                return Result.Fail($"User does not exist. {id}");
            }

            fakeUsersDb.Remove(id);

            return Result.Ok();
        }

        public static Result PromoteUser(int id)
        {
            if (!fakeUsersDb.ContainsKey(id))
            {
                return Result.Fail($"User does not exist. {id}");
            }

            var user = fakeUsersDb[id];

            if (!user.CanChangeMembership)
            {
                return Result.Fail("User has already the highest membership.");
            }

            user.Membership = user.Membership switch
            {
                Membership.Basic => Membership.Extended,
                Membership.Extended => Membership.Gold,
                Membership.Gold => Membership.Diamond,
                Membership.Diamond => Membership.Enterprise,
                _ => throw new System.InvalidOperationException()
            };

            return Result.Ok();
        }
    }
}
