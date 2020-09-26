using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeskBooker.Web.Pages
{
    public class BookDeskModelTests
    {
        private readonly Mock<IDeskBookingRequestProcessor> _processorMock;
        private readonly BookDeskModel _bookDeskModel;
        private readonly DeskBookingResult _deskBookingResult;

        public BookDeskModelTests()
        {
            _processorMock = new Mock<IDeskBookingRequestProcessor>();
            _bookDeskModel = new BookDeskModel(_processorMock.Object)
            {
                DeskBookingRequest = new DeskBookingRequest()
            };
            _deskBookingResult = new DeskBookingResult
            {
                Code = DeskBookingResultCode.Success
            };
            _processorMock.Setup(x => x.BookDesk(_bookDeskModel.DeskBookingRequest)).Returns(_deskBookingResult);
        }


        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void ShouldCallBookDeskMethodOfProcessorIfModelIsValid(int expectedBookDeskCalls, bool isModelValid)
        {

            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("JustAKey", "Error massage");
            }
            _bookDeskModel.OnPost();
            _processorMock.Verify(x => x.BookDesk(_bookDeskModel.DeskBookingRequest), Times.Exactly(expectedBookDeskCalls));
        }

        [Fact]

        public void ShouldAddModelErrorIfNoDeskIsAvailable()
        {

            _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;


            _bookDeskModel.OnPost();
            var modelStateEntry =
            Assert.Contains("DeskBookingRequest.Date", _bookDeskModel.ModelState);
            var modelError = Assert.Single(modelStateEntry.Errors);
            Assert.Equal("No desk available for selected date", modelError.ErrorMessage);
        }
        [Fact]

        public void ShouldNotAddModelErrorIfNoDeskIsAvailable()
        {

            _deskBookingResult.Code = DeskBookingResultCode.Success;
            _bookDeskModel.OnPost();
            Assert.DoesNotContain("DeskBookingRequest.Date", _bookDeskModel.ModelState);




        }

        [Theory]
        [InlineData(typeof(PageResult),false,null)]
        [InlineData(typeof(PageResult),true,DeskBookingResultCode.NoDeskAvailable)]
          [InlineData(typeof(RedirectToPageResult),true,DeskBookingResultCode.Success)]
        public void ShouldReturnExpectedActionResult(Type expectedActionResultType,bool isModelValid,
        DeskBookingResultCode? deskBookingResultCode){
if(!isModelValid){
  _bookDeskModel.ModelState.AddModelError("JustAKey","AnErrorMessage");
  }

  if (deskBookingResultCode.HasValue){
    _deskBookingResult.Code=deskBookingResultCode.Value;
  }

  IActionResult actionResult=_bookDeskModel.OnPost();
  Assert.IsType(expectedActionResultType,actionResult);



        }

        




    }
}
