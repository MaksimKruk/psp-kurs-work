using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PSP.GameAPI.Services.CarService;
using PSP.GameAPI.Services.LevelService;
using PSP.GameAPI.Services.PrizeService;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;

namespace PSP.GameAPI.Services.GameService
{
    public class GameService : IGameService
    {
        private Timer prizeTimer;
       
        private readonly ICarService _carService;
        private readonly IPrizeService _prizeService;
        private readonly ILevelService _levelService;

        public GameService(ICarService carService, 
            IPrizeService prizeService, ILevelService levelService)
        {
            _carService = carService;
            _prizeService = prizeService;
            _levelService = levelService;

            prizeTimer = new Timer(new TimerCallback(RefreshPrizes), null, 0, 60000);
            RefreshPrizes(null);
        }

        public List<GameObject> GetAllObjects()
        {
            var gameObjects = new List<GameObject>(_prizeService.GetGamePrizes());
            gameObjects = gameObjects.Concat(_levelService.GetLevel()).ToList();
            gameObjects = gameObjects.Concat(_carService.GetCars()).ToList();

            return gameObjects;
        }

        private void RefreshPrizes(object obj)
        {
            var gameobj = GetAllObjects().ToArray();
            _prizeService.RefreshPrizes(gameobj);
        }

        public Car AddGamer(string clientId)
        {
            var count = _carService.GetCars().Count;

            Car car;
            if (count < 2)
            {
                car = _carService.CreateCar(clientId);
                _ = _carService.AddCar(car);
            }
            else
            {
                car = _carService.GetCars().FirstOrDefault();
            }

            return car;
        }

        public void ResetGame()
        {
            RefreshPrizes(null);

            _carService.ResetCars();
        }
    }
}
