
using System;

namespace NetBaires.Events.DomainEvents
{
    public abstract class DomainEvents<TDomainEvent> : IEquatable<TDomainEvent>, IDomainEvents
    {
        protected bool Equals(DomainEvents<TDomainEvent> other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return GetHashCode().Equals(other.GetHashCode());
        }

        public bool Equals(TDomainEvent other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DomainEvents<TDomainEvent>) obj);
        }

        public abstract override int GetHashCode();
    }

    public interface IDomainEvents { }
}