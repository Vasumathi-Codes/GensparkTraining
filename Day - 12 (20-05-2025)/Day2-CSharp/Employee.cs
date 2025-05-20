class Employee : IComparable<Employee>
{
    int id, age;
    string name;
    double salary;

    public Employee() { }

    public Employee(int id, int age, string name, double salary)
    {
        this.id = id;
        this.age = age;
        this.name = name;
        this.salary = salary;
    }

    public static int ReadPositiveInt(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result) && result > 0)
                return result;
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
    }

    public static int ReadNonNegativeInt(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out result) && result >= 0)
                return result;
            Console.WriteLine("Invalid input. Please enter a non-negative integer.");
        }
    }

    public static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input;
            Console.WriteLine("Input cannot be empty.");
        }
    }

    public static double ReadNonNegativeDouble(string prompt)
    {
        double result;
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out result) && result >= 0)
                return result;
            Console.WriteLine("❗ Please enter a valid non-negative number.");
        }
    }

    public void TakeEmployeeDetailsFromUser()
    {
        id = ReadNonNegativeInt("Please enter the employee ID : ");

        name = ReadNonEmptyString("Please enter the employee name : ");

        age = ReadNonNegativeInt("Please enter the employee age : ");

        salary = ReadNonNegativeDouble("Please enter the employee salary : ");
    }

    public override string ToString()
    {
        return $"Employee ID : {id}\nName : {name}\nAge : {age}\nSalary : {salary}";
    }

    public int CompareTo(Employee other)
    {
        return this.salary.CompareTo(other.salary); // Ascending order
    }

    public int Id { get => id; set => id = value; }
    public int Age { get => age; set => age = value; }
    public string Name { get => name; set => name = value; }
    public double Salary { get => salary; set => salary = value; }
}
