using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Models;

public class TaskModel : EntityBase
{
    [Required]
    public Guid TaskId { get; set; } = Guid.NewGuid();
    [Required]
    public int TemplateId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TemplateType TemplateType { get; set; } = TemplateType.Statement;
    [Required]
    public string TemplateData { get; set; } = "{}";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskStatus Status { get; set; } = TaskStatus.Pending; // Pending, Processing, Completed, Failed
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    [StringLength(511)]
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int? UserId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Priority Priority{ get; set; } = Priority.Low;
    // Для повторных попыток
    public int RetryCount { get; set; } = 0;
    public DateTime? LastAttemptAt { get; set; }
}
public enum Priority
{
    Low,
    Normal,
    High
}

public enum TaskStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

public enum TemplateType
{
    Statement,
    Contract
}

