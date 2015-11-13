using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JonVanLeuven
{
    public class DayStream : IEnumerable<DateTime>
    {
        public DayStream(DateTime start, DateTime end)
        {
            Start = start.Date;
            End = end.Date;
        }

        public static DayStream ForYear(int year)
        {
            return new DayStream(new DateTime(year, 1, 1), new DateTime(year, 12, 31));
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return new DayStreamEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DayStream Reverse()
        {
            return new DayStream(End, Start);
        }

        public class DayStreamEnumerator : IEnumerator<DateTime>
        {
            private readonly DayStream period;
            private readonly int direction;
            private bool isFirst;
            public DayStreamEnumerator(DayStream p)
            {
                period = p;
                direction = p.Start < p.End ? 1 : -1;
                Reset();
            }

            public bool MoveNext()
            {
                if (isFirst)
                {
                    isFirst = false;
                    return true;
                }
                if (direction == 1 && Current >= period.End)
                    return false;
                if (direction == -1 && Current <= period.End)
                    return false;
                Current = Current.AddDays(direction);
                return true;
            }

            public void Reset()
            {
                Current = period.Start;
                isFirst = true;
            }

            public DateTime Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }
        }
    }
}
