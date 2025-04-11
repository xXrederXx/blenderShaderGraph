using System.Drawing;
using blenderShaderGraph.Benchmark;
using blenderShaderGraph.Nodes.TextureNodes;

new NoiseTextureBench().Run(10);
new NoiseTextureBench().Run(10);

NoiseTextureNode.ApplyNoise(new(1024, 1024)).Save("c.png");
