using PSP.GameCommon;

namespace PSP.GameAPI.Services.PrizeService
{
    public interface IPrizeService
    {
        GameObject[] GetGamePrizes();
        Point[] GetPrizesState();
        void RefreshPrizes(GameObject[] objects);
        void UpdateGamePrize(string prizeId, bool isDeactivate);
        GameObject GetGamePrize(string id);
    }
}
