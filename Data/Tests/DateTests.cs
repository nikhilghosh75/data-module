using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Data.Tests
{
    public class DateTests
    {
        [Test]
        public void DateSimple()
        {
            Date yearDate = new Date(1951);
            Date monthDate = new Date(1965, 2);
            Date fullDate = new Date(1979, 4, 20);

            Assert.AreEqual(1951, yearDate.year);

            Assert.AreEqual(1965, monthDate.year);
            Assert.AreEqual(2, monthDate.month);

            Assert.AreEqual(1979, fullDate.year);
            Assert.AreEqual(4, fullDate.month);
            Assert.AreEqual(20, fullDate.day);
        }

        [Test]
        public void DateOperators()
        {
            Date lessDate = new Date(1982, 8, 13);
            Date greaterDate = new Date(1983, 1, 6);

            Assert.True(lessDate < greaterDate);
            Assert.False(greaterDate < lessDate);

            Assert.True(greaterDate > lessDate);
            Assert.False(lessDate > greaterDate);
        }

        [Test]
        public void DateOperatorComplex()
        {
            Date startDate = new Date(1982, 8, 13);
            Date dayDate = new Date(1982, 8, 16);
            Date monthDate = new Date(1982, 9, 9);
            Date yearDate = new Date(1984, 4, 25);

            Assert.True(startDate < dayDate);
            Assert.True(dayDate > startDate);

            Assert.True(startDate < monthDate);
            Assert.True(monthDate > startDate);

            Assert.True(startDate < yearDate);
            Assert.True(yearDate > startDate);
        }

        [Test]
        public void DateToString()
        {
            Date date = new Date(1995, 6, 24);

            Assert.AreEqual(date.ToString(), "June 24, 1995");
        }

        [Test]
        public void DateParse()
        {
            Date date = new Date(1995, 6, 24);

            Assert.AreEqual(date, Date.Parse("6/24/1995"));
            Assert.AreEqual(date, Date.Parse("06/24/1995"));
            Assert.AreEqual(date, Date.Parse("6-24-1995"));
            Assert.AreEqual(date, Date.Parse("06-24-1995"));
        }
    }
}
