using PSP.GameAPI.Services.LevelService;
using PSP.GameAPI.Services.MoveService;
using PSP.GameAPI.Services.PrizeService;
using PSP.GameCommon;
using PSP.GameCommon.Decorator;
using PSP.GameCommon.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSP.GameAPI.Services.CarService
{
    public class CarService : ICarService
    {
        private int maxFuel;
        private int startCartridges;
        private float maxSpeed;

        private List<Car> gamers;
        private List<Bullet> _shorts;

        private readonly IMoveService _moveService;
        private readonly IPrizeService _prizeService;
        private readonly ILevelService _levelService;

        public CarService(IMoveService moveService,
            IPrizeService prizeService, ILevelService levelService)
        {
            _moveService = moveService;
            _prizeService = prizeService;
            _levelService = levelService;

            gamers = new List<Car>();
            _shorts = new List<Bullet>();

            maxSpeed = 100;
            maxFuel = 300;
            startCartridges = 10;
        }

        public Bullet[] GetBullets()
        {
            BulletsStep();
            return _shorts.ToArray();
        }

        public Bullet GetShot(string carId)
        {
            var gamer = GetCar(carId);

            if (gamer.Cartridges <= 0)
            {
                return null;
            }

            var shot = new Bullet
            {
                Id = Guid.NewGuid().ToString(),
                SizeX = 10,
                SizeY = 10,
                Speed = 0.9f,
                SpeedChange = gamer.SpeedChange * 2,
                PositionX = gamer.PositionX,
                PositionY = gamer.PositionY,
                Angle = gamer.Angle,
                OwnerId = carId
            };

            var shotOld = _shorts.FirstOrDefault(s => s.IsDeactivate);

            _shorts.Remove(shotOld);

            _shorts.Add(shot);

            var car = gamers.FirstOrDefault(c => c.Id.Equals(carId));
            car.Cartridges -= 1;


            return shot;
        }

        public void ResetCars()
        {
            try
            {
                gamers[0] = CreateCar(gamers[0].Id);
                gamers[1] = CreateCar(gamers[1].Id);
            }
            catch
            {
            }
        }

        public Car CreateCar(string clientId)
        {
            float positionX = 110; //150
            float positionY = 210; //90

            float sizeX = 36;
            float sizeY = 16;

            if (gamers.Count > 0)
            {
                positionY = gamers[0].PositionY + sizeY + 10;
            }

            var car = new Car()
            {
                Id = clientId,
                PositionX = positionX,
                PositionY = positionY,
                SizeX = sizeX,
                SizeY = sizeY,
                Speed = 0,
                Tire = true,
                MaxSpeed = maxSpeed,
                MaxFuel = maxFuel,
                SpeedChange = 2,
                Angle = 0,
                Fuel = maxFuel / 2,
                Cartridges = 0,
                MaxCartridges = startCartridges,
                IsFailingTire = false,
            };

            var rs = _levelService.GetLevelRightSequence();
            car.LevelsSequence = new bool[rs.Length];

            return car;
        }

        public Car GetEnemyCar(string clientId)
        {
            return gamers.FirstOrDefault(c => !c.Id.Equals(clientId));
        }

        public Car GetCar(string clientId)
        {
            return gamers.FirstOrDefault(c => c.Id.Equals(clientId));
        }

        public int AddCar(Car car)
        {
            gamers.Add(car);

            return gamers.Count();
        }

        public void UpdateCar(Car car)
        {
            var originCar = gamers.FirstOrDefault(c => c.Id.Equals(car.Id));
            if (originCar != null)
            {
                originCar = car;
            }
        }

        public void UpdateCarTexture(Car car)
        {
            var originCar = gamers.FirstOrDefault(c => c.Id.Equals(car.Id));
            if (originCar != null)
            {
                originCar.SpriteId = car.SpriteId;
                originCar.SpriteSizeX = car.SpriteSizeX;
                originCar.SpriteSizeY = car.SpriteSizeY;
            }
        }

        public List<Car> GetCars()
        {
            return gamers;
        }

        public void DeleteCar(string id)
        {
            var removedGamer = gamers.First(g => g.Id.Equals(id));
            gamers.Remove(removedGamer);
        }

        public Car MoveGamer(string clientId, int direction)
        {
            var car = GetCar(clientId);

            switch (direction)
            {
                case 0:
                    {
                        car = (Car)_moveService.UpdatePosition(car);
                        break;
                    }
                case 1:
                    {
                        car = (Car)_moveService.MoveForward(car);
                        break;
                    }
                case 2:
                    {
                        car = (Car)_moveService.MoveBack(car);
                        break;
                    }
                case 3:
                    {
                        car = (Car)_moveService.RotateRight(car);
                        break;
                    }
                case 4:
                    {
                        car = (Car)_moveService.RotateLeft(car);
                        break;
                    }
            }

            car = CheckAndUpdateWithLevelRigntSequenceCollision(car, _levelService.GetLevelRightSequence());
            car = CheckAndUpdateWithPrizeCollision(car, _prizeService.GetGamePrizes());
            car = CheckAndUpdateWithBulletCollision(car, _shorts.ToArray());

            var isLevelCollision = CheckAndUpdateWithLevelCollision(ref car, _levelService.GetLevel());
            if (isLevelCollision)
            {
                UpdateCar(car);
                return car;
            }

            var enemy = GetEnemyCar(car.Id);
            var isEnemyCollision = CheckAndUpdateWithEnemyCollision(ref car, enemy);
            if (isLevelCollision)
            {
                UpdateCar(car);
                return car;
            }

            UpdateCar(car);

            return car;
        }

        private void BulletsStep()
        {
            for (int i = 0; i < _shorts.Count; i++)
            {
                if (!_shorts[i].IsDeactivate)
                {
                    string collisionObjId = null;
                    var isCollizion = CollisionHelper.CheckCollision(_shorts[i],
                        out collisionObjId, _levelService.GetLevel());

                    if (isCollizion)
                    {
                        _shorts[i].IsDeactivate = true;
                    }
                    else
                    {
                        _shorts[i] = (Bullet)_moveService.UpdatePosition(_shorts[i]);
                    }
                }
            }
        }

        private bool CheckAndUpdateWithEnemyCollision(ref Car car, Car enemy)
        {
            string collisionObjId = null;

            var IsCollision = CollisionHelper.CheckCollision(car, out collisionObjId, enemy);
            if (IsCollision)
            {
                car = (Car)_moveService.ReturnPreviousState(car);
            }

            car.IsCollision = IsCollision;
            return IsCollision;
        }

        private Car CheckAndUpdateWithBulletCollision(Car car, Bullet[] bulllets)
        {
            string collisionObjId = null;

            var isCollizion = CollisionHelper.CheckCollision(car, out collisionObjId, bulllets);

            if (isCollizion)
            {
                var bullet = _shorts.FirstOrDefault(s => s.Id.Equals(collisionObjId));

                if (bullet != null && !bullet.OwnerId.Equals(car.Id) && !bullet.IsDeactivate)
                {
                    car.IsCollision = isCollizion;
                    car.Tire = false;
                    car.Speed = car.Speed < 0.4f ? car.Speed : 0.4f;

                    bullet.IsDeactivate = true;
                }
            }

            return car;
        }

        private Car CheckAndUpdateWithLevelRigntSequenceCollision(Car car, GameObject[] levelRight)
        {
            string collisionObjId = null;

            var isCollizion = CollisionHelper.CheckCollision(car, out collisionObjId, levelRight);
            if (isCollizion)
            {
                var id = int.Parse(collisionObjId);

                if (id == 1)
                {
                    car.LevelsSequence[4] = false;
                }

                if (car.LevelsSequence.Where(l => l == false).Count() == 0)
                {
                    car.RightLevelsSequence += 1;

                    car.LevelsSequence = new bool[levelRight.Length];
                }
                else
                {
                    car.LevelsSequence[id] = true;
                }
            }

            return car;
        }

        private bool CheckAndUpdateWithLevelCollision(ref Car car, GameObject[] levels)
        {
            string collisionObjId = null;

            var IsCollision = CollisionHelper.CheckCollision(car, out collisionObjId, levels);
            if (IsCollision)
            {
                car.Speed = 0.1f;
                car = (Car)_moveService.ReturnPreviousState(car);
            }

            car.IsCollision = IsCollision;
            return IsCollision;
        }

        private Car CheckAndUpdateWithPrizeCollision(Car car, GameObject[] prizes)
        {
            string collisionObjId = null;

            var isPrizeCollizion = CollisionHelper.CheckCollision(car, out collisionObjId, prizes);
            if (isPrizeCollizion)
            {
                if (car.PrizeId == null || !car.PrizeId.Equals(collisionObjId))
                {
                    car.PrizeId = collisionObjId;

                    var prize = _prizeService.GetGamePrize(collisionObjId);
                    if (prize != null && !prize.IsDeactivate)
                    {
                        car = Decorate(car, prize.Name);
                    }

                    _prizeService.UpdateGamePrize(collisionObjId, true);
                }
            }

            return car;
        }

        public Car Decorate(Car car, GameObjectType type)
        {
            switch (type)
            {
                case GameObjectType.Fuel:
                    {
                        car = (car.Fuel + 50) <= car.MaxFuel ? new FuelCarDecorator(car, car.Fuel + 50).GetCar() : new FuelCarDecorator(car, car.MaxFuel).GetCar();

                        break;
                    }
                case GameObjectType.Cartridge:
                    {
                        car = (car.Cartridges + 2) <= car.MaxCartridges ? new CartridgeCarDecorator(car, car.Cartridges + 2).GetCar() : new CartridgeCarDecorator(car, car.MaxCartridges).GetCar();
                        break;
                    }
                case GameObjectType.Tire:
                    car = new TireCarDecorator(car, true).GetCar();
                    break;
                default:
                    break;
            }
            return car;
        }

    }
}
