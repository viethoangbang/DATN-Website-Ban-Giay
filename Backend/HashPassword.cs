using System;

class HashPassword
{
    static void Main(string[] args)
    {
        string password = args.Length > 0 ? args[0] : "Admin123";
        string hashed = BCrypt.Net.BCrypt.HashPassword(password, 11);
        Console.WriteLine($"Password: {password}");
        Console.WriteLine($"Hashed: {hashed}");
    }
}
