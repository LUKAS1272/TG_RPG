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



public class Character {
    private string characterName;
    public string CharacterName { get { return characterName; } }
    
    private double endurance; // maxHP is based on this variable
    private double strength; // maxWeight is based on this variable

    private int maxHealth;
    private int health;
    public int Health { get { return health; } set { health = value; } }

    // Inventory settings
    private int maxWeight;
    private int currentWeight = 0;
    private List<Item> characterInv = new List<Item>();

    // Equipment
    private Item leftHand = null!;
    private Item rightHand = null!;
    private Item chest = null!;

    public Character(string _characterName) {
        characterName = _characterName;

        Random rnd = new Random();
        if (Program.testMode) { rnd = new Random(Program.rndSeed); }

        endurance = (double)rnd.Next(75, 151) / 100;
        strength = (double)rnd.Next(75, 151) / 100;

        maxHealth = (int)(1000 * endurance);
        health = maxHealth;
        maxWeight = (int)(800 * strength);
    }

    public void WriteStats() {
        Console.WriteLine($"Name: {characterName}");
        Console.WriteLine($"HP: {health} / {maxHealth}");
        Console.WriteLine($"Inventory weight: {currentWeight} / {maxWeight}");
        Console.WriteLine($"Endurance multiplayer: {endurance}x");
        Console.WriteLine($"Strength multiplayer: {strength}x");

        Console.WriteLine();
        WriteEquipment();
    }

    public void AddToInv(Item item) {
        // Count errors
        if (item.ItemCountGet > 1 && !item.IsStackable) {
            Console.WriteLine($"Error: The {item.Name} is not stackable, you can't add multiple of them at one time!");
            return;
        } else if (item.ItemCountGet <= 0) {
            Console.WriteLine($"Error: You can't add negative / zero ammount of items!");
            return;
        }

        // Weight errors
        if (currentWeight + item.Weight * item.ItemCountGet > maxWeight) {
            Console.WriteLine($"Error: The {characterName} can't hold this item(s), it is too heavy!");
            return;
        } else if (item.Weight < 0) {
            Console.WriteLine($"Error: You can't add an item with negative weight!");
            return;
        }

        // Checks whether the (stackable) item is alrady in the inventory
        foreach (Item invItem in characterInv) {
            if (item.Name == invItem.Name && item.IsStackable) {
                invItem.ItemCountSet += item.ItemCountSet;
                currentWeight += item.Weight * item.ItemCountGet;
                return;
            }
        }

        // Adds the item to the inventory
        characterInv.Add(item);
        currentWeight += item.Weight * item.ItemCountGet;
        
        // Item added - debug purposes
        // Console.WriteLine($"Added {item.name} ({item.itemCount} with weight of {item.itemCount * item.weight}), the current weight is {currentWeight}");
    }

    public void WriteInv() {
        if (characterInv.Count == 0) {
            Console.WriteLine("You have no items in your inventory at the moment");
            return;
        }

        Console.WriteLine("These are the items in your inventory");
        foreach (Item invItem in characterInv) {
            if (invItem.IsStackable) {
                Console.WriteLine($"- {invItem.Name} ({invItem.ItemCountGet})");
            } else {
                Console.WriteLine($"- {invItem.Name} (non-stackable)");
            }
        }
    }

    private void WriteEquipment() {
        if (leftHand == null) {
            Console.WriteLine("No weapon equiped");
        } else {
            Console.Write($"Weapon equiped: {leftHand.Name} ");
            if (leftHand.IsTwoHanded) { Console.WriteLine("(two-handed > cannot equip shield with this weapon)"); }
            else { Console.WriteLine(); }
        }

        if (rightHand == null) { Console.WriteLine("No shield equiped"); }
        else { Console.WriteLine($"Shield equiped: {rightHand.Name}"); }

        if (chest == null) { Console.WriteLine("No armor equiped"); }
        else { Console.WriteLine($"Armor equiped: {chest.Name}"); }

        Console.WriteLine();
    }

    public void ChangeEquipment() {
        WriteEquipment();

        int index = -1;
        List<Item> itemSelection = new List<Item>();
        foreach (Item invItem in characterInv) {
            if (invItem.GetType() == typeof(Weapon) || invItem.GetType() == typeof(Defense)) {
                // Skips already equiped items
                if (invItem == leftHand || invItem == rightHand || invItem == chest) { continue; }

                itemSelection.Add(invItem);
                Console.Write($"{++index} - {invItem.Name} ");

                if (invItem.IsTwoHanded) {
                    Console.WriteLine("(two-handed)");
                } else if (invItem.GetType() == typeof(Weapon)) {
                    Console.WriteLine("(one-handed)");
                } else {
                    Console.WriteLine();
                }
            }
        }

        if (index == -1) {
            Console.WriteLine("There is no equipment available in your inventory!");
            return;
        }

        Console.Write("Choose, which item you want to equip (write number and confirm by enter): ");
        int num;
        if (int.TryParse(Console.ReadLine(), out num) && num >= 0 && num <= index) {
            if (itemSelection.ElementAt(num).GetType() == typeof(Weapon)) {
                leftHand = itemSelection.ElementAt(num);
                if (itemSelection.ElementAt(num).IsTwoHanded) {
                    rightHand = null!;
                }
            } else if (itemSelection.ElementAt(num).DType == DefenseType.Shield) {
                rightHand = itemSelection.ElementAt(num);
                if (leftHand != null && leftHand.IsTwoHanded) {
                    leftHand = null!;
                }
            } else if (itemSelection.ElementAt(num).DType == DefenseType.Armor) {
                chest = itemSelection.ElementAt(num);
            }

            Console.WriteLine();
            WriteEquipment();
        } else {
            Console.WriteLine("\nError: Invalid number");
        }
        
    }

