using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// HealthStatus Value Object.
/// </summary>
public class HealthStatus : IEquatable<HealthStatus>
{
    public HealthStatusValue Value { get; private set; }

    public HealthStatus(HealthStatusValue value)
    {
        Value = value;
    }

    public bool Equals(HealthStatus? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }
    
    public override bool Equals(object obj) => Equals(obj as HealthStatus);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}