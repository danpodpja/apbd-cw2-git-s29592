using apbd_cw2_git_s29592.Domain.Equipment;
using apbd_cw2_git_s29592.Domain.Users;

namespace apbd_cw2_git_s29592.Domain;

public class Rental
{
    public Guid Id { get; } = Guid.NewGuid();
    public User User { get; }
    public EquipmentA Equipment { get; }
    public DateTime RentedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal Penalty { get; private set; }

    public bool IsReturned => ReturnedAt.HasValue;
    public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

    public Rental(User user, EquipmentA equipment, DateTime rentedAt, int rentalDays)
    {
        User = user;
        Equipment = equipment;
        RentedAt = rentedAt;
        DueDate = rentedAt.AddDays(rentalDays);
    }

    public void Complete(DateTime returnedAt, decimal penalty)
    {
        ReturnedAt = returnedAt;
        Penalty = penalty;
    }

    public override string ToString() =>
        $"[{Id.ToString()[..8]}] {User.FullName} → {Equipment.Name} " +
        $"(do: {DueDate:dd.MM.yyyy}, zwrot: {(IsReturned ? ReturnedAt!.Value.ToString("dd.MM.yyyy") : "—")})";
}