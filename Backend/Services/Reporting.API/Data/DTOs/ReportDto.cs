namespace Transaction.Data.DTOs;

public class ReportDto
{
    public Guid Id { get; set; }
    public string AccountId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string FileName { get; set; }  
}