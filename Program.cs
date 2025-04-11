using blenderShaderGraph.Nodes.TextureNodes;
using blenderShaderGraph.Nodes.VectorNodes;

BumpNode.GenerateBump(new(BrickTextureNode.GenerateTexture(new()).fac)).Save("b.png");
