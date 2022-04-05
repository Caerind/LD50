using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspiration from : https://github.com/SebLague/Procedural-Landmass-Generation/blob/master/Proc%20Gen%20E21/Assets/Scripts/MeshGenerator.cs
// Maybe we can reduce vertexCount by using another technique for borders/lod : https://www.flipcode.com/archives/article_geomipmaps.pdf

public static class TerrainGenerator
{
    public static TerrainMeshData GenerateTerrainMesh(TerrainSettings terrainSettings, int lod)
	{
		int numVertsPerLine = terrainSettings.numVertsPerLine;
		float meshWorldSize = terrainSettings.meshWorldSize;

		int skipIncrement = (lod == 0) ? 1 : lod * 2;

		Vector2 topLeft = new Vector2(-1, 1) * meshWorldSize / 2f;

		TerrainMeshData terrainMeshData = new TerrainMeshData(numVertsPerLine, skipIncrement);

		int[,] vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
		int meshVertexIndex = 0;
		int outOfMeshVertexIndex = -1;

		for (int y = 0; y < numVertsPerLine; y++)
		{
			for (int x = 0; x < numVertsPerLine; x++)
			{
				bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
				bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
				if (isOutOfMeshVertex)
				{
					vertexIndicesMap[x, y] = outOfMeshVertexIndex;
					outOfMeshVertexIndex--;
				}
				else if (!isSkippedVertex)
				{
					vertexIndicesMap[x, y] = meshVertexIndex;
					meshVertexIndex++;
				}
			}
		}

		for (int y = 0; y < numVertsPerLine; y++)
		{
			for (int x = 0; x < numVertsPerLine; x++)
			{
				bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

				if (!isSkippedVertex)
				{
					bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
					bool isMeshEdgeVertex = (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2) && !isOutOfMeshVertex;
					bool isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
					bool isEdgeConnectionVertex = (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

					int vertexIndex = vertexIndicesMap[x, y];
					Vector2 percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
					Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshWorldSize;
					float height = 0.0f; // TODO : heightMap[x,y]

					if (isEdgeConnectionVertex)
					{
						bool isVertical = x == 2 || x == numVertsPerLine - 3;
						int dstToMainVertexA = ((isVertical) ? y - 2 : x - 2) % skipIncrement;
						int dstToMainVertexB = skipIncrement - dstToMainVertexA;
						float dstPercentFromAToB = dstToMainVertexA / (float)skipIncrement;

						float heightMainVertexA = 0.0f; // TODO : heightMap[(isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y];
						float heightMainVertexB = 0.0f; // TODO : heightMap[(isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y];

						height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;
					}

					terrainMeshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

					bool createTriangle = x < numVertsPerLine - 1 && y < numVertsPerLine - 1 && (!isEdgeConnectionVertex || (x != 2 && y != 2));

					if (createTriangle)
					{
						int currentIncrement = (isMainVertex && x != numVertsPerLine - 3 && y != numVertsPerLine - 3) ? skipIncrement : 1;

						int a = vertexIndicesMap[x, y];
						int b = vertexIndicesMap[x + currentIncrement, y];
						int c = vertexIndicesMap[x, y + currentIncrement];
						int d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];
						terrainMeshData.AddTriangle(a, d, c);
						terrainMeshData.AddTriangle(d, a, b);
					}
				}
			}
		}

		terrainMeshData.CalculateNormals();

		return terrainMeshData;
	}
}
