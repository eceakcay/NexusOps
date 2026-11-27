
using NexusOps.Domain.Entities;
using NexusOps.Domain.Enums;
using NexusOps.Domain.Exceptions;
using Xunit;

namespace NexusOps.UnitTests.Entities;

public class DeviceTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateDevice()
    {
        // Arrange & Act
        var device = new Device(
            "Sensor 01",
            DeviceType.TemperatureSensor,
            "Ankara");

        // Assert
        Assert.NotEqual(Guid.Empty, device.Id);
        Assert.Equal("Sensor 01", device.Name);
        Assert.Equal(DeviceType.TemperatureSensor, device.Type);
        Assert.Equal("Ankara", device.Location);
        Assert.Equal(DeviceStatus.Offline, device.Status);
        Assert.Null(device.LastSeenAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowException(
        string name)
    {
        Assert.Throws<DomainException>(() =>
            new Device(
                name,
                DeviceType.TemperatureSensor,
                "Ankara"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidLocation_ShouldThrowException(
        string location)
    {
        Assert.Throws<DomainException>(() =>
            new Device(
                "Sensor 01",
                DeviceType.TemperatureSensor,
                location));
    }

    [Fact]
    public void Constructor_WithInvalidType_ShouldThrowException()
    {
        Assert.Throws<DomainException>(() =>
            new Device(
                "Sensor 01",
                (DeviceType)999,
                "Ankara"));
    }

    [Fact]
    public void UpdateName_WithValidName_ShouldUpdateDevice()
    {
        // Arrange
        var device = CreateDevice();

        // Act
        device.UpdateName("New Sensor");

        // Assert
        Assert.Equal("New Sensor", device.Name);
        Assert.NotNull(device.UpdatedAt);
    }

    [Fact]
    public void UpdateName_WithInvalidName_ShouldThrowException()
    {
        var device = CreateDevice();

        Assert.Throws<DomainException>(() =>
            device.UpdateName(""));
    }

    [Fact]
    public void UpdateLocation_ShouldUpdateLocation()
    {
        var device = CreateDevice();

        device.UpdateLocation("Istanbul");

        Assert.Equal("Istanbul", device.Location);
        Assert.NotNull(device.UpdatedAt);
    }

    [Fact]
    public void ChangeStatus_ShouldUpdateStatus()
    {
        var device = CreateDevice();

        device.ChangeStatus(DeviceStatus.Maintenance);

        Assert.Equal(DeviceStatus.Maintenance, device.Status);
    }

    [Fact]
    public void ChangeStatus_WithInvalidStatus_ShouldThrowException()
    {
        var device = CreateDevice();

        Assert.Throws<DomainException>(() =>
            device.ChangeStatus((DeviceStatus)999));
    }

    [Fact]
    public void RecordHeartbeat_ShouldSetDeviceOnline()
    {
        var device = CreateDevice();

        device.RecordHeartbeat();

        Assert.Equal(DeviceStatus.Online, device.Status);
        Assert.NotNull(device.LastSeenAt);
        Assert.NotNull(device.UpdatedAt);
    }

    [Theory]
    [InlineData(DeviceStatus.Maintenance)]
    [InlineData(DeviceStatus.Error)]
    public void RecordHeartbeat_ShouldPreserveSpecialStatus(
        DeviceStatus status)
    {
        var device = CreateDevice();
        device.ChangeStatus(status);

        device.RecordHeartbeat();

        Assert.Equal(status, device.Status);
        Assert.NotNull(device.LastSeenAt);
    }

    private static Device CreateDevice()
    {
        return new Device(
            "Sensor 01",
            DeviceType.TemperatureSensor,
            "Ankara");
    }
}
