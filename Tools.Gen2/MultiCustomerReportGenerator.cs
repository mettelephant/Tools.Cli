using System.Text.Json;

namespace Tools.Gen2;

public class MultiCustomerReportGenerator
{
    public record CustomerResult(
        string CustomerCode,
        CodeMergerSummary Summary,
        List<LogEntry> Logs
    );

    public record OverallMetrics(
        int TotalCustomers,
        int TotalFilesDetected,
        int TotalFilesProcessed,
        int TotalFilesIgnored,
        int TotalErrors,
        Dictionary<string, int> ErrorsByCustomer,
        Dictionary<LogLevel, int> TotalLogsByLevel,
        Dictionary<MessageLogCode, int> TotalLogsByCode
    );

    public static async Task GenerateConsolidatedReport(string baseDirectory, string outputDirectory)
    {
        // Load all customer results
        var customerResults = await LoadCustomerResults(baseDirectory);
        
        // Calculate overall metrics
        var metrics = CalculateOverallMetrics(customerResults);
        
        // Generate main index page and individual customer pages
        await GenerateMainPage(customerResults, metrics, outputDirectory);
        await GenerateCustomerPages(customerResults, outputDirectory);
    }

    private static async Task<List<CustomerResult>> LoadCustomerResults(string baseDirectory)
    {
        var results = new List<CustomerResult>();
        var customerDirs = Directory.GetDirectories(baseDirectory);

        foreach (var customerDir in customerDirs)
        {
            var customerCode = Path.GetFileName(customerDir);
            var summaryPath = Path.Combine(customerDir, "summary.json");
            var logsPath = Path.Combine(customerDir, "logs.json");

            if (File.Exists(summaryPath) && File.Exists(logsPath))
            {
                var summary = JsonSerializer.Deserialize<CodeMergerSummary>(
                    await File.ReadAllTextAsync(summaryPath));
                var logs = JsonSerializer.Deserialize<List<LogEntry>>(
                    await File.ReadAllTextAsync(logsPath));

                if (summary != null && logs != null)
                {
                    results.Add(new CustomerResult(customerCode, summary, logs));
                }
            }
        }

        return results;
    }

    private static OverallMetrics CalculateOverallMetrics(List<CustomerResult> results)
    {
        return new OverallMetrics(
            TotalCustomers: results.Count,
            TotalFilesDetected: results.Sum(r => r.Summary.FilesDetected),
            TotalFilesProcessed: results.Sum(r => r.Summary.FilesProcessed),
            TotalFilesIgnored: results.Sum(r => r.Summary.FilesIgnored),
            TotalErrors: results.Sum(r => r.Summary.FilesProcessedWithErrors),
            ErrorsByCustomer: results
                .Where(r => r.Summary.FilesProcessedWithErrors > 0)
                .OrderByDescending(r => r.Summary.FilesProcessedWithErrors)
                .ToDictionary(r => r.CustomerCode, r => r.Summary.FilesProcessedWithErrors),
            TotalLogsByLevel: results
                .SelectMany(r => r.Logs)
                .GroupBy(l => l.Level)
                .ToDictionary(g => g.Key, g => g.Count()),
            TotalLogsByCode: results
                .SelectMany(r => r.Logs)
                .GroupBy(l => l.MessageLogCode)
                .ToDictionary(g => g.Key, g => g.Count())
        );
    }

