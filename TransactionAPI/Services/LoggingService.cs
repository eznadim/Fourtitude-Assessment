using log4net;
using TransactionAPI.Interfaces;
using TransactionAPI.Utils;

namespace TransactionAPI.Services;

public class LoggingService : ILoggingService
{
    private readonly ILog _logger;
    private readonly string _requestLogPath;
    private readonly string _responseLogPath;

    public LoggingService()
    {
        _logger = LogManager.GetLogger(typeof(LoggingService));
        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        Directory.CreateDirectory(logDirectory);

        _requestLogPath = Path.Combine(logDirectory, "requests.log");
        _responseLogPath = Path.Combine(logDirectory, "responses.log");
    }

    public void LogRequest(TransactionRequest request)
    {
        var logEntry = Utils.LoggingHelper.FormatRequestResponse("REQUEST", request);
        _logger.Info(logEntry);
        
        // Also log to file
        File.AppendAllText(_requestLogPath, 
            $"\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {logEntry}\n");
    }

    public void LogResponse(TransactionResponse response)
    {
        var logEntry = LoggingHelper.FormatRequestResponse("RESPONSE", response);
        _logger.Info(logEntry);
        
        // Also log to file
        File.AppendAllText(_responseLogPath, 
            $"\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {logEntry}\n");
    }

    public void LogError(string message, Exception ex = null)
    {
        if (ex != null)
        {
            _logger.Error($"{message}. Exception: {ex.Message}", ex);
        }
        else
        {
            _logger.Error(message);
        }
    }

    public void LogInfo(string message)
    {
        _logger.Info(message);
    }
}