using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using PSP.GameClient.ApiCaller;
using PSP.GameClient.Render;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;
using PSP.GameCommon.Utility;

namespace PSP.GameClient.Client
{
    public class ClientService : IClientService
    {
        private readonly INetworkService _networkService;
        private readonly IDrawService _drawService;

        private GameObject _bg;
        private GameObject[] _gamePrizes;
        private Bullet[] _bullets;
        private List<GameObject> _level;
        private List<GameObject> _levelRightSequence;
        private Car _gamer;
        private Car _enemyGamer;
        private bool isGamerCreated;

        private int _enemyTextureId;
        private int _tireTextureId;
        private int _fuelTextureId;
        private int _cartriggeTextureId;
        private int _bgTextureId;

        private int _explosionTextureId;

        Timer timer;

        public ClientService()
        {
            _networkService = new NetworkService();
            _drawService = new DrawService();

            _bg = new GameObject()
            {
                PositionY = 315,
                PositionX = 540,
                SizeX = 1080,
                SizeY = 630,
            };
        }

        public bool IsGameEnd()
        {
            if (_enemyGamer == null)
            {
                return false;
            }

            return _gamer.RightLevelsSequence >= ItemsScope.RightLevelsSequence 
                   || _enemyGamer.RightLevelsSequence >= ItemsScope.RightLevelsSequence;
        }

        public bool IsCurrentClientWinner()
        {
            return _gamer.RightLevelsSequence >= ItemsScope.RightLevelsSequence;
        }

        public void ResetGame()
        {
            _networkService.ResetGame();
        }

        private void UpdatePrizes(object obj)
        {
            var state = _networkService.GetPrizesState();
            for (int i = 0; i < _gamePrizes.Length; i++)
            {
                _gamePrizes[i].PositionX = state[i].PositionX;
                _gamePrizes[i].PositionY = state[i].PositionY;
                _gamePrizes[i].IsDeactivate = state[i].IsDeactivate;
            }
        }

        public void GetGameObjects()
        {
            _level = _networkService.GetLevel();
            _levelRightSequence = _networkService.GetLevelRightSequence();
            _gamePrizes = _networkService.GetPrizes();
        }

        /// <summary>
        /// Since client starts
        /// </summary>
        /// <returns></returns>
        public bool ConnectClient()
        {
            var clientId = Guid.NewGuid().ToString();

            var carsCount = _networkService.GetCars().Count;
            if (carsCount == 2)
            {
                return false;
            }

            _gamer = _networkService.CreateGamer(clientId);

            float height = 0;
            float width = 0;
            _gamer.SpriteId = _drawService.LoadSprite("carGreen.png", out height, out width);
            _gamer.SpriteSizeX = width;
            _gamer.SpriteSizeY = height;
            _networkService.UpdateGamerTexture(_gamer);

            _enemyTextureId = _drawService.LoadSprite("carRed.png", out height, out width);
            _cartriggeTextureId = _drawService.LoadSprite("bullet.png", out height, out width);
            _tireTextureId = _drawService.LoadSprite("tire.png", out height, out width);
            _fuelTextureId = _drawService.LoadSprite("fuel.png", out height, out width);
            _bg.SpriteId = _drawService.LoadSprite("RACE2.png", out height, out width);

            isGamerCreated = _gamer != null;

            return isGamerCreated;
        }

        public void ClientAction(Key key)
        {
            var keyValue = KeyToCode(key);

            if (keyValue == 5)
            {
                _networkService.GetShot(_gamer.Id);
            }
            else
            {
                _gamer = _networkService.MoveGamer(_gamer.Id, keyValue);
            }            
        }

        public void Update()
        {
            if(isGamerCreated)
            {
                _gamer = _networkService.MoveGamer(_gamer.Id, 0);
                _enemyGamer = _networkService.GetEnemyGamer(_gamer.Id);
                UpdatePrizes(null);
                _bullets = _networkService.GetBullets();
            }
        }

        public void Draw()
        {
            if (isGamerCreated)
            {
                _drawService.Draw(_bg, Color.White, _bg.SpriteId);

                foreach (var obj in _gamePrizes)
                {
                    if (!obj.IsDeactivate)
                    {
                        if (obj.Name == GameObjectType.Cartridge)
                        {
                            _drawService.Draw(obj, Color.White, _cartriggeTextureId);
                        }
                        else if (obj.Name == GameObjectType.Tire)
                        {
                            _drawService.Draw(obj, Color.White, _tireTextureId);
                        }
                        else if (obj.Name == GameObjectType.Fuel)
                        {
                            _drawService.Draw(obj, Color.White, _fuelTextureId);
                        }
                        else
                        {
                            _drawService.Draw(obj, Color.Yellow, 0);
                        }
                    }
                }    

                if (_enemyGamer != null)
                {
                    _drawService.Draw(_enemyGamer, Color.White, _enemyTextureId);
                }

                if (_gamer.IsCollision)
                {
                    _drawService.Draw(_gamer, Color.Red, _gamer.SpriteId);
                }
                else
                {
                    _drawService.Draw(_gamer, Color.White, _gamer.SpriteId);
                }

                for (int i = 0; i < _bullets.Length; i++)
                {
                    if (!_bullets[i].IsDeactivate)
                    {
                        _drawService.DrawCircle(_bullets[i].PositionX,
                        _bullets[i].PositionY, _bullets[i].SizeX / 2, Color.Black);
                    }
                }

                _drawService.DrawState(_gamer);
            }
        }

        /// <summary>
        /// Player movement
        /// </summary>
        /// <param name="key">Key code</param>
        /// <returns>Move code</returns>
        private int KeyToCode(Key key)
        {
            int code;
            switch (key)
            {
                case Key.W:
                    code = 1;
                    break;
                case Key.S:
                    code = 2;
                    break;
                case Key.A:
                    code = 3;
                    break;
                case Key.D:
                    code = 4;
                    break;
                case Key.E:
                    code = 5;
                    break;
                default:
                    code = 1;
                    break;
            }

            return code;
        }

        public void EndGame()
        {
            _networkService.DeleteGamer(_gamer.Id);
        }
    }
}
