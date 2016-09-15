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
        IShaderProgram shaderProgramColored;
        IShaderUniform<float> multiplicator;
        float uniformValue;

        IVertexBuffer vb1;
        IVertexBufferIndexed vb2;
        IVertexBufferIndexed vb3;

        int width;
        int height;

        /// <summary>
        /// Creates a vertex buffer by using a float array
        /// </summary>
        void CreateVertexBuffer1()
        {
            float[] vertices =
{
                -0.5f, -0.5f, 0.0f,
                0.0f, -0.5f, 0.0f,
                -0.5f, 0.5f, 0.0f
            };
            // create an attribute by specifying vertex component size (Vec1 = 1, Vec2 = 2, Vec3 = 3), data type, stride and byte offset
            var vertexAttribute = new VertexAttribute(3, DataType.Single, 3 * sizeof(float), 0);

            // creates vertex buffer object only with vertex data and an attribute to describe the vertex flow
            vb1 = graphics.CreateVertexBuffer(vertices, BufferUsage.StaticDraw, new VertexAttribute[] { vertexAttribute });
        }

        /// <summary>
        /// Creates a indexed vertex buffer by using a vec3 array
        /// </summary>
        void CreateVertexBuffer2()
        {
            // Either create the attribute manually or use the built-in function
            //var vertexAttribute = new VertexAttribute(3, DataType.Single, 3 * sizeof(float), 0);
            var vertexAttribute = RendererHelper.attributeVertexPosition<float>();

            /// When you have only positions you can either use the pos array directly or use the vertices array
            Vec3<float>[] pos =
            {
                new Vec3<float>(0.0f, -0.5f, 0.0f),
                new Vec3<float>(0.5f, -0.5f, 0.0f),
                new Vec3<float>(0.5f, 0.5f, 0.0f),
                new Vec3<float>(0.0f, 0.5f, 0.0f)
            };

            VertexPosition<float>[] vertices =
            {
                new VertexPosition<float>(pos[0]),
                new VertexPosition<float>(pos[1]),
                new VertexPosition<float>(pos[2]),
                new VertexPosition<float>(pos[3])
            };

            uint[] indices =
            {
                0, 1, 2,
                0, 2, 3
            };

            // create vertex buffer object with vertex data, an attribute and indices
            vb2 = graphics.CreateVertexBufferIndexed(vertices, indices, BufferUsage.StaticDraw, vertexAttribute.Value);
        }

        void CreateVertexBuffer3()
        {
            var vertexAttribute = RendererHelper.attributeVertexPositionColor<float, float>();
            VertexPositionColor<float, float>[] vertices =
            {
                new VertexPositionColor<float, float>(new Vec3<float>(0.0f, -0.5f, 0.0f), new Color<float>(1.0f, 0.0f, 0.0f, 1.0f)),
                new VertexPositionColor<float, float>(new Vec3<float>(-0.5f, 0.5f, 0.0f), new Color<float>(0.0f, 1.0f, 0.0f, 1.0f)),
                new VertexPositionColor<float, float>(new Vec3<float>(0.0f, 0.5f, 0.0f), new Color<float>(0.0f, 0.0f, 1.0f, 1.0f))
            };

            int[] indices =
            {
                0, 1, 2
            };

            vb3 = graphics.CreateVertexBufferIndexed(vertices, indices, BufferUsage.StaticDraw, vertexAttribute.Value);
        }

        /// <summary>
        /// Initializes the graphics engine, the renderer, a plain shader program, vertex buffer and the viewport for a given handle.
        /// </summary>
        void InitializeSurface(IntPtr handle)
        {
            System.Diagnostics.Debug.WriteLine("Initialize Engine..");
            // initialize graphics and a renderer using a window handle
            graphics = Engine.initializeGraphics(RenderingAPI.OpenGL);
            renderer = graphics.CreateRenderer(handle);

            // Create shader program by using shader sources
            ShaderSource[] sources =
            {
                new ShaderSource(ShaderType.VertexShader, Properties.Resources.VertexShader),
                new ShaderSource(ShaderType.PixelShader, Properties.Resources.FragmentShader)
            };
            shaderProgram = RendererHelper.createShaderProgram(graphics, sources).Value;

            // create shader program manually by creating shaders
            using (IShader vertexShader = graphics.CreateShader(ShaderType.VertexShader, Properties.Resources.VertexShaderColored).Value,
                 pixelShader = graphics.CreateShader(ShaderType.PixelShader, Properties.Resources.FragmentShaderColoredUniform).Value)
            {
                shaderProgramColored = graphics.CreateShaderProgram(new IShader[] { vertexShader, pixelShader }).Value;
            }

            // use uniform values for shaders
            multiplicator = shaderProgramColored.GetUniform<float>("multiplicator");
            uniformValue = 0.0f;
            shaderProgramColored.SetUniform(multiplicator, uniformValue);

            CreateVertexBuffer1();

            CreateVertexBuffer2();

            CreateVertexBuffer3();

            renderer.ClearColor(new Color<float>(0.2f, 0.1f, 0.2f, 0.1f));
            renderer.Viewport(new Vec2<short>(0, 0), new Size<short>((short) Width, (short) Height));
        }

        void Closing()
        {
            shaderProgram.Dispose();
            vb1.Dispose();
            vb2.Dispose();
            renderer.Dispose();
        }

        void Update()
        {
            System.Diagnostics.Debug.WriteLine("Draw..");
            renderer.Begin();
            renderer.Clear();

            renderer.UseShader(shaderProgram);

            renderer.UseVertexBuffer(vb1);
            renderer.DrawVertexBuffer(PrimitiveType.Triangles, 0, 3);

            renderer.UseVertexBuffer(vb2.VertexBuffer);
            renderer.DrawVertexBufferIndexed(PrimitiveType.Triangles, 6);

            uniformValue = uniformValue + 0.01f;
            uniformValue = uniformValue > 1.0f ? 1.0f : uniformValue;
            shaderProgramColored.SetUniform(multiplicator, uniformValue);

            renderer.UseShader(shaderProgramColored);

            renderer.UseVertexBuffer(vb3.VertexBuffer);
            renderer.DrawVertexBufferIndexed(PrimitiveType.Triangles, 3);

            renderer.SwapBuffers();

            // end probably useless.. don't know if possible failures are due to wpf
            //renderer.End();
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
