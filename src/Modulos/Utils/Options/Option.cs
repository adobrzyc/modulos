using System;
using System.Collections.Generic;
using System.Threading;
// ReSharper disable UnusedType.Global

namespace Modulos
{
    public abstract class OptionBase<T> : IOption<T>, IEquatable<OptionBase<T>>, IDisposable
    {
        private readonly ReaderWriterLockSlim locker;
        private T value;
        private readonly bool threadSafe;

        protected OptionBase(bool threadSafe)
        {
            this.threadSafe = threadSafe;
            if (threadSafe)
                locker = new ReaderWriterLockSlim();

            Initialize(); //ye...i know
        }

        public virtual string FriendlyName => GetType().Name;

        public T Value
        {
            get
            {
                if (!threadSafe)
                    return value;

                locker.EnterReadLock();
                try
                {
                    return value;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            set
            {
                if (!threadSafe)
                {
                    this.value = value;
                    return;
                }

                locker.EnterWriteLock();
                try
                {
                    this.value = value;
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }

        public void Initialize()
        {
            Value = GetDefaultValue();
        }

        public virtual T GetDefaultValue()
        {
            return default;
        }

        public void Dispose()
        {
            if (threadSafe)
                locker.Dispose();
        }


        public bool Equals(OptionBase<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OptionBase<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static bool operator ==(OptionBase<T> left, OptionBase<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OptionBase<T> left, OptionBase<T> right)
        {
            return !Equals(left, right);
        }
    }
}