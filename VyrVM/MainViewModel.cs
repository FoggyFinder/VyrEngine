using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using VyrCore;
using VyrEngine;

namespace VyrVM
{
    public class MainViewModel : ViewModelBase
    {
        IGraphics graphics;
        IRenderer renderer;
        IShaderProgram shaderProgram;

        IVertexBuffer vb1;
        IVertexBuffer vb2;

        int width;
        int height;

        void InitializeSurface(IntPtr handle)
        {
            System.Diagnostics.Debug.WriteLine("Initialize Engine..");
            // initialize graphics and a renderer using a window handle
            graphics = Engine.initializeGraphics(RenderingAPI.OpenGL);
            renderer = graphics.CreateRenderer(handle);

            // create shader by using their source code
            using (IShader vertexShader = graphics.CreateShader(ShaderType.VertexShader, LoadTemporaryVertexShader()).Value,
                 pixelShader = graphics.CreateShader(ShaderType.PixelShader, LoadTemporaryFragmentShader()).Value)
            {
                shaderProgram = graphics.CreateShaderProgram(new IShader[] { vertexShader, pixelShader }).Value;
            }

            float[] vertices =
            {
                -0.5f, -0.5f, 0.0f,
                0.0f, -0.5f, 0.0f,
                -0.5f, 0.5f, 0.0f
            };

            // create an attribute by specifying vertex component size (Vec1 = 1, Vec2 = 2, Vec3 = 3), data type, stride and byte offset
            var vertexAttribute = new VertexAttribute(3, DataType.Single, 3 * sizeof(float), 0);
            // creates vertex buffer object only with vertex data and an attribute to describe the vertex flow
            vb1 = graphics.CreateVertexBuffer(vertices, BufferUsage.StaticDraw, vertexAttribute);

            float[] vertices2 =
            {
                0.0f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.5f, 0.5f, 0.0f,
                0.0f, 0.5f, 0.0f
            };

            uint[] indices =
            {
                0, 1, 2,
                0, 2, 3
            };

            // create vertex buffer object with vertex data, an attribute and indices
            vb2 = graphics.CreateVertexBufferIndexed(vertices2, indices, BufferUsage.StaticDraw, vertexAttribute);

            renderer.ClearColor(new Color(0.2f, 0.3f, 0.2f, 0.1f));
            renderer.Viewport(new Vec2<short>(0, 0), new Size<short>((short) Width, (short) Height));
        }

        void Closing()
        {
            shaderProgram.Dispose();
            vb1.Dispose();
            renderer.Dispose();
        }

        string LoadTemporaryVertexShader()
        {
            return Properties.Resources.VertexShader;
        }

        string LoadTemporaryFragmentShader()
        {
            return Properties.Resources.FragmentShader;
        }

        void Update()
        {
            System.Diagnostics.Debug.WriteLine("Draw..");
            renderer.Begin();
            renderer.Clear();

            renderer.UseShader(shaderProgram);

            renderer.UseVertexBuffer(vb1);
            renderer.DrawVertexBuffer(PrimitiveType.Triangles, 0, 3);

            renderer.UseVertexBuffer(vb2);
            renderer.DrawVertexBufferIndexed(PrimitiveType.Triangles, 6);

            renderer.SwapBuffers();
            renderer.End();
        }

        public ICommand InitializeSurfaceCommand { get { return new RelayCommand<IntPtr>((parent) => InitializeSurface(parent)); } }

        public ICommand UpdateSurface { get { return new RelayCommand(() => Update()); } }

        public ICommand ClosingCommand { get { return new RelayCommand(() => Closing()); } }

        public int Width {
            get { return width; }
            set { width = value; RaisePropertyChanged("Width"); }
        }

        public int Height
        {
            get { return height; }
            set { height = value;  RaisePropertyChanged("Height"); }
        }
    }
}
