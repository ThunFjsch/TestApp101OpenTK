using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp101
{

    class Shader: IDisposable
    {
        private bool disposedValue = false;

        public int Handle { get; private set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            int VertexShader, FragmentShader;
            string VertexShaderSource, FragmentShaderSource;
            
            using (StreamReader reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                VertexShaderSource = reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader(fragmentPath, Encoding.UTF8)) 
            { 
                FragmentShaderSource = reader.ReadToEnd();
            }

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            CompileCheckShader(ref VertexShader, ref FragmentShader, VertexShaderSource, FragmentShaderSource);

            LinkShader(VertexShader, FragmentShader);

            DetachDelete(VertexShader, FragmentShader);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        private void CompileCheckShader(ref int VertexShader, ref int FragmentShader, string VertexShaderSource, string FragmentShaderSource)
        {
            GL.CompileShader(VertexShader);

            string infoLogVert = GL.GetShaderInfoLog(VertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(FragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(FragmentShader);

            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);
        }

        private void LinkShader(int VertexShader, int FragmentShader)
        {
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);
        }

        private void DetachDelete(int VertexShader, int FragmentShader)
        {
            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
