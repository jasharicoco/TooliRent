using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepo;
        private readonly IToolRepository _toolRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IMapper _mapper;

        public RentalService(
            IRentalRepository rentalRepo,
            IToolRepository toolRepo,
            IPaymentRepository paymentRepo,
            ICustomerRepository customerRepo,
            IMapper mapper)
        {
            _rentalRepo = rentalRepo;
            _toolRepo = toolRepo;
            _paymentRepo = paymentRepo;
            _customerRepo = customerRepo;
            _mapper = mapper;
        }

        // ---------------------------
        // Generiska CRUD
        // ---------------------------
        public async Task<IEnumerable<RentalDto>> GetAllAsync()
        {
            var all = await _rentalRepo.GetAllDetailedAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(all);
        }

        public async Task<RentalDto?> GetByIdAsync(int id)
        {
            var entity = await _rentalRepo.GetDetailedByIdAsync(id);
            return entity is null ? null : _mapper.Map<RentalDto>(entity);
        }

        public async Task<RentalDto> CreateAsync(CreateRentalDto dto)
        {
            throw new NotImplementedException("Use CreateBookingByUserAsync or CreateBookingByCustomerAsync instead.");
        }

        public async Task<RentalDto?> UpdateAsync(int id, UpdateRentalDto dto)
        {
            var entity = await _rentalRepo.GetByIdAsync(id);
            if (entity is null) return null;

            if (dto.EndDate.HasValue)
                entity.EndDate = dto.EndDate.Value;

            entity.ModifiedAt = DateTime.UtcNow;
            await _rentalRepo.UpdateAsync(entity);

            var updated = await _rentalRepo.GetDetailedByIdAsync(id);
            return _mapper.Map<RentalDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _rentalRepo.GetByIdAsync(id);
            if (existing is null) return false;

            await _rentalRepo.DeleteAsync(id);
            return true;
        }

        // ---------------------------
        // Bokning
        // ---------------------------
        public async Task<RentalDto> CreateBookingByUserAsync(CreateRentalByUserDto dto)
        {
            var customer = await _customerRepo.GetByGuidAsync(dto.UserId)
                           ?? throw new KeyNotFoundException("Customer not found for UserId");

            return await CreateBookingInternal(customer.Id, dto.ToolId, dto.StartDate, dto.EndDate);
        }

        public async Task<RentalDto> CreateBookingByCustomerAsync(CreateRentalByCustomerDto dto)
        {
            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId)
                           ?? throw new KeyNotFoundException("Customer not found");

            return await CreateBookingInternal(customer.Id, dto.ToolId, dto.StartDate, dto.EndDate);
        }

        private async Task<RentalDto> CreateBookingInternal(int customerId, int toolId, DateTime start, DateTime end)
        {
            var tool = await _toolRepo.GetByIdAsync(toolId)
                       ?? throw new KeyNotFoundException("Tool not found");

            var available = await _rentalRepo.ToolIsAvailableAsync(toolId, start.Date, end.Date);
            if (!available)
                throw new InvalidOperationException("Tool is not available for selected dates");

            var days = (end.Date - start.Date).Days;
            if (days < 1) days = 1;
            var totalPrice = tool.Price * days;

            var rental = new Rental
            {
                CustomerId = customerId,
                ToolId = tool.Id,
                StartDate = start,
                EndDate = end,
                TotalPrice = totalPrice,
                Status = RentalStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _rentalRepo.AddAsync(rental);

            var payment = new Payment
            {
                RentalId = rental.Id,
                Amount = totalPrice,
                PaymentMethod = PaymentMethod.Invoice,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);

            var createdRental = await _rentalRepo.GetDetailedByIdAsync(rental.Id);
            return _mapper.Map<RentalDto>(createdRental);
        }

        // ---------------------------
        // Status
        // ---------------------------
        public async Task<RentalDto?> UpdateStatusAsync(int id, RentalStatus status)
        {
            var rental = await _rentalRepo.GetDetailedByIdAsync(id);
            if (rental is null) return null;

            switch (status)
            {
                case RentalStatus.PickedUp:
                    if (rental.Status != RentalStatus.Confirmed)
                        throw new InvalidOperationException("Rental must be Confirmed before being PickedUp.");
                    break;

                case RentalStatus.Returned:
                    if (rental.Status != RentalStatus.PickedUp)
                        throw new InvalidOperationException("Rental must be PickedUp before being Returned.");
                    break;
            }

            rental.Status = status;
            rental.ModifiedAt = DateTime.UtcNow;

            await _rentalRepo.UpdateAsync(rental);

            var updated = await _rentalRepo.GetDetailedByIdAsync(id);
            return _mapper.Map<RentalDto>(updated);
        }

        // ---------------------------
        // Hämta bokningar
        // ---------------------------
        public async Task<IEnumerable<RentalDto>> GetByUserIdAsync(string userId)
        {
            var rentals = await _rentalRepo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<IEnumerable<RentalDto>> GetByCustomerIdAsync(int customerId)
        {
            var rentals = await _rentalRepo.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        // ---------------------------
        // Avboka
        // ---------------------------
        public async Task<bool> CancelBookingByUserAsync(string userId, int bookingId)
        {
            var rental = await _rentalRepo.GetDetailedByIdAsync(bookingId);
            if (rental == null) return false;

            if (rental.Customer?.User?.Id != userId) return false;
            if (rental.Status == RentalStatus.Cancelled) return false;
            if (rental.Status == RentalStatus.Returned || rental.Status == RentalStatus.Overdue) return false;

            rental.Status = RentalStatus.Cancelled;
            await _rentalRepo.UpdateAsync(rental);

            return true;
        }

        // ---------------------------
        // Statistik
        // ---------------------------
        public async Task<RentalStatisticsDto> GetStatisticsAsync()
        {
            var totalRentals = await _rentalRepo.CountAsync();
            var activeRentals = await _rentalRepo.CountByStatusAsync(RentalStatus.PickedUp)
                                + await _rentalRepo.CountByStatusAsync(RentalStatus.Confirmed);
            var overdueRentals = await _rentalRepo.CountByStatusAsync(RentalStatus.Overdue);
            var returnedRentals = await _rentalRepo.CountByStatusAsync(RentalStatus.Returned);
            var mostPopularTool = await _rentalRepo.GetMostPopularToolAsync();
            var avgDays = await _rentalRepo.GetAverageRentalDaysAsync();

            return new RentalStatisticsDto(
                totalRentals,
                activeRentals,
                overdueRentals,
                returnedRentals,
                mostPopularTool ?? "Ingen data",
                Math.Round(avgDays, 1)
            );
        }
    }
}
