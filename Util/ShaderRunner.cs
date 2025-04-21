// Cleaned-up and enhanced version of ShaderRunner.cs
// Issues fixed, resources managed, additional uniform support added

using System.Runtime.InteropServices;
using blenderShaderGraph.Types;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

public static class ShaderRunner
{
    private static GameWindow _window;
    private static bool _initialized = false;

    private static int _fbo = -1;
    private static int _tex = -1;
    private static int _quadVAO = -1;
    private static int _currentWidth = 0;
    private static int _currentHeight = 0;

    private static int _nextTextureUnit = 1;
    private static readonly Dictionary<string, int> _shaderCache = new();
    private static readonly List<int> _tempTextures = new();

    public static void PreloadShaders(IEnumerable<string> shaderPaths)
    {
        InitOpenGLContext(1024, 1024);
        EnsureFramebuffer(1024, 1024);
        foreach (var path in shaderPaths)
        {
            if (!_shaderCache.ContainsKey(path))
                LoadOrGetShader(path);
        }
    }

    public static MyColor[,] RunShaderToColorArray(
        string fragShaderPath,
        int width,
        int height,
        Dictionary<string, object> uniforms
    )
    {
        InitOpenGLContext(width, height);
        EnsureFramebuffer(width, height);

        int shader = LoadOrGetShader(fragShaderPath);
        GL.UseProgram(shader);

        SetUniforms(shader, uniforms);

        // Pass resolution as uniforms
        GL.Uniform1(GL.GetUniformLocation(shader, "width"), (float)width);
        GL.Uniform1(GL.GetUniformLocation(shader, "height"), (float)height);

        // Render to FBO
        GL.Viewport(0, 0, width, height);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.BindVertexArray(_quadVAO);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        GL.Flush();

        // Read pixels
        byte[] pixels = new byte[width * height * 4];
        GL.ReadPixels(0, 0, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        // Convert to color array (Y flipped)
        var result = new MyColor[width, height];
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            int index = ((height - 1 - y) * width + x) * 4;
            result[x, y] = new MyColor(
                pixels[index + 0],
                pixels[index + 1],
                pixels[index + 2],
                pixels[index + 3]
            );
        }

        CleanupTempTextures();
        return result;
    }

    private static void SetUniforms(int shader, Dictionary<string, object> uniforms)
    {
        foreach (var (key, val) in uniforms)
        {
            int loc = GL.GetUniformLocation(shader, key);
            if (loc == -1)
                continue;

            switch (val)
            {
                case float f:
                    GL.Uniform1(loc, f);
                    break;
                case int i:
                    GL.Uniform1(loc, i);
                    break;
                case bool b:
                    GL.Uniform1(loc, b ? 1 : 0);
                    break;
                case Vector2 v2:
                    GL.Uniform2(loc, v2);
                    break;
                case Vector3 v3:
                    GL.Uniform3(loc, v3);
                    break;
                case Vector4 v4:
                    GL.Uniform4(loc, v4);
                    break;
                case Matrix4 m4:
                    GL.UniformMatrix4(loc, false, ref m4);
                    break;
                case MyColor c:
                    GL.Uniform4(loc, c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
                    break;
                case float[,] floats:
                    UploadUniformTexture(key, floats);
                    break;
                case bool[,] bools:
                    UploadUniformTexture(key, ConvertBoolToFloatArray(bools));
                    break;
                case MyColor[,] colors:
                    UploadUniformTexture(key, colors);
                    break;
                default:
                    throw new Exception($"Unsupported uniform type for '{key}': {val?.GetType()}");
            }
        }
    }

    private static void UploadUniformTexture(string name, float[,] data)
    {
        int width = data.GetLength(0),
            height = data.GetLength(1);
        int texId = GL.GenTexture();
        _tempTextures.Add(texId);

        GL.ActiveTexture(TextureUnit.Texture0 + _nextTextureUnit);
        GL.BindTexture(TextureTarget.Texture2D, texId);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.R32f,
            width,
            height,
            0,
            PixelFormat.Red,
            PixelType.Float,
            data
        );
        SetTexParams();

        int shader = GL.GetInteger(GetPName.CurrentProgram);
        int loc = GL.GetUniformLocation(shader, name);
        GL.Uniform1(loc, _nextTextureUnit);
        _nextTextureUnit++;
    }

