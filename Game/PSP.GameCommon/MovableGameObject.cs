namespace PSP.GameCommon
{
    public class MovableGameObject : GameObject
    {
        public virtual float Speed { get; set; }
        public virtual float MaxSpeed { get; set; }
        public virtual float Fuel { get; set; }
        public virtual bool Tire { get; set; }
        public virtual float SpeedChange { get; set; }
        public virtual float OldPositionX { get; set; }
        public virtual float OldPositionY { get; set; }
    }
}
