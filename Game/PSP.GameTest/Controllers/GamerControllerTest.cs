using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PSP.GameAPI.Controllers;
using PSP.GameAPI.Services.CarService;
using PSP.GameAPI.Services.GameService;
using PSP.GameCommon.GameObjects;
using Xunit;

namespace PSP.GameTest.Controllers
{
    public class GamerControllerTest
    {
        [Fact]
        public void Post_AddPlayer_ReturnsCarObject()
        {
            // Arrange
            var moqGameService = new Mock<IGameService>();

            var carId = Guid.NewGuid().ToString();
            moqGameService.Setup(exp => exp.AddGamer(carId))
                .Returns(new Car());
            var moqCarService = new Mock<ICarService>();

            var controller = new GamerController(moqGameService.Object, moqCarService.Object);

            // Act
            var result = controller.Post(carId);

            // Assert
            Assert.IsType<Car>(result);
        }

        [Fact]
        public void Get_ReturnsEnemyCarObject()
        {
            // Arrange
            var moqGameService = new Mock<IGameService>();
            var carId = Guid.NewGuid().ToString();
            var moqCarService = new Mock<ICarService>();
            moqCarService.Setup(exp => exp.GetEnemyCar(carId))
                .Returns(new Car {Id = string.Empty});

            var controller = new GamerController(moqGameService.Object, moqCarService.Object);

            // Act
            var result = controller.Get(carId);

            // Assert
            Assert.IsType<Car>(result);
            Assert.NotEqual(result.Id, carId);
        }

        [Fact]
        public void Put_WhenShotOk_ReturnsOkResult()
        {
            // Arrange
            var moqGameService = new Mock<IGameService>();
            var carId = Guid.NewGuid().ToString();
            var moqCarService = new Mock<ICarService>();
            moqCarService.Setup(exp => exp.GetShot(carId))
                .Returns(new Bullet());

            var controller = new GamerController(moqGameService.Object, moqCarService.Object);

            // Act
            var result = controller.PutGetShot(carId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Put_WhenMove_ReturnsResultCarObject()
        {
            // Arrange
            var moqGameService = new Mock<IGameService>();

            var carId = Guid.NewGuid().ToString();
            var direction = 0;

            var moqCarService = new Mock<ICarService>();
            moqCarService.Setup(exp => exp.MoveGamer(carId, direction))
                .Returns(new Car());

            var controller = new GamerController(moqGameService.Object, moqCarService.Object);

            // Act
            var result = controller.PutMove(carId, direction);

            // Assert
            Assert.IsType<Car>(result);
        }
    }
}
