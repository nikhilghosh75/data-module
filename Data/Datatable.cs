using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class Datatable
    {
        public enum ColumnType
        {
            NONE,
            STRING,
            FLOAT,
            INT,
            DATETIME,
            DATE
        }

        public class Column
        {
            public string name;
            public ColumnType columnType;
            public IList data;

            public object this[int i] => data[i];
        }

        public string name;
        public List<Column> columns = new List<Column>();

        public void AddColumn(ColumnType columnType, string columnName)
        {
            Column newColumn = new Column();
            newColumn.name = columnName;
            newColumn.columnType = columnType;

            switch (columnType)
            {
                case ColumnType.STRING:
                    newColumn.data = new List<string>();
                    break;
                case ColumnType.FLOAT:
                    newColumn.data = new List<float>();
                    break;
                case ColumnType.INT:
                    newColumn.data = new List<int>();
                    break;
                case ColumnType.DATETIME:
                    newColumn.data = new List<DateTime>();
                    break;
                case ColumnType.DATE:
                    newColumn.data = new List<Date>();
                    break;
            }

            columns.Add(newColumn);
        }

        public void AddColumns(List<ColumnType> columnTypes, List<string> names)
        {
            for(int i = 0; i < columnTypes.Count; i++)
            {
                AddColumn(columnTypes[i], names[i]);
            }
        }

        public void AddData(List<string> data)
        {
            for(int i = 0; i < data.Count; i++)
            {
                int columnIndex = i % columns.Count;
                Column column = columns[columnIndex];
                switch(column.columnType)
                {
                    case ColumnType.STRING:
                        column.data.Add(data[i]);
                        break;
                    case ColumnType.FLOAT:
                        column.data.Add(float.Parse(data[i]));
                        break;
                    case ColumnType.INT:
                        column.data.Add(int.Parse(data[i]));
                        break;
                    case ColumnType.DATETIME:
                        column.data.Add(DateTime.Parse(data[i]));
                        break;
                    case ColumnType.DATE:
                        column.data.Add(Date.Parse(data[i]));
                        break;
                }
            }
        }

        public int ColumnCount { get { return columns.Count; } }

        public int RowCount { get {
                
                return columns.Max(column => column.data.Count); } }

        public int Count { get { return ColumnCount * RowCount; } }

        public object Get(int row, int column)
        {
            if (row >= RowCount || column >= ColumnCount)
                return null;

            return columns[column].data[row];
        }

        public object Get(int row, string columnName)
        {
            if (row >= RowCount)
                return null;

            Column foundColumn = GetColumnByName(columnName);
            if (foundColumn == null)
                return null;

            return foundColumn.data[row];
        }

        public Column GetColumnByName(string columnName)
        {
            for(int i = 0; i < columns.Count; i++)
            {
                if (columnName == columns[i].name)
                    return columns[i];
            }
            return null;
        }
    }
}