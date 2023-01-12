using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using SortingAPI.Enumerators;
using SortingAPI.FileManagement;
using SortingAPI.Helpers;
using SortingAPI.Interfaces;
using SortingAPI.SortingAlgorithms;

namespace SortingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SortingController : ControllerBase
    {
        private readonly string 
            resultDirectory = Environment.CurrentDirectory + "\\Results",
            resultFileName = "result.json";

        private ISortingHelper<int> sortingHelper;
        private IFileManager fileManager;

        public SortingController(ISortingHelper<int> sortingHelper, IFileManager fileManager)
        {
            this.sortingHelper = sortingHelper;
            this.fileManager = fileManager;
        }

        [HttpGet]
        [Route("[Action]")]
        public ActionResult<int[]?> Sort([FromQuery] int[] unsorted, [FromQuery] SortingAlgorithm sortingAlgorithm = SortingAlgorithm.BubbleSort)
        {
            int[]? sorted = null;

            if (unsorted.Length == 0)
            {
                return BadRequest("Array must have atleast one element");
            }
            if(unsorted.Any(x => x < 1 || x > 10))
            {
                return BadRequest("Array contains unsuported numbers, all numbers should be from 1-10");
            }

            try
            {
                sorted = sortingHelper.Sort(unsorted, sortingAlgorithm)?.ToArray();
                fileManager.SaveStringToFile($"{resultDirectory}\\{resultFileName}", JsonConvert.SerializeObject(sorted));
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }

            return sorted;
        }

        [HttpGet]
        [Route("[Action]")]
        public ActionResult<int[]?> GetLastResult()
        {
            int[]? array = null;
            try
            {
                var fileText = fileManager.GetFile(resultDirectory);
                if (fileText == "")
                {
                    return NotFound("Results not found");
                }
                array = JsonConvert.DeserializeObject<int[]>(fileText);
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }

            return array;
        }
    }
}
