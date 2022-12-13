using System;
using Moq;
using PSP.GameAPI.Services.CarService;
using PSP.GameAPI.Services.LevelService;
using PSP.GameAPI.Services.MoveService;
using PSP.GameAPI.Services.PrizeService;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;
using Xunit;

namespace PSP.GameTest.Services
{
    public class CarServiceTest
    {
        [Fact]
        public void When_CreateCar_Then_NewCarObjectReturned()
        {
            // Arrange
            var moqLevelService = new Mock<ILevelService>();
            var moqPrizeService = new Mock<IPrizeService>();
            var moqMoveService = new Mock<IMoveService>();

            var carService = new CarService(moqMoveService.Object, moqPrizeService.Object, moqLevelService.Object);
            var carId = Guid.NewGuid().ToString();

            // Act
            var result = carService.CreateCar(carId);

            // Assert
            Assert.IsType<Car>(result);
        }

        [Fact]
        public void When_FuelDecorate_Then_UpdatedCarReturned()
        {
            // Arrange
            var moqLevelService = new Mock<ILevelService>();
            var moqPrizeService = new Mock<IPrizeService>();
            var moqMoveService = new Mock<IMoveService>();

            var carService = new CarService(moqMoveService.Object, moqPrizeService.Object, moqLevelService.Object);
            var carId = Guid.NewGuid().ToString();
            var car = carService.CreateCar(carId);
            car.Fuel = 0;

            var prizeType  = GameObjectType.Fuel;

            // Act
            var result = carService.Decorate(car, prizeType);

            // Assert
            Assert.IsType<Car>(result);
            Assert.Equal(50, car.Fuel);
        }

        [Fact]
        public void When_TireDecorate_Then_UpdatedCarReturned()
        {
            // Arrange
            var moqLevelService = new Mock<ILevelService>();
            var moqPrizeService = new Mock<IPrizeService>();
            var moqMoveService = new Mock<IMoveService>();

            var carService = new CarService(moqMoveService.Object, moqPrizeService.Object, moqLevelService.Object);
            var carId = Guid.NewGuid().ToString();
            var car = carService.CreateCar(carId);
            car.Tire = false;

            var prizeType = GameObjectType.Tire;

            // Act
            var result = carService.Decorate(car, prizeType);

            // Assert
            Assert.IsType<Car>(result);
            Assert.True(car.Tire);
        }

        [Fact]
        public void When_BulletsDecorate_Then_UpdatedCarReturned()
        {
            // Arrange
            var moqLevelService = new Mock<ILevelService>();
            var moqPrizeService = new Mock<IPrizeService>();
            var moqMoveService = new Mock<IMoveService>();

            var carService = new CarService(moqMoveService.Object, moqPrizeService.Object, moqLevelService.Object);
            var carId = Guid.NewGuid().ToString();
            var car = carService.CreateCar(carId);
            car.Cartridges = 0;

            var prizeType = GameObjectType.Cartridge;

            // Act
            var result = carService.Decorate(car, prizeType);

            // Assert
            Assert.IsType<Car>(result);
            Assert.Equal(2, car.Cartridges);
        }
    }
}
