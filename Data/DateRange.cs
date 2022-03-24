using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public struct DateRange
    {
        public Date startDate;
        public Date endDate;

        public DateRange(Date newStartDate, Date newEndDate)
        {
            startDate = newStartDate;
            endDate = newEndDate;
        }

        public DateRange(int startYear, int endYear)
        {
            startDate = new Date(startYear);
            endDate = new Date(endYear);
        }

        public bool Within(Date date)
        {
            return date >= startDate && date <= endDate;
        }
    }
}
