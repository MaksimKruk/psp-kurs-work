using System.Collections.Generic;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;

namespace PSP.GameAPI.Services.GameService
{
    public interface IGameService
    {
        public Car AddGamer(string clientId);
        public List<GameObject> GetAllObjects();
        public void ResetGame();
    }
}
