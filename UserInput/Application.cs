using ElasticEmailAPI;
using Microsoft.Extensions.Logging;

namespace UserInput
{
    public class Application
    {
        private readonly ILogger<Application> _logger;
        private readonly EmailService _emailService;
        private readonly UserInputManager _userInputManager;

        public Application(ILogger<Application> logger, EmailService emailService, UserInputManager userInputManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _userInputManager = userInputManager ?? throw new ArgumentNullException(nameof(userInputManager));
        }

        public async Task Run()
        {
            _logger.LogInformation("Application started");

            await SendMail();

            _logger.LogInformation("Application exits");

            await Task.CompletedTask;
        }

        public async Task SendMail()
        {
            var userInput = _userInputManager.GatherUserInput();

            try
            {
                foreach (var item in userInput)
                {
                    _userInputManager.DisplayMessage(new string[]
                        {
                        $"Working on mail from: {item.Sender}"
                        });

                    var emailSendResult = await _emailService.DeclareSenderAdress(item.Sender)
                                                   .AddRecipient(item.Recipients)
                                                   .DeclareContentType(item.ContentType)
                                                   .AddContent(item.Content)
                                                   .SendMail();

                    _userInputManager.DisplayMessage(new string[]
                        {
                        $"Your mail from {item.Sender} - was sent",
                        $"TransactionID: {emailSendResult.TransactionID}"
                        });

                    _logger.LogInformation(emailSendResult.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                _userInputManager.DisplayMessage(new string[]
                    {
                        $"Something went wrong",
                        ex.Message
    });
            }
        }
    }
}
