namespace RailwayProgrammingExample.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public struct Maybe<T> : IEquatable<Maybe<T>>
        where T : class
    {
        private readonly T value;

        private Maybe([AllowNull]T value)
            => this.value = value;

        public T Value
        {
            get
            {
                if (this.HasNoValue)
                {
                    throw new InvalidOperationException("Cannot set value to null.");
                }

                return this.value;
            }
        }

        public bool HasValue => this.value != null;

        public bool HasNoValue => !this.HasValue;

        public static implicit operator Maybe<T>([AllowNull]T value)
            => new Maybe<T>(value);

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            if (maybe.HasNoValue)
            {
                return false;
            }

            return maybe.value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
            {
                return false;
            }

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            if (this.HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (this.HasNoValue || other.HasNoValue)
            {
                return false;
            }

            return this.value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override string ToString()
        {
            if (this.HasNoValue)
            {
                return "No value";
            }

            return this.Value.ToString();
        }

        public T Unwrap([AllowNull]T defaultValue = default)
        {
            if (this.HasNoValue)
            {
                return defaultValue;
            }

            return this.Value;
        }
    }
}
