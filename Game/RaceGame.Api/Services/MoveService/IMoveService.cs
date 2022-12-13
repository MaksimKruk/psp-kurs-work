using PSP.GameCommon;

namespace PSP.GameAPI.Services.MoveService
{
    public interface IMoveService
    {
        public MovableGameObject RotateLeft(MovableGameObject moveObject);
        public MovableGameObject RotateRight(MovableGameObject moveObject);
        public MovableGameObject MoveBack(MovableGameObject moveObject);
        public MovableGameObject MoveForward(MovableGameObject moveObject);
        public MovableGameObject UpdatePosition(MovableGameObject moveObject);
        public MovableGameObject ReturnPreviousState(MovableGameObject moveObject);
    }
}
