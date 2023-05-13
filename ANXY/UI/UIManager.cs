﻿using ANXY.EntityComponent.Components;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System;

namespace ANXY.UI
{
    internal class UIManager
    {
        public event Action PauseToggled;

        private PauseMenu _pauseMenu;
        private ControlsMenu _controlsMenu;
        private FpsOverlay _fpsOverlay;
        private float _fps;
        private Desktop _desktop;

        private LastUiState _lastUiState = LastUiState.PauseMenu;

        ///Singleton Pattern
        private static readonly Lazy<UIManager> lazy = new(() => new UIManager());


        /// <summary>
        ///     Singleton Pattern return the only instance there is
        /// </summary>
        public static UIManager Instance => lazy.Value;
        private UIManager()
        {
            _desktop = new Desktop();
            _pauseMenu = new PauseMenu();
            _pauseMenu.ResumePressed += OnResumeBtnPressed;
            _pauseMenu.ControlsPressed += OnControlsBtnPressed;
            _pauseMenu.ExitGamePressed += OnExitGamePressed;

            _controlsMenu = new ControlsMenu();
            _controlsMenu.ReturnPressed += OnReturnClicked;
            _controlsMenu.LoadDefaultsPressed += OnLoadDefaultsClicked;
            _controlsMenu.SaveChangesPressed += OnSaveChangesClicked;

            _desktop.Root = _pauseMenu;
            _desktop.Root.Visible = false;
            PlayerInputController.Instance.GamePausedChanged += OnGamePausedChanged;
            PlayerInputController.Instance.ShowFpsKeyPressed += OnShowFpsKeyPressed;

            _fpsOverlay = new FpsOverlay();
        }

        public void UpdateFPS(GameTime gameTime)
        {
            if (_desktop.Root == _fpsOverlay && _desktop.Root.Visible)
            {
                _fps = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
                _fpsOverlay.FpsValue = _fps;
                _fpsOverlay.Update();
            }
        }

        public void Draw()
        {
            _desktop.Render();
        }

        private void SwitchUiViews()
        {
            if (_lastUiState == LastUiState.FpsOverlay && _desktop.Root == _fpsOverlay)
            {
                _desktop.Root = _pauseMenu;
                _desktop.Root.Visible = true;
            }
            else if (_lastUiState == LastUiState.FpsOverlay && _desktop.Root != _fpsOverlay)
            {
                _desktop.Root = _fpsOverlay;
                _desktop.Root.Visible = true;
            }
            else
            {
                _desktop.Root = _pauseMenu;
                _desktop.Root.Visible = !_desktop.Root.Visible;
            }
        }

        public void OnResumeBtnPressed()
        {
            SwitchUiViews();
            PauseToggled?.Invoke();
        }

        public void OnControlsBtnPressed()
        {
            _controlsMenu.LoadButtonLayout();
            _desktop.Root = _controlsMenu;
        }

        public void OnGamePausedChanged(bool gamePaused)
        {
            SwitchUiViews();
        }

        public void OnReturnClicked()
        {
            PlayerInputController.Instance.LoadUserSettings();
            _desktop.Root = _pauseMenu;
        }

        public void OnLoadDefaultsClicked()
        {
            PlayerInputController.Instance.ResetToDefaults();
        }

        public void OnSaveChangesClicked()
        {
            PlayerInputController.Instance.Save();
        }
        private void OnExitGamePressed()
        {
            Environment.Exit(0);
        }

        private void OnShowFpsKeyPressed()
        {
            if (_lastUiState == LastUiState.FpsOverlay)
            {
                _desktop.Root = _pauseMenu;
                _lastUiState = LastUiState.PauseMenu;
            }
            else if (!_desktop.Root.Visible)
            {
                _desktop.Root = _fpsOverlay;
                _lastUiState = LastUiState.FpsOverlay;
            }
        }
    }

    public enum LastUiState
    {
        PauseMenu,
        FpsOverlay
    }
}
