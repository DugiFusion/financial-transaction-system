using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.API.Entities;

public class Report
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("account_id")]
    public string AccountId { get; set; }
    [Column("generated_at")]
    public DateTime GeneratedAt { get; set; }
    [Column("file_name")]
    public string FileName { get; set; }  
    
}