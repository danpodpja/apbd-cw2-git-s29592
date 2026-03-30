# Uczelniana Wypożyczalnia Sprzętu

Aplikacja konsolowa w C# (.NET 8) obsługująca wypożyczalnię sprzętu uczelnianego.

## Jak uruchomić

```bash
dotnet run
```

## Struktura kodu i decyzje projektowe

### Podział na warstwy

Projekt jest podzielony na wyraźne obszary:

- **`Domain/`** - czyste klasy modelowe bez żadnych zależności zewnętrznych. Tutaj żyją `Equipment`, `User`, `Rental` i
  enum `EquipmentStatus`. Warstwa ta nie wie nic o logice biznesowej.
- **`Services/`** - logika aplikacji. Każda klasa serwisowa ma jedną odpowiedzialność: `EquipmentService` zarządza
  sprzętem, `UserService` użytkownikami, `RentalService` wypożyczeniami. `RentalPolicy` to jedno, centralne miejsce
  wszystkich reguł biznesowych.

### Kohezja

Każda klasa ma jedną wyraźną odpowiedzialność:

- `Rental` przechowuje stan wypożyczenia i udostępnia metodę `Complete()` - nie liczy kar.
- `RentalPolicy` liczy kary i limity - nie przechowuje żadnych danych.
- `RentalService` orchestruje operacje - nie formatuje outputu.

### Coupling

`RentalService` zależy od `EquipmentService` i `UserService` przez pola, nie przez `static` ani singletony - można je
podmienić lub testować niezależnie. `Domain` nie importuje nic z `Services`.

### Dziedziczenie tylko tam, gdzie wynika z domeny

- `EquipmentA` -> `Laptop`, `Projector`, `Camera` - każdy typ sprzętu **jest** sprzętem i ma własne pola specyficzne.
  Metoda abstrakcyjna `GetTypeName()` wymuszona na każdym typie.
- `User` -> `Student`, `Employee` - różne typy użytkowników, różnie obsługiwane przez `RentalPolicy`.

### Reguły biznesowe w jednym miejscu

Klasa `RentalPolicy` zawiera wszystkie wartości konfigurowalne:

- `StudentRentalLimit = 2`
- `EmployeeRentalLimit = 5`
- `DailyPenaltyRate = 5.00 PLN`

Zmiana stawki kary nie wymaga szukania jej w pięciu miejscach.

### Obsługa błędów

Zamiast rzucać wyjątki na operacje biznesowe, użytkowane jest `OperationResult` / `OperationResult<T>`. Wywołujący
zawsze wie, czy operacja się powiodła i dlaczego nie. Wyjątki są zarezerwowane na sytuacje naprawdę nieoczekiwane (np.
nieznany typ użytkownika w `RentalPolicy`).

