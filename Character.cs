namespace TGproject;

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

    public void RemoveFromInv(Item item) {
        characterInv.RemoveAt(characterInv.IndexOf(item));
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

    public void UseItem() {
        int index = -1;
        List<Item> itemSelection = new List<Item>();
        foreach (Item invItem in characterInv) {
            if (invItem.GetType() == typeof(Letter) || invItem.GetType() == typeof(Food)) {
                itemSelection.Add(invItem);
                Console.WriteLine($"{++index} - {invItem.Name} ");
            }
        }

        if (index == -1) {
            Console.WriteLine("There are no items in your inventory that you could use!");
            return;
        }

        Console.Write("Choose, which item you want to use (write number and confirm by enter): ");
        int num;
        if (int.TryParse(Console.ReadLine(), out num) && num >= 0 && num <= index) {
            if (itemSelection.ElementAt(num).GetType() == typeof(Letter)) {
                itemSelection.ElementAt(num).Read();
            } else if (itemSelection.ElementAt(num).GetType() == typeof(Food)) {
                health += itemSelection.ElementAt(num).Eat(this);
                if (health > maxHealth) { health = maxHealth; }
            }
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