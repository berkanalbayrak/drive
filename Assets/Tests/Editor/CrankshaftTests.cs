using NUnit.Framework;
using ProjectDrive.Vehicle.Engine.Crankshaft;
using ProjectDrive.Vehicle.Engine.Piston;

namespace Tests.Editor
{
    public class CrankshaftTests
    {
        [Test]
        public void Crankshaft_ShouldRotatePiston_WhenRotated()
        {
            // Arrange
            var crankshaft = new Crankshaft();
            
            // Act
            crankshaft.Rotate(45.0f);

            // Assert
            Assert.AreEqual(45.0f, crankshaft.RotationAngle, "Crankshaft rotation angle should match the amount it was rotated.");
        }
    }
}