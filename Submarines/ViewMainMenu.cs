using System;
using System.Windows.Forms;
using Engine;
using Engine.Visualization;

namespace Submarines
{
	internal class ViewMainMenu : ViewComponent
	{
		public Action OnStartGame;
        public Action OnStartGeometryEditor;
        public Action OnStartItemMapEditor;
        public Action OnStartItemMapRelationEditor;

        protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			SetParams(0, 0, visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight, "ViewMainMenu");

			var buttonStartGame = new ViewButton();
			AddComponent(buttonStartGame);
			buttonStartGame.InitButton(StartGame, "StartGame", "Запустить игру", Keys.E);
			buttonStartGame.SetParams(110, 15, 140, 25, "StartGame");
			buttonStartGame.InitTexture("textRB", "textRB");

			var buttonStartGeometryEditor = new ViewButton();
			AddComponent(buttonStartGeometryEditor);
			buttonStartGeometryEditor.InitButton(StartGeometryEditor, "StartGeometryEditor", "Запустить редактор геометрии", Keys.G);
			buttonStartGeometryEditor.SetParams(110, 55, 140, 25, "StartGeometryEditor");
			buttonStartGeometryEditor.InitTexture("textRB", "textRB");

            var buttonStartItemMapEditor = new ViewButton();
            AddComponent(buttonStartItemMapEditor);
            buttonStartItemMapEditor.InitButton(StartItemMapEditor, "StartItemMapEditor", "Запустить редактор карт", Keys.M);
            buttonStartItemMapEditor.SetParams(110, 95, 140, 25, "StartItemMapEditor");
            buttonStartItemMapEditor.InitTexture("textRB", "textRB");

            var buttonStartItemMapRelationEditor = new ViewButton();
            AddComponent(buttonStartItemMapRelationEditor);
            buttonStartItemMapRelationEditor.InitButton(StartItemMapRelationEditor, "StartItemMapRelationEditor", "Запустить редактор глобальной карты", Keys.N);
            buttonStartItemMapRelationEditor.SetParams(110, 135, 140, 25, "StartItemMapRelationEditor");
            buttonStartItemMapRelationEditor.InitTexture("textRB", "textRB");
        }

        private void StartGame()
		{
            OnStartGame?.Invoke();
        }

        private void StartGeometryEditor()
		{
			OnStartGeometryEditor?.Invoke();
		}

        private void StartItemMapEditor()
        {
            OnStartItemMapEditor?.Invoke();
        }
        private void StartItemMapRelationEditor() {
            OnStartItemMapRelationEditor?.Invoke();
        }
    }
}