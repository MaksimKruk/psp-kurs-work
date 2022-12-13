using PSP.GameAPI.Services.MoveService;
using PSP.GameCommon;
using Xunit;

namespace PSP.GameTest.Services
{
    public class MoveServiceTest
    {
        [Fact]
        public void When_CarMoveForward_Then_SpeedChanged()
        {
            // Arrange
            var moqMovableGameObject = new MovableGameObject
            {
                Speed = 0.04f,
                MaxSpeed = 1f,
                Fuel = 100,
                Tire = true
            };

            var moveService = new MoveService();

            // Act
            var result = moveService.MoveForward(moqMovableGameObject);

            // Assert
            Assert.IsType<MovableGameObject>(result);
            Assert.Equal(0.08f, result.Speed);
        }

        [Fact]
        public void When_CarRotateLeft_Then_AngleChanged()
        {
            // Arrange
            var moqMovableGameObject = new MovableGameObject
            {
                Speed = 0.04f,
                MaxSpeed = 1f,
                Fuel = 100,
                Tire = true,
                Angle = 0
            };

            var moveService = new MoveService();

            // Act
            var result = moveService.RotateLeft(moqMovableGameObject);

            // Assert
            Assert.IsType<MovableGameObject>(result);
            Assert.Equal(0.08f, result.Angle);
        }

        [Fact]
        public void When_CarRotateRight_Then_AngleChanged()
        {
            // Arrange
            var moqMovableGameObject = new MovableGameObject
            {
                Speed = 0.04f,
                MaxSpeed = 1f,
                Fuel = 100,
                Tire = true,
                Angle = 0
            };

            var moveService = new MoveService();

            // Act
            var result = moveService.RotateRight(moqMovableGameObject);

            // Assert
            Assert.IsType<MovableGameObject>(result);
            Assert.Equal(-0.08f, result.Angle);
        }

        [Fact]
        public void When_CarMoveBack_Then_SpeedChanged()
        {
            // Arrange
            var moqMovableGameObject = new MovableGameObject
            {
                Speed = 0.04f,
                MaxSpeed = 1f,
                Fuel = 100,
                Tire = true,
                Angle = 0
            };

            var moveService = new MoveService();

            // Act
            var result = moveService.MoveBack(moqMovableGameObject);

            // Assert
            Assert.IsType<MovableGameObject>(result);
            Assert.Equal(0, result.Speed);
        }
    }
}
