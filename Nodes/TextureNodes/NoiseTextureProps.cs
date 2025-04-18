namespace blenderShaderGraph.Nodes.TextureNodes;

public record NoiseTextureProps(
    int ImgWidth = 1024,
    int ImgHeight = 1024,
    float Scale = 1,
    float Detail = 2,
    float Roughness = 0.5f
);
