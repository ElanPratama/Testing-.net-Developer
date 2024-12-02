using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        ParkingLot? parkingLot = null;
        string? input;

        Console.WriteLine("Welcome to Parking System!");
        Console.WriteLine("Enter a command (e.g., create_parking_lot 6) or type 'exit' to quit:");

        while (!string.IsNullOrEmpty(input = Console.ReadLine()))
        {
            string[] commands = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (commands.Length == 0)
            {
                Console.WriteLine("Invalid command. Please try again.");
                continue;
            }

            string action = commands[0].ToLower();

            try
            {
                switch (action)
                {
                    case "create_parking_lot":
                        if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: create_parking_lot <number_of_slots>");
                            break;
                        }
                        int capacity = int.Parse(commands[1]);
                        parkingLot = new ParkingLot(capacity);
                        Console.WriteLine($"Created a parking lot with {capacity} slots");
                        break;

                    case "park":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 4)
                        {
                            Console.WriteLine("Usage: park <registration_number> <color> <type>");
                        }
                        else
                        {
                            string registrationNumber = commands[1];
                            string color = commands[2];
                            string type = commands[3];
                            string result = parkingLot.Park(new Vehicle(registrationNumber, color, type));
                            Console.WriteLine(result);
                        }
                        break;

                    case "leave":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: leave <slot_number>");
                        }
                        else
                        {
                            int slot = int.Parse(commands[1]);
                            Console.WriteLine(parkingLot.Leave(slot));
                        }
                        break;

                    case "status":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else
                        {
                            parkingLot.PrintStatus();
                        }
                        break;

                    case "type_of_vehicles":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: type_of_vehicles <type>");
                        }
                        else
                        {
                            string type = commands[1];
                            Console.WriteLine(parkingLot.CountVehiclesByType(type));
                        }
                        break;

                    case "registration_numbers_for_vehicles_with_colour":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: registration_numbers_for_vehicles_with_colour <color>");
                        }
                        else
                        {
                            string color = commands[1];
                            Console.WriteLine(parkingLot.GetRegistrationNumbersByColor(color));
                        }
                        break;

                    case "slot_numbers_for_vehicles_with_colour":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: slot_numbers_for_vehicles_with_colour <color>");
                        }
                        else
                        {
                            string color = commands[1];
                            Console.WriteLine(parkingLot.GetSlotNumbersByColor(color));
                        }
                        break;

                    case "slot_number_for_registration_number":
                        if (parkingLot == null)
                        {
                            Console.WriteLine("Parking lot not created yet.");
                        }
                        else if (commands.Length < 2)
                        {
                            Console.WriteLine("Usage: slot_number_for_registration_number <registration_number>");
                        }
                        else
                        {
                            string registrationNumber = commands[1];
                            Console.WriteLine(parkingLot.GetSlotByRegistrationNumber(registrationNumber));
                        }
                        break;

                    case "exit":
                        Console.WriteLine("Exiting the Parking System. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Enter the next command or type 'exit' to quit:");
        }
    }
}

class ParkingLot
{
    private readonly int capacity;
    private readonly Dictionary<int, Vehicle> slots;

    public ParkingLot(int capacity)
    {
        this.capacity = capacity;
        slots = new Dictionary<int, Vehicle>();
    }

    public string Park(Vehicle vehicle)
    {
        if (slots.Count >= capacity) return "Sorry, parking lot is full";

        for (int i = 1; i <= capacity; i++)
        {
            if (!slots.ContainsKey(i))
            {
                slots[i] = vehicle;
                return $"Allocated slot number: {i}";
            }
        }

        return "Sorry, parking lot is full";
    }

    public string Leave(int slot)
    {
        if (!slots.ContainsKey(slot)) return $"Slot number {slot} is already empty";
        slots.Remove(slot);
        return $"Slot number {slot} is free";
    }

    public void PrintStatus()
    {
        if (slots.Count == 0)
        {
            Console.WriteLine("Parking lot is empty.");
            return;
        }

        Console.WriteLine("Slot\tNo.\t\tType\tRegistration No\tColour");
        foreach (var slot in slots)
        {
            Console.WriteLine($"{slot.Key}\t{slot.Value.RegistrationNumber}\t{slot.Value.Type}\t{slot.Value.Color}");
        }
    }

    public int CountVehiclesByType(string type)
    {
        return slots.Values.Count(v => v.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    public string GetRegistrationNumbersByColor(string color)
    {
        return string.Join(", ", slots.Values
            .Where(v => v.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(v => v.RegistrationNumber));
    }

    public string GetSlotNumbersByColor(string color)
    {
        return string.Join(", ", slots
            .Where(s => s.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Key));
    }

    public string GetSlotByRegistrationNumber(string registrationNumber)
    {
        var slot = slots.FirstOrDefault(s => s.Value.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
        return slot.Key != 0 ? slot.Key.ToString() : "Not found";
    }
}

class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}
