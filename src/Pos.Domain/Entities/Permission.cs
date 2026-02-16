namespace Pos.Domain.Entities;

public class Permission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
