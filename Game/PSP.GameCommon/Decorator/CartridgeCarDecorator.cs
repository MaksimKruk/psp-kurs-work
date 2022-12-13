using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Decorator
{
    public class CartridgeCarDecorator : CarDecorator
    {
        public CartridgeCarDecorator(Car car, int cartridges) : base(car)
        {
            car.Cartridges = cartridges;
            IsDecorate = false;
        }
    }
}
