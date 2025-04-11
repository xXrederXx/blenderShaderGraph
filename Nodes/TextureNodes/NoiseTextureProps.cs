namespace blenderShaderGraph.Nodes.TextureNodes;

public record NoiseTextureProps(
    int imgWidth = 1024,
    int imgHeight = 1024,
    float size = 1,
    float detail = 2,
    float roughness = 0.5f
);
