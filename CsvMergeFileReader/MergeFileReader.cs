using CsvHelper;
using CsvMergeFileReader.Model;
using System.Globalization;

namespace CsvMergeFileReader
{
    public class MergeFileReader
    {
        public static List<FileModel> Read()
        {
            using var reader = new StreamReader("PreconfiguredEmailsData.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<FileModel>();
            return records.ToList();
        }
    }
}