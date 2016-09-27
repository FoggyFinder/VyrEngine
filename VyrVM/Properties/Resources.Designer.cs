﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VyrVM.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("VyrVM.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap BrickGroutless0095_2_XL {
            get {
                object obj = ResourceManager.GetObject("BrickGroutless0095_2_XL", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///out vec4 color;
        ///
        ///void main()
        ///{
        ///	color = vec4(1.0f, 0.5f, 0.2f, 1.0f);
        ///}.
        /// </summary>
        internal static string FragmentShader {
            get {
                return ResourceManager.GetString("FragmentShader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform float multiplicator;
        ///
        ///in vec4 vertexColor;
        ///
        ///out vec4 color;
        ///
        ///void main()
        ///{
        ///	color = vertexColor * multiplicator;
        ///}.
        /// </summary>
        internal static string FragmentShaderColoredUniform {
            get {
                return ResourceManager.GetString("FragmentShaderColoredUniform", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///in vec2 TexCoord;
        ///
        ///out vec4 color;
        ///
        ///uniform sampler2D tex;
        ///uniform sampler2D tex2;
        ///
        ///void main()
        ///{
        ///	vec4 t = texture(tex, TexCoord);
        ///	vec4 t2 = texture(tex2, TexCoord);
        ///	color = t * 0.2f + t2 * 0.8f;
        ///}.
        /// </summary>
        internal static string FragmentShaderTex {
            get {
                return ResourceManager.GetString("FragmentShaderTex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Moss0177_1_XL {
            get {
                object obj = ResourceManager.GetObject("Moss0177_1_XL", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///
        ///void main()
        ///{
        ///	gl_Position = vec4(position.x, position.y, position.z, 1.0);
        ///}.
        /// </summary>
        internal static string VertexShader {
            get {
                return ResourceManager.GetString("VertexShader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec4 color;
        ///
        ///out vec4 vertexColor;
        ///
        ///void main()
        ///{
        ///	gl_Position = vec4(position, 1.0);
        ///	vertexColor = color;
        ///}.
        /// </summary>
        internal static string VertexShaderColored {
            get {
                return ResourceManager.GetString("VertexShaderColored", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec3 position;
        ///layout (location = 1) in vec2 texCoord;
        ///
        ///out vec2 TexCoord;
        ///
        ///void main()
        ///{
        ///	gl_Position = vec4(position.x, position.y, position.z, 1.0);
        ///	TexCoord = texCoord;
        ///}.
        /// </summary>
        internal static string VertexShaderTex {
            get {
                return ResourceManager.GetString("VertexShaderTex", resourceCulture);
            }
        }
    }
}
