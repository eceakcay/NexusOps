
using NexusOps.Domain.Common;
using NexusOps.Domain.Enums;
using NexusOps.Domain.Exceptions;

namespace NexusOps.Domain.Entities;

public class Device : BaseEntity
{
    public string Name { get; private set; }

    public DeviceType Type { get; private set; }

    public DeviceStatus Status { get; private set; }

    public string Location { get; private set; }

    public DateTime? LastSeenAt { get; private set; }

    public Device(
        string name,
        DeviceType type,
        string location)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Device name cannot be empty.");

        if (string.IsNullOrWhiteSpace(location))
            throw new DomainException(
                "Device location cannot be empty.");

        if (!Enum.IsDefined(type))
            throw new DomainException(
                "Invalid device type.");

        Name = name.Trim();
        Type = type;
        Location = location.Trim();
        Status = DeviceStatus.Offline;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(
                "Device name cannot be empty.");

        Name = name.Trim();
        MarkAsUpdated();
    }

    public void UpdateLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new DomainException(
                "Device location cannot be empty.");

        Location = location.Trim();
        MarkAsUpdated();
    }

    public void ChangeStatus(DeviceStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new DomainException(
                "Invalid device status.");

        Status = status;
        MarkAsUpdated();
    }

    public void RecordHeartbeat()
    {
        LastSeenAt = DateTime.UtcNow;

        if (Status == DeviceStatus.Offline)
            Status = DeviceStatus.Online;

        MarkAsUpdated();
    }
}
