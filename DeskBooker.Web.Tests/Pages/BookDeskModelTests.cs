using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;

using Xunit;
using Moq;

namespace DeskBooker.Web.Pages
{
  public class BookDeskModelTests
  {
    [Theory]
  [InlineData(1,true)]
  [InlineData(0,false)]
  public void ShouldCallBookDeskMethodOfProcessorIfModelIsValid(int expectedBookDeskCalls,bool isModelValid){

    var processorMock=new Mock<IDeskBookingRequestProcessor>();
    var bookDeskModel=new BookDeskModel(processorMock.Object){
      DeskBookingRequest=new DeskBookingRequest()
    };

    if (!isModelValid){
      bookDeskModel.ModelState.AddModelError("JustAKey","Error massage");
    }
    bookDeskModel.OnPost();
    processorMock.Verify(x =>x.BookDesk(bookDeskModel.DeskBookingRequest),Times.Exactly(expectedBookDeskCalls));
  }

  }
}
