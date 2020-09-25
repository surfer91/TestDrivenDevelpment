using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;

using Xunit;
using Moq;

namespace DeskBooker.Web.Pages
{
  public class BookDeskModelTests
  {[Fact]
  public void ShouldCallBookDeskMethodOfProcessor(){

    var processorMock=new Mock<IDeskBookingRequestProcessor>();
    var bookDeskModel=new BookDeskModel(processorMock.Object){
      DeskBookingRequest=new DeskBookingRequest()
    };
    bookDeskModel.OnPost();
    processorMock.Verify(x =>x.BookDesk(bookDeskModel.DeskBookingRequest),Times.Once);
  }

  }
}
