# Benchmark Summary For Nodes

<p align="center">
  <a href="#brick-texture">Brick Texture</a> •
  <a href="#bump">Bump</a> •
  <a href="#color-ramp">Color Ramp</a> •
  <a href="#mask-texture">Mask Texture</a> •
  <a href="#mix-color">Mix Color</a> •
  <a href="#noise-texture">Noise Texture</a> •
  <a href="#output">Output</a> •
  <a href="#resize">Resize</a> •
  <a href="#texture-coordinate">Texture Coordinate</a> •
  <a href="#tile-fixer">Tile Fixer</a> •
  <a href="#benchmark-summary-for-nodes">Back to top</a>
</p>

## Brick-Texture

| Method       | size | smooth | force |        Mean |      Error |     StdDev |      Median |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| ------------ | ---- | ------ | ----- | ----------: | ---------: | ---------: | ----------: | -------: | -------: | -------: | ----------: |
| BrickTexture | 128  | 0      | False |    37.28 us |   0.414 us |   0.367 us |    37.18 us |   8.1177 |   2.3804 |        - |   132.01 KB |
| BrickTexture | 128  | 0      | True  |    38.13 us |   0.468 us |   0.390 us |    38.11 us |   8.1177 |   2.3193 |        - |   131.96 KB |
| BrickTexture | 128  | 1      | False |    36.62 us |   0.226 us |   0.189 us |    36.61 us |   8.1177 |   2.5635 |        - |   132.01 KB |
| BrickTexture | 128  | 1      | True  |    40.67 us |   0.430 us |   0.382 us |    40.79 us |   8.1177 |   2.3193 |        - |   131.96 KB |
| BrickTexture | 512  | 0      | False |   721.60 us |   3.522 us |   2.941 us |   721.01 us | 502.9297 | 499.0234 | 499.0234 |     2072 KB |
| BrickTexture | 512  | 0      | True  |   717.83 us |   2.297 us |   1.918 us |   717.96 us | 502.9297 | 499.0234 | 499.0234 |  2071.46 KB |
| BrickTexture | 512  | 1      | False |   717.69 us |   2.486 us |   1.941 us |   717.32 us | 502.9297 | 499.0234 | 499.0234 |  2072.55 KB |
| BrickTexture | 512  | 1      | True  |   735.62 us |  14.389 us |  21.974 us |   728.08 us | 502.9297 | 499.0234 | 499.0234 |  2073.01 KB |
| BrickTexture | 1024 | 0      | False | 2,291.88 us |  35.973 us |  42.823 us | 2,277.16 us | 496.0938 | 496.0938 | 496.0938 |  8202.09 KB |
| BrickTexture | 1024 | 0      | True  | 2,852.51 us |  54.318 us |  68.695 us | 2,838.05 us | 484.3750 | 484.3750 | 484.3750 |  8204.62 KB |
| BrickTexture | 1024 | 1      | False | 2,668.75 us |  77.128 us | 224.986 us | 2,755.99 us | 496.0938 | 496.0938 | 496.0938 |     8203 KB |
| BrickTexture | 1024 | 1      | True  | 2,321.70 us |  14.472 us |  13.537 us | 2,319.20 us | 496.0938 | 496.0938 | 496.0938 |  8201.97 KB |
| BrickTexture | 2024 | 0      | False | 9,410.14 us | 102.987 us |  80.406 us | 9,415.06 us | 500.0000 | 500.0000 | 500.0000 | 32009.05 KB |
| BrickTexture | 2024 | 0      | True  | 9,439.38 us |  62.689 us |  58.639 us | 9,424.67 us | 500.0000 | 500.0000 | 500.0000 | 32009.05 KB |
| BrickTexture | 2024 | 1      | False | 9,293.76 us | 132.001 us | 117.016 us | 9,267.65 us | 500.0000 | 500.0000 | 500.0000 | 32009.04 KB |
| BrickTexture | 2024 | 1      | True  | 9,310.19 us |  58.945 us |  55.137 us | 9,329.95 us | 500.0000 | 500.0000 | 500.0000 | 32009.04 KB |


## Bump

| Method | size |         Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| ------ | ---- | -----------: | --------: | --------: | -------: | -------: | -------: | ----------: |
| Bump   | 128  |     87.25 us |  1.306 us |  1.158 us |   4.1504 |   0.8545 |        - |    68.15 KB |
| Bump   | 512  |    950.30 us |  2.464 us |  2.184 us | 250.9766 | 249.0234 | 249.0234 |  1037.68 KB |
| Bump   | 1024 |  3,555.42 us | 10.278 us |  8.583 us | 394.5313 | 394.5313 | 394.5313 |  4113.97 KB |
| Bump   | 2024 | 16,963.57 us | 86.868 us | 72.539 us | 875.0000 | 875.0000 | 875.0000 | 16007.19 KB |


## Color-Ramp

