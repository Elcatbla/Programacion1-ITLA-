while (true)
{
    Console.WriteLine("\n=== CALCULATOR ===");
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Subtract");
    Console.WriteLine("3. Multiply");
    Console.WriteLine("4. Divide");
    Console.WriteLine("5. Exit");

    Console.Write("Select an option: ");
    string? option = Console.ReadLine();

    if (option == "5")
    {
        Console.WriteLine("Program finished.");
        break;
    }

    Console.Write("Enter the first number: ");
    double num1 = Convert.ToDouble(Console.ReadLine());

    Console.Write("Enter the second number: ");
    double num2 = Convert.ToDouble(Console.ReadLine());

    switch (option)
    {
        case "1":
            Console.WriteLine($"Result: {num1 + num2}");
            break;

        case "2":
            Console.WriteLine($"Result: {num1 - num2}");
            break;

        case "3":
            Console.WriteLine($"Result: {num1 * num2}");
            break;

        case "4":
            Console.WriteLine($"Result: {num1 / num2}");
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}