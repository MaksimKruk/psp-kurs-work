using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Factory
{
    public class CartridgePrizeCreator : PrizeCreator
    {
        public override GameObject GetObject(float w, float h)
        {
            var obj = new Cartridge
            {
                SizeX = w,
                SizeY = h
            };

            return obj;
        }
    }
}
