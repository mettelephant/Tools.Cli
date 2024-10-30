using System.Globalization;
using System.Text;

namespace HtmlReportBuilder;

public static class HtmlTemplate
{
    public static string GetReportTemplate()
    {
        return """
               <script>
                   // Function to calculate log level counts
                   function calculateLogLevelCounts(logs) {
                       let counts = { Info: 0, Warning: 0, Error: 0 };
                       logs.forEach(customer => {
                           customer.forEach(log => {
                               if (counts.hasOwnProperty(log.logLevel)) {
                                   counts[log.logLevel]++;
                               }
                           });
                       });
                       return counts;
                   }
               
                   // Function to calculate top 5 log codes
                   function calculateTopLogCodes(logs) {
                       let codeCounts = {};
                       logs.forEach(customer => {
                           customer.forEach(log => {
                               if (codeCounts.hasOwnProperty(log.code)) {
                                   codeCounts[log.code]++;
                               } else {
                                   codeCounts[log.code] = 1;
                               }
                           });
                       });
                       // Sort and get top 5
                       return Object.entries(codeCounts)
                           .sort((a, b) => b[1] - a[1])
                           .slice(0, 5)
                           .reduce((acc, [code, count]) => {
                               acc[code] = count;
                               return acc;
                           }, {});
                   }
               
                   // Extract all logs
                   let allLogs = [];
                   for (let customer in customerLogs) {
                       if (customerLogs.hasOwnProperty(customer)) {
                           allLogs = allLogs.concat(customerLogs[customer][0]); // Assuming logs are in the first array
                       }
                   }
               
                   // Calculate counts
                   let logLevelCounts = calculateLogLevelCounts(customerLogs);
                   let topLogCodes = calculateTopLogCodes(customerLogs);
               
                   // Initialize Log Level Pie Chart
                   var ctx = document.getElementById('logLevelPieChart').getContext('2d');
                   var logLevelPieChart = new Chart(ctx, {
                       type: 'pie',
                       data: {
                           labels: ['Info', 'Warning', 'Error'],
                           datasets: [{
                               data: [logLevelCounts.Info, logLevelCounts.Warning, logLevelCounts.Error],
                               backgroundColor: ['#3498db', '#f1c40f', '#e74c3c']
                           }]
                       },
                       options: {
                           responsive: true
                       }
                   });
               
                   // Calculate Success and Failure counts per customer
                   // Assuming 'Successful Operations' and 'Failed Operations' are in columns 3 and 4
                   let successFailLabels = [];
                   let successData = [];
                   let failData = [];
               
                   $('#customerTable tbody tr').each(function() {
                       var data = table.row(this).data();
                       if (data) {
                           successFailLabels.push(data[1]); // Customer ID
                           successData.push(parseInt(data[3]));
                           failData.push(Math.abs(parseInt(data[4]))); // Assuming failed operations are negative
                       }
                   });
               
                   // Initialize Successful vs. Failed Operations Bar Chart
                   var ctx = document.getElementById('successFailBarChart').getContext('2d');
                   var successFailBarChart = new Chart(ctx, {
                       type: 'bar',
                       data: {
                           labels: successFailLabels,
                           datasets: [
                               {
                                   label: 'Successful Operations',
                                   data: successData,
                                   backgroundColor: '#2ecc71'
                               },
                               {
                                   label: 'Failed Operations',
                                   data: failData,
                                   backgroundColor: '#e74c3c'
                               }
                           ]
                       },
                       options: {
                           responsive: true,
                           scales: {
                               y: { beginAtZero: true }
                           }
                       }
                   });
               
                   // Initialize Top Log Codes Bar Chart
                   var ctx = document.getElementById('topLogCodesBarChart').getContext('2d');
                   var topLogCodesBarChart = new Chart(ctx, {
                       type: 'bar',
                       data: {
                           labels: Object.keys(topLogCodes),
                           datasets: [{
                               label: 'Frequency',
                               data: Object.values(topLogCodes),
                               backgroundColor: ['#e74c3c', '#f1c40f', '#3498db', '#9b59b6', '#2ecc71']
                           }]
                       },
                       options: {
                           responsive: true,
                           scales: {
                               y: { beginAtZero: true }
                           }
                       }
                   });
               </script>
               
               """;
    }

    public static string GetCustomerReportTemplate()
    {
        return """
                   <div class="customer-summary">
                       <h2>Customer: {CustomerId}</h2>
                       <p><strong>Files Processed:</strong> {FilesProcessed}</p>
                       <p><strong>Successful Operations:</strong> {SuccessfulOperations}</p>
                       <p><strong>Failed Operations:</strong> {FailedOperations}</p>
                       <p><strong>Warnings:</strong> {Warnings}</p>
               
                       <!-- Log Sections -->
                       {LogSections}
                   </div>
               """;
    }

    public static string GetLogSectionTemplate(string code, List<LogResult> logResults)
    {
        var logEntries = new StringBuilder();
        foreach (var log in logResults)
        {
            string logClass = log.LogLevel switch
            {
                LogLevel.Info => "log-info",
                LogLevel.Warning => "log-warning",
                LogLevel.Error => "log-error",
                _ => ""
            };

            string icon = log.LogLevel switch
            {
                LogLevel.Info => "ℹ️",
                LogLevel.Warning => "⚠️",
                LogLevel.Error => "❌",
                _ => ""
            };

            logEntries.Append(CultureInfo.InvariantCulture,
                $"""
                   <div class="log-entry">
                       <p class="{logClass}"><strong>{icon} {log.LogLevel}:</strong> {log.Message}</p>
                       <p><strong>Data:</strong> {log.Data}</p>
                       <p><strong>Source File:</strong> {log.SourceFile}</p>
                       <p><strong>Destination File:</strong> {log.DestinationFile}</p>
                       {(!string.IsNullOrEmpty(log.HelpUrl) ? $"<p><a href=\"{log.HelpUrl}\" target=\"_blank\">Help</a></p>" : "")}
                   </div>
               """);
        }

        return $"""
                    <button class="collapsible">{code} ({logResults.Count})</button>
                    <div class="content">
                        {logEntries}
                    </div>
                """;
    }
}