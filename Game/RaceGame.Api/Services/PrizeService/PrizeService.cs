using System;
using System.Linq;
using PSP.GameAPI.Services.GameService;
using PSP.GameCommon;
using PSP.GameCommon.Factory;

namespace PSP.GameAPI.Services.PrizeService
{
    public class PrizeService : IPrizeService
    {
        private GameObject[] _gamePrizes;
        private int _prizeSize;
        private int _prizeCount;

        private readonly FuelPrizeCreator _fuelPrizeFactory;
        private readonly CartridgePrizeCreator _cartridgePrizeFactory;
        private readonly TirePrizeCreator _tirePrizeFactory;

        public PrizeService()
        {
            _tirePrizeFactory = new TirePrizeCreator();
            _cartridgePrizeFactory = new CartridgePrizeCreator();
            _fuelPrizeFactory = new FuelPrizeCreator();

            _prizeSize = 20;
            CreatePrizes();
        }

        private void CreatePrizes()
        {
            _gamePrizes = new GameObject[]
            {
                _fuelPrizeFactory.GetObject(_prizeSize, _prizeSize),
                _fuelPrizeFactory.GetObject(_prizeSize, _prizeSize),
                _fuelPrizeFactory.GetObject(_prizeSize, _prizeSize),
                _cartridgePrizeFactory.GetObject(_prizeSize, _prizeSize),
                _cartridgePrizeFactory.GetObject(_prizeSize, _prizeSize),
                _tirePrizeFactory.GetObject(_prizeSize, _prizeSize),
                _tirePrizeFactory.GetObject(_prizeSize, _prizeSize),
                _tirePrizeFactory.GetObject(_prizeSize, _prizeSize),
            };

            _prizeCount = _gamePrizes.Length;

            for (int i = 0; i < _gamePrizes.Length; i++)
            {
                _gamePrizes[i].IsDeactivate = true;
                _gamePrizes[i].Id = Guid.NewGuid().ToString();
            }
        }

        public void UpdateGamePrize(string priseId, bool isDeactivate)
        {
            var prize = _gamePrizes.FirstOrDefault(p => p.Id.Equals(priseId));

            if(prize != null)
            {
                prize.IsDeactivate = isDeactivate;
            }
        }

        public GameObject[] GetGamePrizes()
        {
            return _gamePrizes;
        }

        public Point[] GetPrizesState()
        {
            Point[] points = new Point[_prizeCount];

            for (int i = 0; i < _prizeCount; i++)
            {
                points[i] = new Point()
                {
                    PositionX = _gamePrizes[i].PositionX,
                    PositionY = _gamePrizes[i].PositionY,
                    IsDeactivate = _gamePrizes[i].IsDeactivate,
                };
            }

            return points;
        }

        public void RefreshPrizes(GameObject[] objects)
        {
            for (int i = 0; i < _gamePrizes.Length; i++)
            {
                _gamePrizes[i].IsDeactivate = false;
                _gamePrizes[i] = PositionHelper.RandomNoCollizionPosition(_gamePrizes[i], objects);
            }
        }

        public GameObject GetGamePrize(string id)
        {
            return _gamePrizes.FirstOrDefault(p => p.Id.Equals(id));
        }
    }
}
