using System.Windows.Forms;
using OpenGL4NET;

namespace VisualizationOpenGL
{
	public partial class FormOpenGL4 : Form
	{
		public OpenGL4NET.RenderingContext rc;

		public FormOpenGL4()
		{
			InitializeComponent();
			rc = RenderingContext.CreateContext(this);
			Cursor.Hide();
		}

	}
}
