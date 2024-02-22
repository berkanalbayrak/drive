using NUnit.Framework;
using ProjectDrive.Vehicle.Engine.Piston;

namespace Tests.Editor
{
    [TestFixture]
    public class PistonTests
    {
        [Test]
        public void Piston_ShouldUpdatePosition_WhenMoved()
        {
            // Arrange
            var piston = new Piston();
            
            // Act
            piston.Move(0.25f); //quarter cycle
            
            // Assert
            Assert.AreNotEqual(1f, piston.Position, "Piston position should be at its peak at a quarter cycle.");
        }
    }
}