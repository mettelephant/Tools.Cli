using System.Text.Json;
using Bogus;

var html = LogReportGenerator.GenerateReport(new CodeMergerSummary("JOY", "v5.9.18.2", "joybeta", 200, 153, 30, 17));
await File.WriteAllTextAsync("log-report.html", html);

public record CodeMergerSummary(string CustomerCode, string AssemblyVersion, string SubDomain, int FilesDetected, int FilesProcessed, int FilesIgnored, int FilesProcessedWithErrors);

public enum LogLevel
{
    Debug,
    Information,
    Summary,
    Warning
}

public enum MessageLogCode
{
    Information,
    ConfigFileRemoved,
    ConfigNoLongerUsed,
    TranslationNotAllowed,
    CodeChangeInSourceFile
}

public record LogEntry
{
    public LogLevel Level { get; set; }
    public required string Message { get; set; }
    public MessageLogCode MessageLogCode { get; set; }
    public List<string>? Data { get; set; }
    public List<string>? Files { get; set; }
    public List<string>? ReferencedBy { get; set; }
    public string? HelpUrl { get; set; }
}

public class LogReportGenerator
{
    // Previous LogEntry sample data generation code remains the same...

    private static string GetHtmlHeader()
    {
        return """
               <!DOCTYPE html>
               <html lang="en">
               <head>
                   <meta charset="UTF-8">
                   <meta name="viewport" content="width=device-width, initial-scale=1.0">
                   <title>Log Report</title>
                   <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.0/chart.min.js"></script>
                   <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet">
                   <style>
                       .chart-container {
                           position: relative;
                           height: 300px;
                           width: 100%;
                           max-height: 300px;
                       }
                       .expandable-content {
                           max-height: 300px;
                           overflow-y: auto;
                       }
                   </style>
               </head>
               """;
    }

    private static string GenerateLogGroup(IGrouping<MessageLogCode, LogEntry> group)
    {
        return $$"""
                 <div class="border rounded-lg overflow-hidden">
                     <button
                         onclick="toggleGroup('{{group.Key}}')"
                         class="w-full px-4 py-2 bg-gray-50 hover:bg-gray-100 flex items-center justify-between"
                     >
                         <div class="flex items-center">
                             <svg id="icon-{{group.Key}}" class="w-4 h-4 mr-2 transform transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                 <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"></path>
                             </svg>
                             <span class="font-medium">{{group.Key}}</span>
                             <span class="ml-2 text-sm text-gray-500">({{group.Count()}} entries)</span>
                         </div>
                     </button>
                     <div id="group-{{group.Key}}" class="hidden divide-y expandable-content">
                         {{string.Join("\n", group.Select((log, index) => GenerateLogEntry(log, group.Key.ToString(), index)))}}
                     </div>
                 </div>
                 """;
    }

    private static string GenerateLogEntry(LogEntry log, string groupKey, int index)
    {
        return $$"""
                 <div class="p-4 space-y-2">
                     <div class="inline-block px-2 py-1 rounded-full text-sm {{GetLogLevelColor(log.Level)}}">
                         {{log.Level}}
                     </div>
                     <div class="text-lg">{{log.Message}}</div>
                     {{GenerateExpandableList(log.Data, "Data", $"{groupKey}-{index}-data")}}
                     {{GenerateExpandableList(log.Files, "Files", $"{groupKey}-{index}-files")}}
                     {{GenerateExpandableList(log.ReferencedBy, "Referenced By", $"{groupKey}-{index}-refs")}}
                     {{GenerateHelpLink(log.HelpUrl)}}
                 </div>
                 """;
    }

    private static string GenerateHelpLink(string? helpUrl)
    {
        if (string.IsNullOrEmpty(helpUrl)) return "";

        return $$"""
                 <div class="mt-2">
                     <a href="{{helpUrl}}" 
                        target="_blank"
                        rel="noopener noreferrer"
                        class="text-blue-600 hover:text-blue-800 flex items-center text-sm">
                         Help Documentation
                         <svg class="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                             <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14"></path>
                         </svg>
                     </a>
                 </div>
                 """;
    }

    private static string GenerateExpandableList(List<string>? items, string title, string id)
    {
        if (items == null || !items.Any()) return "";

        return $$"""
                 <div class="mt-2">
                     <button
                         onclick="toggleList('{{id}}')"
                         class="flex items-center text-sm text-gray-600 hover:text-gray-900"
                     >
                         <svg id="icon-list-{{id}}" class="w-4 h-4 transform transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                             <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"></path>
                         </svg>
                         {{title}} ({{items.Count}})
                     </button>
                     <div id="list-{{id}}" class="hidden ml-6 mt-1">
                         {{string.Join("\n", items.Select(item => $"""
                                                                       <div class="text-sm text-gray-600">{item}</div>
                                                                   """))}}
                     </div>
                 </div>
                 """;
    }

