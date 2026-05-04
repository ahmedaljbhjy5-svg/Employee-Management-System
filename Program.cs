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
    }