| Method           | size | mode     |         Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| ---------------- | ---- | -------- | -----------: | ---------: | ---------: | -------: | -------: | -------: | ----------: |
| ColorRamp2Stops  | 128  | Linear   |     70.67 us |   0.163 us |   0.144 us |   4.1504 |   0.8545 |        - |    68.41 KB |
| ColorRamp8Stops  | 128  | Linear   |     95.44 us |   0.190 us |   0.168 us |   4.1504 |   0.8545 |        - |    68.59 KB |
| ColorRamp16Stops | 128  | Linear   |     95.29 us |   0.274 us |   0.229 us |   4.1504 |   0.8545 |        - |    68.77 KB |
| ColorRamp2Stops  | 128  | Constant |     48.91 us |   0.108 us |   0.101 us |   4.2114 |   0.8545 |        - |    68.34 KB |
| ColorRamp8Stops  | 128  | Constant |     70.56 us |   0.306 us |   0.287 us |   4.1504 |   0.8545 |        - |    68.57 KB |
| ColorRamp16Stops | 128  | Constant |     74.43 us |   0.213 us |   0.178 us |   4.1504 |   0.9766 |        - |    68.75 KB |
| ColorRamp2Stops  | 512  | Linear   |    821.11 us |   1.364 us |   1.209 us | 250.9766 | 249.0234 | 249.0234 |  1038.03 KB |
| ColorRamp8Stops  | 512  | Linear   |  1,045.20 us |   4.500 us |   3.757 us | 250.0000 | 248.0469 | 248.0469 |   1036.8 KB |
| ColorRamp16Stops | 512  | Linear   |  1,096.38 us |   2.797 us |   2.479 us | 250.0000 | 248.0469 | 248.0469 |  1036.07 KB |
| ColorRamp2Stops  | 512  | Constant |    594.93 us |   2.726 us |   2.276 us | 250.9766 | 249.0234 | 249.0234 |  1038.03 KB |
| ColorRamp8Stops  | 512  | Constant |    806.81 us |   2.149 us |   2.010 us | 250.9766 | 249.0234 | 249.0234 |  1038.08 KB |
| ColorRamp16Stops | 512  | Constant |    853.55 us |   2.083 us |   1.847 us | 250.9766 | 249.0234 | 249.0234 |  1037.76 KB |
| ColorRamp2Stops  | 1024 | Linear   |  3,017.03 us |   7.727 us |   6.452 us | 394.5313 | 394.5313 | 394.5313 |  4109.69 KB |
| ColorRamp8Stops  | 1024 | Linear   |  3,863.46 us |  41.892 us |  41.143 us | 394.5313 | 394.5313 | 394.5313 |  4107.49 KB |
| ColorRamp16Stops | 1024 | Linear   |  4,006.91 us |   7.009 us |   5.853 us | 390.6250 | 390.6250 | 390.6250 |  4107.67 KB |
| ColorRamp2Stops  | 1024 | Constant |  2,150.95 us |   6.810 us |   6.370 us | 394.5313 | 394.5313 | 394.5313 |  4111.12 KB |
| ColorRamp8Stops  | 1024 | Constant |  2,957.74 us |  10.186 us |   9.528 us | 394.5313 | 394.5313 | 394.5313 |  4110.34 KB |
| ColorRamp16Stops | 1024 | Constant |  3,132.78 us |  17.262 us |  14.415 us | 394.5313 | 394.5313 | 394.5313 |  4109.83 KB |
| ColorRamp2Stops  | 2024 | Linear   | 12,155.47 us |  70.455 us |  65.903 us | 937.5000 | 937.5000 | 937.5000 |  16007.3 KB |
| ColorRamp8Stops  | 2024 | Linear   | 15,291.66 us | 292.205 us | 286.985 us | 937.5000 | 937.5000 | 937.5000 | 16007.48 KB |
| ColorRamp16Stops | 2024 | Linear   | 16,101.77 us | 241.512 us | 225.910 us | 875.0000 | 875.0000 | 875.0000 | 16007.64 KB |
| ColorRamp2Stops  | 2024 | Constant |  8,740.51 us |  57.547 us |  51.014 us | 937.5000 | 937.5000 | 937.5000 | 16007.28 KB |
| ColorRamp8Stops  | 2024 | Constant | 11,968.20 us |  73.960 us |  61.760 us | 937.5000 | 937.5000 | 937.5000 | 16007.47 KB |
| ColorRamp16Stops | 2024 | Constant | 12,617.30 us |  56.179 us |  52.550 us | 937.5000 | 937.5000 | 937.5000 | 16007.64 KB |
 

## Mask-Texture

