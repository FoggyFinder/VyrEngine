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
using VyrCore.Graphics;
using VyrEngine;

namespace VyrVM
{
    public class MainViewModel : ViewModelBase
    {
        IGraphics graphics;
        IRenderer renderer;

        // Normal Shader
        IShaderProgram shaderProgram;

        // Colored Shader
        IShaderProgram shaderProgramColored;
        IShaderUniform<float> multiplicator;
        float uniformValue;

        // Textured Shader
        IShaderProgram shaderProgramTextured;
        ITexture2D brickTexture;
        ITexture2D mossTexture;
        IShaderUniform<int> sampler1;
        IShaderUniform<int> sampler2;

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
            var vertexAttribute = RendererHelper.attributeVertexPositionTexture<float, float>();

            /// When you have only positions you can either use the pos array directly or use the vertices array
            Vec3<float>[] pos =
            {
                new Vec3<float>(0.0f, -0.5f, 0.0f),
                new Vec3<float>(0.5f, -0.5f, 0.0f),
                new Vec3<float>(0.5f, 0.5f, 0.0f),
                new Vec3<float>(0.0f, 0.5f, 0.0f)
            };

            Vec2<float>[] tex =
            {
                new Vec2<float>(0.0f, 1.0f),
                new Vec2<float>(1.0f, 1.0f),
                new Vec2<float>(1.0f, 0.0f),
                new Vec2<float>(0.0f, 0.0f)
            };

            VertexPosTex<float, float>[] vertices =
            {
                new VertexPosTex<float, float>(pos[0], tex[0]),
                new VertexPosTex<float, float>(pos[1], tex[1]),
                new VertexPosTex<float, float>(pos[2], tex[2]),
                new VertexPosTex<float, float>(pos[3], tex[3])
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
            renderer.UseSRGBFramebuffer();

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

            // create textured shader
            ShaderSource[] sourcesTexture =
            {
                new ShaderSource(ShaderType.VertexShader, Properties.Resources.VertexShaderTex),
                new ShaderSource(ShaderType.PixelShader, Properties.Resources.FragmentShaderTex)
            };
            shaderProgramTextured = RendererHelper.createShaderProgram(graphics, sourcesTexture).Value;

            // Diffuse textures mostly srgb, normals mostly normal rgb
            // create texture
            var settings = new TextureSettings2D(TextureTargetFormat.SRGB8, TextureOriginalFormat.BGR, TextureWrapMode.MirroredRepeat,
                TextureWrapMode.MirroredRepeat, TextureMinifyingFilter.Linear, TextureMagnifyingFilter.Linear, Maybe<Color<float>>.Nothing);
            brickTexture = graphics.CreateTexture2D(settings, @"Resources\BrickGroutless0095_2_XL.jpg").Value;
            mossTexture = graphics.CreateTexture2D(settings, @"Resources\Moss0177_1_XL.jpg").Value;
            sampler1 = shaderProgramTextured.GetUniform<int>("tex");
            sampler2 = shaderProgramTextured.GetUniform<int>("tex2");

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
            brickTexture.Dispose();
            mossTexture.Dispose();

            shaderProgramTextured.Dispose();
            shaderProgramColored.Dispose();
            shaderProgram.Dispose();

            vb1.Dispose();
            vb2.Dispose();
            vb3.Dispose();

            renderer.Dispose();
        }

        void Update()
        {
            System.Diagnostics.Debug.WriteLine("Draw..");
            renderer.Begin();
            renderer.Clear();

            // render normal triangle
            renderer.UseShader(shaderProgram);
            renderer.UseVertexBuffer(vb1);
            renderer.DrawVertexBuffer(PrimitiveType.Triangles, 0, 3);

            // render textured triangle
            renderer.UseShader(shaderProgramTextured);
            renderer.UseVertexBuffer(vb2.VertexBuffer);
            //renderer.UseTexture(brickTexture); // Either use only one texture
            renderer.UseTextures(new ITexture2D[] { brickTexture, mossTexture }); // or multiple textures
            shaderProgramTextured.SetUniform(sampler1, 0); // the order of the textures is determined by setting the uniform to the right index of the texture
            shaderProgramTextured.SetUniform(sampler2, 1);
            renderer.DrawVertexBufferIndexed(PrimitiveType.Triangles, 6);

            // animate color
            uniformValue = uniformValue + 0.01f;
            uniformValue = uniformValue > 1.0f ? 1.0f : uniformValue;
            shaderProgramColored.SetUniform(multiplicator, uniformValue);

            // render colored triangle
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
