using Moq;
using NUnit.Framework;
using ProjectDrive.Vehicle.Engine.FuelInjector;

namespace Tests.Editor
{
    [TestFixture]
    public class FuelInjectorTests
    {
        [Test]
        public void InjectFuel_WithPositiveAmount_ShouldStartInjecting()
        {
            // Arrange
            var injector = new FuelInjector();

            // Act
            injector.InjectFuel(1.0f);

            // Assert
            Assert.IsTrue(injector.IsInjecting, "Injector should be injecting when a positive amount of fuel is injected.");
        }

        [Test]
        public void InjectFuel_WithZeroAmount_ShouldNotInject()
        {
            // Arrange
            var injector = new FuelInjector();

            // Act
            injector.InjectFuel(0f);

            // Assert
            Assert.IsFalse(injector.IsInjecting, "Injector should not be injecting when the amount of fuel is zero.");
        }
    }
}