| Method      | size | type       | numDots | distCalc |        Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| ----------- | ---- | ---------- | ------- | -------- | ----------: | ---------: | ---------: | -------: | -------: | -------: | ----------: |
| MaskTexture | 128  | SquareFade | 3       | False    |    70.79 us |   0.521 us |   0.462 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | SquareFade | 3       | True     |    71.59 us |   0.711 us |   0.665 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | SquareFade | 10      | False    |   216.31 us |   1.190 us |   1.113 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | SquareFade | 10      | True     |   238.50 us |   1.856 us |   1.736 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | SquareFade | 20      | False    |   492.52 us |   7.042 us |   6.587 us |   3.9063 |        - |        - |    65.19 KB |
| MaskTexture | 128  | SquareFade | 20      | True     |   479.62 us |   2.657 us |   2.485 us |   3.9063 |        - |        - |    65.18 KB |
| MaskTexture | 128  | SquareFade | 50      | False    | 1,159.42 us |   5.515 us |   5.158 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | SquareFade | 50      | True     | 1,198.52 us |   8.096 us |   7.573 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | EaseInSine | 3       | False    |   123.86 us |   0.783 us |   0.732 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | EaseInSine | 3       | True     |   109.37 us |   0.703 us |   0.657 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | EaseInSine | 10      | False    |   425.44 us |   1.933 us |   1.808 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | EaseInSine | 10      | True     |   386.61 us |   2.192 us |   2.050 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | EaseInSine | 20      | False    |   838.91 us |   3.969 us |   3.519 us |   3.9063 |        - |        - |    65.19 KB |
| MaskTexture | 128  | EaseInSine | 20      | True     |   781.45 us |   6.583 us |   6.158 us |   3.9063 |        - |        - |    65.19 KB |
| MaskTexture | 128  | EaseInSine | 50      | False    | 2,161.26 us |  17.591 us |  16.454 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | EaseInSine | 50      | True     | 1,956.71 us |  14.990 us |  14.021 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | Square     | 3       | False    |    66.63 us |   0.391 us |   0.346 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | Square     | 3       | True     |    72.75 us |   0.581 us |   0.544 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | Square     | 10      | False    |   244.94 us |   2.087 us |   1.952 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | Square     | 10      | True     |   244.29 us |   1.207 us |   1.008 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | Square     | 20      | False    |   461.82 us |   2.353 us |   2.201 us |   3.9063 |        - |        - |    65.18 KB |
| MaskTexture | 128  | Square     | 20      | True     |   493.72 us |   4.104 us |   3.839 us |   3.9063 |        - |        - |    65.19 KB |
| MaskTexture | 128  | Square     | 50      | False    | 1,095.23 us |   6.533 us |   6.111 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | Square     | 50      | True     | 1,232.93 us |   6.916 us |   6.131 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | Cube       | 3       | False    |    70.55 us |   0.435 us |   0.386 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | Cube       | 3       | True     |    75.15 us |   0.478 us |   0.424 us |   3.9063 |        - |        - |    64.24 KB |
| MaskTexture | 128  | Cube       | 10      | False    |   218.28 us |   1.052 us |   0.984 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | Cube       | 10      | True     |   254.37 us |   1.374 us |   1.286 us |   3.9063 |        - |        - |    64.65 KB |
| MaskTexture | 128  | Cube       | 20      | False    |   463.67 us |   2.585 us |   2.291 us |   3.9063 |        - |        - |    65.19 KB |
| MaskTexture | 128  | Cube       | 20      | True     |   509.83 us |   2.934 us |   2.744 us |   3.9063 |        - |        - |    65.18 KB |
| MaskTexture | 128  | Cube       | 50      | False    | 1,155.36 us |   4.375 us |   4.093 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 128  | Cube       | 50      | True     | 1,275.86 us |   5.083 us |   3.969 us |   3.9063 |        - |        - |    66.21 KB |
| MaskTexture | 512  | SquareFade | 3       | False    |   297.88 us |   2.723 us |   2.547 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | SquareFade | 3       | True     |   307.15 us |   3.321 us |   3.106 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | SquareFade | 10      | False    |   754.08 us |   6.126 us |   5.730 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | SquareFade | 10      | True     |   747.17 us |   4.280 us |   4.004 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | SquareFade | 20      | False    | 1,347.43 us |  10.293 us |   9.628 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | SquareFade | 20      | True     | 1,301.63 us |   9.351 us |   8.289 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | SquareFade | 50      | False    | 2,874.50 us |  22.896 us |  21.417 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 512  | SquareFade | 50      | True     | 2,976.13 us |  28.050 us |  26.238 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 512  | EaseInSine | 3       | False    |   424.59 us |   4.607 us |   4.310 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | EaseInSine | 3       | True     |   396.88 us |   4.491 us |   4.201 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | EaseInSine | 10      | False    | 1,170.16 us |  10.340 us |   8.073 us | 332.0313 | 332.0313 | 332.0313 |  1024.77 KB |
| MaskTexture | 512  | EaseInSine | 10      | True     | 1,072.54 us |  11.552 us |  10.806 us | 332.0313 | 332.0313 | 332.0313 |  1024.77 KB |
| MaskTexture | 512  | EaseInSine | 20      | False    | 2,224.74 us |  19.086 us |  15.938 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | EaseInSine | 20      | True     | 1,965.86 us |  14.625 us |  13.680 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | EaseInSine | 50      | False    | 4,981.21 us |  46.193 us |  43.209 us | 328.1250 | 328.1250 | 328.1250 |  1026.32 KB |
| MaskTexture | 512  | EaseInSine | 50      | True     | 4,573.32 us |  56.096 us |  52.472 us | 328.1250 | 328.1250 | 328.1250 |  1026.32 KB |
| MaskTexture | 512  | Square     | 3       | False    |   287.44 us |   3.202 us |   2.995 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | Square     | 3       | True     |   304.25 us |   2.988 us |   2.795 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | Square     | 10      | False    |   769.67 us |   4.568 us |   4.049 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | Square     | 10      | True     |   773.55 us |   5.243 us |   4.904 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | Square     | 20      | False    | 1,356.93 us |  14.036 us |  11.720 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | Square     | 20      | True     | 1,360.46 us |   9.538 us |   8.921 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | Square     | 50      | False    | 2,716.50 us |  18.217 us |  16.149 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 512  | Square     | 50      | True     | 3,031.63 us |  13.016 us |  12.175 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 512  | Cube       | 3       | False    |   294.83 us |   2.523 us |   2.360 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | Cube       | 3       | True     |   307.42 us |   2.717 us |   2.542 us | 333.0078 | 333.0078 | 333.0078 |  1024.35 KB |
| MaskTexture | 512  | Cube       | 10      | False    |   770.08 us |   3.155 us |   2.797 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | Cube       | 10      | True     |   793.81 us |   5.311 us |   4.708 us | 333.0078 | 333.0078 | 333.0078 |  1024.77 KB |
| MaskTexture | 512  | Cube       | 20      | False    | 1,348.49 us |   7.138 us |   6.677 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | Cube       | 20      | True     | 1,387.26 us |  10.437 us |   9.763 us | 332.0313 | 332.0313 | 332.0313 |   1025.3 KB |
| MaskTexture | 512  | Cube       | 50      | False    | 3,020.52 us |  21.692 us |  20.290 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 512  | Cube       | 50      | True     | 3,115.45 us |  16.756 us |  15.673 us | 332.0313 | 332.0313 | 332.0313 |  1026.32 KB |
| MaskTexture | 1024 | SquareFade | 3       | False    |   426.42 us |   3.345 us |   3.129 us | 500.4883 | 500.0000 | 500.0000 |  4098.22 KB |
| MaskTexture | 1024 | SquareFade | 3       | True     |   423.30 us |   2.222 us |   1.970 us | 500.4883 | 500.0000 | 500.0000 |  4098.24 KB |
| MaskTexture | 1024 | SquareFade | 10      | False    |   912.63 us |   8.093 us |   7.570 us | 500.0000 | 500.0000 | 500.0000 |  4098.45 KB |
| MaskTexture | 1024 | SquareFade | 10      | True     |   888.70 us |   6.420 us |   5.691 us | 500.0000 | 500.0000 | 500.0000 |  4098.34 KB |
| MaskTexture | 1024 | SquareFade | 20      | False    | 1,552.03 us |   7.224 us |   6.404 us | 500.0000 | 500.0000 | 500.0000 |  4098.92 KB |
| MaskTexture | 1024 | SquareFade | 20      | True     | 1,535.37 us |  10.694 us |  10.003 us | 500.0000 | 500.0000 | 500.0000 |  4098.97 KB |
| MaskTexture | 1024 | SquareFade | 50      | False    | 3,037.83 us |  32.179 us |  30.100 us | 500.0000 | 500.0000 | 500.0000 |  4099.72 KB |
| MaskTexture | 1024 | SquareFade | 50      | True     | 3,282.58 us |  30.888 us |  28.893 us | 500.0000 | 500.0000 | 500.0000 |  4099.69 KB |
| MaskTexture | 1024 | EaseInSine | 3       | False    |   545.07 us |   3.564 us |   3.334 us | 500.0000 | 500.0000 | 500.0000 |  4098.17 KB |
| MaskTexture | 1024 | EaseInSine | 3       | True     |   514.42 us |   4.811 us |   4.501 us | 500.0000 | 500.0000 | 500.0000 |  4098.22 KB |
| MaskTexture | 1024 | EaseInSine | 10      | False    | 1,330.84 us |  11.774 us |  11.013 us | 500.0000 | 500.0000 | 500.0000 |  4098.29 KB |
| MaskTexture | 1024 | EaseInSine | 10      | True     | 1,222.26 us |  13.455 us |  12.586 us | 500.0000 | 500.0000 | 500.0000 |  4098.39 KB |
| MaskTexture | 1024 | EaseInSine | 20      | False    | 2,420.53 us |  25.236 us |  23.606 us | 500.0000 | 500.0000 | 500.0000 |  4098.76 KB |
| MaskTexture | 1024 | EaseInSine | 20      | True     | 2,158.69 us |  12.605 us |  11.791 us | 500.0000 | 500.0000 | 500.0000 |  4098.81 KB |
| MaskTexture | 1024 | EaseInSine | 50      | False    | 5,557.24 us |  62.991 us |  58.922 us | 500.0000 | 500.0000 | 500.0000 |   4099.8 KB |
| MaskTexture | 1024 | EaseInSine | 50      | True     | 5,127.30 us | 100.907 us | 144.718 us | 500.0000 | 500.0000 | 500.0000 |  4099.73 KB |
| MaskTexture | 1024 | Square     | 3       | False    |   432.19 us |   8.478 us |  10.093 us | 500.0000 | 499.5117 | 499.5117 |  4098.25 KB |
| MaskTexture | 1024 | Square     | 3       | True     |   428.14 us |   3.836 us |   3.588 us | 500.0000 | 499.5117 | 499.5117 |  4098.26 KB |
| MaskTexture | 1024 | Square     | 10      | False    |   916.32 us |   5.808 us |   5.433 us | 500.0000 | 500.0000 | 500.0000 |  4098.18 KB |
| MaskTexture | 1024 | Square     | 10      | True     |   918.57 us |   3.396 us |   2.836 us | 500.0000 | 500.0000 | 500.0000 |  4098.34 KB |
| MaskTexture | 1024 | Square     | 20      | False    | 1,549.99 us |  13.423 us |  12.556 us | 500.0000 | 500.0000 | 500.0000 |  4098.76 KB |
| MaskTexture | 1024 | Square     | 20      | True     | 1,544.90 us |   7.274 us |   6.074 us | 500.0000 | 500.0000 | 500.0000 |  4098.82 KB |
| MaskTexture | 1024 | Square     | 50      | False    | 3,040.41 us |  19.877 us |  18.593 us | 500.0000 | 500.0000 | 500.0000 |  4099.47 KB |
| MaskTexture | 1024 | Square     | 50      | True     | 3,277.25 us |  14.742 us |  13.790 us | 500.0000 | 500.0000 | 500.0000 |  4099.76 KB |
| MaskTexture | 1024 | Cube       | 3       | False    |   422.63 us |   2.909 us |   2.721 us | 500.4883 | 500.0000 | 500.0000 |  4098.24 KB |
| MaskTexture | 1024 | Cube       | 3       | True     |   431.82 us |   3.166 us |   2.961 us | 500.0000 | 499.5117 | 499.5117 |  4098.26 KB |
| MaskTexture | 1024 | Cube       | 10      | False    |   919.44 us |   6.835 us |   6.059 us | 500.0000 | 500.0000 | 500.0000 |  4098.42 KB |
| MaskTexture | 1024 | Cube       | 10      | True     |   942.14 us |  14.864 us |  13.176 us | 500.0000 | 500.0000 | 500.0000 |  4098.32 KB |
| MaskTexture | 1024 | Cube       | 20      | False    | 1,554.39 us |   8.484 us |   7.936 us | 500.0000 | 500.0000 | 500.0000 |  4098.92 KB |
| MaskTexture | 1024 | Cube       | 20      | True     | 1,604.94 us |  13.961 us |  12.376 us | 500.0000 | 500.0000 | 500.0000 |  4098.92 KB |
| MaskTexture | 1024 | Cube       | 50      | False    | 3,050.52 us |  14.668 us |  12.248 us | 500.0000 | 500.0000 | 500.0000 |  4099.76 KB |
| MaskTexture | 1024 | Cube       | 50      | True     | 3,366.86 us |  22.509 us |  19.953 us | 500.0000 | 500.0000 | 500.0000 |  4099.67 KB |
| MaskTexture | 2024 | SquareFade | 3       | False    | 1,223.91 us |   6.384 us |   5.972 us | 599.6094 | 599.6094 | 599.6094 | 16003.82 KB |
| MaskTexture | 2024 | SquareFade | 3       | True     | 1,230.62 us |  10.001 us |   9.355 us | 599.6094 | 599.6094 | 599.6094 |  16003.7 KB |
| MaskTexture | 2024 | SquareFade | 10      | False    | 2,082.79 us |  18.958 us |  17.733 us | 597.6563 | 597.6563 | 597.6563 | 16004.14 KB |
| MaskTexture | 2024 | SquareFade | 10      | True     | 2,044.54 us |  21.861 us |  20.449 us | 597.6563 | 597.6563 | 597.6563 | 16004.47 KB |
| MaskTexture | 2024 | SquareFade | 20      | False    | 3,008.79 us |  17.294 us |  16.177 us | 597.6563 | 597.6563 | 597.6563 |  16004.9 KB |
| MaskTexture | 2024 | SquareFade | 20      | True     | 3,088.60 us |  20.120 us |  18.820 us | 597.6563 | 597.6563 | 597.6563 | 16004.84 KB |
| MaskTexture | 2024 | SquareFade | 50      | False    | 5,614.32 us |  35.477 us |  31.450 us | 593.7500 | 593.7500 | 593.7500 | 16005.85 KB |
| MaskTexture | 2024 | SquareFade | 50      | True     | 5,693.10 us |  38.247 us |  35.776 us | 593.7500 | 593.7500 | 593.7500 | 16005.87 KB |
| MaskTexture | 2024 | EaseInSine | 3       | False    | 1,329.78 us |  11.693 us |   9.764 us | 599.6094 | 599.6094 | 599.6094 |  16003.8 KB |
| MaskTexture | 2024 | EaseInSine | 3       | True     | 1,298.68 us |   7.991 us |   7.475 us | 599.6094 | 599.6094 | 599.6094 | 16003.81 KB |
| MaskTexture | 2024 | EaseInSine | 10      | False    | 2,440.06 us |  18.283 us |  17.102 us | 597.6563 | 597.6563 | 597.6563 |  16004.5 KB |
| MaskTexture | 2024 | EaseInSine | 10      | True     | 2,330.95 us |  26.619 us |  23.597 us | 597.6563 | 597.6563 | 597.6563 | 16004.56 KB |
| MaskTexture | 2024 | EaseInSine | 20      | False    | 3,975.71 us |  51.097 us |  45.296 us | 593.7500 | 593.7500 | 593.7500 | 16004.98 KB |
| MaskTexture | 2024 | EaseInSine | 20      | True     | 3,672.33 us |  39.028 us |  36.507 us | 597.6563 | 597.6563 | 597.6563 | 16004.91 KB |
| MaskTexture | 2024 | EaseInSine | 50      | False    | 7,806.28 us |  99.337 us |  92.920 us | 593.7500 | 593.7500 | 593.7500 | 16005.94 KB |
| MaskTexture | 2024 | EaseInSine | 50      | True     | 7,282.16 us |  57.559 us |  53.840 us | 593.7500 | 593.7500 | 593.7500 | 16005.76 KB |
| MaskTexture | 2024 | Square     | 3       | False    | 1,232.78 us |  12.162 us |  10.782 us | 599.6094 | 599.6094 | 599.6094 | 16003.76 KB |
| MaskTexture | 2024 | Square     | 3       | True     | 1,235.02 us |  12.411 us |  11.002 us | 599.6094 | 599.6094 | 599.6094 | 16003.79 KB |
| MaskTexture | 2024 | Square     | 10      | False    | 2,066.81 us |  16.884 us |  15.793 us | 597.6563 | 597.6563 | 597.6563 | 16004.26 KB |
| MaskTexture | 2024 | Square     | 10      | True     | 2,052.85 us |  22.291 us |  20.851 us | 597.6563 | 597.6563 | 597.6563 | 16004.35 KB |
| MaskTexture | 2024 | Square     | 20      | False    | 3,106.13 us |  20.389 us |  19.072 us | 597.6563 | 597.6563 | 597.6563 | 16004.92 KB |
| MaskTexture | 2024 | Square     | 20      | True     | 3,111.31 us |  21.746 us |  19.277 us | 597.6563 | 597.6563 | 597.6563 | 16004.95 KB |
| MaskTexture | 2024 | Square     | 50      | False    | 5,636.53 us |  38.574 us |  36.082 us | 593.7500 | 593.7500 | 593.7500 | 16005.79 KB |
| MaskTexture | 2024 | Square     | 50      | True     | 5,768.42 us |  52.615 us |  49.216 us | 593.7500 | 593.7500 | 593.7500 | 16005.85 KB |
| MaskTexture | 2024 | Cube       | 3       | False    | 1,223.89 us |   7.526 us |   7.040 us | 599.6094 | 599.6094 | 599.6094 | 16003.61 KB |
| MaskTexture | 2024 | Cube       | 3       | True     | 1,235.70 us |   7.378 us |   6.541 us | 599.6094 | 599.6094 | 599.6094 | 16003.52 KB |
| MaskTexture | 2024 | Cube       | 10      | False    | 2,044.68 us |  22.040 us |  20.617 us | 597.6563 | 597.6563 | 597.6563 | 16004.59 KB |
| MaskTexture | 2024 | Cube       | 10      | True     | 2,060.44 us |  19.259 us |  18.015 us | 597.6563 | 597.6563 | 597.6563 | 16004.45 KB |
| MaskTexture | 2024 | Cube       | 20      | False    | 3,124.69 us |  14.034 us |  13.127 us | 597.6563 | 597.6563 | 597.6563 | 16004.92 KB |
| MaskTexture | 2024 | Cube       | 20      | True     | 3,150.52 us |  27.745 us |  25.953 us | 597.6563 | 597.6563 | 597.6563 | 16004.86 KB |
| MaskTexture | 2024 | Cube       | 50      | False    | 5,636.04 us |  42.550 us |  39.802 us | 593.7500 | 593.7500 | 593.7500 | 16005.85 KB |
| MaskTexture | 2024 | Cube       | 50      | True     | 5,844.83 us |  60.554 us |  56.642 us | 593.7500 | 593.7500 | 593.7500 |  16005.8 KB |


