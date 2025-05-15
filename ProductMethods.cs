using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;





public class ProductMethods

{

    // Method to view all products
   public static void ViewProducts()
{
    using var db = new DataContext();
    var products = db.Products.OrderBy(p => p.ProductName).ToList();
    Console.WriteLine("Products:");
   PrintProductList(products);
}

    // Method to add a new product

    public static void AddProduct()
    {
        using var db = new DataContext();
        var product = new NorthwindConsole.Model.Product();

        Console.WriteLine("Enter Product Name:");
        product.ProductName = Console.ReadLine()!;
        Console.WriteLine("Enter Product Price:");
        product.UnitPrice = decimal.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Units in Stock:");
        product.UnitsInStock = short.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Quantity Per Unit:");
        product.QuantityPerUnit = Console.ReadLine();
        Console.WriteLine("Is the product discontinued? (true/false):");
        product.Discontinued = bool.Parse(Console.ReadLine()!);

        Console.WriteLine("Select a category for the product:");
        var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
        }

        if (int.TryParse(Console.ReadLine(), out int categoryId) && categories.Any(c => c.CategoryId == categoryId))
        {
            product.CategoryId = categoryId;
        }
        else
        {
            Console.WriteLine("Invalid category selected. Product will not be associated with a category.");
        }

        db.Products.Add(product);
        db.SaveChanges();
        Console.WriteLine($"Product '{product.ProductName}' added successfully.");
    }

    //overloaded add product method to add a product with a category
    public static void AddProduct(int categoryId)
    {
        using var db = new DataContext();
        var product = new NorthwindConsole.Model.Product();

        Console.WriteLine("Enter Product Name:");
        product.ProductName = Console.ReadLine()!;
        Console.WriteLine("Enter Product Price:");
        product.UnitPrice = decimal.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Units in Stock:");
        product.UnitsInStock = short.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Quantity Per Unit:");
        product.QuantityPerUnit = Console.ReadLine();
        Console.WriteLine("Is the product discontinued? (true/false):");
        product.Discontinued = bool.Parse(Console.ReadLine()!);

        if (db.Categories.Any(c => c.CategoryId == categoryId))
        {
            product.CategoryId = categoryId;
            db.Products.Add(product);
            db.SaveChanges();
            Console.WriteLine($"Product '{product.ProductName}' added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid category ID. Product will not be added.");
        }
    }   
    // Method to view products that are discontinued
    public static void ViewDiscontinuedProducts()
{
    using var db = new DataContext();
    var discontinuedProducts = db.Products.Where(p => p.Discontinued).OrderBy(p => p.ProductName).ToList();
    Console.WriteLine("Discontinued Products:");
    PrintProductList(discontinuedProducts);
}
    // Method to view products that are active green

   public static void ViewActiveProducts()
{
    using var db = new DataContext();
    var activeProducts = db.Products.Where(p => !p.Discontinued).ToList();
    PrintProductList(activeProducts);
}
//display product with its id
public static void DisplayProductandId()
{
    using var db = new DataContext();
    var products = db.Products.OrderBy(p => p.ProductId).ToList();
    Console.WriteLine("Products:");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.ProductId}: {product.ProductName}");
    }
}

public static void DisplayProductById(int productId)
{
 
    Console.WriteLine("Enter the product ID to view details:");
    if (!int.TryParse(Console.ReadLine(), out productId))
    {
        Console.WriteLine("Invalid product ID.");
        return;
    }
    using var db = new DataContext();
    var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
    if (product != null)
    {
        Console.WriteLine($"Product ID: {product.ProductId}");
        Console.WriteLine($"Product Name: {product.ProductName}");
        Console.WriteLine($"Unit Price: {product.UnitPrice}");
        Console.WriteLine($"Units in Stock: {product.UnitsInStock}");
        Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
        Console.WriteLine($"Discontinued: {(product.Discontinued ? "Yes" : "No")}");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}

    // Method to edit an existing product
    public static void EditProduct()
    {
        using var db = new DataContext();
        var products = db.Products.OrderBy(p => p.ProductId).ToList();

        Console.WriteLine("Select a product to edit:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName}");
        }

        if (!int.TryParse(Console.ReadLine(), out int productId) || !products.Any(p => p.ProductId == productId))
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        var selectedProduct = db.Products.First(p => p.ProductId == productId);
        Console.WriteLine($"Current Name: {selectedProduct.ProductName}");
        Console.WriteLine($"Current Price: {selectedProduct.UnitPrice}");
        Console.WriteLine($"Current Units in Stock: {selectedProduct.UnitsInStock}");
        Console.WriteLine($"Current Quantity Per Unit: {selectedProduct.QuantityPerUnit}");
        Console.WriteLine($"Current Status: {(selectedProduct.Discontinued ? "Discontinued" : "Active")}");

        Console.WriteLine("Enter new name (or press Enter to keep current):");
        var newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            selectedProduct.ProductName = newName;
        }

        Console.WriteLine("Enter new price (or press Enter to keep current):");
        var newPrice = Console.ReadLine();
        if (decimal.TryParse(newPrice, out decimal price))
        {
            selectedProduct.UnitPrice = price;
        }

        Console.WriteLine("Enter new units in stock (or press Enter to keep current):");
        var newStock = Console.ReadLine();
        if (short.TryParse(newStock, out short stock))
        {
            selectedProduct.UnitsInStock = stock;
        }

        Console.WriteLine("Enter new quantity per unit (or press Enter to keep current):");
        var newQuantity = Console.ReadLine();
        if (!string.IsNullOrEmpty(newQuantity))
        {
            selectedProduct.QuantityPerUnit = newQuantity;
        }

        Console.WriteLine("Is the product discontinued? (true/false, or press Enter to keep current):");
        var newStatus = Console.ReadLine();
        if (bool.TryParse(newStatus, out bool discontinued))
        {
            selectedProduct.Discontinued = discontinued;
        }

        db.SaveChanges();
        Console.WriteLine("Product updated successfully.");
    }

    // Method to delete a product
    public static void DeleteProduct()
    {
        using var db = new DataContext();
        var products = db.Products.OrderBy(p => p.ProductId).ToList();

        Console.WriteLine("Select a product to delete:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName}");
        }

        if (!int.TryParse(Console.ReadLine(), out int productId) || !products.Any(p => p.ProductId == productId))
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        var selectedProduct = db.Products.First(p => p.ProductId == productId);
        db.Products.Remove(selectedProduct);
        db.SaveChanges();
        Console.WriteLine("Product deleted successfully.");

    }


    public static void PrintProductList(IEnumerable<Product> products)
{
    foreach (var p in products)
    {
        if (p.Discontinued)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t{p.ProductName} (Discontinued)");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t{p.ProductName}");
        }
        Console.ResetColor();
    }
}






}