using NLog;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");



do
{
  // Display menu
  //TODO change menu to 1 display ----categories 2 display catigires and related products 3 display all products 4 display
  //2 add -- categoriy 2 add product 2 add both
  //TODO edit category 2 edit product 2 edit both
  //TODO delete category 2 delete product 2 delete both


  
  Console.WriteLine("Select an option:");
  Console.WriteLine("1)Display");
  Console.WriteLine("2)Add");
  Console.WriteLine("3)Edit");
  Console.WriteLine("4)Delete");
  
//   Console.WriteLine("1) Display categories");
//   Console.WriteLine("2) Add category");
//   //TODO Edit a specified record from the Categories table
//   Console.WriteLine("3) Display Category and related products");
//   Console.WriteLine("4) Display all Categories and their active products");
  
//   Console.WriteLine("5) Display all Categories and their related products"); //TODO show discontinued products in red
//   //TODO show active products in green
//   Console.WriteLine("6) Edit Category");
//  Console.WriteLine("7) Delete Category");
//  Console.WriteLine("8) Display Products"); 
//  //TODO Display all records in the Products table (ProductName only) 
//  // - user decides if they want to see all products, discontinued products,
//  //  or active (not discontinued) products. 
//  // Discontinued products should be distinguished from active products.
 
//   Console.WriteLine("9) Add Product");
//   Console.WriteLine("10) Edit Product");
// Console.WriteLine("11) Delete Product");
  
//   //TODO Display a specific Product (all product fields should be displayed)
// // TODO Use NLog to track user functions

// //TODO Use data annotations and handle ALL user errors gracefully & log all errors using NLog

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
    logger.Info("Option {addChoice} selected", displayChoice);
    switch (displayChoice)
    {
      case "1":
       DisplayAllCategories();
        break;
      case "2":
       DisplayCategoyAndProducts();
        break;
      case "3":
        DisplayCategoryAndActiveProducts();
        break;
      default:
        Console.WriteLine("Invalid choice. Please try again.");
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

    switch (addChoice)
    {
        case "1":
            // Add category functionality
            AddCategory();
            break;

        case "2":
            // Add product functionality
            AddProduct();
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
            EditCategory();
            break;

        case "2":
            // Edit product functionality
            EditProduct();
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
            DeleteCategory();
            break;

        case "2":
            // Delete product functionality
            DeleteProduct();
            break;

        default:
            Console.WriteLine("Invalid choice. Returning to main menu.");
            break;
    }


    break;  
  default:
    Console.WriteLine("Invalid choice. Please try again.");
    break;  
//   if (choice == "1")
//   {
//     // display categories
//     var configuration = new ConfigurationBuilder()
//             .AddJsonFile($"appsettings.json");

//     var config = configuration.Build();

//     var db = new DataContext();
//     var query = db.Categories.OrderBy(p => p.CategoryName);

//     Console.ForegroundColor = ConsoleColor.Green;
//     Console.WriteLine($"{query.Count()} records returned");
//     Console.ForegroundColor = ConsoleColor.Magenta;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryName} - {item.Description}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//   }
//   else if (choice == "2")
//   {
//     // Add category
//     Category category = new();
//     Console.WriteLine("Enter Category Name:");
//     category.CategoryName = Console.ReadLine()!;
//     Console.WriteLine("Enter the Category Description:");
//     category.Description = Console.ReadLine();
//     ValidationContext context = new ValidationContext(category, null, null);
//     List<ValidationResult> results = new List<ValidationResult>();

//     var isValid = Validator.TryValidateObject(category, context, results, true);
//     if (isValid)
//     {
//       var db = new DataContext();
//       // check for unique name
//       if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
//       {
//         // generate validation error
//         isValid = false;
//         results.Add(new ValidationResult("Name exists", ["CategoryName"]));
//       }
//       else
//       {
//        {
//             logger.Info("Validation passed");
//             db.Categories.Add(category); // Add the category to the database context
//             db.SaveChanges(); // Save changes to the database
//             logger.Info($"Category '{category.CategoryName}' added successfully.");
//         }
//       }
//     }
//     if (!isValid)
//     {
//       foreach (var result in results)
//       {
//         logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
//       }
//     }
//   }
//   else if (choice == "3")
//   {
//     var db = new DataContext();
//     var query = db.Categories.OrderBy(p => p.CategoryId);

//     Console.WriteLine("Select the category whose products you want to display:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();
//     logger.Info($"CategoryId {id} selected");
//     Category category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id)!;
//     Console.WriteLine($"{category.CategoryName} - {category.Description}");
//     foreach (Product p in category.Products)
//     {
//       Console.WriteLine($"\t{p.ProductName}");
//     }
//   }

//   else if (choice == "4")
//   {
//     var db = new DataContext();
//     var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryName}");
//       foreach (Product p in item.Products)
//       {
//         Console.WriteLine($"\t{p.ProductName}");
//       }
//     }
//   }
//   else if (choice == "5")
//   {
//     var db = new DataContext();
//     var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryName}");
//       foreach (Product p in item.Products)
//       {
//         Console.WriteLine($"\t{p.ProductName}");
//       }
//     }
//   }
//   else if (choice == "6")
//   {
//     // Edit Category
//     var db = new DataContext();
//     var query = db.Categories.OrderBy(p => p.CategoryId);
//     Console.WriteLine("Select the category you want to edit:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();
//     logger.Info($"CategoryId {id} selected");
//     Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id)!;
//     Console.WriteLine($"Current Name: {category.CategoryName}");
//     Console.WriteLine($"Current Description: {category.Description}");
//     Console.WriteLine("Enter new name:");
//     string? name = Console.ReadLine();
//     if (!string.IsNullOrEmpty(name))
//       category.CategoryName = name;
//     Console.WriteLine("Enter new description:");
//     string? desc = Console.ReadLine();
//     if (!string.IsNullOrEmpty(desc))
//       category.Description = desc;
    
//     ValidationContext context = new ValidationContext(category, null, null);
//     List<ValidationResult> results = new List<ValidationResult>();
//     var isValid = Validator.TryValidateObject(category, context, results, true);
//     if (isValid)
//     {
//       // check for unique name
//       if (db.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != id))
//       {
//         // generate validation error
//         isValid = false;
//         results.Add(new ValidationResult("Name exists", ["CategoryName"]));
//       }
//       else
//       {
//         logger.Info("Validation passed");
//         db.Categories.Update(category); // Update the category in the database context
//         db.SaveChanges(); // Save changes to the database
//         logger.Info($"Category '{category.CategoryName}' updated successfully.");
//       }
//     }
//     if (!isValid)
//     {
//       foreach (var result in results)
//       {
//         logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
//       }
//     }
//   }
//   else if (choice == "7")

//   {
//     // Delete Category
//     var db = new DataContext();
//     var query = db.Categories.OrderBy(p => p.CategoryId);
//     Console.WriteLine("Select the category you want to delete:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();
//     logger.Info($"CategoryId {id} selected");
//     Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id)!;
//     db.Categories.Remove(category); // Remove the category from the database context
//     db.SaveChanges(); // Save changes to the database
//     logger.Info($"Category '{category.CategoryName}' deleted successfully.");
//   }
//   else if (choice == "8")
//   {
//     // Display Products
//     var db = new DataContext();
//     var query = db.Products.OrderBy(p => p.ProductId);
//     Console.WriteLine("Select the product you want to display:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.ProductId}) {item.ProductName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;

//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();
//     logger.Info($"ProductId {id} selected");
//     Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
//     Console.WriteLine($"Product Name: {product.ProductName}");
//     Console.WriteLine($"Product ID: {product.ProductId}");
   

//   }
//   else if (choice == "9")
// {
//     // Add Product
//     var db = new DataContext();
//     var query = db.Categories.OrderBy(p => p.CategoryId);
//     Console.WriteLine("Select the category you want to add a product to:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//         Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     if (!int.TryParse(Console.ReadLine(), out int categoryId))
//     {
//         Console.WriteLine("Invalid category ID entered.");
//         logger.Error("Invalid category ID entered.");
//         return;
//     }
//     Console.Clear();
//     logger.Info($"CategoryId {categoryId} selected");

//     // Find the selected category
//     Category? category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
//     if (category == null)
//     {
//         Console.WriteLine("Invalid category selected.");
//         logger.Error("Invalid category selected.");
//         return;
//     }

//     // Collect product details
//     Product product = new Product();
//     Console.WriteLine("Enter new product name:");
//     product.ProductName = Console.ReadLine()!;
//     if (db.Products.Any(p => p.ProductName == product.ProductName))
//     {
//         Console.WriteLine("A product with this name already exists.");
//         logger.Error("Duplicate product name.");
//         return;
//     }

//     Console.WriteLine("Enter new product price:");
//     if (!decimal.TryParse(Console.ReadLine(), out decimal price))
//     {
//         Console.WriteLine("Invalid price entered.");
//         logger.Error("Invalid price entered.");
//         return;
//     }
//     product.UnitPrice = price;

//     Console.WriteLine("Enter new product quantity:");
//     if (!short.TryParse(Console.ReadLine(), out short quantity))
//     {
//         Console.WriteLine("Invalid quantity entered.");
//         logger.Error("Invalid quantity entered.");
//         return;
//     }
//     product.UnitsInStock = quantity;

//     Console.WriteLine("Enter new product quantity per unit:");
//     product.QuantityPerUnit = Console.ReadLine();

//     Console.WriteLine("Enter new product reorder level:");
//     if (!short.TryParse(Console.ReadLine(), out short reorderLevel))
//     {
//         Console.WriteLine("Invalid reorder level entered.");
//         logger.Error("Invalid reorder level entered.");
//         return;
//     }
//     product.ReorderLevel = reorderLevel;

//     Console.WriteLine("Is the product discontinued? (true/false):");
//     if (!bool.TryParse(Console.ReadLine(), out bool discontinued))
//     {
//         Console.WriteLine("Invalid value for discontinued status.");
//         logger.Error("Invalid value for discontinued status.");
//         return;
//     }
//     product.Discontinued = discontinued;

//     // Associate product with the selected category
//     product.CategoryId = categoryId;

//     // Validate and save the product
//     ValidationContext context = new ValidationContext(product, null, null);
//     List<ValidationResult> results = new List<ValidationResult>();
//     var isValid = Validator.TryValidateObject(product, context, results, true);

//     if (isValid)
//     {
//         db.Products.Add(product); // Add the product to the database context
//         db.SaveChanges(); // Save changes to the database
//         logger.Info($"Product '{product.ProductName}' added successfully.");
//         Console.WriteLine($"Product '{product.ProductName}' added successfully.");
//     }
//     else
//     {
//         foreach (var result in results)
//         {
//             Console.WriteLine($"{result.MemberNames.First()} : {result.ErrorMessage}");
//             logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
//         }
//     }
// }
// else if (choice == "10")
//   {
//     // Edit Product
//     var db = new DataContext();
//     var query = db.Products.OrderBy(p => p.ProductId);
//     Console.WriteLine("Select the product you want to edit:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.ProductId}) {item.ProductName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();

//     logger.Info($"ProductId {id} selected");
//     Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
//     Console.WriteLine($"Current Name: {product.ProductName}");
//     Console.WriteLine($"Current ID: {product.ProductId}");
//     Console.WriteLine("Enter new name:");
//     string? name = Console.ReadLine();
//     if (!string.IsNullOrEmpty(name))
//       product.ProductName = name;

//     Console.WriteLine("Enter new price:");
//     decimal price = decimal.Parse(Console.ReadLine()!);
//     if (price > 0)
//       product.UnitPrice = price;  
//     Console.WriteLine("Enter new quantity:");
    
//   }

//   else if (choice == "11")
//   {
//     // Delete Product
//     var db = new DataContext();
//     var query = db.Products.OrderBy(p => p.ProductId);
//     Console.WriteLine("Select the product you want to delete:");
//     Console.ForegroundColor = ConsoleColor.DarkRed;
//     foreach (var item in query)
//     {
//       Console.WriteLine($"{item.ProductId}) {item.ProductName}");
//     }
//     Console.ForegroundColor = ConsoleColor.White;
//     int id = int.Parse(Console.ReadLine()!);
//     Console.Clear();
//     logger.Info($"ProductId {id} selected");
//     Product product = db.Products.FirstOrDefault(c => c.ProductId == id)!;
//     db.Products.Remove(product); // Remove the product from the database context
//     db.SaveChanges(); // Save changes to the database
//     logger.Info($"Product '{product.ProductName}' deleted successfully.");

//   }
//   else if (string.IsNullOrEmpty(choice))
//   {
//     break;
//   }
//   else
//   {
//     Console.WriteLine("Invalid choice. Please try again.");
//   }
 
Console.WriteLine();
  break;
} while (true);

logger.Info("Program ended");