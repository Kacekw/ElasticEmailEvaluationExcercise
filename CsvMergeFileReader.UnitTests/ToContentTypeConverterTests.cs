using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvMergeFileReader.Converter;
using CsvMergeFileReader.Model;

namespace CsvMergeFileReader.UnitTests
{
    [TestClass]
    public class ToContentTypeConverterTests
    {
        [TestMethod]
        public void ConvertFromString_HtmlDefiningStringWasProvided_ReturnProperType()
        {
            var contentConverter = new ToContentTypeConverter();
            var stringifiedTypeDeclared = "text/html";

            var convertedToTypeEnum = (DeclaredContentTypeForEmail)contentConverter.ConvertFromString(stringifiedTypeDeclared, null, null);

            Assert.IsNotNull(convertedToTypeEnum);
            Assert.AreEqual(DeclaredContentTypeForEmail.HTML, convertedToTypeEnum);
            Assert.AreNotEqual(DeclaredContentTypeForEmail.PlainText, convertedToTypeEnum);
        }

        [TestMethod]
        public void ConvertFromString_PlainTextDefiningStringWasProvided_ReturnProperType()
        {
            var contentConverter = new ToContentTypeConverter();
            var stringifiedTypeDeclared = "text/plain";

            var convertedToTypeEnum = (DeclaredContentTypeForEmail)contentConverter.ConvertFromString(stringifiedTypeDeclared, null, null);

            Assert.IsNotNull(convertedToTypeEnum);
            Assert.AreEqual(DeclaredContentTypeForEmail.PlainText, convertedToTypeEnum);
            Assert.AreNotEqual(DeclaredContentTypeForEmail.HTML, convertedToTypeEnum);
        }

        [TestMethod]
        public void ConvertFromString_DefiningStringWasProvidedAsIncorrectType_ThrowsException()
        {
            var contentConverter = new ToContentTypeConverter();
            var stringifiedTypeDeclared = "textplain";

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                return (DeclaredContentTypeForEmail)contentConverter.ConvertFromString(stringifiedTypeDeclared, null, null);
            });
        }
    }
}
