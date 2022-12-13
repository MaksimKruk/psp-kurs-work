using System;
using OpenTK.Mathematics;
using PSP.GameAPI.Services.MoveService;
using PSP.GameCommon;

namespace PSP.GameAPI.Services.GameService
{
    public static class PositionHelper
    {
        private static Random _random = new Random();

        public static GameObject RandomNoCollizionPosition(GameObject currentObj, GameObject[] gameObjects)
        {
            string collisionObjId = null;

            do
            {
                currentObj = RandomPosition(currentObj);
            }
            while (CollisionHelper.CheckCollision(currentObj, out collisionObjId, gameObjects));

            return currentObj;
        }

        private static GameObject RandomPosition(GameObject obj)
        {
            var position = RandomPosition(new Vector2(50, 50), new Vector2(1100, 600));

            obj.PositionX = position.X;
            obj.PositionY = position.Y;

            return obj;
        }

        private static Vector2 RandomPosition(Vector2 min, Vector2 max)
        {
            Vector2 cord = new Vector2();

            cord.X = (float)(_random.NextDouble() * (max.X - min.X) + min.X);
            cord.Y = (float)(_random.NextDouble() * (max.Y - min.Y) + min.Y);

            return cord;
        }
    }
}
