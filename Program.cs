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

        Console.WriteLine($"It's {currentHero.characterName}'s turn!");
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
                if (currentHero.health == 0 || nextHero.health == 0) { return; }
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
    public string characterName; // Use get set
    private int maxHealth;
    public int health; // Use get set

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
        if (item.itemCount > 1 && !item.isStackable) {
            Console.WriteLine($"Error: The {item.name} is not stackable, you can't add multiple of them at one time!");
            return;
        } else if (item.itemCount <= 0) {
            Console.WriteLine($"Error: You can't add negative / zero ammount of items!");
            return;
        }

        // Weight errors
        if (currentWeight + item.weight * item.itemCount > maxWeight) {
            Console.WriteLine($"Error: The {characterName} can't hold this item(s), it is too heavy!");
            return;
        } else if (item.weight < 0) {
            Console.WriteLine($"Error: You can't add an item with negative weight!");
            return;
        }

        // Checks whether the (stackable) item is alrady in the inventory
        foreach (Item invItem in characterInv) {
            if (item.name == invItem.name && item.isStackable) {
                invItem.itemCount += item.itemCount;
                return;
            }
        }

        // Adds the item to the inventory
        characterInv.Add(item);
        currentWeight += item.weight * item.itemCount;
        //Console.WriteLine($"Added {item.name} ({item.itemCount} with weight of {item.itemCount * item.weight}), the current weight is {currentWeight}");
    }

    public void WriteInv() {
        if (characterInv.Count == 0) {
            Console.WriteLine("You have no items in your inventory at the moment");
            return;
        }

        Console.WriteLine("These are the items in your inventory");
        foreach (Item invItem in characterInv) {
            if (invItem.isStackable) {
                Console.WriteLine($"- {invItem.name} ({invItem.itemCount})");
            } else {
                Console.WriteLine($"- {invItem.name} (non-stackable)");
            }
        }
    }

    private void WriteEquipment() {
        if (leftHand == null) {
            Console.WriteLine("No weapon equiped");
        } else {
            Console.Write($"Weapon equiped: {leftHand.name} ");
            if (leftHand.isTwoHanded) { Console.WriteLine("(two-handed > cannot equip shield with this weapon)"); }
            else { Console.WriteLine(); }
        }

        if (rightHand == null) { Console.WriteLine("No shield equiped"); }
        else { Console.WriteLine($"Shield equiped: {rightHand.name}"); }

        if (chest == null) { Console.WriteLine("No armor equiped"); }
        else { Console.WriteLine($"Armor equiped: {chest.name}"); }

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
                Console.Write($"{++index} - {invItem.name} ");

                if (invItem.isTwoHanded) {
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
                if (itemSelection.ElementAt(num).isTwoHanded) {
                    rightHand = null!;
                }
            } else if (itemSelection.ElementAt(num).dType == DefenseType.Shield) {
                rightHand = itemSelection.ElementAt(num);
                if (leftHand != null && leftHand.isTwoHanded) {
                    leftHand = null!;
                }
            } else if (itemSelection.ElementAt(num).dType == DefenseType.Armor) {
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
        if (aim.chest != null) { absorptionPercent += aim.chest.absorption; }
        if (aim.rightHand != null) { absorptionPercent += aim.rightHand.absorption; }

        Random rnd = new Random();
        int rndNum = rnd.Next(1, 6);

        int damageDealt = 0;
        if (leftHand != null) { damageDealt = (int)(rndNum * leftHand.damage * (100 - absorptionPercent) / 100); }
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
                if (invItem.cType == CoinType.Copper) {
                    copper = invItem.itemCount;
                } else if (invItem.cType == CoinType.Silver) {
                    silver = invItem.itemCount;
                } else if (invItem.cType == CoinType.Gold) {
                    gold = invItem.itemCount;
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
    public string name;
    public int weight;
    public int itemCount;
    public bool isStackable;

    public Item(string _name, int _weight, int _itemCount, bool _isStackable) {
        name = _name;
        weight = _weight;
        itemCount = _itemCount;
        isStackable = _isStackable;
    }

    // Weapon
    public virtual int damage { get; set; }
    public virtual bool isTwoHanded { get; set; }

    // Defense
    public virtual int absorption { get; set; }
    public virtual DefenseType dType { get; set; }

    // Coins
    public virtual CoinType cType { get; set; }
}

public class Weapon : Item {
    public override int damage { get; set; }
    public override bool isTwoHanded { get; set; }

    public Weapon(string _name, int _weight, int _itemCount, bool _isStackable, int _damage, bool _isTwoHanded) : base(_name, _weight, _itemCount, _isStackable) {
        damage = _damage;
        isTwoHanded = _isTwoHanded;
    }
}

public enum DefenseType { Shield, Armor }
public class Defense : Item {
    public override int absorption { get; set; }
    public override DefenseType dType { get; set; }
    
    public Defense(string _name, int _weight, int _itemCount, bool _isStackable, int _absorption, DefenseType _dType) : base(_name, _weight, _itemCount, _isStackable) {
        absorption = _absorption;
        dType = _dType;
    }
}

public enum CoinType { Copper, Silver, Gold }
public class Coin : Item {
    public override CoinType cType { get; set; }

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