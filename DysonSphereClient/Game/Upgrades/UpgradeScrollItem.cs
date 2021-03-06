﻿using Engine.Visualization.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Visualization;
using System.Drawing;
using Engine;
using System.Windows.Forms;

namespace DysonSphereClient.Game.Upgrades
{
	class UpgradeScrollItem:ScrollItem
	{
		protected override void InitObject(VisualizationProvider visualizationProvider, Input input)
		{
			base.InitObject(visualizationProvider, input);

			var btnRecreatePoints = new ViewButton();
			AddComponent(btnRecreatePoints);
			btnRecreatePoints.InitButton(RecreatePoints, "Buy", "", Keys.None);
			btnRecreatePoints.SetParams(20, 120, 60, 30, "RecreatePoints");
			btnRecreatePoints.InitTexture("textRB", "textRB");
		}

		private void RecreatePoints()
		{
			throw new NotImplementedException();
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(
				CursorOver
				? Color.YellowGreen
				: Color.Red);
			vp.Rectangle(X, Y, Width, Height);

			vp.SetColor(Color.White);
			vp.Print(X + 10, Y + 10, Name);
		}
	}
}
