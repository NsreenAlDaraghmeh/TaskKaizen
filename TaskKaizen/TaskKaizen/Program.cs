//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using TaskKaizen;


namespace YourNamespace
{
    class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Excel Import Application!");

            Console.Write("Please enter the path of the Excel file to process: ");
            string filePath = Console.ReadLine();
            var connectionString = "Server=DESKTOP-30G2DHS;Database=TaskKaizen;Integrated Security=true;";

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        Console.WriteLine("Error: The Excel file does not contain any worksheets.");
                        return;
                    }

                    var worksheet = package.Workbook.Worksheets[1]; // Access the first worksheet at index 1

                    int rowCount = worksheet.Dimension.Rows;
                    int successCount = 0;
                    var users = new List<User>(); // List to store valid User objects

                    for (int row = 2; row <= rowCount; row++) // Assuming data starts from row 2 (header in row 1)
                    {
                        string name = worksheet.Cells[row, 1].Value?.ToString();
                        string email = worksheet.Cells[row, 2].Value?.ToString();
                        bool isActive = bool.Parse(worksheet.Cells[row, 3].Value?.ToString());

                        // Validate the parsed data
                        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                        {
                            Console.WriteLine("Error: Invalid data found in row " + row + ". Name and Email fields are required.");
                            break; // Stop processing the file
                        }

                        // Create User object from the validated data
                        var user = new User
                        {
                            NAME = name,
                            Email = email,
                            IsActive = isActive
                        };

                        users.Add(user); // Add the User object to the list

                        successCount++;
                    }

                    // Initialize a new instance of the DataContext class
                    using (var dataContext = new DataContext(connectionString))
                    {
                        // Insert the list of User objects into the database
                        dataContext.Users.AddRange(users);
                        dataContext.SaveChanges();

                        int totalUsersInserted = successCount; // Use the 'successCount' variable instead of a fixed value
                        Console.WriteLine("Data processed successfully. Total users inserted: " + totalUsersInserted);
                    }

                    Console.WriteLine("Data processed successfully. Total users inserted: " + successCount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

    }
}
