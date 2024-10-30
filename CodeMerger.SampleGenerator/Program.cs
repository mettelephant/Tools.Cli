// Program.cs

using System.Text.Json;
using Bogus;
using TestDataGenerator;

// Configuration
int numberOfCustomers = 5; // Adjust as needed
string[] customerIds = { "ABC", "XYZ", "JKL", "MNO", "PQR" }; // Add or modify customer IDs
string logsRoot = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

// Create Logs directory if it doesn't exist
if (!Directory.Exists(logsRoot))
{
    Directory.CreateDirectory(logsRoot);
}

// Generate timestamped report directory
string timestamp = DateTime.Now.ToString("yyyy_MM_dd-HH_mm");
string reportDir = Path.Combine(logsRoot, timestamp);
Directory.CreateDirectory(reportDir);

Console.WriteLine($"Generating test data in: {reportDir}");

// Initialize Faker for Summary
var summaryFaker = new Faker<Summary>()
    .RuleFor(s => s.FilesProcessed, f => f.Random.Int(50, 200))
    .RuleFor(s => s.SuccessfulOperations, f => f.Random.Int(30, 180))
    .RuleFor(s => s.FailedOperations, (f, s) => s.FilesProcessed - s.SuccessfulOperations - f.Random.Int(0, 10))
    .RuleFor(s => s.Warnings, f => f.Random.Int(0, 20));

// Initialize Faker for LogResult
var logResultFaker = new Faker<LogResult>()
    .RuleFor(lr => lr.Code, f => f.PickRandom("NoLongerNeeded", "TranslationNotSupported", "ChangeNotAllowed", "DataValidationFailed", "DependencyMissing", "ResourceUnavailable", "ConfigurationError", "TimeoutOccurred"))
    .RuleFor(lr => lr.LogLevel, f => f.PickRandom<LogLevel>())
    .RuleFor(lr => lr.Message, f => f.Lorem.Sentence())
    .RuleFor(lr => lr.Data, f => f.Lorem.Paragraph())
    .RuleFor(lr => lr.SourceFile, f => $"src/{f.System.FileName("cs")}")
    .RuleFor(lr => lr.DestinationFile, f => $"dest/{f.System.FileName("cs")}")
    .RuleFor(lr => lr.HelpUrl, (f, lr_) => f.Internet.Url());

// Generate data for each customer
for (int i = 0; i < numberOfCustomers; i++)
{
    string customerId = customerIds[i % customerIds.Length];
    string customerDir = Path.Combine(reportDir, customerId);
    Directory.CreateDirectory(customerDir);

    // Generate Summary
    Summary summary = summaryFaker.Generate();

    // Ensure consistency: FilesProcessed = Successful + Failed + Warnings
    // Adjust FailedOperations if necessary
    if (summary.SuccessfulOperations + summary.FailedOperations + summary.Warnings > summary.FilesProcessed)
    {
        summary.FailedOperations = summary.FilesProcessed - summary.SuccessfulOperations - summary.Warnings;
    }
    var jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };


    // Serialize Summary to JSON
    string summaryJson = JsonSerializer.Serialize(summary, jsonOptions);
    File.WriteAllText(Path.Combine(customerDir, "summary.json"), summaryJson);
    Console.WriteLine($"Generated summary.json for customer {customerId}");

    // Generate LogResults
    int numberOfLogs = new Random().Next(10, 50); // Adjust as needed
    List<LogResult> logResults = logResultFaker.Generate(numberOfLogs);

    // Serialize LogResults to JSON
    string logResultsJson = JsonSerializer.Serialize(logResults, jsonOptions);
    File.WriteAllText(Path.Combine(customerDir, "code-merger.json"), logResultsJson);
    Console.WriteLine($"Generated code-merger.json for customer {customerId}");
}

Console.WriteLine("Test data generation completed successfully.");