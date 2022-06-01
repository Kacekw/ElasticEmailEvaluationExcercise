using CsvHelper.Configuration.Attributes;
using CsvMergeFileReader.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvMergeFileReader.Model
{
    public enum DeclaredContentTypeForEmail
    {
        HTML,
        PlainText
    }

    public class FileModel
    {
        [Name("Sender")]
        public string? Sender { get; set; }
        [Name("Recipients")]
        [TypeConverter(typeof(ToArrayOfStringsConverter))]
        public string[]? Recipients { get; set; }
        [Name("Content-Type")]
        [TypeConverter(typeof(ToContentTypeConverter))]
        public DeclaredContentTypeForEmail ContentType { get; set; }
        [Name("Content")]
        public string? Content { get; set; }
    }
}
