using NexusOps.Domain.Common;
using NexusOps.Domain.Enums;
using NexusOps.Domain.Exceptions;

namespace NexusOps.Domain.Entities;

public class Telemetry : BaseEntity
{
    public Guid DeviceId { get; private set; }

    public MetricType MetricType { get; private set; }

    public double Value { get; private set; }

    public DateTime Timestamp { get; private set; }

    public Telemetry(
        Guid deviceId,
        MetricType metricType,
        double value,
        DateTime timestamp)
    {
        if (deviceId == Guid.Empty)
            throw new DomainException(
                "Device ID cannot be empty.");

        if (!Enum.IsDefined(metricType))
            throw new DomainException(
                "Invalid metric type.");

        if (!double.IsFinite(value))
            throw new DomainException(
                "Telemetry value must be finite.");

        if (timestamp == default)
            throw new DomainException(
                "Timestamp cannot be empty.");

        if (timestamp.Kind != DateTimeKind.Utc)
            throw new DomainException(
                "Timestamp must be in UTC.");

        ValidateValue(metricType, value);

        DeviceId = deviceId;
        MetricType = metricType;
        Value = value;
        Timestamp = timestamp;
    }

    private static void ValidateValue(
        MetricType metricType,
        double value)
    {
        switch (metricType)
        {
            case MetricType.Temperature
                when value < -273.15:
                throw new DomainException(
                    "Temperature cannot be below absolute zero.");

            case MetricType.Humidity
                when value < 0 || value > 100:
                throw new DomainException(
                    "Humidity must be between 0 and 100.");

            case MetricType.Pressure
                when value < 0:
                throw new DomainException(
                    "Pressure cannot be negative.");

            case MetricType.Energy
                when value < 0:
                throw new DomainException(
                    "Energy cannot be negative.");

            case MetricType.Motion
                when value != 0 && value != 1:
                throw new DomainException(
                    "Motion value must be 0 or 1.");
        }
    }
}
