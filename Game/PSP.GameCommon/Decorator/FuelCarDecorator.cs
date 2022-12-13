using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Decorator
{
    public class FuelCarDecorator : CarDecorator
    {
        public FuelCarDecorator(Car car, float fuel) : base(car)
        {
            car.Fuel = fuel;
            IsDecorate = false;
        }
    }
}
