using Spectre.Console;
using Tools.Cli.Logging;
using Tools.Cli.Logging.File;
using Tools.Core.Settings;

namespace Tools.Cli.Commands;

public abstract class BaseHandler<TSettings> where TSettings : BaseSettings
{
    protected void ConfigureLogger(TSettings settings)
    {
        // Create the file logger if log directory is specified
        ILogger? fileLogger = null;
        if (settings.LogDirectory is not null)
        {
            fileLogger = new FileLogger(settings.LogDirectory);
        }

        // Configure the logger instance
        if (settings.EnableConsoleLogging)
        {
            // Use Spectre.Console for progress reporting
            var progress = AnsiConsole.Progress();

            progress.Start(ctx =>
            {
                // Create the console logger
                var consoleLogger = new ConsoleProgressLogger(ctx);

                // Combine loggers
                if (fileLogger != null)
                {
                    Logger.Instance = new CompositeLogger(consoleLogger, fileLogger);
                }
                else
                {
                    Logger.Instance = consoleLogger;
                }

                // Call the execution method
                Execute(ctx, settings);
            });
        }
        else
        {
            // Use only file logger
            Logger.Instance = fileLogger ?? NullLogger.Instance;

            // Call the execution method
            Execute(null, settings);
        }
    }

    protected abstract void Execute(ProgressContext? progressContext, TSettings settings);
}