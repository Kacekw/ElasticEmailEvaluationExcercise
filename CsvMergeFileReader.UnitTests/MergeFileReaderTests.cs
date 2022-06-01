using CsvMergeFileReader.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvMergeFileReader.UnitTests
{
    [TestClass]
    public class MergeFileReaderTests
    {
        [TestMethod]
        public void Read_ReadingPredefinedFile_ListOfObjectsMatches()
        {
            List<FileModel> fileData = new List<FileModel>
            {
                new FileModel
                {
                    Sender = "kaclaw+elasticemail@gmail.com",
                    Recipients = new string[] { "kaclaw+Tests1@gmail.com", "kaclaw+Tests2@gmail.com" },
                    ContentType = DeclaredContentTypeForEmail.HTML,
                    Content = "<center><b>That’s just a test message</b><br>A plain html markup formatted text to test the case.</center>"
                },
                new FileModel
                {
                    Sender = "kaclaw+elasticemail@gmail.com",
                    Recipients = new string[] { "kaclaw+Tests3@gmail.com", "kaclaw+elasticemailTests4@gmail.com" },
                    ContentType = DeclaredContentTypeForEmail.PlainText,
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                    "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                    "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                    "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                }
            };

            var objectsList = MergeFileReader.Read();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(fileData[0].Sender, objectsList[0].Sender);
            Assert.AreEqual(fileData[0].Recipients[0], objectsList[0].Recipients[0]);
            Assert.AreEqual(fileData[0].Recipients[1], objectsList[0].Recipients[1]);
            Assert.AreEqual(fileData[0].ContentType, objectsList[0].ContentType);
            Assert.AreEqual(fileData[0].Content, objectsList[0].Content);

            Assert.AreEqual(fileData[1].Sender, objectsList[1].Sender);
            Assert.AreEqual(fileData[1].Recipients[0], objectsList[1].Recipients[0]);
            Assert.AreEqual(fileData[1].Recipients[1], objectsList[1].Recipients[1]);
            Assert.AreEqual(fileData[1].ContentType, objectsList[1].ContentType);
            Assert.AreEqual(fileData[1].Content, objectsList[1].Content);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
    }
