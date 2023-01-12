using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SortingAPI.Controllers;
using SortingAPI.Enumerators;
using SortingAPI.FileManagement;
using SortingAPI.Helpers;
using SortingAPI.Interfaces;
using System.Net;

namespace UnitTests
{
    public class SortingControllerTests
    {
        private SortingController target;
        private Mock<ISortingHelper<int>> sortingHelper;
        private Mock<IFileManager> fileManager;

        public SortingControllerTests()
        {
            sortingHelper = new Mock<ISortingHelper<int>>();
            sortingHelper
                .Setup(x => x.Sort(
                    It.IsAny<IList<int>>(),
                    It.IsAny<SortingAlgorithm>())
                )
                .Returns(new int[] { 1, 2, 3, 4 });

            fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(x => x.GetFile(It.IsAny<string>()))
                .Returns("[1,2,3,4]");

            target = new SortingController(sortingHelper.Object, fileManager.Object);
        }

        [Fact]
        public void Sort_ArrayEmpty_BadRequest()
        {
            int expectedStatusCode = (int)HttpStatusCode.BadRequest;
            string expectedMessage = "Array must have atleast one element";

            var actual = target.Sort(new int[0]).Result;

            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(expectedStatusCode, badRequestObjectResult.StatusCode);
            Assert.Equal(expectedMessage, badRequestObjectResult.Value);
        }

        [Fact]
        public void Sort_UnsupportedNumbersInArray_BadRequest()
        {
            int expectedStatusCode = (int)HttpStatusCode.BadRequest;
            string expectedMessage = "Array contains unsuported numbers, all numbers should be from 1-10";

            var actual = target.Sort(new int[] { 11, 1 }).Result;

            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actual);
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(expectedStatusCode, badRequestObjectResult.StatusCode);
            Assert.Equal(expectedMessage, badRequestObjectResult.Value);
        }

        [Fact]
        public void Sort_ExceptionInTryBlock_InternalServerError()
        {
            int expected = (int)HttpStatusCode.InternalServerError;
            var fileManager = new Mock<IFileManager>();
            var sortingHelper = new Mock<ISortingHelper<int>>();
            sortingHelper
                .Setup(x => x.Sort(
                        It.IsAny<IList<int>>(), 
                        It.IsAny<SortingAlgorithm>()
                    )
                )
                .Throws(new Exception());
            target = new SortingController(sortingHelper.Object, fileManager.Object);

            var actual = target.Sort(new int[] { 10, 1 }).Result;

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(actual);
            Assert.NotNull(statusCodeResult);
            Assert.Equal(expected, statusCodeResult.StatusCode);
        }

        [Fact]
        public void Sort_ArrayWithSupportedNumbers_GetSortedArrayAndWriteToFile()
        {
            var expected = new int[] { 1, 2, 3, 4 };

            var actual = target.Sort(new int[] { 10, 1 }).Value;

            Assert.Equal(expected, actual);
            Assert.Equal(1, sortingHelper.Invocations.Count);
            Assert.Equal(1, fileManager.Invocations.Count);
        }

        [Fact]
        public void GetLastResult_FileExists_ReturnsFileContents()
        {
            var expected = new int[] { 1, 2, 3, 4};

            var actual = target.GetLastResult().Value;

            Assert.Equal(expected, actual);
            Assert.Equal(1, fileManager.Invocations.Count);
        }

        [Fact]
        public void GetLastResult_FileDoesNotExist_NotFound()
        {
            var expected = (int)HttpStatusCode.NotFound;
            var sortingHelper = new Mock<ISortingHelper<int>>();
            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(x => x.GetFile(It.IsAny<string>()))
                .Returns("");
            target = new SortingController(sortingHelper.Object, fileManager.Object);

            var actual = target.GetLastResult().Result;

            NotFoundObjectResult notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actual);
            Assert.NotNull(notFoundObjectResult);
            Assert.Equal(expected, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public void GetLastResult_ExceptionInTryBlock_InternalServerError()
        {
            int expected = (int)HttpStatusCode.InternalServerError;
            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(x => x.GetFile(It.IsAny<string>()))
                .Throws(new Exception());
            var sortingHelper = new Mock<ISortingHelper<int>>();
            target = new SortingController(sortingHelper.Object, fileManager.Object);

            var actual = target.GetLastResult().Result;

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(actual);
            Assert.NotNull(statusCodeResult);
            Assert.Equal(expected, statusCodeResult.StatusCode);
        }
    }
}