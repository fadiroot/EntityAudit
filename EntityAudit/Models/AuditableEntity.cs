using System;
using EntityAudit.Interfaces;

namespace EntityAudit.Models;

public abstract class AuditableEntity : IAuditable
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
}
