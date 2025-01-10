namespace TransactionAPI.Interfaces;

public interface ILoggingService
{
    void LogRequest(TransactionRequest request);
    void LogResponse(TransactionResponse response);
    void LogError(string message, Exception ex = null);
    void LogInfo(string message);
}