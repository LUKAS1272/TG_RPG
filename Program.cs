using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TGproject;

// Character - name
// Item - name, weight
// Stackable - name, weight, itemCount
// Weapon - name, weight, damage, isTwoHanded
// Defense - name, weight, absorption, DefenseType
// Coin - name, weight, itemCount, isStackable, CoinType
 
public class Program {
    public static bool testMode = false;
    public static int rndSeed = new Random().Next(); // Seed for all random numbers > unit tests purpose

    static Character currentHero = new Character("mighty knight");
    static Character nextHero = new Character("cruel king");

    static void Main(string[] args) {
        // First hero inventory init
        currentHero.AddToInv(new Weapon("golden sword", 70, 15, false));
        currentHero.AddToInv(new Weapon("iron sword", 85, 35, true));
        currentHero.AddToInv(new Defense("iron shield", 90, 35, DefenseType.Shield));
        currentHero.AddToInv(new Defense("iron chestplate", 100, 25, DefenseType.Armor));
        currentHero.AddToInv(new Coin("", 1, 15, CoinType.Copper));
        currentHero.AddToInv(new Coin("", 10, 2, CoinType.Silver));
        currentHero.AddToInv(new Coin("", 100, 1, CoinType.Gold));
        currentHero.AddToInv(new Coin("", 1, 15, CoinType.Copper));
        currentHero.AddToInv(new Letter("basic letter", 10, "Some letter text..."));
        currentHero.AddToInv(new Food("cooked beef", 1, 2, 10));

        // Second hero inventory init
        nextHero.AddToInv(new Weapon("golden sword", 70, 15, false));
        nextHero.AddToInv(new Weapon("iron sword", 85, 35, true));
        nextHero.AddToInv(new Defense("iron shield", 90, 35, DefenseType.Shield));
        nextHero.AddToInv(new Defense("iron chestplate", 100, 25, DefenseType.Armor));
        nextHero.AddToInv(new Coin("", 1, 15, CoinType.Copper));
        nextHero.AddToInv(new Coin("", 10, 2, CoinType.Silver));
        nextHero.AddToInv(new Coin("", 100, 1, CoinType.Gold));
        nextHero.AddToInv(new Coin("", 1, 15, CoinType.Copper));

        Console.WriteLine("\n\nPress any key to start the game!");
        Console.ReadKey();
        Menu();
    }

    static void Menu() {
        Console.Clear();

        Console.WriteLine($"It's {currentHero.CharacterName}'s turn!");
        Console.WriteLine("--------------------------------------------------------------------");
        Console.WriteLine("i - write out inventory");
        Console.WriteLine("s - write out your hero stats");
        Console.WriteLine("a - attack");
        Console.WriteLine("c - change your equipment");
        Console.WriteLine("m - see how much money you have in total");
        Console.WriteLine("u - use item");
        Console.WriteLine("e - exit game");
        Console.WriteLine("--------------------------------------------------------------------");

        Console.Write("Please choose, what you want to do next by pressing any key above: ");
        ConsoleKey choice = Console.ReadKey().Key;
        Console.WriteLine("\n");

        switch (choice) {
            case ConsoleKey.I:
                currentHero.WriteInv();
                break;
            case ConsoleKey.S:
                currentHero.WriteStats();
                break;
            case ConsoleKey.A:
                currentHero.Attack(nextHero, false);
                if (currentHero.Health == 0 || nextHero.Health == 0) { return; }
                SwitchTurn();
                break;
            case ConsoleKey.C:
                currentHero.ChangeEquipment();
                break;
            case ConsoleKey.M:
                currentHero.AllMoney();
                break;
            case ConsoleKey.U:
                currentHero.UseItem();
                break;
            case ConsoleKey.E:
                Console.WriteLine("Turning the game off...");
                return;
            default:
                Menu();
                break;
        }

        Console.Write("\n\nPress any key to continue...");
        Console.ReadKey();
        Menu();
    }

    static void SwitchTurn() {
        Character temp = currentHero;
        currentHero = nextHero;
        nextHero = temp;
    }
}