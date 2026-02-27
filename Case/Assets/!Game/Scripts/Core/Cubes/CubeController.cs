using System;
using System.Collections.Generic;
using _Game.Scripts.Data;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    [Serializable]
    public class CubeData
    {
        public CubeColor color;
        public int value;
    }

    [Serializable]
    public class ColumnData
    {
        [LabelText("Columns")]
        public List<CubeData> columns = new();
    }   
    
    public class CubeController : MonoBehaviour, ICubeProvider
    {
        [Title("Settings")]
        [SerializeField] private CubeVisualData cubeVisualData;
        [SerializeField] private Transform cubeContainer;
        [SerializeField] private float spacing = 1.1f;

        private readonly List<List<Cube>> _spawnedCubes = new();

        public void InitCubes(LevelData levelData)
        {
            ClearCubes();
            GenerateCubes(levelData.cubeRows);
        }

        private void GenerateCubes(List<ColumnData> rows)
        {
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = new List<Cube>();
                var columnData = rows[rowIndex];

                float offset = (rows.Count - 1) * spacing / 2f;

                for (int colIndex = 0; colIndex < columnData.columns.Count; colIndex++)
                {
                    var cubeData = columnData.columns[colIndex];
                    var position = new Vector3((rowIndex * spacing) - offset, 0, -colIndex * spacing);

                    var cube = Instantiate(cubeVisualData.cubePrefab, cubeContainer.position + position, Quaternion.identity, cubeContainer);
                    var material = cubeVisualData.GetMaterial(cubeData.color);
                    cube.SetMaterial(material);
                    cube.name = $"Cube_R{rowIndex}_C{colIndex} (Value: {cubeData.value})";

                    row.Add(cube);
                }

                _spawnedCubes.Add(row);
            }
        }

        public void ClearCubes()
        {
            _spawnedCubes.Clear();
            
            if (!cubeContainer) return;
            for (int i = cubeContainer.childCount - 1; i >= 0; i--)
                DestroyImmediate(cubeContainer.GetChild(i).gameObject);
        }
    }
}