## Mix-Color

| Method   | size | mode        |         Mean |      Error |     StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| -------- | ---- | ----------- | -----------: | ---------: | ---------: | -------: | -------: | -------: | ----------: |
| MixColor | 128  | Mix         |     49.02 us |   0.184 us |   0.154 us |   4.1504 |   0.9155 |        - |    68.03 KB |
| MixColor | 128  | Hue         |    127.89 us |   0.253 us |   0.237 us |   4.1504 |   0.7324 |        - |    68.23 KB |
| MixColor | 128  | Saturation  |    127.31 us |   0.502 us |   0.445 us |   4.1504 |   0.7324 |        - |    68.21 KB |
| MixColor | 128  | Value       |    129.65 us |   0.588 us |   0.491 us |   4.1504 |   0.7324 |        - |    68.22 KB |
| MixColor | 128  | Darken      |     53.65 us |   0.082 us |   0.077 us |   4.2114 |   0.8545 |        - |    68.07 KB |
| MixColor | 128  | LinearLight |     61.36 us |   0.101 us |   0.079 us |   4.2114 |   0.8545 |        - |     68.1 KB |
| MixColor | 128  | Lighten     |     65.47 us |   0.250 us |   0.234 us |   4.1504 |   0.8545 |        - |    68.13 KB |
| MixColor | 512  | Mix         |    580.19 us |   1.156 us |   0.902 us | 250.9766 | 249.0234 | 249.0234 |  1037.11 KB |
| MixColor | 512  | Hue         |  1,464.13 us |  26.246 us |  24.550 us | 250.0000 | 248.0469 | 248.0469 |  1036.71 KB |
| MixColor | 512  | Saturation  |  1,446.73 us |   3.134 us |   2.778 us | 250.0000 | 248.0469 | 248.0469 |  1036.54 KB |
| MixColor | 512  | Value       |  1,487.01 us |  27.256 us |  33.473 us | 250.0000 | 248.0469 | 248.0469 |  1037.05 KB |
| MixColor | 512  | Darken      |    624.39 us |   1.180 us |   1.046 us | 250.9766 | 249.0234 | 249.0234 |  1037.53 KB |
| MixColor | 512  | LinearLight |    708.06 us |   2.197 us |   1.835 us | 250.9766 | 249.0234 | 249.0234 |  1038.06 KB |
| MixColor | 512  | Lighten     |    747.31 us |   2.110 us |   1.647 us | 250.9766 | 249.0234 | 249.0234 |   1037.5 KB |
| MixColor | 1024 | Mix         |  2,080.48 us |  11.372 us |  10.637 us | 156.2500 | 156.2500 | 156.2500 |  4104.25 KB |
| MixColor | 1024 | Hue         |  5,345.30 us |  24.742 us |  20.661 us | 156.2500 | 156.2500 | 156.2500 |  4103.99 KB |
| MixColor | 1024 | Saturation  |  5,340.50 us |  18.894 us |  16.749 us | 156.2500 | 156.2500 | 156.2500 |  4103.89 KB |
| MixColor | 1024 | Value       |  5,443.30 us |  16.369 us |  13.669 us | 156.2500 | 156.2500 | 156.2500 |  4104.15 KB |
| MixColor | 1024 | Darken      |  2,257.54 us |   9.281 us |   7.246 us | 156.2500 | 156.2500 | 156.2500 |  4104.53 KB |
| MixColor | 1024 | LinearLight |  2,581.43 us |   5.961 us |   5.284 us | 156.2500 | 156.2500 | 156.2500 |  4104.18 KB |
| MixColor | 1024 | Lighten     |  2,725.18 us |  10.820 us |   9.035 us | 156.2500 | 156.2500 | 156.2500 |  4104.12 KB |
| MixColor | 2024 | Mix         |  7,649.91 us |  46.090 us |  40.857 us | 156.2500 | 156.2500 | 156.2500 | 16007.11 KB |
| MixColor | 2024 | Hue         | 20,336.31 us |  60.656 us |  53.770 us | 125.0000 | 125.0000 | 125.0000 | 16009.78 KB |
| MixColor | 2024 | Saturation  | 20,379.20 us | 102.067 us |  90.480 us | 125.0000 | 125.0000 | 125.0000 | 16010.37 KB |
| MixColor | 2024 | Value       | 20,573.69 us |  85.323 us |  71.249 us | 125.0000 | 125.0000 | 125.0000 | 16010.16 KB |
| MixColor | 2024 | Darken      |  8,375.51 us |  22.396 us |  19.853 us | 156.2500 | 156.2500 | 156.2500 | 16008.06 KB |
| MixColor | 2024 | LinearLight |  9,747.75 us | 181.903 us | 186.801 us | 156.2500 | 156.2500 | 156.2500 | 16007.82 KB |
| MixColor | 2024 | Lighten     | 10,246.11 us | 105.390 us |  93.426 us | 156.2500 | 156.2500 | 156.2500 | 16008.31 KB |


