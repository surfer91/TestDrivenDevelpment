﻿using DeskBooker.Core.Domain;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using System.Linq;

namespace DeskBooker.Core.Processor
{
  public class DeskBookingRequestProcessorTests
  {
        private readonly DeskBookingRequest _request;
        private readonly List<Desk> _availableDesks;
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private readonly Mock<IDeskRepository> _deskRepositoryMock;
        private readonly DeskBookingRequestProcessor _processor;

    public DeskBookingRequestProcessorTests()
    {  _request = new DeskBookingRequest
      {
        FirstName = "Thomas",
        LastName = "Huber",
        Email = "thomas@thomasclaudiushuber.com",
        Date = new DateTime(2020, 1, 28)
      };

      _availableDesks=new List<Desk>{new Desk{Id=7}};
      _deskBookingRepositoryMock=new Mock<IDeskBookingRepository>();
      _deskRepositoryMock=new Mock<IDeskRepository>();
      _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_request.Date)).Returns(_availableDesks);
      _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object,_deskRepositoryMock.Object);
      
    }

    [Fact]
    public void ShouldReturnDeskBookingResultWithRequestValues()
    {
      // Arrange
    

      // Act
      DeskBookingResult result = _processor.BookDesk(_request);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(_request.FirstName, result.FirstName);
      Assert.Equal(_request.LastName, result.LastName);
      Assert.Equal(_request.Email, result.Email);
      Assert.Equal(_request.Date, result.Date);
    }

    [Fact]
    public void ShouldThrowExceptionIfRequestIsNull()
    {
      var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

      Assert.Equal("request", exception.ParamName);
    }

       [Fact]
    public void ShouldSaveDeskBooking()
    {
      DeskBooking savedDeskBooking=null;
      
      _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
      .Callback<DeskBooking>(deskBooking=>{savedDeskBooking=deskBooking;});
      _processor.BookDesk(_request);
      _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()),Times.Once);

      Assert.NotNull(savedDeskBooking);
      Assert.Equal(_request.FirstName,savedDeskBooking.FirstName);
      Assert.Equal(_request.LastName,savedDeskBooking.LastName);
      Assert.Equal(_request.Email,savedDeskBooking.Email);
       Assert.Equal(_request.Date,savedDeskBooking.Date);
       Assert.Equal(_availableDesks.First().Id,savedDeskBooking.DeskId);
      
    }
 
        [Fact]
    public void ShouldNotSaveDeskBookingIfNoDeskAvailable()
    {
      _availableDesks.Clear();

      _processor.BookDesk(_request);
      _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()),Times.Never);

      
    }
 
  }
}
