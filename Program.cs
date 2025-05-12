using NLog;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

do
{
  Console.WriteLine("1) Display categories");
  Console.WriteLine("2) Add category");
  //TODO Edit a specified record from the Categories table
  Console.WriteLine("3) Display Category and related products");
  Console.WriteLine("4) Display all Categories and their active products");
  
  Console.WriteLine("5) Display all Categories and their related products"); //TODO show discontinued products in red
  //TODO show active products in green
  Console.WriteLine("6) Edit Category");
 Console.WriteLine("7) Delete Category");
 Console.WriteLine("8) Display Products"); 
 //TODO Display all records in the Products table (ProductName only) 
 // - user decides if they want to see all products, discontinued products,
 //  or active (not discontinued) products. 
 // Discontinued products should be distinguished from active products.
 
  Console.WriteLine("9) Add Product by Category");//(TODO) Remove the add product by category option
  //TODO Add new records to the Products table
  Console.WriteLine("10) Edit Product");
Console.WriteLine("11) Delete Product");
  
  //TODO Display a specific Product (all product fields should be displayed)
// TODO Use NLog to track user functions

//TODO Delete a specified existing record from the Products table (account for Orphans in related tables)
//TODO Delete a specified existing record from the Categories table (account for Orphans in related tables)

//TODO Use data annotations and handle ALL user errors gracefully & log all errors using NLog

  Console.WriteLine("Enter to quit");
  string? choice = Console.ReadLine();
  Console.Clear();
  logger.Info("Option {choice} selected", choice);

  if (choice == "1")
  {
    // display categories
    var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json");

    var config = configuration.Build();

    var db = new DataContext();
    var query = db.Categories.OrderBy(p => p.CategoryName);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"{query.Count()} records returned");
    Console.ForegroundColor = ConsoleColor.Magenta;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.CategoryName} - {item.Description}");
    }
    Console.ForegroundColor = ConsoleColor.White;
  }
  else if (choice == "2")
  {
    // Add category
    Category category = new();
    Console.WriteLine("Enter Category Name:");
    category.CategoryName = Console.ReadLine()!;
    Console.WriteLine("Enter the Category Description:");
    category.Description = Console.ReadLine();
    ValidationContext context = new ValidationContext(category, null, null);
    List<ValidationResult> results = new List<ValidationResult>();

    var isValid = Validator.TryValidateObject(category, context, results, true);
    if (isValid)
    {
      var db = new DataContext();
      // check for unique name
      if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
      {
        // generate validation error
        isValid = false;
        results.Add(new ValidationResult("Name exists", ["CategoryName"]));
      }
      else
      {
       {
            logger.Info("Validation passed");
            db.Categories.Add(category); // Add the category to the database context
            db.SaveChanges(); // Save changes to the database
            logger.Info($"Category '{category.CategoryName}' added successfully.");
        }
      }
    }
    if (!isValid)
    {
      foreach (var result in results)
      {
        logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
      }
    }
  }
  else if (choice == "3")
  {
    var db = new DataContext();
    var query = db.Categories.OrderBy(p => p.CategoryId);

    Console.WriteLine("Select the category whose products you want to display:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"CategoryId {id} selected");
    Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id)!;
    Console.WriteLine($"{category.CategoryName} - {category.Description}");
    foreach (Product p in category.Products)
    {
      Console.WriteLine($"\t{p.ProductName}");
    }
  }

  else if (choice == "4")
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
  else if (choice == "5")
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
  else if (choice == "6")
  {
    // Edit Category
    var db = new DataContext();
    var query = db.Categories.OrderBy(p => p.CategoryId);
    Console.WriteLine("Select the category you want to edit:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"CategoryId {id} selected");
    Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id)!;
    Console.WriteLine($"Current Name: {category.CategoryName}");
    Console.WriteLine($"Current Description: {category.Description}");
    Console.WriteLine("Enter new name:");
    string? name = Console.ReadLine();
    if (!string.IsNullOrEmpty(name))
      category.CategoryName = name;
    Console.WriteLine("Enter new description:");
    string? desc = Console.ReadLine();
    if (!string.IsNullOrEmpty(desc))
      category.Description = desc;
    
    ValidationContext context = new ValidationContext(category, null, null);
    List<ValidationResult> results = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(category, context, results, true);
    if (isValid)
    {
      // check for unique name
      if (db.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != id))
      {
        // generate validation error
        isValid = false;
        results.Add(new ValidationResult("Name exists", ["CategoryName"]));
      }
      else
      {
        logger.Info("Validation passed");
        db.Categories.Update(category); // Update the category in the database context
        db.SaveChanges(); // Save changes to the database
        logger.Info($"Category '{category.CategoryName}' updated successfully.");
      }
    }
    if (!isValid)
    {
      foreach (var result in results)
      {
        logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
      }
    }
  }
  else if (choice == "7")

  {
    // Delete Category
    var db = new DataContext();
    var query = db.Categories.OrderBy(p => p.CategoryId);
    Console.WriteLine("Select the category you want to delete:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"CategoryId {id} selected");
    Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id)!;
    db.Categories.Remove(category); // Remove the category from the database context
    db.SaveChanges(); // Save changes to the database
    logger.Info($"Category '{category.CategoryName}' deleted successfully.");
  }
  else if (choice == "8")
  {
    // Display Products
    var db = new DataContext();
    var query = db.Products.OrderBy(p => p.ProductId);
    Console.WriteLine("Select the product you want to display:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.ProductId}) {item.ProductName}");
    }
    Console.ForegroundColor = ConsoleColor.White;

    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"ProductId {id} selected");
    Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
    Console.WriteLine($"Product Name: {product.ProductName}");
    Console.WriteLine($"Product ID: {product.ProductId}");
   

  }
  else if (choice == "9")
  {
    // Add Product by Category

    var db = new DataContext();
    var query = db.Categories.OrderBy(p => p.CategoryId);
    Console.WriteLine("Select the category you want to add a product to:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"CategoryId {id} selected");




    Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id)!;
    Console.WriteLine($"Current Name: {category.CategoryName}");
    Console.WriteLine($"Current Description: {category.Description}");
    Console.WriteLine("Enter new product name:");
    string? name = Console.ReadLine();
    Console.WriteLine("Enter new product price:");
    decimal price = decimal.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product quantity:");
    int quantity = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product supplier:");
    string? supplier = Console.ReadLine();
    Console.WriteLine("Enter new product quantity per unit:");
    string? quantityPerUnit = Console.ReadLine();
    Console.WriteLine("Enter new product reorder level:");
    int reorderLevel = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product discontinued (true/false):");
    bool discontinued = bool.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product minimum order quantity:");
    int minimumOrderQuantity = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product maximum order quantity:");
    int maximumOrderQuantity = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter new product unit:");
    string? unit = Console.ReadLine();


  }
  else if (choice == "10")
  {
    // Edit Product
    var db = new DataContext();
    var query = db.Products.OrderBy(p => p.ProductId);
    Console.WriteLine("Select the product you want to edit:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.ProductId}) {item.ProductName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();

    logger.Info($"ProductId {id} selected");
    Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
    Console.WriteLine($"Current Name: {product.ProductName}");
    Console.WriteLine($"Current ID: {product.ProductId}");
    Console.WriteLine("Enter new name:");
    string? name = Console.ReadLine();
    if (!string.IsNullOrEmpty(name))
      product.ProductName = name;

    Console.WriteLine("Enter new price:");
    decimal price = decimal.Parse(Console.ReadLine()!);
    if (price > 0)
      product.UnitPrice = price;  
    Console.WriteLine("Enter new quantity:");
    
  }

  else if (choice == "11")
  {
    // Delete Product
    var db = new DataContext();
    var query = db.Products.OrderBy(p => p.ProductId);
    Console.WriteLine("Select the product you want to delete:");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    foreach (var item in query)
    {
      Console.WriteLine($"{item.ProductId}) {item.ProductName}");
    }
    Console.ForegroundColor = ConsoleColor.White;
    int id = int.Parse(Console.ReadLine()!);
    Console.Clear();
    logger.Info($"ProductId {id} selected");
    Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
    db.Products.Remove(product); // Remove the product from the database context
    db.SaveChanges(); // Save changes to the database
    logger.Info($"Product '{product.ProductName}' deleted successfully.");

  }
  else if (string.IsNullOrEmpty(choice))
  {
    break;
  }
  else
  {
    Console.WriteLine("Invalid choice. Please try again.");
  }
 
  Console.WriteLine();
} while (true);

logger.Info("Program ended");
