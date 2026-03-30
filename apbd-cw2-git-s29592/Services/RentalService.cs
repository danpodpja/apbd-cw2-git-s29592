using apbd_cw2_git_s29592.Domain;

namespace apbd_cw2_git_s29592.Services;

public class RentalService
{
    private readonly List<Rental>  _rentals  = [];
    private readonly RentalPolicy  _policy;
    private readonly EquipmentService _equipmentService;
    private readonly UserService      _userService;

    public RentalService(RentalPolicy policy, EquipmentService equipmentService, UserService userService)
    {
        _policy           = policy;
        _equipmentService = equipmentService;
        _userService      = userService;
    }

    public OperationResult Rent(Guid userId, Guid equipmentId, int days, DateTime? rentedAt = null)
    {
        var user = _userService.FindById(userId);
        if (user is null) return OperationResult.Fail("Nie znaleziono użytkownika.");

        var equipment = _equipmentService.FindById(equipmentId);
        if (equipment is null)     return OperationResult.Fail("Nie znaleziono sprzętu.");
        if (!equipment.IsAvailable) return OperationResult.Fail($"Sprzęt '{equipment.Name}' jest niedostępny (status: {equipment.Status}).");

        var activeCount = GetActiveRentalsByUser(userId).Count();
        if (!_policy.CanRent(user, activeCount))
            return OperationResult.Fail(
                $"{user.GetUserType()} może mieć max {_policy.GetRentalLimit(user)} aktywne wypożyczenia. " +
                $"Aktualnie: {activeCount}.");

        var rental = new Rental(user, equipment, rentedAt ?? DateTime.Now, days);
        equipment.MarkAsRented();
        _rentals.Add(rental);

        return OperationResult.Ok();
    }

    public OperationResult<decimal> Return(Guid rentalId, DateTime? returnedAt = null)
    {
        var rental = _rentals.FirstOrDefault(r => r.Id == rentalId);
        if (rental is null)       return OperationResult<decimal>.Fail("Nie znaleziono wypożyczenia.");
        if (rental.IsReturned)    return OperationResult<decimal>.Fail("Sprzęt został już zwrócony.");

        var returnDate = returnedAt ?? DateTime.Now;
        var penalty    = _policy.CalculatePenalty(rental, returnDate);

        rental.Complete(returnDate, penalty);
        rental.Equipment.MarkAsAvailable();

        return OperationResult<decimal>.Ok(penalty);
    }

    public IEnumerable<Rental> GetActiveRentalsByUser(Guid userId) =>
        _rentals.Where(r => r.User.Id == userId && !r.IsReturned);

    public IEnumerable<Rental> GetOverdueRentals() =>
        _rentals.Where(r => r.IsOverdue);

    public IReadOnlyList<Rental> GetAll() => _rentals.AsReadOnly();

    public string GenerateReport()
    {
        var total       = _rentals.Count;
        var active      = _rentals.Count(r => !r.IsReturned);
        var overdue     = _rentals.Count(r => r.IsOverdue);
        var returned    = _rentals.Count(r => r.IsReturned);
        var totalPenalty = _rentals.Where(r => r.IsReturned).Sum(r => r.Penalty);
        var available   = _equipmentService.GetAvailable().Count();

        return $"""
        ╔══════════════════════════════════════════╗
        ║       RAPORT WYPOŻYCZALNI SPRZĘTU        ║
        ╠══════════════════════════════════════════╣
        ║  Wszystkie wypożyczenia : {total,5}           ║
        ║  Aktywne                : {active,5}           ║
        ║  Przeterminowane        : {overdue,5}           ║
        ║  Zwrócone               : {returned,5}           ║
        ║  Dostępny sprzęt        : {available,5}           ║
        ║  Suma naliczonych kar   : {totalPenalty,5:F2} PLN      ║
        ╚══════════════════════════════════════════╝
        """;
    }
}