## Noise-Texture

| Method       | size |        Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |  Allocated |
| ------------ | ---- | ----------: | --------: | --------: | -------: | -------: | -------: | ---------: |
| NoiseTexture | 128  |    103.5 us |   0.21 us |   0.20 us |   4.1504 |   0.8545 |        - |    68.1 KB |
| NoiseTexture | 512  |  1,129.5 us |   4.19 us |   3.92 us | 332.0313 | 332.0313 | 332.0313 | 1028.24 KB |
| NoiseTexture | 1024 |  4,038.0 us |  20.35 us |  18.04 us | 500.0000 | 500.0000 | 500.0000 | 4107.25 KB |
| NoiseTexture | 2024 | 16,089.9 us | 318.80 us | 341.11 us | 593.7500 | 593.7500 | 593.7500 | 16011.9 KB |


## Output

| Method    | size |        Mean |     Error |    StdDev | Allocated |
| --------- | ---- | ----------: | --------: | --------: | --------: |
| TileFixer | 128  |    571.3 us |   4.80 us |   4.49 us |     153 B |
| TileFixer | 512  |  4,096.0 us |  32.31 us |  30.22 us |     156 B |
| TileFixer | 1024 | 15,240.2 us |  49.23 us |  43.64 us |     159 B |
| TileFixer | 2024 | 58,230.2 us | 129.75 us | 108.35 us |     202 B |