    public void Attack(Character aim, bool isDefense) {
        int absorptionPercent = 0;
        if (aim.chest != null) { absorptionPercent += aim.chest.Absorption; }
        if (aim.rightHand != null) { absorptionPercent += aim.rightHand.Absorption; }

        Random rnd = new Random(Program.rndSeed);
        int rndNum = rnd.Next(1, 6);

        int damageDealt = 0;
        if (leftHand != null) { damageDealt = (int)(rndNum * leftHand.Damage * (100 - absorptionPercent) / 100); }
        else { damageDealt = (int)(rndNum * 2 * (100 - absorptionPercent) / 100); }

        if (isDefense) { damageDealt = (int)(damageDealt * strength); }
        else { damageDealt = (int)(damageDealt * endurance / 2); }

        aim.health -= damageDealt;
        if (aim.health < 0) { aim.health = 0; }
        Console.WriteLine($"{characterName} dealt damage of {damageDealt} to {aim.characterName} > {aim.characterName} now has {aim.health} HP");

        if (aim.health == 0) { Console.WriteLine($"{characterName} won the game!"); }

        if (isDefense) { return; }
        else if (aim.health != 0) { aim.Attack(this, true); }
    }

    public void AllMoney() {
        int gold = 0, silver = 0, copper = 0;

        foreach (Item invItem in characterInv) {
            if (invItem.GetType() == typeof(Coin)) {
                if (invItem.CType == CoinType.Copper) {
                    copper = invItem.ItemCountGet;
                } else if (invItem.CType == CoinType.Silver) {
                    silver = invItem.ItemCountGet;
                } else if (invItem.CType == CoinType.Gold) {
                    gold = invItem.ItemCountGet;
                }

                if (gold != 0 && silver != 0 && copper != 0) { break; }
            }
        }

        if (gold == 0 && silver == 0 && copper == 0) {
            Console.WriteLine("You have no money at the moment!");
        } else {
            Console.WriteLine($"You have a total of {copper + 10 * silver + 100 * gold} copper coins\n- {copper} x copper\n- {silver} x silver\n- {gold} x gold");
        }
    }
}

public class Item {
    protected string name;
    public string Name { get { return name; } }

    protected int weight;
    public int Weight { get { return weight; } set { weight = value; } }

    protected bool isStackable = false;
    public bool IsStackable { get { return isStackable; } }

    protected int itemCount;
    public int ItemCountGet { get { return itemCount; } }

    public Item(string _name, int _weight) {
        name = _name;
        weight = _weight;
        itemCount = 1;
    }


    // ------------------- //
    // Subclass properties //
    // ------------------- //

    // Weapon
    protected virtual int damage { get; set; }
    public virtual int Damage { get { return damage; } }
    protected virtual bool isTwoHanded { get; set; }
    public virtual bool IsTwoHanded { get { return isTwoHanded; } }

    // Defense
    protected virtual int absorption { get; set; }
    public virtual int Absorption { get { return absorption; } set { absorption = value; } }
    protected virtual DefenseType dType { get; set; }
    public virtual DefenseType DType { get { return dType; } }

    // Coins
    protected virtual CoinType cType { get; set; }
    public virtual CoinType CType { get { return cType; } }

    // Stackable
    public virtual int ItemCountSet { get; set; }
}

public class Stackable : Item {
    public override int ItemCountSet { get { return itemCount; } set { itemCount = value; } }

    public Stackable(string _name, int _weight, int _itemCount) : base(_name, _weight) {
        itemCount = _itemCount;
        isStackable = true;
    }
}

public class Weapon : Item {
    public Weapon(string _name, int _weight, int _damage, bool _isTwoHanded) : base(_name, _weight) {
        damage = _damage;
        isTwoHanded = _isTwoHanded;
    }
}

public enum DefenseType { Shield, Armor }
public class Defense : Item {
    public Defense(string _name, int _weight, int _absorption, DefenseType _dType) : base(_name, _weight) {
        absorption = _absorption;
        dType = _dType;
    }
}

public enum CoinType { Copper, Silver, Gold }
public class Coin : Stackable {
    public Coin(string _name, int _weight, int _itemCount, CoinType _cType) : base(_name, _weight, _itemCount) {
        cType = _cType;
        isStackable = true;

        if (cType == CoinType.Copper) {
            name = "copper";
            weight = 1;
        } else if (cType == CoinType.Silver) {
            name = "silver";
            weight = 10;
        } else if (cType == CoinType.Gold) {
            name = "gold";
            weight = 100;
        }
    }
}