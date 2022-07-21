using System;

string name = Console.ReadLine() ?? "";
Console.WriteLine($"Hello, {name}!");

if (name.Contains(' ')) {
	string[] names = name.Split(' ');             // aa
	string firstName = names[0];
	string lastName = names[1];
	Console.WriteLine($"Your first name is {firstName} and last name is {lastName}");
}