## Resize

| Method | size | newSize | mode   |        Mean |     Error |    StdDev | Allocated |
| ------ | ---- | ------- | ------ | ----------: | --------: | --------: | --------: |
| Resize | 128  | 64      | Object |    103.0 us |   0.32 us |   0.27 us |     168 B |
| Resize | 128  | 256     | Object |    103.9 us |   0.55 us |   0.49 us |     168 B |
| Resize | 128  | 1024    | Object |    103.5 us |   0.35 us |   0.31 us |     168 B |
| Resize | 128  | 4048    | Object |    103.4 us |   0.65 us |   0.55 us |     168 B |
| Resize | 512  | 64      | Object |    940.4 us |   6.95 us |   6.50 us |     169 B |
| Resize | 512  | 256     | Object |    944.1 us |   7.45 us |   6.97 us |     168 B |
| Resize | 512  | 1024    | Object |    942.3 us |   5.46 us |   4.84 us |     168 B |
| Resize | 512  | 4048    | Object |    942.4 us |   5.49 us |   4.87 us |     168 B |
| Resize | 1024 | 64      | Object |  3,645.5 us |  27.86 us |  26.06 us |     170 B |
| Resize | 1024 | 256     | Object |  3,624.3 us |   9.21 us |   7.19 us |     168 B |
| Resize | 1024 | 1024    | Object |  3,658.7 us |  24.97 us |  23.36 us |     168 B |
| Resize | 1024 | 4048    | Object |  3,637.4 us |  28.16 us |  26.34 us |     168 B |
| Resize | 2024 | 64      | Object | 14,600.7 us | 106.20 us |  94.14 us |     169 B |
| Resize | 2024 | 256     | Object | 14,619.7 us | 104.88 us |  87.58 us |     170 B |
| Resize | 2024 | 1024    | Object | 14,814.1 us | 252.06 us | 247.55 us |     174 B |
| Resize | 2024 | 4048    | Object | 14,787.3 us | 157.86 us | 131.82 us |     174 B |


