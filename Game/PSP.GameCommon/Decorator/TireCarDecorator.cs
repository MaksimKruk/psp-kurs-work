using PSP.GameCommon.GameObjects;

namespace PSP.GameCommon.Decorator
{
    public class TireCarDecorator : CarDecorator
    {
        public TireCarDecorator(Car car, bool tire) : base(car)
        {
            car.Tire = tire;
            IsDecorate = false;
        }
    }
}
