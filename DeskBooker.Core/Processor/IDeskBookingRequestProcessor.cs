using DeskBooker.Core.Domain;
   public interface IDeskBookingRequestProcessor
    {
        DeskBookingResult BookDesk(DeskBookingRequest request);
    }