## Texture-Coordinate

| Method            | size | mode   |        Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| ----------------- | ---- | ------ | ----------: | --------: | --------: | -------: | -------: | -------: | ----------: |
| TextureCoordinate | 128  | Object |    31.91 us |  0.246 us |  0.205 us |   8.0566 |   1.8311 |        - |   132.01 KB |
| TextureCoordinate | 512  | Object |   470.88 us |  1.139 us |  1.010 us | 503.9063 | 499.5117 | 499.5117 |  2073.24 KB |
| TextureCoordinate | 1024 | Object | 1,489.70 us |  8.952 us |  8.374 us | 500.0000 | 498.0469 | 498.0469 |   8202.1 KB |
| TextureCoordinate | 2024 | Object | 5,609.86 us | 90.473 us | 84.628 us | 500.0000 | 500.0000 | 500.0000 | 32008.99 KB |


## Tile-Fixer

| Method    | size | newSize | blendBandSize |        Mean |     Error |    StdDev |     Gen0 |     Gen1 |     Gen2 |   Allocated |
| --------- | ---- | ------- | ------------- | ----------: | --------: | --------: | -------: | -------: | -------: | ----------: |
| TileFixer | 128  | 64      | 8             |    32.72 us |  0.139 us |  0.123 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 64      | 16            |    65.00 us |  0.369 us |  0.345 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 64      | 32            |   127.84 us |  0.326 us |  0.289 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 8             |    32.97 us |  0.147 us |  0.137 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 16            |    65.41 us |  1.022 us |  0.956 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 256     | 32            |   127.98 us |  0.433 us |  0.405 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 8             |    32.86 us |  0.169 us |  0.150 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 16            |    64.82 us |  0.244 us |  0.228 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 1024    | 32            |   127.96 us |  0.336 us |  0.281 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 8             |    32.77 us |  0.107 us |  0.100 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 16            |    64.63 us |  0.346 us |  0.324 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 128  | 4048    | 32            |   128.02 us |  0.355 us |  0.332 us |   3.9063 |        - |        - |    64.11 KB |
| TileFixer | 512  | 64      | 8             |   203.16 us |  0.748 us |  0.625 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 64      | 16            |   340.99 us |  0.742 us |  0.694 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 64      | 32            |   597.37 us |  1.713 us |  1.602 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 256     | 8             |   210.41 us |  0.392 us |  0.327 us | 250.0000 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 256     | 16            |   334.77 us |  0.953 us |  0.891 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 256     | 32            |   596.64 us |  2.264 us |  1.891 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 1024    | 8             |   204.41 us |  0.437 us |  0.387 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 1024    | 16            |   341.09 us |  0.642 us |  0.601 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 1024    | 32            |   603.09 us |  2.602 us |  2.434 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 512  | 4048    | 8             |   210.58 us |  0.340 us |  0.318 us | 249.7559 | 249.7559 | 249.7559 |  1024.19 KB |
| TileFixer | 512  | 4048    | 16            |   341.17 us |  0.924 us |  0.865 us | 249.5117 | 249.5117 | 249.5117 |  1024.19 KB |
| TileFixer | 512  | 4048    | 32            |   605.98 us |  1.213 us |  1.135 us | 249.0234 | 249.0234 | 249.0234 |  1024.19 KB |
| TileFixer | 1024 | 64      | 8             |   591.64 us |  1.559 us |  1.458 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 64      | 16            |   843.87 us |  2.069 us |  1.834 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 64      | 32            | 1,332.65 us |  3.259 us |  2.722 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 8             |   578.59 us |  1.300 us |  1.085 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 16            |   849.23 us |  6.184 us |  5.784 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 256     | 32            | 1,336.16 us |  2.987 us |  2.332 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 8             |   582.91 us |  1.904 us |  1.687 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 16            |   850.06 us |  4.344 us |  4.064 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 1024    | 32            | 1,337.34 us |  5.301 us |  4.958 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 8             |   580.55 us |  1.667 us |  1.560 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 16            |   842.98 us |  3.623 us |  3.389 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 1024 | 4048    | 32            | 1,353.22 us |  5.330 us |  4.725 us | 398.4375 | 398.4375 | 398.4375 |  4096.23 KB |
| TileFixer | 2024 | 64      | 8             | 3,289.88 us | 30.214 us | 28.262 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 64      | 16            | 3,789.60 us | 54.449 us | 50.932 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 64      | 32            | 4,741.54 us | 48.631 us | 45.489 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 256     | 8             | 3,011.76 us | 31.468 us | 29.435 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 256     | 16            | 3,633.91 us | 50.475 us | 39.407 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 256     | 32            | 4,500.80 us | 11.561 us | 10.249 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 8             | 2,944.50 us | 29.659 us | 27.743 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 16            | 3,460.44 us | 11.697 us | 10.941 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 1024    | 32            | 4,813.76 us | 22.553 us | 19.993 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 8             | 2,923.28 us | 25.277 us | 23.644 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 16            | 3,589.36 us | 16.928 us | 15.834 us | 984.3750 | 984.3750 | 984.3750 | 16002.68 KB |
| TileFixer | 2024 | 4048    | 32            | 4,520.85 us | 30.317 us | 26.875 us | 968.7500 | 968.7500 | 968.7500 | 16002.68 KB |


<p align="center">
  <a href="#brick-texture">Brick Texture</a> •
  <a href="#bump">Bump</a> •
  <a href="#color-ramp">Color Ramp</a> •
  <a href="#mask-texture">Mask Texture</a> •
  <a href="#mix-color">Mix Color</a> •
  <a href="#noise-texture">Noise Texture</a> •
  <a href="#output">Output</a> •
  <a href="#resize">Resize</a> •
  <a href="#texture-coordinate">Texture Coordinate</a> •
  <a href="#tile-fixer">Tile Fixer</a> •
  <a href="#benchmark-summary-for-nodes">Back to top</a>
</p>