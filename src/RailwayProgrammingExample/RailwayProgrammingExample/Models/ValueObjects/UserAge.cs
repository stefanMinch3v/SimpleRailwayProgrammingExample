namespace RailwayProgrammingExample.Models.ValueObjects
{
    using Common;
    using System.Collections.Generic;

    public class UserAge : ValueObject
    {
        private UserAge(int value)
            => this.Value = value;

        public int Value { get; private set; }

        public static Result<UserAge> Create(int userAgeOrZero)
            => userAgeOrZero
                .ToResult()
                .Ensure(age => age <= 130 && age >= 1, "User age is invalid.")
                .Map(name => new UserAge(name));

        public static explicit operator UserAge(int userAge)
        {
            return Create(userAge).Value;
        }

        public static implicit operator int(UserAge userAge)
        {
            return userAge.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
