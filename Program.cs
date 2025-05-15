using NLog;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data.Common;

string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

do
{
    Console.WriteLine("Select an option:");
    Console.WriteLine("1)Display");//TODO test all display options
    Console.WriteLine("2)Add"); //ensure logger adds to the log and added item is actually added
    Console.WriteLine("3)Edit"); //TODO makesure all are tested
    Console.WriteLine("4)Delete"); //TODO Make user pass a fail safe for each delete. (Categories delete products)(after last product in a category is deleted all categorie is deleted) 
    //TODO maybe add a Clean up option to delet all orphaned products and categories
    Console.WriteLine("5)Exit");
    Console.WriteLine("Enter your choice (1-5):");

    string? choice = Console.ReadLine();
    Console.Clear();
    logger.Info("Option {choice} selected", choice);

    switch (choice)
    {
        case "1":
            Console.WriteLine("Display Options:");
            Console.WriteLine("1) Categories");
            Console.WriteLine("2) Products");
            string? displayType = Console.ReadLine();
            Console.Clear();
            logger.Info("Display type {displayType} selected", displayType);

            switch (displayType)
            {
                case "1": // Categories
                    Console.WriteLine("Category Display Options:");
                    Console.WriteLine("1) Display categories and their descriptions");
                    Console.WriteLine("2) Display all categories and related products");
                    Console.WriteLine("3) Display all categories and their active products");
                    Console.WriteLine("4) Display all categories and their discontinued products");
                    Console.WriteLine("5) Display one category and its active and discontinued products");
                    string? categoryChoice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Category display option {categoryChoice} selected", categoryChoice);

                    switch (categoryChoice)
                    {
                        case "1":
                            CategoryMethods.ViewCategories();
                            break;
                        case "2":
                            DisplayCategoryAndProducts();
                            break;
                        case "3":
                            DisplayCategoryAndActiveProducts();
                            break;
                        case "4":
                            // Your single category display code here
                            //get categorie id from user
                            Console.WriteLine("Enter the category ID to display:");
                            if (!int.TryParse(Console.ReadLine(), out int categoryId))
                            {
                                Console.WriteLine("Invalid category ID.");
                                break;
                            }
                          
                            CategoryMethods.DisplayCategoryById(categoryId);
                            break;
                           
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    break;

                case "2": // Products
                    Console.WriteLine("Product Display Options:");
                    Console.WriteLine("1) Display all products");
                    Console.WriteLine("2) Display all active products");
                    Console.WriteLine("3) Display all discontinued products");
                    Console.WriteLine("4) Display a specific product (all fields)");
                    string? productChoice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Product display option {productChoice} selected", productChoice);

                    switch (productChoice)
                    {
                        case "1":
                            ProductMethods.ViewProducts();
                            break;
                        case "2":
                            ProductMethods.ViewActiveProducts();
                            break;
                        case "3":
                            ProductMethods.ViewDiscontinuedProducts();
                            break;
                        case "4":
                            ProductMethods.DisplayProductById(0); // or however your method signature is
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            break;

        case "2":
            Console.WriteLine("Add Options:");
            Console.WriteLine("1) Add category");
            Console.WriteLine("2) Add product");
            Console.WriteLine("3) Add both");

            string? addChoice = Console.ReadLine();
            Console.Clear();
            logger.Info("Option {addChoice} selected", addChoice);

            switch (addChoice)
            {
                case "1":
                    // Add category functionality
                    CategoryMethods.AddCategory();
                    break;

                case "2":
                    // Add product functionality
                    ProductMethods.AddProduct();
                    break;

                case "3":
                    // Add both category and product functionality
                    AddCategoryAndProduct();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Returning to main menu.");
                    break;
            }
            break;

        case "3":
            Console.WriteLine("Edit Options:");
            Console.WriteLine("1) Edit category");
            Console.WriteLine("2) Edit product");
            //TODO edit a category and add a product
            Console.WriteLine("3) Edit a category and  add a product");

            string? editChoice = Console.ReadLine();
            Console.Clear();
            logger.Info("Option {editChoice} selected", editChoice);
            switch (editChoice)
            {
                case "1":
                    // Edit category functionality
                    CategoryMethods.EditCategory();
                    break;

                case "2":
                    // Edit product functionality
                    ProductMethods.EditProduct();
                    break;

                case "3":
                    // Edit a category and add a product functionality
                    EditCategoryAndAddProduct();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Returning to main menu.");
                    break;
            }
            break;

        case "4":
            Console.WriteLine("Open Delete Options");
            Console.WriteLine("1) Delete category");
            //TODO add a warning that the category will be deleted if a category is selected all products will be deleted
            Console.WriteLine("2) Delete product");
            //TODO add a warning that the product will be deleted

            string? deleteChoice = Console.ReadLine();
            Console.Clear();
            logger.Info("Option {deleteChoice} selected", deleteChoice);
            switch (deleteChoice)
            {
                case "1":
                    // Delete category functionality
                    CategoryMethods.DeleteCategory();
                    break;

                case "2":
                    // Delete product functionality
                    ProductMethods.DeleteProduct();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Returning to main menu.");
                    break;
            }
            break;

        case "5":
            Console.WriteLine("Exiting the program...");
            logger.Info("Exiting the program");
            return; // Exit the loop and end the program

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
} while (true);

//methods
void DisplayCategoryAndProducts()
{
    var db = new DataContext();
    var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
    foreach (var item in query)
    {
        Console.WriteLine($"{item.CategoryName}");
        foreach (Product p in item.Products)
            if (p.Discontinued == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\t{p.ProductName}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\t{p.ProductName}");
                Console.ResetColor();
            }
    }
}
void DisplayCategoryAndActiveProducts()
{
    var db = new DataContext();
    var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
    foreach (var item in query)
    {
        Console.WriteLine($"{item.CategoryName}");
        // Only active products
        var activeProducts = item.Products.Where(p => !p.Discontinued);
        ProductMethods.PrintProductList(activeProducts);
    }
}
void AddCategoryAndProduct()
{
    CategoryMethods.AddCategory();
    ProductMethods.AddProduct();
}
void EditCategoryAndAddProduct()
{
    // Display the list of categories first
    using (var db = new DataContext())
    {
        var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();
        Console.WriteLine("Available Categories:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
        }
    }

    // Get the category ID from the user
    Console.WriteLine("Enter the ID of the category you want to edit:");
    if (!int.TryParse(Console.ReadLine(), out int categoryId))
    {
        Console.WriteLine("Invalid category ID.");
        return;
    }
    CategoryMethods.EditCategory(categoryId);
    ProductMethods.AddProduct(categoryId);
}



