using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Data
{
    public class CSVReader
    {
        public static Datatable ParseCSV(string str)
        {
            // Check if the file is not long enough for data
            if (str.Length < 3)
                throw new InvalidDataException("CSV file data is too short");

            if (!CheckCSVStr(str))
                return new Datatable();

            List<string> columnNames = new List<string>();

            // The first line will always be the column names, split by commas
            StringReader stringReader = new StringReader(str);
            string firstLine = stringReader.ReadLine();
            columnNames.AddRange(firstLine.Split(','));
            columnNames = FixColumnNames(columnNames);

            // Get data from the rest of the file
            string dataStr = stringReader.ReadToEnd();
            List<string> data = GetData(dataStr);
            data = FixData(data);

            // Get the datatype of each column
            List<Datatable.ColumnType> columnTypes = GetColumnTypes(data, columnNames.Count);

            // Create datatable from the data we have
            Datatable datatable = new Datatable();
            datatable.AddColumns(columnTypes, columnNames);
            datatable.AddData(data);

            return datatable;
        }

        static List<string> GetData(string dataStr)
        {
            // Remove the last character if it's special
            if(dataStr[dataStr.Length - 1] == ',' || dataStr[dataStr.Length - 1] == '\n' || dataStr[dataStr.Length - 1] == '\r')
            {
                dataStr.Remove(dataStr.Length - 1, 1);
            }

            List<string> data = new List<string>();

            // Parse each individual cell and add it to the data
            int currentPosition = 0;
            while(currentPosition < dataStr.Length)
            {
                string cell = ParseCell(dataStr, ref currentPosition);
                data.Add(cell);
                currentPosition++;

                // If the current character is a new line, go to the next character
                if (currentPosition < dataStr.Length && (dataStr[currentPosition] == '\n'
                    || dataStr[currentPosition] == '\r'))
                    currentPosition++;
            }

            return data;
        }

        // Currently does nothing, but could in the future
        static List<string> FixData(List<string> data)
        {
            // Fix the white squares
            string cmpStr = "";
            cmpStr += ((char)239) + ((char)191) + ((char)189);
            for (int i = 0; i < data.Count; i++)
            {
                data[i] = data[i].Replace(cmpStr, "\"");
            }

            return data;
        }

        static List<string> FixColumnNames(List<string> data)
        {
            // Sometimes the last column of the first row will have an extra comma
            // If the last one is empty, remove it
            if (data[data.Count - 1].Length == 0)
                data.RemoveAt(data.Count - 1);

            return data;
        }

        static string ParseCell(string dataStr, ref int position)
        {
            if (position >= dataStr.Length)
            {
                return "";
            }

            bool quoted = dataStr[position] == '\'' || dataStr[position] == '"';
            bool singleQuoted = false;
            if (quoted)
            {
                position++;
                if (dataStr[position] == '\'')
                    singleQuoted = true;
            }

            string cell = "";
            while (position < dataStr.Length)
            {
                char c = dataStr[position];
                if (quoted)
                {
                    if (singleQuoted)
                    {
                        if (dataStr[position] == '\'')
                        {
                            if (position + 1 < dataStr.Length && dataStr[position + 1] == '\'')
                                position++;
                            else
                                break;
                        }
                    }
                    else
                    {
                        if (dataStr[position] == '"')
                        {
                            // Check if it's a double quote situation, otherwise break
                            if (position + 1 < dataStr.Length && dataStr[position + 1] == '"')
                                position++;
                            else
                                break;
                        }
                    }
                }
                else
                {
                    if (c == '\n' || c == '\r' || c == ',') 
                        break;
                }

                cell += c;
                position++;
            }

            if (quoted)
                position++;

            return cell;
        }

        static List<Datatable.ColumnType> GetColumnTypes(List<string> data, int numColumns)
        {
            int numRows = data.Count / numColumns;

            List<Datatable.ColumnType> columnTypes = new List<Datatable.ColumnType>();
            for(int i = 0; i < numColumns; i++)
            {
                List<Datatable.ColumnType> typesOfColumn = GetTypesOfColumn(data, numRows, numColumns, i);
                if(typesOfColumn.Any(x => x == Datatable.ColumnType.STRING))
                    columnTypes.Add(Datatable.ColumnType.STRING);
                else if (typesOfColumn.All(x => x == Datatable.ColumnType.DATE))
                    columnTypes.Add(Datatable.ColumnType.DATE);
                else if (typesOfColumn.Any(x => x == Datatable.ColumnType.FLOAT))
                    columnTypes.Add(Datatable.ColumnType.FLOAT);
                else
                    columnTypes.Add(Datatable.ColumnType.INT);
            }

            return columnTypes;
        }

        static List<Datatable.ColumnType> GetTypesOfColumn(List<string> data, int numRows, int numColumns, int column)
        {
            List<Datatable.ColumnType> columnTypes = new List<Datatable.ColumnType>();

            for(int i = 0; i < numRows; i++)
            {
                string currentData = data[i * numColumns + column];

                bool containsAlpha = currentData.Any(x => char.IsLetter(x));
                bool containsPeriod = currentData.Any(x => x == '.');
                if (Date.IsValidDate(currentData))
                    columnTypes.Add(Datatable.ColumnType.DATE);
                else if (containsAlpha)
                    columnTypes.Add(Datatable.ColumnType.STRING);
                else if (containsPeriod)
                    columnTypes.Add(Datatable.ColumnType.FLOAT);
                else
                    columnTypes.Add(Datatable.ColumnType.INT);
            }

            return columnTypes;
        }

        static bool CheckCSVStr(string csv)
        {
            // Check to ensure there are commas
            if (!csv.Contains(","))
            {
                throw new InvalidDataException("CSV file contains no commas");
            }
            // Check to ensure there is a new line (requires headers)
            else if (!csv.Contains("\n") && !csv.Contains("\r"))
            {
                throw new InvalidDataException("CSV file contains no new lines (and therefore no headers)");
            }

            return true;
        }
    }
}