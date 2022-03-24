using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Tests
{
    public class DateRangeTest
    {
        [Test]
        public void DateRangeSimple()
        {
            Date startDate = new Date(1950);
            Date endDate = new Date(1960);

            DateRange dateRange = new DateRange(startDate, endDate);

            Assert.True(dateRange.Within(new Date(1955)));
            Assert.True(dateRange.Within(new Date(1950, 5)));
            Assert.True(dateRange.Within(new Date(1950, 1, 5)));
            Assert.True(dateRange.Within(new Date(1959, 10)));
            Assert.True(dateRange.Within(new Date(1959, 12, 29)));
        }
    }
}