using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Nodes.VectorNodes;

BumpNode.GenerateBump(new(BrickTextureNode.Generate(new()).fac)).Save("b.png");
