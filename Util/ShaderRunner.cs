using System.Drawing;
using System.Drawing.Imaging;
using blenderShaderGraph.Types;
using blenderShaderGraph.Util;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

public static class ShaderRunner
{
    static GameWindow? _window;
    static bool _initialized = false;

    public static MyColor[,] RunShaderToBitmap(
        string fragShaderPath,
        int width,
        int height,
        Dictionary<string, float> uniforms
    )
    {
        InitOpenGLContext(width, height);

        int shader = CompileShader(fragShaderPath);
        int quadVAO = CreateQuad();

        GL.UseProgram(shader);

        foreach (var kv in uniforms)
        {
            int loc = GL.GetUniformLocation(shader, kv.Key);
            if (loc != -1)
                GL.Uniform1(loc, kv.Value);
        }

        GL.Uniform1(GL.GetUniformLocation(shader, "width"), (float)width);
        GL.Uniform1(GL.GetUniformLocation(shader, "height"), (float)height);

        int fbo = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

        int tex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, tex);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            width,
            height,
            0,
            OpenTK.Graphics.OpenGL4.PixelFormat.Rgba,
            PixelType.UnsignedByte,
            IntPtr.Zero
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Linear
        );

        GL.FramebufferTexture2D(
            FramebufferTarget.Framebuffer,
            FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D,
            tex,
            0
        );

        GL.Viewport(0, 0, width, height);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.BindVertexArray(quadVAO);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        GL.Flush();

        byte[] pixels = new byte[width * height * 4];
        GL.ReadPixels(
            0,
            0,
            width,
            height,
            OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
            PixelType.UnsignedByte,
            pixels
        );

        Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        var data = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            bmp.PixelFormat
        );
        System.Runtime.InteropServices.Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
        bmp.UnlockBits(data);
        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

        // Cleanup
        GL.DeleteFramebuffer(fbo);
        GL.DeleteTexture(tex);
        GL.DeleteVertexArray(quadVAO);
        GL.DeleteProgram(shader);

        return bmp.GetMyPixles();
    }

    private static void InitOpenGLContext(int width, int height)
    {
        if (_initialized)
            return;

        var native = new NativeWindowSettings()
        {
            Size = new Vector2i(width, height),
            StartVisible = false,
            Title = "HiddenGL",
        };

        _window = new GameWindow(GameWindowSettings.Default, native);
        _window.MakeCurrent();
        _initialized = true;
    }

    private static int CompileShader(string fragPath)
    {
        string vertexSource =
            @"
            #version 330 core
            layout(location = 0) in vec2 aPos;
            out vec2 TexCoords;
            void main() {
                TexCoords = (aPos + 1.0) / 2.0;
                gl_Position = vec4(aPos, 0.0, 1.0);
            }
        ";

        string fragSource = File.ReadAllText(fragPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexSource);
        GL.CompileShader(vertexShader);
        CheckShader(vertexShader);

        int fragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShader, fragSource);
        GL.CompileShader(fragShader);
        CheckShader(fragShader);

        int program = GL.CreateProgram();
        GL.AttachShader(program, vertexShader);
        GL.AttachShader(program, fragShader);
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
        if (status == 0)
        {
            string log = GL.GetProgramInfoLog(program);
            throw new Exception("Shader linking failed: " + log);
        }

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragShader);

        return program;
    }

    private static void CheckShader(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            string log = GL.GetShaderInfoLog(shader);
            throw new Exception("Shader compile error:\n" + log);
        }
    }

    private static int CreateQuad()
    {
        float[] quad = { -1f, -1f, 1f, -1f, 1f, 1f, -1f, -1f, 1f, 1f, -1f, 1f };

        int vao = GL.GenVertexArray();
        int vbo = GL.GenBuffer();
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            quad.Length * sizeof(float),
            quad,
            BufferUsageHint.StaticDraw
        );
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        return vao;
    }
}
