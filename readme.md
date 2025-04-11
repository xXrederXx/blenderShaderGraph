# Blender Shader Graph (C# Replica)

This project is a C#-based replica of Blender's shader graph system. It allows procedural texture generation using modular, composable nodes similar to Blender's node editor.

Nodes can be connected through a JSON file, allowing rapid iteration without recompiling the C# application. Each node operates on images (`Bitmap`) and can output textures like diffuse, roughness, or normal maps.

## 🚀 Features
- Procedural node-based texture generation
- JSON scene file format for rapid prototyping
- Support for color mixing, procedural bricks, noise, bump maps, and more

## 📁 Project Structure
- **Nodes/** – Contains individual node logic (Brick, Noise, ColorRamp, etc.)
- **Util/** – Helpers for image manipulation and utilities
- **Examples/** – Executable examples and sample workflows
- **Benchmark/** – Some Benchmarks for some things

## 📜 Usage
1. Define a JSON file describing the node graph
2. Ajust the path in Program.cs 
```csharp
    GraphRunner.Run("./graph.json"); <-- Your Path Here
 ```
3. Output images will be saved to disk

```bash
> dotnet run
```

## ✅ Supported Nodes
- `BrickTexture`
- `NoiseTexture`
- `MixColor`
- `ColorRamp`
- `Bump`
- `Output`

---

# 📚 Node Documentation

## BrickTexture Node
**Generates a brick pattern with color and factor output maps.**

### Inputs (via `params`):
- `width` *(int)* – Output image width
- `height` *(int)* – Output image height
- `offset`, `offsetFrequency` *(float/int)* – Horizontal row offset - offset (0 to 1)
- `squash`, `squashFrequency` *(float/int)* – Brick squashing
- `color1`, `color2`, `colorMotar` *(string hex)* – Brick and mortar colors
- `motarSize`, `motarSmoothness` *(float)* – Mortar size and edge softness
- `bias` *(float)* – Brick color bias (0 to 1)
- `brickWidth`, `rowHeight` *(float)* – Brick dimensions

### Outputs:
- `ID.color` – The color image
- `ID.fac` – Factor/mask map

### JSON Example:
```json
{
  "id": "brick1",
  "type": "BrickTexture",
  "params": {
    "width": 1024,
    "height": 1024,
    "color1": "#ffffff",
    "color2": "#000000",
    "colorMotar": "#cccccc",
    "brickWidth": 40,
    "rowHeight": 20
  }
}
```

---

## NoiseTexture Node
**Generates a procedural noise texture.**

### Inputs:
- `width`, `height` *(int)* – Image size
- `size` *(float)* – Scale of noise
- `detail` *(float)* – Number of noise octaves
- `roughness` *(float)* – Noise roughness

### Outputs:
- `ID` – The generated noise image

### JSON Example:
```json
{
  "id": "noise1",
  "type": "NoiseTexture",
  "params": {
    "size": 10,
    "detail": 2.5,
    "roughness": 0.5
  }
}
```

---

## MixColor Node
**Blends two images together with a blend mode and a factor.**

### Inputs:
- `a`, `b` *(Bitmap reference)* – Input images to blend
- `factor` *(Bitmap reference or float)* – Blend factor (0 to 1)
- `mode` *(string)* – Blend mode: `mix`, `hue`, `saturation`, `value`

### Outputs:
- `ID` – The blended image

### JSON Example:
```json
{
  "id": "mixed",
  "type": "MixColor",
  "params": {
    "a": "brick1.color",
    "b": "noise1",
    "factor": 0.5,
    "mode": "mix"
  }
}
```

---

## ColorRamp Node
**Maps input grayscale image values to a color gradient.**

### Inputs:
- `img` *(Bitmap reference)* – Image to apply ramp to
- `colorStops` *(array)* – List of colors and positions
- `mode` *(string)* – Ramp mode: `linear`, `constant`

### Outputs:
- `ID` – The color-ramped image (in-place)

### JSON Example:
```json
{
  "id": "ramped",
  "type": "ColorRamp",
  "params": {
    "img": "noise1",
    "mode": "linear",
    "colorStops": [
      { "color": "#000000", "position": 0 },
      { "color": "#ffffff", "position": 1 }
    ]
  }
}
```

---

## Bump Node
**Converts a grayscale image into a normal map (bump effect).**

### Inputs:
- `heightMap` *(Bitmap reference)* – Grayscale heightmap
- `strength` *(float)* – Intensity of bump effect
- `distance` *(float)* – Sampling distance
- `invert` *(bool)* – Invert bump direction
- `isDX` *(bool)* – Output DirectX vs OpenGL normal format

### Outputs:
- `ID` – Normal map

### JSON Example:
```json
{
  "id": "normal1",
  "type": "Bump",
  "params": {
    "heightMap": "noise1",
    "strength": 0.3,
    "invert": false,
    "isDX": true
  }
}
```

---

## Output Node
**Saves an image to disk.**

### Inputs:
- `image` *(Bitmap reference)* – Image to save
- `filename` *(string)* – Output file name

### Outputs:
- *(none)*

### JSON Example:
```json
{
  "id": "save1",
  "type": "Output",
  "params": {
    "image": "mixed",
    "filename": "diffuse.png"
  }
}
```

---

## 📂 Skeleton for Future Node Docs
```
## <NodeName> Node
**<What the node does>**

### Inputs:
- `<paramName>` *(<type>)* – <Description>

### Outputs:
- `<id>` – <What the output is>

### JSON Example:
```json
{
  "id": "<id>",
  "type": "<NodeName>",
  "params": {
    ...
  }
}
```
```

