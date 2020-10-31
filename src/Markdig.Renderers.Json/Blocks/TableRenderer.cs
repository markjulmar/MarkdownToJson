using System;
using System.Globalization;
using System.Linq;
using Markdig.Extensions.Tables;

namespace Markdig.Renderers.Json.Blocks
{
    public class TableRenderer : JsonObjectRenderer<Table>
    {
        protected override void Write(JsonRenderer renderer, Table table)
        {
            renderer.EnsureLine();

            renderer.Write("{ \"type\": \"table\", ");

            bool hasColumnWidth = table.ColumnDefinitions.Any(tableColumnDefinition 
                => tableColumnDefinition.Width != 0.0f && tableColumnDefinition.Width != 1.0f);
            if (hasColumnWidth)
            {
                renderer.Write("\"column-widths\": [");

                for (var index = 0; index < table.ColumnDefinitions.Count; index++)
                {
                    var tableColumnDefinition = table.ColumnDefinitions[index];
                    var width = Math.Round(tableColumnDefinition.Width * 100) / 100;
                    var widthValue = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", width);
                    renderer.Write($"\"{widthValue}%\"");
                    if (index < table.ColumnDefinitions.Count - 1)
                        renderer.Write(", ");
                }

                renderer.Write("], ");
            }

            renderer.WriteLine("\"rows\": [");

            for (var rowIndex = 0; rowIndex < table.Count; rowIndex++)
            {
                if (rowIndex > 0)
                    renderer.WriteLine(",");

                renderer.Write($"{{ \"row-id\": \"{rowIndex+1}\", "); // row-start

                var row = (TableRow) table[rowIndex];
                if (row.IsHeader)
                    renderer.Write("\"isHeader\": \"true\", ");

                renderer.Write("\"columns\": [ ");

                for (int colIndex = 0; colIndex < row.Count; colIndex++)
                {
                    if (colIndex > 0)
                        renderer.WriteLine(",");

                    renderer.EnsureLine();
                    renderer.Write($"{{ \"col-id\": \"{colIndex+1}\", "); // col-start

                    var cell = (TableCell) row[colIndex];
                    if (cell.ColumnSpan != 1)
                        renderer.Write($"\"colspan\": \"{cell.ColumnSpan}\", ");
                    if (cell.RowSpan != 1)
                        renderer.Write($"\"rowspan\": \"{cell.RowSpan}\", ");

                    if (table.ColumnDefinitions.Count > 0)
                    {
                        var columnIndex = cell.ColumnIndex < 0 || cell.ColumnIndex >= table.ColumnDefinitions.Count
                            ? colIndex
                            : cell.ColumnIndex;
                        columnIndex = columnIndex >= table.ColumnDefinitions.Count
                            ? table.ColumnDefinitions.Count - 1
                            : columnIndex;
                        var alignment = table.ColumnDefinitions[columnIndex].Alignment;
                        if (alignment.HasValue)
                        {
                            switch (alignment)
                            {
                                case TableColumnAlign.Left:
                                    renderer.Write("\"align\": \"left\", ");
                                    break;
                                case TableColumnAlign.Center:
                                    renderer.Write("\"align\": \"center\", ");
                                    break;
                                case TableColumnAlign.Right:
                                    renderer.Write("\"align\": \"right\", ");
                                    break;
                            }
                        }
                    }

                    renderer.Write("\"contents\": ");
                    renderer.Write(cell);
                    renderer.Write(" }"); // col-end
                }

                renderer.Write("]}"); // end columns collection, end of row
            }

            renderer.WriteLine("]}"); // end of rows collection, end of table
        }
    }
}