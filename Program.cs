using NLog;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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

  Console.WriteLine("Enter to quit");
  string? choice = Console.ReadLine();
  Console.Clear();
  logger.Info("Option {choice} selected", choice);

  switch (choice)
  {
    case "1":
    Console.WriteLine("Display Options:");
    Console.WriteLine("1) Display categories and their discriptions");
    Console.WriteLine("2) Display Category and related products");
    //TODO show discontinued products in red
    Console.WriteLine("3) Display Category and their active products");
     string? displayChoice = Console.ReadLine();
    Console.Clear();
    logger.Info("Option {displayChoice} selected", displayChoice);
       switch (displayChoice)
      {
      case "1":
       Category.ViewCategories();
        break;
      case "2":
       DisplayCategoryAndProducts();
        break;
      case "3":
        DisplayCategoryAndActiveProducts();
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
    //TODO add both
    Console.WriteLine("3) Add both");

    string? addChoice = Console.ReadLine();
    Console.Clear();
    logger.Info("Option {addChoice} selected", addChoice);

    switch (addChoice)
    {
        case "1":
            // Add category functionality
            Category.AddCategory();
            break;

        case "2":
            // Add product functionality
            Product1.AddProduct();
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
            Category.EditCategory();
            break;

        case "2":
            // Edit product functionality
            Product1.EditProduct();
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
            Category.DeleteCategory();
            break;

        case "2":
            // Delete product functionality
            Product1.DeleteProduct();
            break;

        default:
            Console.WriteLine("Invalid choice. Returning to main menu.");
            break;
    }


    break;  
  default:
    Console.WriteLine("Invalid choice. Please try again.");
    break;  
}
} while (true);
logger.Info("Exiting the program");



//methods
void DisplayCategoryAndProducts()
{
  var db = new DataContext();
  var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
  foreach (var item in query)
  {
    Console.WriteLine($"{item.CategoryName}");
    foreach (Product p in item.Products)
    {
      Console.WriteLine($"\t{p.ProductName}");
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
    foreach (Product p in item.Products.Where(p => p.Discontinued == false))
    {
      Console.WriteLine($"\t{p.ProductName}");
    }
  }
}
void AddCategoryAndProduct()
{
  Category.AddCategory();
  Product1.AddProduct();
}
void EditCategoryAndAddProduct()
{
  Category.EditCategory();
  Product1.AddProduct();
}

