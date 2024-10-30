using System.Globalization;
using System.Text;

namespace HtmlReportBuilder;

public static class HtmlTemplate
{
    public static string GetReportTemplate()
    {
        return """
               <!DOCTYPE html>
               <html lang="en">
               <head>
                   <meta charset="UTF-8">
                   <meta name="viewport" content="width=device-width, initial-scale=1.0">
                   <title>Comprehensive Operation Report</title>
                   <style>
                       body { font-family: Arial, sans-serif; margin: 20px; background-color: #f9f9f9; }
                       h1 { color: #2c3e50; }
                       h2 { color: #34495e; }
                       .summary, .customer-summary { background-color: #fff; padding: 20px; margin-bottom: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
                       .summary p, .customer-summary p { margin: 5px 0; }
                       .collapsible { background-color: #2980b9; color: white; cursor: pointer; padding: 10px; width: 100%; border: none; text-align: left; outline: none; font-size: 16px; border-radius: 5px; margin-top: 10px; }
                       .collapsible:after { content: '\25BC'; float: right; }
                       .collapsible.active:after { content: "\25B2"; }
                       .content { padding: 0 15px; display: none; background-color: #ecf0f1; overflow: hidden; border-radius: 5px; margin-bottom: 10px; }
                       .log-entry { padding: 10px; border-bottom: 1px solid #bdc3c7; }
                       .log-entry:last-child { border-bottom: none; }
                       .log-info { color: #2980b9; }
                       .log-warning { color: #e67e22; }
                       .log-error { color: #c0392b; }
                       table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                       th, td { border: 1px solid #bdc3c7; padding: 8px; text-align: left; }
                       th { background-color: #34495e; color: white; }
                       a { color: #2980b9; text-decoration: none; }
                       a:hover { text-decoration: underline; }
                   </style>
               </head>
               <body>
               
                   <h1>Comprehensive Operation Report</h1>
               
                   <!-- Overall Summary Section -->
                   <div class="summary">
                       <h2>Overall Summary</h2>
                       <p><strong>Total Customers:</strong> {TotalCustomers}</p>
                       <p><strong>Customers with No Errors:</strong> {CustomersWithNoErrors}</p>
                       <p><strong>Customers with Errors:</strong> {CustomersWithErrors}</p>
                   </div>
               
                   <!-- Individual Customer Reports -->
                   {CustomerReports}
               
                   <script>
                       // Collapsible functionality
                       const coll = document.getElementsByClassName('collapsible');
                       for (let i = 0; i < coll.length; i++) {
                           coll[i].addEventListener('click', function() {
                               this.classList.toggle('active');
                               const content = this.nextElementSibling;
                               if (content.style.display === 'block') {
                                   content.style.display = 'none';
                               } else {
                                   content.style.display = 'block';
                               }
                           });
                       }
                   </script>

               </body>
               </html>
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