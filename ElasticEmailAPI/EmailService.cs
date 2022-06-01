using ElasticEmail;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using ElasticEmailAPI.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElasticEmailAPI
{
    public enum EmailContentType
    {
        HTML,
        Plain
    }

    public class EmailService
    {
        private readonly BasicServiceConfigruation _basicServiceConfigruation;

        private readonly EmailsApi _emailsApi;
        private readonly ILogger<EmailService> _logger;
        private List<BodyPart> _bodyPartContent = new();
        private List<EmailRecipient> _recipients = new();
        private BodyContentType _contentType;
        private string? _sender;

        public EmailService(IOptions<BasicServiceConfigruation> configruation, ILogger<EmailService> logger)
        {
            _basicServiceConfigruation = configruation?.Value ?? throw new ArgumentNullException(nameof(configruation));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Configuration config = new Configuration();
            config.BasePath = _basicServiceConfigruation.ElasticEmailBasePath;
            config.AddApiKey(_basicServiceConfigruation.ApiKeyHeaderParameterName, _basicServiceConfigruation.ApiKey);

            _emailsApi = new EmailsApi(config);
        }

        public async Task<EmailSendResult> SendMail()
        {
            var content = GenerateEmailContent();
            var emailMessageData = new EmailMessageData(_recipients, content);

            try
            {
                var emailSend = await _emailsApi.EmailsPostAsync(emailMessageData);
                return new EmailSendResult(emailSend.TransactionID, emailSend.MessageID);
            }
            catch (ApiException ae)
            {
                _logger.LogError(ae.Message);
                throw;
            }
            finally
            {
                Flush();
            }
        }

        public async Task<string> EmailStatus(string msgId)
        {
            try
            {
                var emailSend = await _emailsApi.EmailsByMsgidViewGetAsync(msgId);
                
                return emailSend.Status.Status.ToString() ?? string.Empty;
            }
            catch (ApiException ae)
            {
                _logger.LogError(ae.Message);
                throw;
            }
        }

        private void Flush()
        {
            _bodyPartContent.Clear();
            _recipients.Clear();

            _sender = string.Empty;
        }

        private EmailContent GenerateEmailContent()
        {
            return new EmailContent(_bodyPartContent, from: _sender);
        }

        public EmailService AddRecipient(string emailAdress)
        {
            if (string.IsNullOrEmpty(emailAdress)) throw new ArgumentOutOfRangeException(nameof(emailAdress));

            _recipients.Add(new EmailRecipient(emailAdress));

            return this;
        }

        public EmailService AddRecipient(string[] emailAdresses)
        {
            foreach (var emailAdress in emailAdresses)
            {
                AddRecipient(emailAdress);
            }

            return this;
        }

        public EmailService DeclareSenderAdress(string emailAdress)
        {
            _sender = emailAdress ?? throw new ArgumentNullException(nameof(emailAdress));

            return this;
        }

        public EmailService DeclareContentType(EmailContentType contentType)
        {
            switch (contentType)
            {
                case EmailContentType.HTML:
                    DeclareContentType(BodyContentType.HTML);
                    break;
                case EmailContentType.Plain:
                    DeclareContentType(BodyContentType.PlainText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_contentType));
            }

            return this;
        }

        private EmailService DeclareContentType(BodyContentType contentType)
        {
            _contentType = contentType;

            return this;
        }

        public EmailService AddContent(string content)
        {
            _bodyPartContent.Add(new BodyPart(_contentType, content));

            return this;
        }


    }
}