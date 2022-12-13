using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Factory
{
    public class TirePrizeCreator : PrizeCreator
    {
        public override GameObject GetObject(float w, float h)
        {
            var tire = new Tire
            {
                SizeX = w,
                SizeY = h
            };

            return tire;
        }
    }
}
