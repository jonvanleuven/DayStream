using System;
using System.Linq;
using NUnit.Framework;

namespace JonVanLeuven.DayStreamTest
{
    [TestFixture]
    public class DayStreamTest
    {
        [Test]
        public void Enumerate()
        {
            var p = new DayStream(new DateTime(2014, 3, 13), new DateTime(2014, 5, 14));

            Assert.AreEqual(63, p.Count());
            Assert.AreEqual(new DateTime(2014, 3, 13), p.First());
            Assert.AreEqual(new DateTime(2014, 5, 14), p.Last());
        }

        [Test]
        public void StreamVersusList()
        {
            var p = new DayStream(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31));
            var stream = p.MoveNextCounter();
            var list = p.MoveNextCounter();

            stream.First();
            list.ToList().First();

            Assert.AreEqual(1, stream.MoveNextCounter);
            Assert.AreEqual(365, list.MoveNextCounter);
        }

        [Test]
        public void Reverse()
        {
            var p = new DayStream(new DateTime(2014, 3, 13), new DateTime(2014, 5, 14)).Reverse();

            Assert.AreEqual(63, p.Count());
            Assert.AreEqual(new DateTime(2014, 5, 14), p.First());
            Assert.AreEqual(new DateTime(2014, 3, 13), p.Last());
        }

        [Test]
        public void LinqQeuries()
        {
            var result = new DayStream(new DateTime(2014, 8, 24), DateTime.MinValue)
                            .Where(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                            .Skip(3);

            Assert.AreEqual(new DateTime(2014, 8, 19), result.First());
        }

        [Test]
        public void WithMaxDate()
        {
            var stream = new DayStream(new DateTime(2010, 1, 1), DateTime.MaxValue);

            Assert.AreEqual(new DateTime(2010, 1, 1), stream.First());
            Assert.AreEqual(new DateTime(2010, 1, 1), stream.ToList().First()); //duurt lang!
        }

        [Test]
        public void ForYear()
        {
            var p = DayStream.ForYear(2014);

            Assert.AreEqual(365, p.Count());
            Assert.AreEqual(new DateTime(2014, 1, 1), p.First());
            Assert.AreEqual(new DateTime(2014, 12, 31), p.Last());
        }

        [Test]
        public void EenDag()
        {
            var dag = new DateTime(2010, 1, 1);
            var p = new DayStream(dag, dag);

            Assert.AreEqual(1, p.Count());
            Assert.AreEqual(new DateTime(2010, 1, 1), p.First());
            Assert.AreEqual(new DateTime(2010, 1, 1), p.Last());
        }

        [Test]
        public void FirstOrDefault()
        {
            var dag = new DateTime(2010, 1, 1);
            var p = new DayStream(dag, dag).FirstOrDefault(d => false);

            Assert.False(p.HasValue);
        }
    }
}
