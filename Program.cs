using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TGproject;

// Character - name
// Item - name, weight, itemCount, isStackable
// Weapon - name, weight, itemCount, isStackable, damage, isTwoHanded
// Defense - name, weight, itemCount, isStackable, absorption, DefenseType
// Coin - name, weight, count, isStackable, CoinType
 
class Program {
    static Character currentHero = new Character("mighty knight");
    static Character nextHero = new Character("cruel king");

    static void Main(string[] args) {
        // First hero inventory init
        currentHero.AddToInv(new Weapon("golden sword", 70, 1, false, 15, false));
        currentHero.AddToInv(new Weapon("iron sword", 85, 1, false, 35, true));
        currentHero.AddToInv(new Defense("iron shield", 90, 1, false, 35, DefenseType.Shield));
        currentHero.AddToInv(new Defense("iron chestplate", 100, 1, false, 25, DefenseType.Armor));
        currentHero.AddToInv(new Coin("", 0, 30, true, CoinType.Copper));
        currentHero.AddToInv(new Coin("", 0, 2, true, CoinType.Silver));
        currentHero.AddToInv(new Coin("", 0, 1, true, CoinType.Gold));

        // Second hero inventory init
        nextHero.AddToInv(new Weapon("golden sword", 70, 1, false, 60, false));
        nextHero.AddToInv(new Weapon("iron sword", 85, 1, false, 80, true));
        nextHero.AddToInv(new Defense("iron shield", 90, 1, false, 90, DefenseType.Shield));
        nextHero.AddToInv(new Defense("iron chestplate", 100, 1, false, 100, DefenseType.Armor));
        nextHero.AddToInv(new Coin("", 0, 30, true, CoinType.Copper));
        nextHero.AddToInv(new Coin("", 0, 2, true, CoinType.Silver));
        nextHero.AddToInv(new Coin("", 0, 1, true, CoinType.Gold));

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
    private int health;
    public int Health { get { return health; } set { health = value; } }
    private int maxHealth;

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
        maxHealth = rnd.Next(300, 501);
        health = maxHealth;

        maxWeight = rnd.Next(500, 751);
    }

    public void WriteStats() {
        Console.WriteLine($"Name: {characterName}");
        Console.WriteLine($"Inventory weight: {currentWeight} / {maxWeight}");
        Console.WriteLine($"Hero HP: {health} / {maxHealth}");

        Console.WriteLine();
        WriteEquipment();
    }

    public void AddToInv(Item item) {
        // Count errors
        if (item.ItemCount > 1 && !item.IsStackable) {
            Console.WriteLine($"Error: The {item.Name} is not stackable, you can't add multiple of them at one time!");
            return;
        } else if (item.ItemCount <= 0) {
            Console.WriteLine($"Error: You can't add negative / zero ammount of items!");
            return;
        }

        // Weight errors
        if (currentWeight + item.Weight * item.ItemCount > maxWeight) {
            Console.WriteLine($"Error: The {characterName} can't hold this item(s), it is too heavy!");
            return;
        } else if (item.Weight < 0) {
            Console.WriteLine($"Error: You can't add an item with negative weight!");
            return;
        }

        // Checks whether the (stackable) item is alrady in the inventory
        foreach (Item invItem in characterInv) {
            if (item.Name == invItem.Name && item.IsStackable) {
                invItem.ItemCount += item.ItemCount;
                return;
            }
        }

        // Adds the item to the inventory
        characterInv.Add(item);
        currentWeight += item.Weight * item.ItemCount;
        
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
                Console.WriteLine($"- {invItem.Name} ({invItem.ItemCount})");
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

    public void Attack(Character aim, bool isRecursion) {
        int absorptionPercent = 0;
        if (aim.chest != null) { absorptionPercent += aim.chest.Absorption; }
        if (aim.rightHand != null) { absorptionPercent += aim.rightHand.Absorption; }

        Random rnd = new Random();
        int rndNum = rnd.Next(1, 6);

        int damageDealt = 0;
        if (leftHand != null) { damageDealt = (int)(rndNum * leftHand.Damage * (100 - absorptionPercent) / 100); }
        else { damageDealt = (int)(rndNum * 2 * (100 - absorptionPercent) / 100); }

        if (isRecursion) { damageDealt /= 2; }

        aim.health -= damageDealt;
        if (aim.health < 0) { aim.health = 0; }
        Console.WriteLine($"{characterName} dealt damage of {damageDealt} to {aim.characterName} > {aim.characterName} now has {aim.health} HP");

        if (aim.health == 0) { Console.WriteLine($"{characterName} won the game!"); }

        if (isRecursion) { return; }
        else if (aim.health != 0) { aim.Attack(this, true); }
    }

    public void AllMoney() {
        int gold = 0, silver = 0, copper = 0;

        foreach (Item invItem in characterInv) {
            if (invItem.GetType() == typeof(Coin)) {
                if (invItem.CType == CoinType.Copper) {
                    copper = invItem.ItemCount;
                } else if (invItem.CType == CoinType.Silver) {
                    silver = invItem.ItemCount;
                } else if (invItem.CType == CoinType.Gold) {
                    gold = invItem.ItemCount;
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
    protected int itemCount;
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    protected bool isStackable;
    public bool IsStackable { get { return isStackable; } }

    public Item(string _name, int _weight, int _itemCount, bool _isStackable) {
        name = _name;
        weight = _weight;
        itemCount = _itemCount;
        isStackable = _isStackable;
    }

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
}

public class Weapon : Item {
    public Weapon(string _name, int _weight, int _itemCount, bool _isStackable, int _damage, bool _isTwoHanded) : base(_name, _weight, _itemCount, _isStackable) {
        damage = _damage;
        isTwoHanded = _isTwoHanded;
    }
}

public enum DefenseType { Shield, Armor }
public class Defense : Item {
    public Defense(string _name, int _weight, int _itemCount, bool _isStackable, int _absorption, DefenseType _dType) : base(_name, _weight, _itemCount, _isStackable) {
        absorption = _absorption;
        dType = _dType;
    }
}

public enum CoinType { Copper, Silver, Gold }
public class Coin : Item {
    public Coin(string _name, int _weight, int _itemCount, bool _isStackable, CoinType _cType) : base(_name, _weight, _itemCount, _isStackable) {
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