    private static readonly Dictionary<LogLevel, (string bgClass, string chartColor)> LogLevelColors = new()
    {
        { LogLevel.Debug, ("bg-gray-200", "rgba(229, 231, 235, 0.8)") },
        { LogLevel.Information, ("bg-blue-200", "rgba(186, 230, 253, 0.8)") },
        { LogLevel.Summary, ("bg-green-200", "rgba(187, 247, 208, 0.8)") },
        { LogLevel.Warning, ("bg-red-200", "rgba(254, 202, 202, 0.8)") }
    };

    private static string GetLogLevelColor(LogLevel level) => 
        LogLevelColors.TryGetValue(level, out var colors) ? colors.bgClass : "bg-gray-200";

    record LogLevelStat(string Level, int Count);
    public static string GenerateReport(CodeMergerSummary summary)
    {
        var logs = GenerateSampleLogs();
        var groupedLogs = logs.GroupBy(l => l.MessageLogCode);
        
        var logLevelStats = logs.GroupBy(l => l.Level)
                               .Select(g => new LogLevelStat(g.Key.ToString(), g.Count()))
                               .ToList();
        
        // Get colors in the same order as the stats
        var chartColors = logLevelStats
            .Select(x => {
                var level = Enum.Parse<LogLevel>(x.Level);
                return LogLevelColors.TryGetValue(level, out var colors) ? colors.chartColor : LogLevelColors[LogLevel.Debug].chartColor;
            })
            .ToList();

        var reportContent = $"""
            <body class="bg-gray-50">
                <div class="max-w-6xl mx-auto p-6 space-y-8">
                    <!-- Summary Section -->
                    <div class="bg-white rounded-lg shadow-lg p-6">
                        <div class="flex items-center justify-between mb-6">
                            <div>
                                <h1 class="text-3xl font-bold text-gray-800">{summary.CustomerCode}</h1>
                                <p class="text-lg text-gray-600">{summary.SubDomain}</p>
                            </div>
                            <div class="text-right">
                                <span class="text-sm text-gray-500">Assembly Version</span>
                                <p class="text-lg font-mono font-medium">{summary.AssemblyVersion}</p>
                            </div>
                        </div>

                        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
                            <div class="bg-blue-50 rounded-lg p-4">
                                <div class="text-blue-800 text-sm font-medium">Files Detected</div>
                                <div class="text-2xl font-bold text-blue-900">{summary.FilesDetected}</div>
                            </div>
                            <div class="bg-green-50 rounded-lg p-4">
                                <div class="text-green-800 text-sm font-medium">Files Processed</div>
                                <div class="text-2xl font-bold text-green-900">{summary.FilesProcessed}</div>
                            </div>
                            <div class="bg-yellow-50 rounded-lg p-4">
                                <div class="text-yellow-800 text-sm font-medium">Files Ignored</div>
                                <div class="text-2xl font-bold text-yellow-900">{summary.FilesIgnored}</div>
                            </div>
                            <div class="bg-red-50 rounded-lg p-4">
                                <div class="text-red-800 text-sm font-medium">Processing Errors</div>
                                <div class="text-2xl font-bold text-red-900">{summary.FilesProcessedWithErrors}</div>
                            </div>
                        </div>
                    </div>

                    <!-- Log Analysis Section -->
                    <div class="bg-white rounded-lg shadow-lg p-6">
                        <h2 class="text-xl font-semibold mb-4">Log Level Distribution</h2>
                        <div class="chart-container">
                            <canvas id="logLevelChart"></canvas>
                        </div>
                    </div>

                    <!-- Logs Section -->
                    <div class="bg-white rounded-lg shadow-lg p-6">
                        <h2 class="text-xl font-semibold mb-4">Detailed Logs</h2>
                        <div class="space-y-4">
                            {string.Join("\n", groupedLogs.Select(GenerateLogGroup))}
                        </div>
                    </div>
                </div>
            """;

        return GetHtmlHeader() + reportContent + GetUpdatedHtmlFooter(logLevelStats, chartColors);
    }
    private static string GetUpdatedHtmlFooter(List<LogLevelStat> logLevelStats, List<string> chartColors) => $$"""
          <script>
              const ctx = document.getElementById('logLevelChart').getContext('2d');
              new Chart(ctx, {
                  type: 'bar',
                  data: {
                      labels: {{JsonSerializer.Serialize(logLevelStats.Select(x => x.Level))}},
                      datasets: [{
                          label: 'Number of Logs',
                          data: {{JsonSerializer.Serialize(logLevelStats.Select(x => x.Count))}},
                          backgroundColor: {{JsonSerializer.Serialize(chartColors)}}
                      }]
                  },
                  options: {
                      responsive: true,
                      maintainAspectRatio: false,
                      plugins: {
                          legend: {
                              display: false
                          }
                      },
                      scales: {
                          y: {
                              beginAtZero: true,
                              ticks: {
                                  stepSize: 1
                              }
                          }
                      }
                  }
              });
          
              // Toggle group visibility
              function toggleGroup(groupId) {
                  const content = document.getElementById(`group-${groupId}`);
                  const icon = document.getElementById(`icon-${groupId}`);
                  if (content && icon) {
                      content.classList.toggle('hidden');
                      icon.style.transform = content.classList.contains('hidden') ? 'rotate(0deg)' : 'rotate(90deg)';
                  }
              }
          
              // Toggle list visibility
              function toggleList(listId) {
                  const content = document.getElementById(`list-${listId}`);
                  const icon = document.getElementById(`icon-list-${listId}`);
                  if (content && icon) {
                      content.classList.toggle('hidden');
                      icon.style.transform = content.classList.contains('hidden') ? 'rotate(0deg)' : 'rotate(90deg)';
                  }
              }
          </script>
          </body>
          </html>
          """;

