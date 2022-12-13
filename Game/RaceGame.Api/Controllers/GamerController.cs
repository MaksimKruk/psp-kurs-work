using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PSP.GameAPI.Services.CarService;
using PSP.GameAPI.Services.GameService;
using PSP.GameCommon.GameObjects;

namespace PSP.GameAPI.Controllers
{
    [Route("api/gamer")]
    [ApiController]
    public class GamerController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ICarService _carService;

        public GamerController(
            IGameService gameService, 
            ICarService carService)
        {
            _gameService = gameService;
            _carService = carService;
        }

        [HttpPost]
        public Car Post([FromBody] string clientId)
        {
            var resultCar = _gameService.AddGamer(clientId);

            return resultCar;
        }

        [HttpGet("{clientId}/enemy")]
        public Car Get(string clientId)
        {
            return _carService.GetEnemyCar(clientId);
        }

        [HttpPut("{clientId}/shot")]
        public IActionResult PutGetShot(string clientId)
        {
            _carService.GetShot(clientId);

            return Ok();
        }

        [HttpGet("bullets")]
        public Bullet[] GetBullets()
        {
            return _carService.GetBullets();
        }

        [HttpGet]
        public List<Car> Get()
        {
            return _carService.GetCars();
        }

        [HttpPut("{clientId}/move/{direction}")]
        public Car PutMove(string clientId, int direction)
        {
            var resultCar = _carService.MoveGamer(clientId, direction);

            return resultCar;
        }

        [HttpPut("texture")]
        public IActionResult PutTexture([FromBody] Car car)
        {
            _carService.UpdateCarTexture(car);

            return Ok();
        } 

        [HttpDelete("{clientId}")]
        public void Delete(string clientId)
        {
            _carService.DeleteCar(clientId);
        }
    }
}
