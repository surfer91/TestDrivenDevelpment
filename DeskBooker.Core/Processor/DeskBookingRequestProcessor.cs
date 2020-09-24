using DeskBooker.Core.Domain;
using System;

namespace DeskBooker.Core.Processor
{
  public class DeskBookingRequestProcessor
  {
        private readonly IDeskBookingRepository _deskBookingRepository;
        private IDeskRepository @object;

        public DeskBookingRequestProcessor()
    {
    }

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository)
        {
         
            _deskBookingRepository = deskBookingRepository;
        }

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository, IDeskRepository @object) : this(deskBookingRepository)
        {
            this.@object = @object;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _deskBookingRepository.Save(Create<DeskBooking>(request));

            return Create<DeskBookingResult>(request);}

         private static T Create<T>(DeskBookingRequest request) where T :DeskBookingBase,new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date,
            };
        }
    }
}