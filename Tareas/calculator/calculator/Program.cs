while (true)
{
    Console.WriteLine("\n===== MAIN MENU =====");
    Console.WriteLine("1. Add");
    Console.WriteLine("2. Subtract");
    Console.WriteLine("3. Multiply");
    Console.WriteLine("4. Divide");
    Console.WriteLine("5. Check Student Grade");
    Console.WriteLine("6. Exit");

    Console.Write("Select an option: ");
    string? option = Console.ReadLine();

    if (option == "6")
    {
        Console.WriteLine("Program finished.");
        break;
    }

    if (option == "5")
    {
        try
        {
            Console.Write("Student name: ");
            string? studentName = Console.ReadLine();

            Console.Write("Final grade: ");
            double grade = Convert.ToDouble(Console.ReadLine());

            if (grade >= 70 && grade <= 100)
            {
                Console.WriteLine($"{studentName} passed the course.");
            }
            else if (grade >= 0)
            {
                Console.WriteLine($"{studentName} failed the course.");
            }
            else
            {
                Console.WriteLine("Grade cannot be negative.");
            }
        }
        catch
        {
            Console.WriteLine("Please enter a valid grade.");
        }

        continue;
    }

    try
    {
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
                if (num2 == 0)
                {
                    Console.WriteLine("Cannot divide by zero.");
                }
                else
                {
                    Console.WriteLine($"Result: {num1 / num2}");
                }
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch
    {
        Console.WriteLine("Error: please enter numeric values.");
    }
}