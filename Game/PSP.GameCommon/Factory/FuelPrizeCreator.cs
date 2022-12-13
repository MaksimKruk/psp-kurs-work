using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Factory
{
    public class FuelPrizeCreator : PrizeCreator
    {
        public override GameObject GetObject(float w, float h)
        {
            var obj = new Fuel
            {
                SizeX = w,
                SizeY = h
            };

            return obj;
        }
    }
}
