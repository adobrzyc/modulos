using System;
using System.Collections.Generic;
using System.Threading;
// ReSharper disable UnusedType.Global

namespace Modulos
{
    public abstract class OptionBase<T> : IOption<T>, IEquatable<OptionBase<T>>, IDisposable
    {
        private readonly ReaderWriterLockSlim _locker;
        private T _value;
        private readonly bool _threadSafe;

        protected OptionBase(bool threadSafe)
        {
            _threadSafe = threadSafe;
            if (threadSafe)
                _locker = new ReaderWriterLockSlim();

            Initialize(); //ye...i know
        }

        public virtual string FriendlyName => GetType().Name;

        public T Value
        {
            get
            {
                if (!_threadSafe)
                    return _value;

                _locker.EnterReadLock();
                try
                {
                    return _value;
                }
                finally
                {
                    _locker.ExitReadLock();
                }
            }
            set
            {
                if (!_threadSafe)
                {
                    _value = value;
                    return;
                }

                _locker.EnterWriteLock();
                try
                {
                    _value = value;
                }
                finally
                {
                    _locker.ExitWriteLock();
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
            if (_threadSafe)
                _locker.Dispose();
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