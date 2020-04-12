namespace RailwayProgrammingExample.Models.ValueObjects
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserMembership : ValueObject
    {
        private static IEnumerable<int> ValidMemberships
            => Enum
                .GetValues(typeof(Membership))
                .Cast<Membership>()
                .ToArray()
                .Select(m => (int)m);

        private UserMembership(int value)
            => this.Value = value;

        public int Value { get; private set; }

        public static Result<UserMembership> Create(int membershipId)
            => membershipId
                .ToResult()
                .Ensure(membership => ValidMemberships.Contains(membership), "Membership id is invalid.")
                .Map(membership => new UserMembership(membership));

        public static explicit operator UserMembership(int membership)
        {
            return Create(membership).Value;
        }

        public static implicit operator int(UserMembership membership)
        {
            return membership.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
