using System;
using System.Collections.Generic;

namespace ipg203
{
    // ============================================================
    // 1. INTERFACE - Defines basic operations for any employee
    // ============================================================
    public interface IEmployeeActions
    {
        double CalculateSalary();
        void DisplayInfo();
        void Work();
    }

    // ============================================================
    // 2. ABSTRACT CLASS - Implements the interface
    // ============================================================
    public abstract class Employee : IEmployeeActions
    {
        // Encapsulation: all fields are private
        private string _name;
        private int _age;
        private double _baseSalary;

        // Static properties to track all employees
        public static int Count { get; private set; } = 0;
        public static double TotalSalaryBudget { get; private set; } = 0;

        // Delegate and Event for high salary alert
        public delegate void SalaryAlertHandler(string employeeName, double salary);
        public event SalaryAlertHandler OnHighSalary;

        // Read-only property - cannot be changed after creation
        public int Id { get; private set; }

        public string Name
        {
            get { return _name; }
            private set
            {
                if (!DataValidator.IsValidName(value))
                    throw new ArgumentException("Invalid name!");
                _name = value;
            }
        }

        public int Age
        {
            get { return _age; }
            private set
            {
                if (!DataValidator.IsValidAge(value))
                    throw new ArgumentException("Age must be between 18 and 65!");
                _age = value;
            }
        }

        public double BaseSalary
        {
            get { return _baseSalary; }
            protected set
            {
                if (!DataValidator.IsValidSalary(value))
                    throw new ArgumentException("Salary must be greater than zero!");
                _baseSalary = value;
            }
        }

        // Constructor
        protected Employee(int id, string name, int age, double baseSalary)
        {
            Id = id;
            Name = name;
            Age = age;
            BaseSalary = baseSalary;
            Count++;
        }

        // Abstract methods - must be implemented in subclasses
        public abstract double CalculateSalary();
        public abstract void Work();

        // Shared method for all employees
        public void DisplayInfo()
        {
            Console.WriteLine($"  ID         : {Id}");
            Console.WriteLine($"  Name       : {Name}");
            Console.WriteLine($"  Age        : {Age}");
            Console.WriteLine($"  Salary     : {CalculateSalary():F2} $");
        }

        //  CheckSalary now only fires the event, does NOT add to TotalSalaryBudget
        // TotalSalaryBudget is calculated once at the end via CalculateTotalBudget()
        public void CheckSalary()
        {
            double salary = CalculateSalary();

            if (salary > 10000)
            {
                OnHighSalary?.Invoke(Name, salary);
            }
        }

        // New static method to calculate total budget once (avoids accumulation bug)
        public static void CalculateTotalBudget(List<Employee> employees)
        {
            TotalSalaryBudget = 0;
            foreach (var emp in employees)
            {
                TotalSalaryBudget += emp.CalculateSalary();
            }
        }
    }============================================================
    // 6. Main Program
    // ============================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            Console.WriteLine("  Employee Management System - OOP  ");
            Console.WriteLine("====================================");

            // Create department
            Department techDept = new Department("Technology");

            // Create employees
            Manager m1 = new Manager(1, "Ali", 40, 8000, 3500);
            Developer d1 = new Developer(2, "Sara", 25, 160, 75);
            Developer d2 = new Developer(3, "Khaled", 30, 200, 60);
            Intern i1 = new Intern(4, "Omar", 22, 1500, "Development");

            // Subscribe to high salary event
            m1.OnHighSalary += HandleHighSalaryAlert;
            d1.OnHighSalary += HandleHighSalaryAlert;
            d2.OnHighSalary += HandleHighSalaryAlert;
            i1.OnHighSalary += HandleHighSalaryAlert;

            // Add employees to department
            techDept.AddEmployee(m1);
            techDept.AddEmployee(d1);
            techDept.AddEmployee(d2);
            techDept.AddEmployee(i1);

            // Show all employees (Polymorphism)
            techDept.ShowAllEmployees();

            // FIX: Calculate total budget ONCE here, not inside CheckSalary()
            Employee.CalculateTotalBudget(techDept.GetEmployees());

            // Show general statistics (Static Properties)
            Console.WriteLine("\n========== General Statistics ==========");
            Console.WriteLine($"  Total Employees      : {Employee.Count}");
            Console.WriteLine($"  Total Salary Budget  : {Employee.TotalSalaryBudget:F2} $");

            // Test DataValidator (Static Class)
            Console.WriteLine("\n========== DataValidator Test ==========");
            Console.WriteLine($"  'Ali' valid name?    : {DataValidator.IsValidName("Ali")}");
            Console.WriteLine($"  '' valid name?       : {DataValidator.IsValidName("")}");
            Console.WriteLine($"  Age 25 valid?        : {DataValidator.IsValidAge(25)}");
            Console.WriteLine($"  Age 70 valid?        : {DataValidator.IsValidAge(70)}");
            Console.WriteLine($"  Salary 5000 valid?   : {DataValidator.IsValidSalary(5000)}");
            Console.WriteLine($"  Salary -100 valid?   : {DataValidator.IsValidSalary(-100)}");
            Console.WriteLine($"  Hours 160 valid?     : {DataValidator.IsValidHours(160)}");
            Console.WriteLine($"  Hours 800 valid?     : {DataValidator.IsValidHours(800)}");

            Console.WriteLine("\n====================================");
            Console.WriteLine("       Program finished successfully");
            Console.WriteLine("====================================");

            Console.ReadKey();
        }

        // Handle high salary alert event
        static void HandleHighSalaryAlert(string employeeName, double salary)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [ALERT] {employeeName} has a high salary = {salary:F2} $");
            Console.ResetColor();
        }
    }
}


