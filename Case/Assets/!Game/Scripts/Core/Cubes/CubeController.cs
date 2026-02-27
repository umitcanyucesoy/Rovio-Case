using System;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Core.Cubes
{
    public class CubeController : MonoBehaviour
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
        
        [Title("Settings")]
        [SerializeField] private Cube cubePrefab;
        [SerializeField] private Transform cubeContainer;
        [SerializeField] private float spacing = 1.1f;
        
        [Title("Cube Grid")]
        [SerializeField] 
        private List<ColumnData> rows = new();
        
        private readonly List<List<Cube>> _spawnedCubes = new();
        
        private void Start()
        {
            GenerateCubes();
        }

        private void GenerateCubes()
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

                    var cube = Instantiate(cubePrefab, cubeContainer.position + position, Quaternion.identity, cubeContainer);
                    cube.SetColor(cubeData.color);
                    cube.name = $"Cube_R{rowIndex}_C{colIndex} (Value: {cubeData.value})";

                    row.Add(cube);
                }

                _spawnedCubes.Add(row);
            }
        }
    }
}
