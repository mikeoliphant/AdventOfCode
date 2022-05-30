using System.Text.Json;

namespace AdventOfCode._2015
{
    internal class Day12 : Day
    {
        long SumNums(JsonElement element)
        {
            long numSum = 0;

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty prop in element.EnumerateObject())
                    {
                        numSum += SumNums(prop.Value);
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (JsonElement child in element.EnumerateArray())
                    {
                        numSum += SumNums(child);
                    }
                    break;
                case JsonValueKind.Number:
                    numSum += element.GetInt64();
                    break;
                default:
                    break;
            }

            return numSum;
        }

        public override long Compute()
        {
            var doc = JsonDocument.Parse(File.ReadAllText(DataFile));

            return SumNums(doc.RootElement);
        }

        long SumNumsNotRed(JsonElement element)
        {
            long numSum = 0;

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty prop in element.EnumerateObject())
                    {
                        if ((prop.Value.ValueKind == JsonValueKind.String) && (prop.Value.GetString() == "red"))
                            return 0;

                        numSum += SumNumsNotRed(prop.Value);
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (JsonElement child in element.EnumerateArray())
                    {
                        numSum += SumNumsNotRed(child);
                    }
                    break;
                case JsonValueKind.Number:
                    numSum += element.GetInt64();
                    break;
                default:
                    break;
            }

            return numSum;
        }

        public override long Compute2()
        {
            var doc = JsonDocument.Parse(File.ReadAllText(DataFile));

            return SumNumsNotRed(doc.RootElement);
        }
    }
}
