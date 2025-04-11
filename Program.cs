using blenderShaderGraph.Benchmark;

BrickTextureNodeBench brickTextureNodeBench = new();

brickTextureNodeBench.Run(10);
new BrickTextureNodeBench().Run(10);

brickTextureNodeBench.img.c?.Save("c.png");
brickTextureNodeBench.img.f?.Save("f.png");
