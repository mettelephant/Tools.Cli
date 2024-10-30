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
                   <!-- DataTables CSS -->
                   <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.13.5/css/jquery.dataTables.min.css"/>
                   
                   <!-- DataTables Buttons CSS -->
                   <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/2.3.6/css/buttons.dataTables.min.css"/>
                   
                   <!-- DataTables RowGroup CSS -->
                   <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/rowgroup/1.3.1/css/rowGroup.dataTables.min.css"/>
               
                   <!-- Select2 CSS (Optional for better multi-select UI) -->
                   <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />
                   
                   <!-- jQuery (required by DataTables) -->
                   <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
                   
                   <!-- DataTables JS -->
                   <script type="text/javascript" src="https://cdn.datatables.net/1.13.5/js/jquery.dataTables.min.js"></script>
                   
                   <!-- DataTables Buttons JS -->
                   <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/dataTables.buttons.min.js"></script>
                   <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/buttons.html5.min.js"></script>

                   <!-- DataTables RowGroup JS -->
                   <script type="text/javascript" src="https://cdn.datatables.net/rowgroup/1.3.1/js/dataTables.rowGroup.min.js"></script>
                   
                   <!-- Select2 JS (Optional) -->
                   <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
                   
                   <!-- Chart.js Library -->
                   <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                   
                   <!-- Modal Styles -->
                   <style>
                       /* Existing Styles */
                       body { font-family: Arial, sans-serif; margin: 20px; background-color: #f9f9f9; }
                       h1 { color: #2c3e50; }
                       h2 { color: #34495e; }
                       .summary, .customer-summary, .chart-container, .filters { background-color: #fff; padding: 20px; margin-bottom: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
                       .summary p, .customer-summary p { margin: 5px 0; }
                       .collapsible { background-color: #2980b9; color: white; cursor: pointer; padding: 10px; width: 100%; border: none; text-align: left; outline: none; font-size: 16px; border-radius: 5px; margin-top: 10px; }
                       .collapsible:after { content: "\25BC"; float: right; }
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
                       .chart-container { position: relative; height:40vh; width:80vw; }
                       .filters {
                           margin-bottom: 20px;
                       }
                       .filters label {
                           margin-right: 10px;
                           font-weight: bold;
                       }
                       
                       /* Modal styles */
                       .modal {
                           display: none; /* Hidden by default */
                           position: fixed; /* Stay in place */
                           z-index: 1; /* Sit on top */
                           padding-top: 100px; /* Location of the box */
                           left: 0;
                           top: 0;
                           width: 100%; /* Full width */
                           height: 100%; /* Full height */
                           overflow: auto; /* Enable scroll if needed */
                           background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
                       }
                       
                       .modal-content {
                           background-color: #fefefe;
                           margin: auto;
                           padding: 20px;
                           border: 1px solid #888;
                           width: 80%;
                           border-radius: 8px;
                       }
                       
                       .close {
                           color: #aaa;
                           float: right;
                           font-size: 28px;
                           font-weight: bold;
                           cursor: pointer;
                       }
                       
                       .close:hover,
                       .close:focus {
                           color: black;
                           text-decoration: none;
                           cursor: pointer;
                       }
                       
                       /* Details Control Icons */
                       td.details-control {
                           background: url('https://www.datatables.net/examples/resources/details_open.png') no-repeat center center;
                           cursor: pointer;
                           width: 18px;
                       }
                       tr.shown td.details-control {
                           background: url('https://www.datatables.net/examples/resources/details_close.png') no-repeat center center;
                       }
                       
                       /* Highlight Group Headers */
                       .dt-rowGroup {
                           background-color: #ecf0f1 !important;
                           font-weight: bold;
                           font-size: 16px;
                       }
                   </style>
               </head>
               <body>
               
                   <h1>Comprehensive Operation Report</h1>
               
                   <!-- Overall Summary Section -->
                   <div class="summary">
                       <h2>Overall Summary</h2>
                       <p><strong>Total Customers:</strong> 500</p>
                       <p><strong>Customers with No Errors:</strong> 350</p>
                       <p><strong>Customers with Errors:</strong> 150</p>
                   </div>
               
                   <!-- Filters Section -->
                   <div class="filters">
                       <label for="filterLogCode">Filter by Log Code:</label>
                       <select id="filterLogCode" multiple style="width: 200px;">
                           <option value="NoLongerNeeded">NoLongerNeeded</option>
                           <option value="TranslationNotSupported">TranslationNotSupported</option>
                           <option value="ChangeNotAllowed">ChangeNotAllowed</option>
                           <option value="DataValidationFailed">DataValidationFailed</option>
                           <option value="DependencyMissing">DependencyMissing</option>
                           <option value="ConfigurationError">ConfigurationError</option>
                           <option value="TimeoutOccurred">TimeoutOccurred</option>
                           <option value="ResourceUnavailable">ResourceUnavailable</option>
                           <!-- Add more codes as needed -->
                       </select>
                       
                       <label for="filterCustomerId">Filter by Customer ID:</label>
                       <input type="text" id="filterCustomerId" placeholder="Enter Customer ID">
                   </div>
               
                   <!-- Master Table Section -->
                   <div class="master-table-container">
                       <h2>Customer Summary</h2>
                       <table id="customerTable" class="display">
                           <thead>
                               <tr>
                                   <th></th> <!-- For the toggle icon -->
                                   <th>Customer ID</th>
                                   <th>Files Processed</th>
                                   <th>Successful Operations</th>
                                   <th>Failed Operations</th>
                                   <th>Warnings</th>
                                   <th>Errors</th>
                                   <th>Log Codes</th>
                               </tr>
                           </thead>
                           <tbody>
                               {CustomerTableRows}
                           </tbody>
                       </table>
                   </div>
               
                   <!-- Modal Structure (Optional if using DataTables' child rows) -->
                   <div id="logModal" class="modal">
                       <div class="modal-content">
                           <span class="close">&times;</span>
                           <h2>Customer Logs: <span id="modalCustomerId"></span></h2>
                           <div id="modalLogContent">
                               <!-- Log details will be injected here -->
                           </div>
                       </div>
                   </div>
               
                   <!-- Chart Sections (Optional) -->
                   <div class="chart-container">
                       <h2>Distribution of Log Levels</h2>
                       <canvas id="logLevelPieChart"></canvas>
                   </div>
               
                   <div class="chart-container">
                       <h2>Successful vs. Failed Operations Per Customer</h2>
                       <canvas id="successFailBarChart"></canvas>
                   </div>
               
                   <div class="chart-container">
                       <h2>Top 5 Most Common Log Codes</h2>
                       <canvas id="topLogCodesBarChart"></canvas>
                   </div>
               
                   <!-- Customer Logs Data (Corrected by Report Generator) -->
                   <script>
                       {CustomerLogs}
                   </script>
               
                   <!-- JavaScript for DataTables, Filters, and Modals -->
                   <script>
                   $(document).ready(function() {
                   // Initialize Select2 for Log Code multi-select
                   $('#filterLogCode').select2({
                       placeholder: "Select Log Codes",
                       allowClear: true
                   });
                   
                   // Function to determine primary log level for grouping
                   function getPrimaryLogLevel(logs) {
                       let levels = logs.map(log => log.logLevel);
                       if (levels.includes('Error')) {
                           return 'Error';
                       } else if (levels.includes('Warning')) {
                           return 'Warning';
                       } else {
                           return 'Info';
                       }
                   }
                   
                   // Initialize DataTables with RowGroup
                   var table = $('#customerTable').DataTable({
                       "pageLength": 25,
                       "lengthMenu": [10, 25, 50, 100],
                       "responsive": true,
                       "order": [[1, "asc"]],
                       "columns": [
                           {
                               "className": 'details-control',
                               "orderable": false,
                               "data": null,
                               "defaultContent": ''
                           },
                           { "data": "Customer ID" },
                           { "data": "Files Processed" },
                           { "data": "Successful Operations" },
                           { "data": "Failed Operations" },
                           { "data": "Warnings" },
                           { "data": "Errors" },
                           { "data": "Log Codes" }
                       ],
                       "dom": 'Bfrtip',
                       "buttons": [
                           'copyHtml5',
                           'excelHtml5',
                           'csvHtml5',
                           'pdfHtml5'
                       ],
                       "rowGroup": {
                       "dataSrc": function(row) {
                           var customerId = row['Customer ID'];
                           var logs = customerLogs[customerId];
                           if (logs && logs.length > 0 && Array.isArray(logs)) {
                               return getPrimaryLogLevel(logs);
                           } else {
                               return 'No Logs';
                           }
                       },
                       "endRender": function (rows, group) {
                           return '<span title="Group: ' + group + '">' + group + '</span> (' + rows.count() + ' customers)';
                       }
                   }
                   });
                   
                   // Add event listener for opening and closing details
                   $('#customerTable tbody').on('click', 'td.details-control', function () {
                       var tr = $(this).closest('tr');
                       var row = table.row(tr);
                   
                       if (row.child.isShown()) {
                           // Close the child row
                           row.child.hide();
                           tr.removeClass('shown');
                       }
                       else {
                           // Open the child row
                           var customerId = row.data()['Customer ID'];
                           var logs = customerLogs[customerId];
                   
                           if (logs && logs.length > 0) {
                               // Build HTML for log entries
                               var logHtml = '<div class="log-details">';
                               logs.forEach(function(log) { // logs is an array of arrays
                                   logHtml += `
                                       <div class="log-entry">
                                           <p class="${log.logLevel.toLowerCase()}"><strong>${getLogIcon(log.logLevel)} ${log.logLevel}:</strong> ${log.message}</p>
                                           <p><strong>Data:</strong> ${log.data}</p>
                                           <p><strong>Code:</strong> ${log.code}</p>
                                           <p><strong>Source File:</strong> ${log.sourceFile}</p>
                                           <p><strong>Destination File:</strong> ${log.destinationFile}</p>
                                           ${log.helpUrl ? `<p><a href="${log.helpUrl}" target="_blank">Help</a></p>` : ''}
                                       </div>
                                   `;
                               });
                               logHtml += '</div>';
                   
                               row.child(logHtml).show();
                               tr.addClass('shown');
                           } else {
                               row.child('<div class="log-details">No logs available.</div>').show();
                               tr.addClass('shown');
                           }
                       }
                   });
               
                       // Function to get log icons based on log level
                       function getLogIcon(logLevel) {
                           switch(logLevel) {
                               case 'Info':
                                   return 'ℹ️';
                               case 'Warning':
                                   return '⚠️';
                               case 'Error':
                                   return '❌';
                               default:
                                   return '';
                           }
                       }
               
                       // Implement Filter by Log Code
                       $('#filterLogCode').on('change', function() {
                           var selectedCodes = $(this).val(); // Array of selected codes
               
                           if (selectedCodes && selectedCodes.length > 0) {
                               // Build regex pattern to match any of the selected codes
                               var regexPattern = selectedCodes.map(code => `(${code})`).join('|');
               
                               // Apply regex search to the "Log Codes" column (index 7)
                               table.column(7).search(regexPattern, true, false).draw();
                           } else {
                               // Reset filter
                               table.column(7).search('').draw();
                           }
                       });
               
                       // Implement Filter by Customer ID
                       $('#filterCustomerId').on('keyup', function() {
                           var customerId = $(this).val().trim();
                           table.column(1).search(customerId, false, true).draw();
                       });
               
                       // Initialize Charts after DataTables is ready
                       initializeCharts(table);
                   });
               
                   // Function to initialize charts
                   function initializeCharts(table) {
                       // Function to calculate log level counts
                       function calculateLogLevelCounts(logs) {
                           let counts = { Info: 0, Warning: 0, Error: 0 };
                           for (let customer in logs) {
                               if (logs.hasOwnProperty(customer)) {
                                   logs[customer].forEach(log => {
                                       if (counts.hasOwnProperty(log.logLevel)) {
                                           counts[log.logLevel]++;
                                       }
                                   });
                               }
                           }
                           return counts;
                       }
               
                       // Function to calculate top 5 log codes
                       function calculateTopLogCodes(logs) {
                           let codeCounts = {};
                           for (let customer in logs) {
                               if (logs.hasOwnProperty(customer)) {
                                   logs[customer].forEach(log => {
                                       if (codeCounts.hasOwnProperty(log.code)) {
                                           codeCounts[log.code]++;
                                       } else {
                                           codeCounts[log.code] = 1;
                                       }
                                   });
                               }
                           }
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
                       let logLevelCounts = calculateLogLevelCounts(customerLogs);
                       let topLogCodes = calculateTopLogCodes(customerLogs);
               
                       // Initialize Log Level Pie Chart
                       var ctx1 = document.getElementById('logLevelPieChart').getContext('2d');
                       var logLevelPieChart = new Chart(ctx1, {
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
                       let successFailLabels = [];
                       let successData = [];
                       let failData = [];
               
                       table.rows().every(function(rowIdx, tableLoop, rowLoop) {
                           var data = this.data();
                           if (data) {
                               successFailLabels.push(data['Customer ID']);
                               successData.push(parseInt(data['Successful Operations']));
                               failData.push(Math.abs(parseInt(data['Failed Operations'])));
                           }
                       });
               
                       // Initialize Successful vs. Failed Operations Bar Chart
                       var ctx2 = document.getElementById('successFailBarChart').getContext('2d');
                       var successFailBarChart = new Chart(ctx2, {
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
                       var ctx3 = document.getElementById('topLogCodesBarChart').getContext('2d');
                       var topLogCodesBarChart = new Chart(ctx3, {
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
                   }
                   </script>
               
               
                   <!-- Charts Initialization Scripts -->
                   <!-- (Already included in the DataTables initialization script above) -->
               
                   <!-- DataTables Buttons CSS -->
                   <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/2.3.6/css/buttons.dataTables.min.css"/>
                   
                   <!-- DataTables Buttons JS -->
                   <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/dataTables.buttons.min.js"></script>
                   <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/buttons.html5.min.js"></script>
               
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