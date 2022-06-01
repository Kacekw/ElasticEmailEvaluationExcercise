using ElasticEmailAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Threading;

namespace ElasticEmailAPI.UnitTests
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void SendMail_ArtificialDataTriggersSendingOfMail_ReturnTransactionOrMessageId()
        {
            var configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json")
                                        .Build()
                                        .GetSection("ElasticEmailSettings")
                                        .Get<BasicServiceConfigruation>();
            var loggerForElasticMock = new Mock<ILogger<EmailService>>();
            var elasticEmailService = new EmailService(Options.Create<BasicServiceConfigruation>(configuration), loggerForElasticMock.Object);

            var result = elasticEmailService.DeclareSenderAdress("kaclaw+elasticemail@gmail.com")
                .AddRecipient("kaclaw+Tests1@gmail.com")
                .DeclareContentType(EmailContentType.HTML)
                .AddContent("<center><b>That’s just a test message</b><br>A plain html markup formatted text to test the case.</center>")
                .SendMail().Result;

            Thread.Sleep(60 * 1000);
            var emailStatus = elasticEmailService.EmailStatus(result.MessageID).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionID.Length > 10 || result.MessageID.Length > 10);
            Assert.IsTrue(emailStatus.Equals("Sent"));
        }

    }
}
