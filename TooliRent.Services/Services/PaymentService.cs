using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IRentalRepository _rentalRepo;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepo, IRentalRepository rentalRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _rentalRepo = rentalRepo;
            _mapper = mapper;
        }

        // IGenericService-implementation
        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            var all = await _paymentRepo.GetAllDetailedAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(all);
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var entity = await _paymentRepo.GetDetailedByIdAsync(id);
            return entity is null ? null : _mapper.Map<PaymentDto>(entity);
        }

        public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
        {
            var entity = _mapper.Map<Payment>(dto);
            entity.PaymentDate = DateTime.UtcNow; // initialt datum

            await _paymentRepo.AddAsync(entity);
            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<PaymentDto?> UpdateAsync(int id, UpdatePaymentDto dto)
        {
            var entity = await _paymentRepo.GetDetailedByIdAsync(id);
            if (entity is null) return null;

            // Uppdatera method om den finns
            if (dto.Method.HasValue)
                entity.PaymentMethod = dto.Method.Value;

            // Uppdatera status om den finns
            if (dto.Status.HasValue)
            {
                entity.Status = dto.Status.Value;

                if (dto.Status.Value == PaymentStatus.Completed)
                {
                    entity.PaymentDate = DateTime.UtcNow;

                    var rental = entity.Rental;
                    if (rental != null && rental.Status == RentalStatus.Pending)
                    {
                        rental.Status = RentalStatus.Confirmed;
                        rental.ModifiedAt = DateTime.UtcNow;
                        await _rentalRepo.UpdateAsync(rental);
                    }
                }
            }

            await _paymentRepo.UpdateAsync(entity);
            return _mapper.Map<PaymentDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _paymentRepo.GetByIdAsync(id);
            if (existing is null) return false;

            await _paymentRepo.DeleteAsync(id);
            return true;
        }

        //public async Task<PaymentDto?> UpdateAsync(int id, UpdatePaymentDto dto)
        //{
        //    var entity = await _paymentRepo.GetByIdAsync(id);
        //    if (entity is null) return null;

        //    _mapper.Map(dto, entity);

        //    // Om Status sätts till Paid → sätt datum + uppdatera Rental
        //    if (dto.Status is not null && Enum.TryParse<PaymentStatus>(dto.Status, out var status))
        //    {
        //        if (status == PaymentStatus.Completed)
        //        {
        //            entity.PaymentDate = DateTime.UtcNow;

        //            var rental = entity.Rental;
        //            if (rental != null && rental.Status == RentalStatus.Pending)
        //            {
        //                rental.Status = RentalStatus.Confirmed;
        //                rental.ModifiedAt = DateTime.UtcNow;
        //                await _rentalRepo.UpdateAsync(rental);
        //            }
        //        }
        //    }

        //    await _paymentRepo.UpdateAsync(entity);
        //    return _mapper.Map<PaymentDto>(entity);
        //}

        //// Extra metod för att bara uppdatera status
        //public async Task<PaymentDto?> UpdateStatusAsync(int id, PaymentStatus status)
        //{
        //    var entity = await _paymentRepo.GetDetailedByIdAsync(id);
        //    if (entity is null) return null;

        //    entity.Status = status;

        //    if (status == PaymentStatus.Completed)
        //    {
        //        entity.PaymentDate = DateTime.UtcNow;

        //        var rental = entity.Rental;
        //        if (rental != null && rental.Status == RentalStatus.Pending)
        //        {
        //            rental.Status = RentalStatus.Confirmed;
        //            rental.ModifiedAt = DateTime.UtcNow;
        //            await _rentalRepo.UpdateAsync(rental);
        //        }
        //    }

        //    await _paymentRepo.UpdateAsync(entity);
        //    return _mapper.Map<PaymentDto>(entity);
        //}

        //public async Task<PaymentDto?> UpdateMethodAsync(int id, PaymentMethod method)
        //{
        //    var entity = await _paymentRepo.GetDetailedByIdAsync(id);
        //    if (entity is null) return null;

        //    entity.PaymentMethod = method;

        //    await _paymentRepo.UpdateAsync(entity);
        //    return _mapper.Map<PaymentDto>(entity);
        //}
    }
}
