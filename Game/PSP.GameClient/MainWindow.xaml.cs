using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Wpf;
using PSP.GameClient.Client;

namespace PSP.GameClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IClientService _clientService = new ClientService();

        public MainWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 6
            };
            OpenTkControl.Start(settings);
        }

        /// <summary>
        /// Drawing game objects and checking the player for victory
        /// </summary>
        /// <param name="delta">Time</param>
        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            #region GL finctions

            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, Height, 0, 0d, 1d);

            #endregion

            _clientService.Update();
            _clientService.Draw();

            if (_clientService.IsGameEnd())
            {
                var message = _clientService.IsCurrentClientWinner() ? "You are the winner!" : "You have lost...";
                var result = MessageBox.Show(message, "Client", MessageBoxButton.OK);
                switch (result)
                {
                    case MessageBoxResult.OK:
                    {
                        _clientService.EndGame();
                        Close();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Connects the player to the server, gets the current state of the game
        /// </summary>
        private void OpenTkControl_OnReady()
        {
            #region GL functions

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            #endregion

            var isCreated = false;

            try
            {
                isCreated = _clientService.ConnectClient();
            }
            catch
            {
                MessageBox.Show("Some problems with the server. Please try to connect later.", "Client", MessageBoxButton.OK);
            }

            if (!isCreated)
            {
                var result = MessageBox.Show("Game already started. Please try to connect later.", "Client", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        OpenTkControl_OnReady();
                        break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                }
            }

            _clientService.GetGameObjects();
        }

        /// <summary>
        /// Updates the state of the game through the player's action
        /// </summary>
        /// <param name="sender">Client</param>
        /// <param name="e">Key param</param>
        private void OpenTkControl_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.W || e.Key == Key.S || e.Key == Key.D || e.Key == Key.E)
            {
                _clientService.ClientAction(e.Key);
            }
        }

        /// <summary>
        /// End game and close window
        /// </summary>
        /// <param name="sender">Client</param>
        /// <param name="e">Route param</param>
        private void EndGame_Button_Click(object sender, RoutedEventArgs e)
        {
            _clientService.EndGame();
            Close();
        }
    }
}
