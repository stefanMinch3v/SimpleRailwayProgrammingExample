namespace RailwayProgrammingExample.Models.ValueObjects
{
    using Common;
    using System.Collections.Generic;

    public class UserName : ValueObject
    {
        private UserName(string value)
            => this.Value = value;

        public string Value { get; private set; }

        public static Result<UserName> Create(Maybe<string> userNameOrNothing)
            => userNameOrNothing
                .ToResult("UserName should not be empty.")
                .OnSuccess(name => name.Trim())
                .Ensure(name => !string.IsNullOrEmpty(name), "UserName should not be empty.")
                .Ensure(name => name.Length <= 20, "UserName is invalid.")
                .Map(name => new UserName(name));

        public static explicit operator UserName(string userName)
            => Create(userName).Value;

        public static implicit operator string(UserName userName)
            => userName.Value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
