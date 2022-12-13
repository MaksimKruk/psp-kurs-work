namespace PSP.GameCommon.GameObjects
{
    public class Car : MovableGameObject
    {
        public virtual bool[] LevelsSequence { get; set; }
        public virtual int RightLevelsSequence { get; set; }
        public virtual bool IsFailingTire { get; set; }
        public virtual float MaxFuel { get; set; }
        public virtual int Cartridges { get; set; }
        public virtual int MaxCartridges { get; set; }
        public virtual bool IsCollision { get; set; }
        public virtual string PrizeId { get; set; }
    }
}
