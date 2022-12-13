using PSP.GameCommon;

namespace PSP.GameAPI.Services.LevelService
{
    public interface ILevelService
    {
        public void CreateLevel();
        public GameObject[] GetLevel();
        public GameObject[] GetLevelRightSequence();
    }
}