    private static async Task GenerateMainPage(
        List<CustomerResult> results, 
        OverallMetrics metrics, 
        string outputDirectory)
    {
        var html = $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Code Merger Results Summary</title>
                <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.7.0/chart.min.js"></script>
                <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet">
            </head>
            <body class="bg-gray-50">
                <div class="max-w-7xl mx-auto p-6 space-y-8">
                    <!-- Overall Summary -->
                    <div class="bg-white rounded-lg shadow-lg p-6">
                        <h1 class="text-3xl font-bold mb-6">Code Merger Summary</h1>
                        <div class="grid grid-cols-1 md:grid-cols-5 gap-4 mb-6">
                            <div class="bg-purple-50 rounded-lg p-4">
                                <div class="text-purple-800 text-sm font-medium">Total Customers</div>
                                <div class="text-2xl font-bold text-purple-900">{{metrics.TotalCustomers}}</div>
                            </div>
                            <div class="bg-blue-50 rounded-lg p-4">
                                <div class="text-blue-800 text-sm font-medium">Files Detected</div>
                                <div class="text-2xl font-bold text-blue-900">{{metrics.TotalFilesDetected}}</div>
                            </div>
                            <div class="bg-green-50 rounded-lg p-4">
                                <div class="text-green-800 text-sm font-medium">Files Processed</div>
                                <div class="text-2xl font-bold text-green-900">{{metrics.TotalFilesProcessed}}</div>
                            </div>
                            <div class="bg-yellow-50 rounded-lg p-4">
                                <div class="text-yellow-800 text-sm font-medium">Files Ignored</div>
                                <div class="text-2xl font-bold text-yellow-900">{{metrics.TotalFilesIgnored}}</div>
                            </div>
                            <div class="bg-red-50 rounded-lg p-4">
                                <div class="text-red-800 text-sm font-medium">Total Errors</div>
                                <div class="text-2xl font-bold text-red-900">{{metrics.TotalErrors}}</div>
                            </div>
                        </div>

                        <!-- Charts Row -->
                        <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                            <div class="bg-gray-50 rounded-lg p-4">
                                <h3 class="text-lg font-semibold mb-2">Log Levels Distribution</h3>
                                <div class="h-64">
                                    <canvas id="logLevelsChart"></canvas>
                                </div>
                            </div>
                            <div class="bg-gray-50 rounded-lg p-4">
                                <h3 class="text-lg font-semibold mb-2">Top Message Types</h3>
                                <div class="h-64">
                                    <canvas id="messageTypesChart"></canvas>
                                </div>
                            </div>
                        </div>

                        <!-- Customers with Errors -->
                        {{(metrics.ErrorsByCustomer.Count != 0
                     ? $"""
                        <div class="mb-6">
                            <h3 class="text-lg font-semibold mb-2">Customers with Errors</h3>
                            <div class="bg-red-50 rounded-lg p-4">
                                <div class="grid grid-cols-2 gap-2">
                                    {string.Join("\n", metrics.ErrorsByCustomer.Select(kvp => $"""
                                    <div class="flex justify-between items-center">
                                        <a href="{kvp.Key}.html" class="text-red-700 hover:text-red-900">{kvp.Key}</a>
                                        <span class="text-red-600 font-medium">{kvp.Value} errors</span>
                                    </div>
                                    """))}
                                </div>
                            </div>
                        </div>
                        """ 
                      : "")}}

                        <!-- Customer List -->
                        <div>
                            <h3 class="text-lg font-semibold mb-2">All Customers</h3>
                            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                                {{string.Join("\n", results.OrderBy(r => r.CustomerCode).Select(r => $"""
                                <a href="{r.CustomerCode}.html" 
                                   class="block p-4 rounded-lg border hover:shadow-lg transition-shadow duration-200">
                                    <div class="flex justify-between items-center">
                                        <div>
                                            <div class="text-lg font-semibold">{r.CustomerCode}</div>
                                            <div class="text-sm text-gray-600">{r.Summary.SubDomain}</div>
                                        </div>
                                        <div class="text-right">
                                            <div class="text-sm text-gray-600">Files Processed</div>
                                            <div class="font-medium">{r.Summary.FilesProcessed}</div>
                                        </div>
                                    </div>
                                </a>
                                """))}}
                            </div>
                        </div>
                    </div>
                </div>

                <script>
                    // Log Levels Chart
                    const logLevelsCtx = document.getElementById('logLevelsChart').getContext('2d');
                    new Chart(logLevelsCtx, {
                        type: 'bar',
                        data: {
                            labels: {{JsonSerializer.Serialize(metrics.TotalLogsByLevel.Select(x => x.Key.ToString()))}},
                            datasets: [{
                                data: {{JsonSerializer.Serialize(metrics.TotalLogsByLevel.Values)}},
                                backgroundColor: [
                                    'rgba(229, 231, 235, 0.8)',  // Debug
                                    'rgba(186, 230, 253, 0.8)',  // Information
                                    'rgba(187, 247, 208, 0.8)',  // Summary
                                    'rgba(254, 202, 202, 0.8)'   // Warning
                                ]
                            }]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            plugins: { legend: { display: false } },
                            scales: { y: { beginAtZero: true } }
                        }
                    });

                    // Message Types Chart
                    const messageTypesCtx = document.getElementById('messageTypesChart').getContext('2d');
                    new Chart(messageTypesCtx, {
                        type: 'bar',
                        data: {
                            labels: {{JsonSerializer.Serialize(metrics.TotalLogsByCode.Select(x => x.Key.ToString()))}},
                            datasets: [{
                                data: {{JsonSerializer.Serialize(metrics.TotalLogsByCode.Values)}},
                                backgroundColor: 'rgba(147, 197, 253, 0.8)'
                            }]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            plugins: { legend: { display: false } },
                            scales: { y: { beginAtZero: true } }
                        }
                    });
                </script>
            </body>
            </html>
            """;

        if(!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
        await File.WriteAllTextAsync(Path.Combine(outputDirectory, "index.html"), html);
    }

    private static async Task GenerateCustomerPages(List<CustomerResult> results, string outputDirectory)
    {
        foreach (var result in results)
        {
            // Use the existing LogReportGenerator for individual customer pages
            var html = LogReportGenerator.GenerateReport(result.Summary, result.Logs);
            
            // Add a back to summary link at the top
            html = html.Replace("<body class=\"bg-gray-50\">",
                """
                <body class="bg-gray-50">
                    <div class="max-w-6xl mx-auto pt-4">
                        <a href="index.html" class="inline-flex items-center text-blue-600 hover:text-blue-800">
                            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path>
                            </svg>
                            Back to Summary
                        </a>
                    </div>
                """);

            await File.WriteAllTextAsync(Path.Combine(outputDirectory, $"{result.CustomerCode}.html"), html);
        }
    }
}