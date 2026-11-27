using NexusOps.Domain.Entities;
using NexusOps.Domain.Enums;
using NexusOps.Domain.Exceptions;
using Xunit;

namespace NexusOps.UnitTests.Entities;

public class TelemetryTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateTelemetry()
    {
        // Arrange
        var deviceId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        // Act
        var telemetry = new Telemetry(
            deviceId,
            MetricType.Temperature,
            24.5,
            timestamp);

        // Assert
        Assert.NotEqual(Guid.Empty, telemetry.Id);
        Assert.Equal(deviceId, telemetry.DeviceId);
        Assert.Equal(MetricType.Temperature, telemetry.MetricType);
        Assert.Equal(24.5, telemetry.Value);
        Assert.Equal(timestamp, telemetry.Timestamp);
    }

    [Fact]
    public void Constructor_WithEmptyDeviceId_ShouldThrowException()
    {
        Assert.Throws<DomainException>(() =>
            new Telemetry(
                Guid.Empty,
                MetricType.Temperature,
                24.5,
                DateTime.UtcNow));
    }

    [Fact]
    public void Constructor_WithInvalidMetricType_ShouldThrowException()
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry((MetricType)999, 24.5));
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_WithNonFiniteValue_ShouldThrowException(
        double value)
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry(MetricType.Temperature, value));
    }

    [Fact]
    public void Constructor_WithDefaultTimestamp_ShouldThrowException()
    {
        Assert.Throws<DomainException>(() =>
            new Telemetry(
                Guid.NewGuid(),
                MetricType.Temperature,
                24.5,
                default));
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void Constructor_WithNonUtcTimestamp_ShouldThrowException(
        DateTimeKind kind)
    {
        var timestamp = new DateTime(
            2026, 1, 1, 12, 0, 0, kind);

        Assert.Throws<DomainException>(() =>
            new Telemetry(
                Guid.NewGuid(),
                MetricType.Temperature,
                24.5,
                timestamp));
    }

    [Theory]
    [InlineData(-273.15)]
    [InlineData(-10)]
    [InlineData(0)]
    [InlineData(100)]
    public void Constructor_WithValidTemperature_ShouldSucceed(
        double value)
    {
        var telemetry = CreateTelemetry(
            MetricType.Temperature, value);

        Assert.Equal(value, telemetry.Value);
    }

    [Fact]
    public void Constructor_WithTemperatureBelowAbsoluteZero_ShouldThrowException()
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry(MetricType.Temperature, -273.16));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Constructor_WithValidHumidity_ShouldSucceed(
        double value)
    {
        var telemetry = CreateTelemetry(
            MetricType.Humidity, value);

        Assert.Equal(value, telemetry.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Constructor_WithInvalidHumidity_ShouldThrowException(
        double value)
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry(MetricType.Humidity, value));
    }

    [Theory]
    [InlineData(MetricType.Pressure, 0)]
    [InlineData(MetricType.Pressure, 6.5)]
    [InlineData(MetricType.Energy, 0)]
    [InlineData(MetricType.Energy, 150.5)]
    public void Constructor_WithValidNonNegativeMetric_ShouldSucceed(
        MetricType metricType,
        double value)
    {
        var telemetry = CreateTelemetry(metricType, value);

        Assert.Equal(value, telemetry.Value);
    }

    [Theory]
    [InlineData(MetricType.Pressure, -1)]
    [InlineData(MetricType.Energy, -1)]
    public void Constructor_WithNegativeMetric_ShouldThrowException(
        MetricType metricType,
        double value)
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry(metricType, value));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Constructor_WithValidMotion_ShouldSucceed(
        double value)
    {
        var telemetry = CreateTelemetry(
            MetricType.Motion, value);

        Assert.Equal(value, telemetry.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.5)]
    [InlineData(2)]
    public void Constructor_WithInvalidMotion_ShouldThrowException(
        double value)
    {
        Assert.Throws<DomainException>(() =>
            CreateTelemetry(MetricType.Motion, value));
    }

    private static Telemetry CreateTelemetry(
        MetricType metricType,
        double value)
    {
        return new Telemetry(
            Guid.NewGuid(),
            metricType,
            value,
            DateTime.UtcNow);
    }
}
