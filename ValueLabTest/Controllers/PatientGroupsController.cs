using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ValueLabTest.Model;
using ValueLabTest.ViewModel;

namespace ValueLabTest.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PatientGroupsController : ControllerBase
    {
        [HttpPost]
        public PatientGroupVM Calculate([FromBody] PatientGroupModel model)
        {
            PatientGroupVM result = new PatientGroupVM();

            result.NumberOfGroups = CalculateNumberOfGroups(model.Matrix);

            return result;
        }

        /// <summary>
        /// Method to calculate number of patient groups
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public int CalculateNumberOfGroups(int[,] matrix)
        {
            int rowLength = matrix.GetLength(0);
            int columnLength = matrix.GetLength(1);

            // Boolean Array for checked position
            bool[,] isChecked = new bool[rowLength, columnLength];

            int count = 0;
            for (int i = 0; i < rowLength; ++i)
            {
                for (int j = 0; j < columnLength; ++j)
                {
                    if (matrix[i, j] == 1 && !isChecked[i, j])
                    {
                        //Checking if patient is sick and whether position is visited if yes then increase count
                        CheckNumberOfGroups(matrix, i, j, isChecked, rowLength, columnLength);
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Check number of Groups
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="isChecked"></param>
        /// <param name="rowLength"></param>
        /// <param name="columnLength"></param>
        public void CheckNumberOfGroups(int[,] matrix, int row, int col, bool[,] isChecked, int rowLength, int columnLength)
        {
            int[] rowPositions = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] columnPositions = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

            // making value as true which are checked
            isChecked[row, col] = true;

            for (int k = 0; k < 8; ++k)
            {
                if (IsInRange(matrix, row + rowPositions[k], col + columnPositions[k], isChecked, rowLength, columnLength))
                {
                    CheckNumberOfGroups(matrix, row + rowPositions[k], col + columnPositions[k], isChecked, rowLength, columnLength);
                }
            }
        }

        /// <summary>
        /// Check whether given matrix is in range or not
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="isChecked"></param>
        /// <param name="rowLength"></param>
        /// <param name="columnLength"></param>
        /// <returns></returns>
        public bool IsInRange(int[,] matrix, int row, int col, bool[,] isChecked, int rowLength, int columnLength) => ((row >= 0) && (row < rowLength) && (col >= 0) && (col < columnLength) && (matrix[row, col] == 1 && !isChecked[row, col]));
    }
}