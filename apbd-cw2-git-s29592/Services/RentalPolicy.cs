using apbd_cw2_git_s29592.Domain;
using apbd_cw2_git_s29592.Domain.Users;

namespace apbd_cw2_git_s29592.Services;

public class RentalPolicy
{
    public int StudentRentalLimit { get; init; } = 2;
    public int EmployeeRentalLimit { get; init; } = 5;
    public decimal DailyPenaltyRate { get; init; } = 5.00m;

    public int GetRentalLimit(User user) => user switch
    {
        Student => StudentRentalLimit,
        Employee => EmployeeRentalLimit,
        _ => throw new ArgumentException($"Nieznany typ użytkownika: {user.GetType().Name}")
    };

    public bool CanRent(User user, int activeRentalsCount) =>
        activeRentalsCount < GetRentalLimit(user);

    public decimal CalculatePenalty(Rental rental, DateTime returnDate)
    {
        if (returnDate <= rental.DueDate) return 0m;

        var daysLate = (int)Math.Ceiling((returnDate - rental.DueDate).TotalDays);
        return daysLate * DailyPenaltyRate;
    }
}