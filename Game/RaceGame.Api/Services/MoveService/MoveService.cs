using System;
using OpenTK.Mathematics;
using PSP.GameCommon;

namespace PSP.GameAPI.Services.MoveService
{
    public class MoveService : IMoveService
    {
        public MovableGameObject RotateLeft(MovableGameObject moveObject)
        {
            moveObject.Angle += 0.08f;

            return moveObject;
        }

        public MovableGameObject RotateRight(MovableGameObject moveObject)
        {
            moveObject.Angle -= 0.08f;

            return moveObject;
        }

        public MovableGameObject MoveBack(MovableGameObject moveObject)
        {
            if (moveObject.Speed > -moveObject.MaxSpeed / 2 && moveObject.Fuel > 0 && moveObject.Tire) //вниз, движение назад
            {
                moveObject.Speed -= 0.04f;
                moveObject.Fuel -= Math.Abs(moveObject.Speed);
            }

            return moveObject;
        }

        public MovableGameObject MoveForward(MovableGameObject moveObject)
        {
            if (moveObject.Speed < moveObject.MaxSpeed && moveObject.Fuel > 0 && moveObject.Tire)  //вверх, вперед
            {
                moveObject.Speed += 0.04f;
                moveObject.Fuel -= Math.Abs(moveObject.Speed);
            }

            return moveObject;
        }

        public MovableGameObject UpdatePosition(MovableGameObject moveObject)
        {
            var vector = Vector2.Transform(Vector2.UnitX,
                Quaternion.FromEulerAngles(0, 0, moveObject.Angle)) * (moveObject.Speed * moveObject.SpeedChange);

            moveObject.OldPositionX = moveObject.PositionX;
            moveObject.OldPositionY = moveObject.PositionY;

            moveObject.PositionX += vector.X;
            moveObject.PositionY += vector.Y;

            return moveObject;
        }

        public MovableGameObject ReturnPreviousState(MovableGameObject moveObject)
        {
            moveObject.PositionX = moveObject.OldPositionX;
            moveObject.PositionY = moveObject.OldPositionY;

            return moveObject;
        }
    }
}
