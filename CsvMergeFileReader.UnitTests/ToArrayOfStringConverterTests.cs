using CsvMergeFileReader.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CsvMergeFileReader.UnitTests
{
    [TestClass]
    public class ToArrayOfStringConverterTests
    {
        [TestMethod]
        public void ConvertFromString_OneEmailProvided_ReturnsStringOfArrayOfLenghtOne()
        {
            var converter = new ToArrayOfStringsConverter();
            var emailCellToDigest = "abc@domain.com";

            var receipentsArray = converter.ConvertFromString(emailCellToDigest, null, null) as string[];

            Assert.IsNotNull(receipentsArray);
            Assert.AreEqual(emailCellToDigest, receipentsArray[0]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[1]);
        }

        [TestMethod]
        public void ConvertFromString_TwoEmailsProvided_ReturnsStringOfArrayOfLenghtTwo()
        {
            var converter = new ToArrayOfStringsConverter();
            var emailsCellToDigest = "abc@domain.com, cba@domain.com";
            var emailAdressOne = "abc@domain.com";
            var emailAdressTwo = "cba@domain.com";

            var receipentsArray = converter.ConvertFromString(emailsCellToDigest, null, null) as string[];

            Assert.IsNotNull(receipentsArray);
            Assert.AreEqual(emailAdressOne, receipentsArray[0]);
            Assert.AreEqual(emailAdressTwo, receipentsArray[1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[2]);
        }

        [TestMethod]
        public void ConvertFromString_TwoEmailsProvidedWithColonAtTheEnd_ReturnsStringOfArrayOfLenghtTwoIgnoringThirdEmptyString()
        {
            var converter = new ToArrayOfStringsConverter();
            var emailsCellToDigest = "abc@domain.com, cba@domain.com, ";
            var emailAdressOne = "abc@domain.com";
            var emailAdressTwo = "cba@domain.com";

            var receipentsArray = converter.ConvertFromString(emailsCellToDigest, null, null) as string[];

            Assert.IsNotNull(receipentsArray);
            Assert.AreEqual(emailAdressOne, receipentsArray[0]);
            Assert.AreEqual(emailAdressTwo, receipentsArray[1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[2]);
        }

        [TestMethod]
        public void ConvertFromString_TwoEmailsProvidedWithColonAtTheEndAndInTheMiddle_ReturnsStringOfArrayOfLenghtTwoIgnoringEmptyStrings()
        {
            var converter = new ToArrayOfStringsConverter();
            var emailsCellToDigest = "abc@domain.com, , cba@domain.com, ";
            var emailAdressOne = "abc@domain.com";
            var emailAdressTwo = "cba@domain.com";

            var receipentsArray = converter.ConvertFromString(emailsCellToDigest, null, null) as string[];

            Assert.IsNotNull(receipentsArray);
            Assert.AreEqual(emailAdressOne, receipentsArray[0]);
            Assert.AreEqual(emailAdressTwo, receipentsArray[1]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[2]);
        }

        [TestMethod]
        public void ConvertFromString_OnlySpaceProvided_ReturnsEmptyStringArray()
        {
            var converter = new ToArrayOfStringsConverter();
            var emailsCellToDigest = " ";

            var receipentsArray = converter.ConvertFromString(emailsCellToDigest, null, null) as string[];

            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[0]);
        }

        [TestMethod]
        public void ConvertFromString_NullWasPushedThroughByExternalLibrary_ReturnEmptyArray()
        {
            var converter = new ToArrayOfStringsConverter();

            var receipentsArray = converter.ConvertFromString(null, null, null) as string[];

            Assert.ThrowsException<IndexOutOfRangeException>(() => receipentsArray[0]);
        }
    }
}