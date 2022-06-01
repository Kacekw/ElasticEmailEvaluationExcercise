using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvMergeFileReader.Model;

namespace CsvMergeFileReader.Converter
{
    public class ToContentTypeConverter : TypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            switch (text.ToLower())
            {
                case "text/html":
                    return DeclaredContentTypeForEmail.HTML;
                case "text/plain":
                    return DeclaredContentTypeForEmail.PlainText;
                default:
                    throw new ArgumentOutOfRangeException(nameof(text));
            }
        }
    }
}
