using PSP.GameCommon;
using System.Collections.Generic;
using PSP.GameCommon.GameObjects;

namespace PSP.GameClient.ApiCaller
{
    public interface INetworkService
    {
        List<GameObject> GetGameObjects(string gamerId);
        Car CreateGamer(string clientId);
        Car GetEnemyGamer(string currentGamerId);
        Car MoveGamer(string gamerId, int direction);
        void DeleteGamer(string clientId);
        void UpdateGamerTexture(Car car);
        List<GameObject> GetLevel();
        GameObject[] GetPrizes();
        List<Car> GetCars();
        Point[] GetPrizesState();
        void ResetGame();
        List<GameObject> GetLevelRightSequence();
        Bullet[] GetBullets();
        void GetShot(string clientId);
    }
}
