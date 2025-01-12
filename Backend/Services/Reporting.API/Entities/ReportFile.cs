using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.API.Entities;

public class ReportFile
{
    [Column("id")] public Guid Id { get; set; }

    [Column("report_id")] public Guid ReportId { get; set; }

    [Column("file_data")] public byte[] FileData { get; set; }
}