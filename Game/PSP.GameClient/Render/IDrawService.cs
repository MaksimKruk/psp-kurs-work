using System.Drawing;
using OpenTK;
using PSP.GameCommon;
using PSP.GameCommon.GameObjects;

namespace PSP.GameClient.Render
{
    public interface IDrawService
    {
        /// <summary>
        /// Rendering of player indicators
        /// </summary>
        /// <param name="obj"></param>
        void DrawState(Car obj);

        void Draw(GameObject gameObject, Color color, int textureId);

        void DrawRectangle(Vector2 position, Vector2 size, Color color);

        int LoadSprite(string filePath, out float height, out float width);

        void DrawCircle(float x, float y, float radius, Color color);
    }
}
