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
    Console.WriteLine("1)Display");
    Console.WriteLine("2)Add");
    Console.WriteLine("3)Edit");
    Console.WriteLine("4)Delete");
    Console.WriteLine("5)Exit");
    Console.WriteLine("Enter your choice (1-5):");

    string? choice = Console.ReadLine();
    Console.Clear();
    logger.Info("Main menu option selected: {choice}", choice);

    try
    {
        switch (choice)
        {
            case "1":
                Console.WriteLine("Display Options:");
                Console.WriteLine("1) Categories");
                Console.WriteLine("2) Products");
                string? displayType = Console.ReadLine();
                Console.Clear();
                logger.Info("Display type selected: {displayType}", displayType);

                switch (displayType)
                {
                    case "1": // Categories
                        Console.WriteLine("Category Display Options:");
                        Console.WriteLine("1) Display categories and their descriptions");
                        Console.WriteLine("2) Display all categories and related products");
                        Console.WriteLine("3) Display all categories and their active products");
                        Console.WriteLine("4) Display one category and its active and discontinued products");

                        string? categoryChoice = Console.ReadLine();
                        Console.Clear();
                        logger.Info("Category display option selected: {categoryChoice}", categoryChoice);

                        switch (categoryChoice)
                        {
                            case "1":
                                logger.Info("Displaying all categories and descriptions.");
                                CategoryMethods.ViewCategories();
                                break;
                            case "2":
                                logger.Info("Displaying all categories and related products.");
                                DisplayCategoryAndProducts();
                                break;
                            case "3":
                                logger.Info("Displaying all categories and their active products.");
                                DisplayCategoryAndActiveProducts();
                                break;
                            case "4":
                                Console.WriteLine("Enter the category ID to display:");
                                if (!int.TryParse(Console.ReadLine(), out int categoryId))
                                {
                                    logger.Warn("Invalid category ID entered.");
                                    Console.WriteLine("Invalid category ID.");
                                    break;
                                }
                                logger.Info("Displaying category by ID: {categoryId}", categoryId);
                                CategoryMethods.DisplayCategoryById(categoryId);
                                break;
                            default:
                                logger.Warn("Invalid category display option selected: {categoryChoice}", categoryChoice);
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
                        logger.Info("Product display option selected: {productChoice}", productChoice);

                        switch (productChoice)
                        {
                            case "1":
                                logger.Info("Displaying all products.");
                                ProductMethods.ViewProducts();
                                break;
                            case "2":
                                logger.Info("Displaying all active products.");
                                ProductMethods.ViewActiveProducts();
                                break;
                            case "3":
                                logger.Info("Displaying all discontinued products.");
                                ProductMethods.ViewDiscontinuedProducts();
                                break;
                            case "4":
                                logger.Info("Displaying a specific product by ID.");
                                ProductMethods.DisplayProductById(0); // or however your method signature is
                                break;
                            default:
                                logger.Warn("Invalid product display option selected: {productChoice}", productChoice);
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                        break;

                    default:
                        logger.Warn("Invalid display type selected: {displayType}", displayType);
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
                logger.Info("Add option selected: {addChoice}", addChoice);

                switch (addChoice)
                {
                    case "1":
                        logger.Info("Adding new category.");
                        CategoryMethods.AddCategory();
                        break;
                    case "2":
                        logger.Info("Adding new product.");
                        ProductMethods.AddProduct();
                        break;
                    case "3":
                        logger.Info("Adding new category and product.");
                        AddCategoryAndProduct();
                        break;
                    default:
                        logger.Warn("Invalid add option selected: {addChoice}", addChoice);
                        Console.WriteLine("Invalid choice. Returning to main menu.");
                        break;
                }
                break;

            case "3":
                Console.WriteLine("Edit Options:");
                Console.WriteLine("1) Edit category");
                Console.WriteLine("2) Edit product");
                Console.WriteLine("3) Edit a category and  add a product");

                string? editChoice = Console.ReadLine();
                Console.Clear();
                logger.Info("Edit option selected: {editChoice}", editChoice);

                switch (editChoice)
                {
                    case "1":
                        logger.Info("Editing category.");
                        CategoryMethods.EditCategory();
                        break;
                    case "2":
                        logger.Info("Editing product.");
                        ProductMethods.EditProduct();
                        break;
                    case "3":
                        logger.Info("Editing category and adding product.");
                        EditCategoryAndAddProduct();
                        break;
                    default:
                        logger.Warn("Invalid edit option selected: {editChoice}", editChoice);
                        Console.WriteLine("Invalid choice. Returning to main menu.");
                        break;
                }
                break;

            case "4":
                Console.WriteLine("Open Delete Options");
                Console.WriteLine("1) Delete category");
                Console.WriteLine("2) Delete product");

                string? deleteChoice = Console.ReadLine();
                Console.Clear();
                logger.Info("Delete option selected: {deleteChoice}", deleteChoice);

                switch (deleteChoice)
                {
                    case "1":
                        logger.Info("Deleting category.");
                        CategoryMethods.DeleteCategory();
                        break;
                    case "2":
                        logger.Info("Deleting product.");
                        ProductMethods.DeleteProduct();
                        break;
                    default:
                        logger.Warn("Invalid delete option selected: {deleteChoice}", deleteChoice);
                        Console.WriteLine("Invalid choice. Returning to main menu.");
                        break;
                }
                break;

            case "5":
                logger.Info("Exiting the program.");
                Console.WriteLine("Exiting the program...");
                return;

            default:
                logger.Warn("Invalid main menu option selected: {choice}", choice);
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex, "An error occurred during processing.");
        Console.WriteLine("An unexpected error occurred. Please check the log for details.");
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



