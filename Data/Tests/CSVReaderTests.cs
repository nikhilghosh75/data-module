using Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Data.Tests
{
    public class CSVReaderTests
    {
        [Test]
        public void CSVReaderEmpty()
        {
            Assert.Throws<InvalidDataException>(() => CSVReader.ParseCSV(""));
        }

        [Test]
        public void CSVReaderNoCommas()
        {
            Assert.Throws<InvalidDataException>(() => CSVReader.ParseCSV("there are no commas here"));
        }

        [Test]
        public void CSVReaderHeader()
        {
            string testData = "Name,Email\nJoe,joebiden@gmail.com";
            Datatable datatable = CSVReader.ParseCSV(testData);
            
            Assert.AreEqual(datatable.ColumnCount, 2);
            Assert.AreEqual(datatable.columns[0].name, "Name");
            Assert.AreEqual(datatable.columns[1].name, "Email");
        }

        [Test]
        public void CSVReaderColumnType()
        {
            string testData = "Name,Email,Age,Rating\nJoe,joebiden@gmail.com,78,4.2";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 4);
            Assert.AreEqual(datatable.columns[0].columnType, Datatable.ColumnType.STRING);
            Assert.AreEqual(datatable.columns[1].columnType, Datatable.ColumnType.STRING);
            Assert.AreEqual(datatable.columns[2].columnType, Datatable.ColumnType.INT);
            Assert.AreEqual(datatable.columns[3].columnType, Datatable.ColumnType.FLOAT);
        }

        [Test]
        public void CSVReaderSmall()
        {
            string testData = "Name,Email,Age,Rating\nJoe,joebiden@gmail.com,78,4.2\nFranklin,fdr@gmail.com,62,9.3";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 4);
            Assert.AreEqual(datatable.RowCount, 2);
            Assert.AreEqual(datatable.Count, 8);

            Assert.IsNull(datatable.Get(5, 2));
            Assert.IsNull(datatable.Get(1, 7));
            Assert.IsNull(datatable.Get(2, 3));

            Assert.AreEqual((string)datatable.Get(1, 1), "fdr@gmail.com");
            Assert.AreEqual((int)datatable.Get(1, 2), 62);
            Assert.AreEqual((double)((float)datatable.Get(1, 3)), 9.3, 0.01);
        }
        
        [Test]
        public void CSVReaderMedium()
        {
            string header = "Name,Email,Age,Rating\n";
            string topRows = "Joe,joebiden@gmail.com,78,4.2\nFranklin,fdr@gmail.com,62,9.3\nReagan,,84, 3.2\n";
            string midRows = "Barack,bobama@gmail.com,48,7.6\nWilliam,billc@gmail.com,52,6.6,";

            string testData = header + topRows + midRows;
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 4);
            Assert.AreEqual(datatable.RowCount, 5);
            Assert.AreEqual(datatable.Count, 20);
        }

        [Test]
        public void CSVReaderQuote()
        {
            string testData = "Name,Email,Age,Rating\n\"Biden, Joe\",joebiden@gmail.com,78,4.2\nFranklin,fdr@gmail.com,62,9.3";
            Datatable datatable = CSVReader.ParseCSV(testData);

            // Debug.Log(datatable.Get(2, 0));
            //Debug.Log(datatable.Get(0, 1));

            Assert.AreEqual(datatable.ColumnCount, 4);
            Assert.AreEqual(datatable.RowCount, 2);
            Assert.AreEqual(datatable.Count, 8);

            Assert.AreEqual((string)datatable.Get(0, 0), "Biden, Joe");
        }

        [Test]
        public void CSVReaderCountryData()
        {
            string testData = "Country,GDP,Population,\nUnited States,20,330,\nGermany,5,80,\nJapan,5,130,\nIran,1,16,\nChina,19,1400,\nAustrailia,2,40,";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 3);
            Assert.AreEqual(datatable.RowCount, 6);
            Assert.AreEqual(datatable.Count, 18);

            Assert.AreEqual((string)datatable.Get(2, 0), "Japan");
            Assert.AreEqual((int)datatable.Get(4, "Population"), 1400);
        }

        [Test]
        public void CSVReaderAdvancedColumnType()
        {
            string testData = "Temp,Temp 2,Temp 3,\n4,5,,\n10,Nico,Nico";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 3);
            Assert.AreEqual(datatable.RowCount, 2);
            Assert.AreEqual(datatable.columns[1].columnType, Datatable.ColumnType.STRING);
            Assert.AreEqual(datatable.Get(1, 0), 10);
        }

        [Test]
        public void CSVReaderDoubleQuotes()
        {
            string testData = "Temp, Temp 2\n10,\"Hello \"\"Wolvey\"\"\"";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 2);
            Assert.AreEqual(datatable.RowCount, 1);
            Assert.AreEqual(datatable.Get(0, 1), "Hello \"Wolvey\"");
        }

        [Test]
        public void CSVReaderDate()
        {
            string testData = "Name, Date\nNaveen,03-29-1999\nGeorge,03-1999,\nNikhil,08-14-2001,\nYarger,1992";
            Datatable datatable = CSVReader.ParseCSV(testData);

            Assert.AreEqual(datatable.ColumnCount, 2);
            Assert.AreEqual(datatable.RowCount, 4);

            Assert.AreEqual(datatable.columns[0].columnType, Datatable.ColumnType.STRING);
            Assert.AreEqual(datatable.columns[1].columnType, Datatable.ColumnType.DATE);

            Assert.AreEqual((Date)datatable.Get(0, 1), new Date(1999, 03, 29));
            Assert.AreEqual((Date)datatable.Get(1, 1), new Date(1999, 03));
            Assert.AreEqual((Date)datatable.Get(3, 1), new Date(1992));
        }
    }
}