    private static void UploadUniformTexture(string name, MyColor[,] data)
    {
        int width = data.GetLength(0),
            height = data.GetLength(1);
        byte[] pixels = new byte[width * height * 4];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            int i = (y * width + x) * 4;
            MyColor c = data[x, y];
            pixels[i + 0] = c.R;
            pixels[i + 1] = c.G;
            pixels[i + 2] = c.B;
            pixels[i + 3] = c.A;
        }

        int texId = GL.GenTexture();
        _tempTextures.Add(texId);

        GL.ActiveTexture(TextureUnit.Texture0 + _nextTextureUnit);
        GL.BindTexture(TextureTarget.Texture2D, texId);
        GL.TexImage2D(
            TextureTarget.Texture2D,
            0,
            PixelInternalFormat.Rgba,
            width,
            height,
            0,
            PixelFormat.Rgba,
            PixelType.UnsignedByte,
            pixels
        );
        SetTexParams();

        int shader = GL.GetInteger(GetPName.CurrentProgram);
        int loc = GL.GetUniformLocation(shader, name);
        GL.Uniform1(loc, _nextTextureUnit);
        _nextTextureUnit++;
    }

    private static void SetTexParams()
    {
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Nearest
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)TextureMagFilter.Nearest
        );
    }

    private static float[,] ConvertBoolToFloatArray(bool[,] src)
    {
        int w = src.GetLength(0),
            h = src.GetLength(1);
        var result = new float[w, h];
        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
            result[x, y] = src[x, y] ? 1f : 0f;
        return result;
    }

    private static void CleanupTempTextures()
    {
        foreach (var tex in _tempTextures)
            GL.DeleteTexture(tex);
        _tempTextures.Clear();
        _nextTextureUnit = 1;
    }

    private static void InitOpenGLContext(int width, int height)
    {
        if (_initialized)
            return;

        // Create a GameWindow instance with the desired settings
        var native = new NativeWindowSettings
        {
            Size = new Vector2i(width, height),
            StartVisible = false,
            Title = "HiddenGL",
        };

        _window = new GameWindow(GameWindowSettings.Default, native);

        // Make the OpenGL context current to initialize OpenGL bindings
        _window.MakeCurrent();

        // Now the context is initialized, we can proceed with OpenGL operations
        _initialized = true;
    }

    private static void EnsureFramebuffer(int width, int height)
    {
        if (_fbo != -1 && (_currentWidth == width && _currentHeight == height))
            return;

        if (_fbo != -1)
            GL.DeleteFramebuffer(_fbo);
        if (_tex != -1)
            GL.DeleteTexture(_tex);

        _fbo = GL.GenFramebuffer();
        _tex = GL.GenTexture();

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
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
        SetTexParams();

        GL.FramebufferTexture2D(
            FramebufferTarget.Framebuffer,
            FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D,
            _tex,
            0
        );

        FramebufferErrorCode status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferErrorCode.FramebufferComplete)
            throw new Exception("Framebuffer is not complete: " + status);

        _currentWidth = width;
        _currentHeight = height;

        if (_quadVAO == -1)
            _quadVAO = CreateQuad();
    }

    private static int LoadOrGetShader(string fragPath)
    {
        if (_shaderCache.TryGetValue(fragPath, out var cached))
            return cached;

        string vertexSource =
            @"#version 330 core
layout(location = 0) in vec2 aPos;
out vec2 TexCoords;
void main() {
    TexCoords = (aPos + 1.0) / 2.0;
    gl_Position = vec4(aPos, 0.0, 1.0);
}";

        string fragSource = File.ReadAllText(fragPath);

        int vert = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vert, vertexSource);
        GL.CompileShader(vert);
        CheckShader(vert);

        int frag = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(frag, fragSource);
        GL.CompileShader(frag);
        CheckShader(frag);

        int prog = GL.CreateProgram();
        GL.AttachShader(prog, vert);
        GL.AttachShader(prog, frag);
        GL.LinkProgram(prog);

        GL.GetProgram(prog, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
            throw new Exception("Shader link error: " + GL.GetProgramInfoLog(prog));

        GL.DeleteShader(vert);
        GL.DeleteShader(frag);

        _shaderCache[fragPath] = prog;
        return prog;
    }

    private static void CheckShader(int shader)
    {
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int ok);
        if (ok == 0)
            throw new Exception("Shader compile error: " + GL.GetShaderInfoLog(shader));
    }

    private static int CreateQuad()
    {
        float[] quad = { -1, -1, 1, -1, 1, 1, -1, -1, 1, 1, -1, 1 };
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
