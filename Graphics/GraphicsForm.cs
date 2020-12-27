using System.Windows.Forms;
using System.Threading;
namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        
        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            initialize();
            programTimer.Enabled = true;
        }
        void initialize()
        {
            renderer.Initialize();   
        }
        
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Update();
            renderer.Draw();
        }

        private void programTimer_Tick(object sender, System.EventArgs e)
        {
            simpleOpenGlControl1.Refresh();
        }

        private void simpleOpenGlControl1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 'd')
            {
                renderer.TranslateShape(new GlmNet.vec3(1f, 0, 0));
            }
            if (e.KeyChar == 'a')
            {
                renderer.TranslateShape(new GlmNet.vec3(-1f, 0, 0));
            }
            if(e.KeyChar == 's')
            {
                renderer.RotateShape(new GlmNet.vec3(0, 1, 0));
            }
            if (e.KeyChar == 'w')
            {
                renderer.ScaleShape(new GlmNet.vec3(1, 1, 1));
            }
            simpleOpenGlControl1.Refresh();
        }

    }
}
