namespace Shared.Abstractions.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}