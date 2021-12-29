using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ValueLabTest.Model;
using ValueLabTest.ViewModel;

namespace ValueLabTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientGroupsController : ControllerBase
    {
        [HttpPost]
        public PatientGroupVM Calculate(PatientGroupModel model)
        {
            PatientGroupVM result = new PatientGroupVM();

            result.NumberOfGroups = calculateNumberOfGroups(model.Matrix);

            return result;
        }

        public int calculateNumberOfGroups(int[,] matrix)
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
                        count++;
                    }
                }
            }

            return count;
        }

    }
}