    private static List<LogEntry> GenerateSampleLogs()
    {
        // Set a constant seed for reproducible test data
        Randomizer.Seed = new Random(8675309);

        // Create faker for file paths
        var fileFaker = new Faker<string>()
            .CustomInstantiator(f => $"/{f.System.CommonFileName()}.{f.System.FileExt()}");

        // Create faker for data entries
        var dataFaker = new Faker<string>()
            .CustomInstantiator(f => f.Random.ArrayElement(new[]
            {
                $"Author: {f.Name.FullName()}",
                $"Timestamp: {f.Date.Recent().ToString("yyyy-MM-dd HH:mm:ss")}",
                $"Environment: {f.PickRandom("Development", "Staging", "Production", "Testing")}",
                $"Version: {f.System.Version()}",
                $"Duration: {f.Random.Number(1, 1000)}ms",
                $"Status: {f.PickRandom("Success", "Pending", "In Progress", "Completed")}",
                $"ID: {f.Random.Guid()}",
                $"Component: {f.Hacker.Abbreviation()}-{f.Random.Number(100, 999)}",
                $"Priority: {f.PickRandom("Low", "Medium", "High", "Critical")}",
                $"Category: {f.Commerce.Department()}",
                $"Branch: {f.Lorem.Word()}",
                $"Commit: {f.Lorem.Letter(17)}",
                $"Memory Usage: {f.Random.Number(50, 8192)}MB",
                $"CPU Usage: {f.Random.Number(0, 100)}%",
                $"Thread ID: {f.Random.Number(1, 100)}",
                $"Process ID: {f.Random.Number(1000, 9999)}",
                $"Database: {f.Database.Type()}",
                $"Table: {f.Database.Column()}",
                $"Query Time: {f.Random.Double(0, 10):F2}s",
                $"Cache Hit: {f.Random.Bool()}"
            }));

        // Create faker for service names
        var serviceFaker = new Faker<string>()
            .CustomInstantiator(f => f.Random.ArrayElement(new[]
            {
                $"{f.Hacker.Verb()}{f.Hacker.Noun()}Service",
                $"{f.Company.CatchPhrase()}Handler",
                $"{f.Hacker.Adjective()}Processor",
                $"{f.Company.CatchPhrase()}Manager",
                $"Core{f.Hacker.Noun()}Service",
                $"{f.Hacker.Abbreviation()}Service",
                $"{f.Company.CatchPhrase()}Controller"
            }));

        // Create faker for help URLs
        var urlFaker = new Faker<string>()
            .CustomInstantiator(f =>
                $"https://docs.example.com/{f.Internet.DomainWord()}/{f.Internet.DomainWord()}/{f.Random.Number(1000, 9999)}");

        // Create main log entry faker
        var logFaker = new Faker<LogEntry>()
            .RuleFor(l => l.Level, f => f.PickRandom<LogLevel>())
            .RuleFor(l => l.Message, f => f.Random.ArrayElement(new[]
            {
                $"Processing {f.System.CommonFileName()} in {f.Hacker.Verb()} module",
                $"Configuration update detected in {f.System.CommonFileName()}",
                $"Service {f.Company.CatchPhrase()} {f.Hacker.Verb()} completed",
                $"Database {f.Hacker.Verb()} operation finished",
                $"User {f.Internet.UserName()} performed {f.Hacker.Verb()}",
                $"Cache {f.Hacker.Verb()} for {f.Company.CatchPhrase()}",
                $"Network connection to {f.Internet.DomainName()} {f.Hacker.Verb()}",
                $"Thread {f.Random.Number(1, 100)} {f.Hacker.Verb()} state changed",
                $"Module {f.Hacker.Abbreviation()} status update: {f.Hacker.Verb()}",
                $"System {f.Hacker.Verb()} check completed"
            }))
            .RuleFor(l => l.MessageLogCode, f => f.PickRandom<MessageLogCode>())
            .RuleFor(l => l.Data, f => f.Random.Number(0, 100) < 80
                ? // 80% chance to have data
                dataFaker.Generate(f.Random.Number(0, 75))
                : null)
            .RuleFor(l => l.Files, f => f.Random.Number(0, 100) < 70
                ? // 70% chance to have files
                fileFaker.Generate(f.Random.Number(0, 75))
                : null)
            .RuleFor(l => l.ReferencedBy, f => f.Random.Number(0, 100) < 60
                ? // 60% chance to have references
                serviceFaker.Generate(f.Random.Number(0, 75))
                : null)
            .RuleFor(l => l.HelpUrl, f => f.Random.Number(0, 100) < 50
                ? // 50% chance to have help URL
                urlFaker.Generate()
                : null);

        // Generate 100 log entries
        return logFaker.Generate(100);
    }
}