using System.Text.Json;
using Reporting.API.Repositories.Interfaces;
using Transaction.Data.DTOs;

namespace Reporting.API.EventBusConsumer;


public class MessageHandler
{
    private readonly IReportRepository _reportRepository;

    public MessageHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<int> HandleMessage(string message)
    {
        var transactions = JsonSerializer.Deserialize<List<TransactionDto>>(message);
        if (transactions == null)
        {
            throw new Exception("Invalid message format.");
        } 
        return await _reportRepository.CreateReport(transactions.ToArray());
    }
}