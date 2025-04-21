using blenderShaderGraph.Types;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

public static class ShaderRunner
{
    static GameWindow _window;
    static bool _initialized = false;

    static int _fbo = -1;
    static int _tex = -1;
    static int _quadVAO = -1;
    static int _currentWidth = 0;
    static int _currentHeight = 0;

    static Dictionary<string, int> _shaderCache = new();

    public static MyColor[,] RunShaderToColorArray(
        string fragShaderPath,
        int width,
        int height,
        Dictionary<string, float> uniforms
    )
    {
        InitOpenGLContext(width, height);
        EnsureFramebuffer(width, height);

        int shader = LoadOrGetShader(fragShaderPath);
        GL.UseProgram(shader);

        foreach (var kv in uniforms)
        {
            int loc = GL.GetUniformLocation(shader, kv.Key);
            if (loc != -1)
                GL.Uniform1(loc, kv.Value);
        }

        GL.Uniform1(GL.GetUniformLocation(shader, "width"), (float)width);
        GL.Uniform1(GL.GetUniformLocation(shader, "height"), (float)height);

        GL.Viewport(0, 0, width, height);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.BindVertexArray(_quadVAO);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        GL.Flush();

        byte[] pixels = new byte[width * height * 4];
        GL.ReadPixels(0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);

        var result = new MyColor[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = ((height - 1 - y) * width + x) * 4; // flip Y
                byte b = pixels[index + 0];
                byte g = pixels[index + 1];
                byte r = pixels[index + 2];
                byte a = pixels[index + 3];
                result[x, y] = new MyColor(a, r, g, b);
            }
        }

        return result;
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

    private static void EnsureFramebuffer(int width, int height)
    {
        if (_fbo == -1 || _tex == -1 || _currentWidth != width || _currentHeight != height)
        {
            if (_fbo != -1)
                GL.DeleteFramebuffer(_fbo);
            if (_tex != -1)
                GL.DeleteTexture(_tex);

            _fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            _tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _tex);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                width,
                height,
                0,
                PixelFormat.Rgba,
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
                _tex,
                0
            );

            _currentWidth = width;
            _currentHeight = height;

            if (_quadVAO == -1)
                _quadVAO = CreateQuad();
        }
    }

    private static int LoadOrGetShader(string fragPath)
    {
        if (_shaderCache.TryGetValue(fragPath, out int cached))
            return cached;

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

        _shaderCache[fragPath] = program;
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
