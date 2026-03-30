using apbd_cw2_git_s29592.Domain.Equipment;
using apbd_cw2_git_s29592.Domain.Users;
using apbd_cw2_git_s29592.Services;

namespace apbd_cw2_git_s29592;

public class Demo
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService      _userService;
    private readonly RentalService    _rentalService;

    public Demo(EquipmentService equipmentService, UserService userService, RentalService rentalService)
    {
        _equipmentService = equipmentService;
        _userService      = userService;
        _rentalService    = rentalService;
    }

    public void RunDemo()
    {
        Header("DEMO: Uczelniana Wypożyczalnia Sprzętu");

        Section("1. Dodawanie sprzętu");
        var laptop1   = new Laptop("Dell XPS 15", 16, 512);
        var laptop2   = new Laptop("MacBook Pro 14", 32, 1024);
        var projector = new Projector("Epson EB-W51", "WXGA 1280x800", 4000);
        var camera    = new Camera("Canon EOS R50", 24.2, hasTripod: true);
        var laptop3   = new Laptop("ThinkPad X1 Carbon", 16, 256);

        foreach (var e in new EquipmentA[] { laptop1, laptop2, projector, camera, laptop3 })
        {
            _equipmentService.Add(e);
            Console.WriteLine($"   Dodano: {e}");
        }

        Section("2. Dodawanie użytkowników");
        var student1 = new Student("Anna", "Kowalska", "s12345");
        var student2 = new Student("Piotr", "Nowak", "s67890");
        var employee = new Employee("Marek", "Wiśniewski", "Informatyki", "E001");

        foreach (var u in new User[] { student1, student2, employee })
        {
            _userService.Add(u);
            Console.WriteLine($"   Dodano: {u}");
        }

        Section("3. Poprawne wypożyczenia");
        TryRent(student1.Id, laptop1.Id, 7);
        TryRent(student1.Id, camera.Id,  3);
        TryRent(employee.Id, projector.Id, 14);

                Section("4. Próby niepoprawnych operacji");

        Console.WriteLine("   Student próbuje wypożyczyć 3. sprzęt (limit: 2):");
        TryRent(student1.Id, laptop2.Id, 5);

        Console.WriteLine("   Próba wypożyczenia już wypożyczonego laptopa:");
        TryRent(student2.Id, laptop1.Id, 3);

        Console.WriteLine("   Oznaczenie projektora jako uszkodzonego i próba wypożyczenia:");
        _equipmentService.MarkUnavailable(projector.Id);
        Console.WriteLine($"    Status projektora: {projector.Status}");
        TryRent(student2.Id, projector.Id, 7);

        Section("5. Dostępny sprzęt");
        foreach (var e in _equipmentService.GetAvailable())
            Console.WriteLine($"   {e}");

        Section($"6. Aktywne wypożyczenia: {student1.FullName}");
        foreach (var r in _rentalService.GetActiveRentalsByUser(student1.Id))
            Console.WriteLine($"   {r}");

        Section("7. Zwrot w terminie");
        var rentalCamera = _rentalService.GetAll()
            .First(r => r.Equipment.Id == camera.Id && !r.IsReturned);
        var returnOnTime = rentalCamera.DueDate.AddDays(-1);
        var result = _rentalService.Return(rentalCamera.Id, returnOnTime);
        Console.WriteLine(result.IsSuccess
            ? $"   Zwrócono aparat. Kara: {result.Value:F2} PLN"
            : $"   Błąd: {result.Error}");

                Section("8. Zwrot z opóźnieniem");
        var rentalLaptop = _rentalService.GetAll()
            .First(r => r.Equipment.Id == laptop1.Id && !r.IsReturned);
        var returnLate = rentalLaptop.DueDate.AddDays(4);
        var lateResult = _rentalService.Return(rentalLaptop.Id, returnLate);
        Console.WriteLine(lateResult.IsSuccess
            ? $"   Zwrócono laptop (4 dni po terminie). Kara: {lateResult.Value:F2} PLN"
            : $"   Błąd: {lateResult.Error}");

                Section("9. Przeterminowane wypożyczenia");
        var overdue = _rentalService.GetOverdueRentals().ToList();
        if (overdue.Count == 0)
            Console.WriteLine("  Brak przeterminowanych wypożyczeń.");
        else
            foreach (var r in overdue)
                Console.WriteLine($"   {r}");

        Section("10. Raport końcowy");
        Console.WriteLine(_rentalService.GenerateReport());
    }

    
    private void TryRent(Guid userId, Guid equipmentId, int days)
    {
        var r = _rentalService.Rent(userId, equipmentId, days);
        var user = _userService.FindById(userId)!;
        var eq   = _equipmentService.FindById(equipmentId)!;
        Console.WriteLine(r.IsSuccess
            ? $"   {user.FullName} wypożyczył(a) '{eq.Name}' na {days} dni."
            : $"   Odmowa [{user.FullName}  '{eq.Name}']: {r.Error}");
    }

    private static void Header(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n{'=',1}{'=',44}");
        Console.WriteLine($"  {text}");
        Console.WriteLine($"{'=',1}{'=',44}\n");
        Console.ResetColor();
    }

    private static void Section(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n-- {text} --");
        Console.ResetColor();
    }
}