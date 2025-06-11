using Empolyees.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace YourProjectNamespace
{
    public static class DatabaseHelper
    {
        private static readonly string dbFilePath = "company_db.sqlite";
        private static readonly string connectionString = $"Data Source={dbFilePath};Version=3;";

        public static string ConnectionString => connectionString;

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createCompaniesTable = @"
                    CREATE TABLE IF NOT EXISTS Companies (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        LogoPath TEXT,
                        Description TEXT,
                        PhoneNumber TEXT,
                        Email TEXT
                    );";

                string createEmployeesTable = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CompanyId INTEGER NOT NULL,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        Age INTEGER,
                        Position TEXT,
                        Salary REAL,
                        Email TEXT,
                        PhoneNumber TEXT,
                        PhotoPath TEXT,
                        FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
                    );";

                using (var command = new SQLiteCommand(createCompaniesTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createEmployeesTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Company> GetAllCompanies()
        {
            List<Company> list = new List<Company>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Companies";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Company
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            LogoPath = reader["LogoPath"].ToString(),
                            Description = reader["Description"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public static List<Employee> GetEmployeesByCompanyId(int companyId)
        {
            List<Employee> list = new List<Employee>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Employees WHERE CompanyId = @CompanyId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Employee
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                CompanyId = Convert.ToInt32(reader["CompanyId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Position = reader["Position"].ToString(),
                                Salary = Convert.ToDouble(reader["Salary"]),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                PhotoPath = reader["PhotoPath"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }
        public static void SeedTestData()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Check if Companies table is already filled
                string checkQuery = "SELECT COUNT(*) FROM Companies";
                using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                {
                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0) return; // Already seeded
                }

                // Insert sample companies
                string[] insertCompanies = new string[]
                {
                @"INSERT INTO Companies (Name, LogoPath, Description, PhoneNumber, Email)
                  VALUES ('Tech Solutions Ltd.', 'TechSolutionsLtd.png', 'IT and software services provider.', '123-456-7890', 'info@techsolutions.com');",

                @"INSERT INTO Companies (Name, LogoPath, Description, PhoneNumber, Email)
                  VALUES ('Green Earth Corp', 'GreenEarthCorp.png', 'Environmental engineering company.', '987-654-3210', 'contact@greenearth.com');",

                @"INSERT INTO Companies (Name, LogoPath, Description, PhoneNumber, Email)
                  VALUES ('HealthPlus Inc.', 'HealthPlusInc.png', 'Healthcare and medical services.', '456-789-0123', 'support@healthplus.com');",

                @"INSERT INTO Companies (Name, LogoPath, Description, PhoneNumber, Email)
                  VALUES ('FinSecure LLC', 'FinSecureLLC.png', 'Financial consulting and auditing.', '321-654-0987', 'info@finsecure.com');",

                };

                foreach (var insertCompany in insertCompanies)
                {
                    using (var cmd = new SQLiteCommand(insertCompany, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Insert employees
                string insertEmployees = @"
                INSERT INTO Employees (CompanyId, FirstName, LastName, Age, Position, Salary, Email, PhoneNumber, PhotoPath)
                VALUES
                -- Tech Solutions Ltd (1)
                (1, 'Ali', 'Rahimi', 22, 'AI/ML Engineer', 200000, 'ali.rahimi@techsolutions.com', '579-2431', 'AliRahimi.png'),
                (1, 'John', 'Doe', 28, 'Software Engineer', 60000, 'john.doe@techsolutions.com', '555-1111', 'JohnDoe.png'),
                (1, 'Sarah', 'Mills', 32, 'Project Manager', 72000, 'sarah.mills@techsolutions.com', '555-1112', 'SarahMills.png'),
                (1, 'David', 'Clark', 30, 'QA Analyst', 50000, 'david.clark@techsolutions.com', '555-1113', 'DavidClark.png'),
                (1, 'Emily', 'Stone', 26, 'UI/UX Designer', 52000, 'emily.stone@techsolutions.com', '555-1114', 'EmilyStone.png'),
                (1, 'Ryan', 'Baker', 34, 'DevOps Engineer', 65000, 'ryan.baker@techsolutions.com', '555-1115', 'RyanBaker.png'),
                (1, 'Grace', 'Lee', 29, 'Frontend Developer', 58000, 'grace.lee@techsolutions.com', '555-1116', 'GraceLee.png'),
                (1, 'Mark', 'Davis', 38, 'CTO', 90000, 'mark.davis@techsolutions.com', '555-1117', 'MarkDavis.png'),
                (1, 'Lia', 'Watson', 35, 'Research Analyst', 58000, 'lia.watson@techsolutions.com', '555-4444', 'LiaWatson.png');";

                using (var cmd = new SQLiteCommand(insertEmployees, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
