using System.Windows.Input;

namespace PSP.GameClient.Client
{
    public interface IClientService
    {
        void GetGameObjects();
        bool ConnectClient();
        void ClientAction(Key key);
        void Update();
        void Draw();
        void EndGame();
        bool IsGameEnd();
        bool IsCurrentClientWinner();
        void ResetGame();
    }
}
