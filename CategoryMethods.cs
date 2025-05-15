using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NorthwindConsole.Model;

public class CategoryMethods
{
    // Method to view all categories
    public static void ViewCategories()
    {
        using var db = new DataContext();
        var categories = db.Categories.OrderBy(c => c.CategoryName).ToList();

        Console.WriteLine("Categories:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName} - {category.Description}");
        }
    }

    // Method to add a new category
    public static void AddCategory()
    {
        using var db = new DataContext();
        var category = new NorthwindConsole.Model.Category();

        Console.WriteLine("Enter Category Name:");
        category.CategoryName = Console.ReadLine()!;
        Console.WriteLine("Enter Category Description:");
        category.Description = Console.ReadLine();

        if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
        {
            Console.WriteLine("A category with this name already exists.");
        }
        else
        {
            db.Categories.Add(category);
            db.SaveChanges();
            Console.WriteLine($"Category '{category.CategoryName}' added successfully.");
        }
    }

    // Method to edit an existing category
    public static void EditCategory()
    {
        using var db = new DataContext();
        var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();

        Console.WriteLine("Select a category to edit:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
        }

        if (!int.TryParse(Console.ReadLine(), out int categoryId) || !categories.Any(c => c.CategoryId == categoryId))
        {
            Console.WriteLine("Invalid category ID.");
            return;
        }

        var selectedCategory = db.Categories.First(c => c.CategoryId == categoryId);
        Console.WriteLine($"Current Name: {selectedCategory.CategoryName}");
        Console.WriteLine($"Current Description: {selectedCategory.Description}");

        Console.WriteLine("Enter new name (or press Enter to keep current):");
        var newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            selectedCategory.CategoryName = newName;
        }

        Console.WriteLine("Enter new description (or press Enter to keep current):");
        var newDescription = Console.ReadLine();
        if (!string.IsNullOrEmpty(newDescription))
        {
            selectedCategory.Description = newDescription;
        }

        db.SaveChanges();
        Console.WriteLine("Category updated successfully.");
    }
    //overlod the edit method to take categoryId as a parameter
    public static void EditCategory(int categoryId)
    {
        using var db = new DataContext();
        var selectedCategory = db.Categories.First(c => c.CategoryId == categoryId);
        Console.WriteLine($"Current Name: {selectedCategory.CategoryName}");
        Console.WriteLine($"Current Description: {selectedCategory.Description}");

        Console.WriteLine("Enter new name (or press Enter to keep current):");
        var newName = Console.ReadLine();
        if (!string.IsNullOrEmpty(newName))
        {
            selectedCategory.CategoryName = newName;
        }

        Console.WriteLine("Enter new description (or press Enter to keep current):");
        var newDescription = Console.ReadLine();
        if (!string.IsNullOrEmpty(newDescription))
        {
            selectedCategory.Description = newDescription;
        }

        db.SaveChanges();
        Console.WriteLine("Category updated successfully.");
    }

    // Method to delete a category
    public static void DeleteCategory()
    {
        using var db = new DataContext();
        var categories = db.Categories.OrderBy(c => c.CategoryId).ToList();

        Console.WriteLine("Select a category to delete:");
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
        }

        if (!int.TryParse(Console.ReadLine(), out int categoryId) || !categories.Any(c => c.CategoryId == categoryId))
        {
            Console.WriteLine("Invalid category ID.");
            return;
        }

        var selectedCategory = db.Categories.First(c => c.CategoryId == categoryId);
        db.Categories.Remove(selectedCategory);
        db.SaveChanges();
        Console.WriteLine("Category deleted successfully.");
    }

//display categorie by id
    public static void DisplayCategoryById(int categoryId)
    {
        using var db = new DataContext();
        var category = db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);

        if (category != null)
        {
            Console.WriteLine($"Category ID: {category.CategoryId}");
            Console.WriteLine($"Category Name: {category.CategoryName}");
            Console.WriteLine($"Description: {category.Description}");
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }

}