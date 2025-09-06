using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

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

    // IGenericService implementation
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
        => await CreateBookingAsync(dto);

    public async Task<RentalDto?> UpdateAsync(int id, UpdateRentalDto dto)
    {
        var entity = await _rentalRepo.GetByIdAsync(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        entity.ModifiedAt = DateTime.UtcNow;
        await _rentalRepo.UpdateAsync(entity);

        return _mapper.Map<RentalDto>(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _rentalRepo.GetByIdAsync(id);
        if (existing is null) return false;
        await _rentalRepo.DeleteAsync(id);
        return true;
    }

    // Bokning (extra metod)
    public async Task<RentalDto> CreateBookingAsync(CreateRentalDto dto)
    {
        // 1) Hämta kund
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId)
                       ?? throw new KeyNotFoundException("Customer not found");

        // 2) Hämta verktyg
        var tool = await _toolRepo.GetByIdAsync(dto.ToolId)
                   ?? throw new KeyNotFoundException("Tool not found");

        // 3) Kontrollera tillgänglighet
        var available = await _rentalRepo.ToolIsAvailableAsync(dto.ToolId, dto.StartDate.Date, dto.EndDate.Date);
        if (!available)
            throw new InvalidOperationException("Tool is not available for selected dates");

        // 4) Beräkna pris
        var days = (dto.EndDate.Date - dto.StartDate.Date).Days;
        if (days < 1) days = 1;
        var totalPrice = tool.Price * days;

        // 5) Skapa Rental
        var rental = _mapper.Map<Rental>(dto);
        rental.CustomerId = customer.Id;
        rental.TotalPrice = totalPrice;
        rental.Status = RentalStatus.Active; // eller Pending om du vill
        rental.CreatedAt = DateTime.UtcNow;
        rental.ModifiedAt = DateTime.UtcNow;

        await _rentalRepo.AddAsync(rental);

        // 6) Skapa en "pending" Payment kopplad till Rental
        var payment = new Payment
        {
            RentalId = rental.Id,
            Amount = totalPrice,
            PaymentMethod = PaymentMethod.Invoice,
            Status = PaymentStatus.Pending, // Pending tills kunden betalar
        };
        await _paymentRepo.AddAsync(payment);

        // 7) Hämta detaljerad Rental med inkluderade relationer
        var createdRental = await _rentalRepo.GetDetailedByIdAsync(rental.Id);

        // 8) Returnera RentalDto
        return _mapper.Map<RentalDto>(createdRental);
    }

}
