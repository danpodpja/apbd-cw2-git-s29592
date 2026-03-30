using apbd_cw2_git_s29592.Domain.Equipment;

namespace apbd_cw2_git_s29592.Services;

public class EquipmentService
{
    private readonly List<EquipmentA> _equipment = [];

    public void Add(EquipmentA equipment) => _equipment.Add(equipment);

    public IReadOnlyList<EquipmentA> GetAll() => _equipment.AsReadOnly();

    public IEnumerable<EquipmentA> GetAvailable() =>
        _equipment.Where(e => e.IsAvailable);

    public EquipmentA? FindById(Guid id) =>
        _equipment.FirstOrDefault(e => e.Id == id);

    public OperationResult MarkUnavailable(Guid id)
    {
        var eq = FindById(id);
        if (eq is null) return OperationResult.Fail("Nie znaleziono sprzętu.");
        if (!eq.IsAvailable)
            return OperationResult.Fail($"Sprzęt '{eq.Name}' nie jest dostępny (status: {eq.Status}).");

        eq.MarkAsUnavailable();
        return OperationResult.Ok();
    }
}