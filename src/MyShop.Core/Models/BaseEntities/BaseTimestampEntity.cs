namespace MyShop.Core.Models.BaseEntities;
public abstract class BaseTimestampEntity : BaseEntity, ITimestampEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public interface ITimestampEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
