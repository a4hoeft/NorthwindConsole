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
        var products = db.Products.Include(p => p.Category).OrderBy(p => p.ProductName).ToList();

        Console.WriteLine("Products:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName} - {product.Category?.CategoryName ?? "No Category"} - {(product.Discontinued ? "Discontinued" : "Active")}");
        }
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
}