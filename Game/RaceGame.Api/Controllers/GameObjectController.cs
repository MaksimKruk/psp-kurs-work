using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PSP.GameAPI.Services.GameService;
using PSP.GameAPI.Services.LevelService;
using PSP.GameAPI.Services.PrizeService;
using PSP.GameCommon;

namespace PSP.GameAPI.Controllers
{
    [Route("api/game-object")]
    [ApiController]
    public class GameObjectController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILevelService _levelService;
        private readonly IPrizeService _prizeService;

        public GameObjectController(
            IGameService gameService, 
            ILevelService levelService, 
            IPrizeService prizeService)
        {
            _gameService = gameService;
            _levelService = levelService;
            _prizeService = prizeService;
        }

        [HttpPut("reset")]
        public IActionResult ResetGame()
        {
            _gameService.ResetGame();

            return Ok();
        }

        [HttpGet("all")]
        public List<GameObject> GetAllGameObjects()
        {
            return _gameService.GetAllObjects();
        }

        [HttpGet("prizes")]
        public GameObject[] GetPrizes()
        {
            return _prizeService.GetGamePrizes();
        }

        [HttpGet("prizes/state")]
        public Point[] GetPrizesState()
        {
            return _prizeService.GetPrizesState();
        }

        [HttpGet("level")]
        public List<GameObject> GetLevel()
        {
            return _levelService.GetLevel().ToList();
        }

        [HttpGet("level/right")]
        public List<GameObject> GetLevelRightSequence()
        {
            return _levelService.GetLevelRightSequence().ToList();
        }
    }
}
