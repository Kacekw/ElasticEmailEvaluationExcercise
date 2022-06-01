using CsvMergeFileReader;
using CsvMergeFileReader.Model;
using ElasticEmailAPI;
using Microsoft.Extensions.Logging;
using UserInput.ConsolePrompter;
using UserInput.Model;

namespace UserInput
{
    internal enum LoadingDataScheme
    {
        Manually,
        UsingCSV
    }

    public class UserInputManager
    {
        private readonly ILogger<UserInputManager> _logger;
        private readonly IConsoleManager _console;

        public UserInputManager(ILogger<UserInputManager> logger, IConsoleManager console)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public List<UserInputData> GatherUserInput()
        {
            List<UserInputData> emailData = new();

            do
            {
                var processingDataScheme = PromptUserForProcessingType();

                if (processingDataScheme == LoadingDataScheme.Manually)
                {
                    emailData.AddRange(GetUserInputManually());
                }
                else
                {
                    emailData.AddRange(GetUserDataFromFile());
                }

            } while (PromptUserIfWantsToContinue());

            return emailData;
        }

        private List<UserInputData> GetUserInputManually()
        {
            List<UserInputData> userInputs = new List<UserInputData>();

            var inputData = new UserInputData();

            inputData.Sender = PromptUserForSenderAddress();
            inputData.Recipients = PromptUserForRecipients();
            inputData.ContentType = PromptUserForContentType();
            inputData.Content = PromptUserForActualContent();

            userInputs.Add(inputData);

            return userInputs;
        }

        private bool PromptUserIfWantsToContinue()
        {
            var simpleInstruction = "Do you have more emails to send? y/n";
            var contentTypePromptInput = PromptUserForInput(simpleInstruction);

            switch (contentTypePromptInput.ToLower())
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    _logger.LogWarning($"User input ({contentTypePromptInput}) does not match any option");
                    DisplayWarningMessage("Input does not match any option");
                    return PromptUserIfWantsToContinue();
            }
        }

        private List<UserInputData> GetUserDataFromFile()
        {
            var fileData = MergeFileReader.Read();

            var userInputs = fileData.Select(x => new UserInputData
            {
                Sender = x.Sender ?? throw new ArgumentNullException(nameof(x.Sender)),
                Recipients = x.Recipients ?? throw new ArgumentNullException(nameof(x.Recipients)),
                Content = x.Content ?? throw new ArgumentNullException(nameof(x.Content)),
                ContentType = x.ContentType == DeclaredContentTypeForEmail.HTML ? EmailContentType.HTML : EmailContentType.Plain
            }).ToList();

            return userInputs;
        }

        public void DisplayMessage(string[] message)
        {
            foreach (var item in message)
            {
                _console.WriteLine($"[ {item} ]");
            }
        }

        private string PromptUserForInput(string inputDescriptionToBeShown, bool showBraces = true)
        {
            var messageToBeShown = showBraces ? $"[ {inputDescriptionToBeShown} ]" : inputDescriptionToBeShown;

            _console.WriteLine(messageToBeShown);
            _console.Write(">> ");

            var userInput = _console.ReadLine();
            if (string.IsNullOrEmpty(userInput))
            {
                _logger.LogWarning("UserInput is empty");
                DisplayWarningMessage("Input cannot be empty");
                return PromptUserForInput(inputDescriptionToBeShown, showBraces);
            }
            else
            {
                return userInput;
            }
        }

        private void DisplayWarningMessage(string message)
        {
            _console.WriteLine($"[ {message} ]", ConsoleColor.Red);
        }

        private string PromptUserForSenderAddress()
        {
            return PromptUserForInput("Specify sender email address") ?? PromptUserForSenderAddress();
        }

        private string[] PromptUserForRecipients()
        {
            var simpleInstruction = "Please specify your recipient"
                                    + Environment.NewLine
                                    + "If there is more than one recipient, you may provide it using comma"
                                    + Environment.NewLine
                                    + "eg \"adress1@domain.com, adress2@domain.com\"";

            var userInput = PromptUserForInput(simpleInstruction, false).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return userInput;
        }

        private string PromptUserForActualContent()
        {
            return PromptUserForInput("Type your email body content");
        }

        private EmailContentType PromptUserForContentType()
        {
            var simpleInstruction = "Please specify your content type"
                                    + Environment.NewLine
                                    + "Type \"1\" for text/plain"
                                    + Environment.NewLine
                                    + "Type \"2\" for text/html";
            var contentTypePromptInput = PromptUserForInput(simpleInstruction, false);

            switch (contentTypePromptInput)
            {
                case "1":
                    return EmailContentType.Plain;
                case "2":
                    return EmailContentType.HTML;
                default:
                    _logger.LogWarning($"User input ({contentTypePromptInput}) does not match any option");
                    DisplayWarningMessage("Input does not match any option");
                    return PromptUserForContentType();
            }
        }

        private LoadingDataScheme PromptUserForProcessingType()
        {
            var simpleInstruction = "How would you like to insert data?"
                                    + Environment.NewLine
                                    + "Type \"1\" for manual data input"
                                    + Environment.NewLine
                                    + "Type \"2\" for loading data from CSV file";
            var contentTypePromptInput = PromptUserForInput(simpleInstruction, false);

            switch (contentTypePromptInput)
            {
                case "1":
                    return LoadingDataScheme.Manually;
                case "2":
                    return LoadingDataScheme.UsingCSV;
                default:
                    _logger.LogWarning($"User input ({contentTypePromptInput}) does not match any option");
                    DisplayWarningMessage("Input does not match any option");
                    return PromptUserForProcessingType();
            }
        }
    }
}
