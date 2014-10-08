using System.Collections;
using System.Collections.Generic;

namespace JonVanLeuven.DayStreamTest
{
    /// <summary>
    /// Class om bij te houden hoe vaak een MoveNext wordt aangeroepen op een IEnumerable class.
    /// Zie MoveNextCounter property om het aantal aanroepen van MoveNext uit te vragen.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MoveNextWrapper<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> delegateEnumerable;

        public static MoveNextWrapper<T> Wrap(IEnumerable<T> delegateEnumerable)
        {
            return new MoveNextWrapper<T>(delegateEnumerable);
        }

        private MoveNextWrapper(IEnumerable<T> delegateEnumerable)
        {
            MoveNextCounter = 0;
            this.delegateEnumerable = delegateEnumerable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new MoveNextCounterEnumerator<T>(delegateEnumerable.GetEnumerator(), this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int MoveNextCounter { get; private set; }

        private void IncreaseMoveNextCounter()
        {
            MoveNextCounter++;
        }

        public class MoveNextCounterEnumerator<T> : IEnumerator<T>
        {
            private readonly IEnumerator<T> delegateEnumerator;
            private readonly MoveNextWrapper<T> delegateEnumerable;
            public MoveNextCounterEnumerator(IEnumerator<T> delegateEnumerator, MoveNextWrapper<T> delegateEnumerable)
            {
                this.delegateEnumerator = delegateEnumerator;
                this.delegateEnumerable = delegateEnumerable;
            }

            public void Dispose()
            {
                delegateEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                var next = delegateEnumerator.MoveNext();
                if (next)
                    delegateEnumerable.IncreaseMoveNextCounter();
                return next;
            }

            public void Reset()
            {
                delegateEnumerator.Reset();
            }

            public T Current { get { return delegateEnumerator.Current; } }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }

    public static class MoveNextWrapperExtensions
    {
        public static MoveNextWrapper<T> MoveNextCounter<T>(this IEnumerable<T> source)
        {
            return MoveNextWrapper<T>.Wrap(source);
        }
    }
}
