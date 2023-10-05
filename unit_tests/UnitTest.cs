using Xunit;
using TGproject;

namespace unit_tests;

public class UnitTests {
    // AddToInv
    [Fact] // Error
    public void MaxWeightOneItem() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 1500, 1)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void MaxWeightMultipleItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 100, 13)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void MaxWeightMultipleRecords() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 100, 5));
        hero.AddToInv(new Stackable("genericItem", 100, 8)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void NegativeWeight() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", -1)); // Error

        Assert.Equal("Error: You can't add an item with negative weight!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void NegativeCount() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 100, -1)); // Error

        Assert.Equal("Error: You can't add negative / zero ammount of items!\n", stringWriter.ToString());
    }


    // WriteInv
    [Fact] // Pass
    public void EmptyInventory() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.WriteInv();

        Assert.Equal("You have no items in your inventory at the moment\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void StackableInvItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 0, 10));
        hero.AddToInv(new Stackable("genericItem2", 0, 5));
        hero.AddToInv(new Stackable("genericItem", 0, 10));
        hero.AddToInv(new Stackable("genericItem2", 0, 5));
        hero.WriteInv();

        Assert.Equal("These are the items in your inventory\n- genericItem (20)\n- genericItem2 (10)\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void UnstackableInvItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 0));
        hero.AddToInv(new Item("genericItem", 0));
        hero.WriteInv();

        Assert.Equal("These are the items in your inventory\n- genericItem (non-stackable)\n- genericItem (non-stackable)\n", stringWriter.ToString());
    }


    // AllMoney
    [Fact] // Pass
    public void NoMoney() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AllMoney();

        Assert.Equal("You have no money at the moment!\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void OnlyGold() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Coin("gold", 100, 3, CoinType.Gold));
        hero.AllMoney();

        Assert.Equal("You have a total of 300 copper coins\n- 0 x copper\n- 0 x silver\n- 3 x gold\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void AnyCoins() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Coin("gold", 100, 3, CoinType.Gold));
        hero.AddToInv(new Coin("silver", 10, 10, CoinType.Silver));
        hero.AddToInv(new Coin("copper", 1, 20, CoinType.Copper));
        hero.AllMoney();

        Assert.Equal("You have a total of 420 copper coins\n- 20 x copper\n- 10 x silver\n- 3 x gold\n", stringWriter.ToString());
    }


    // WriteStats
    [Fact] // Pass
    public void EmptyInv() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        
        Random rnd = new Random(Program.rndSeed);
        double endurance = (double)rnd.Next(75, 151) / 100;
        double strength = (double)rnd.Next(75, 151) / 100;
        int maxHealth = (int)(1000 * endurance);
        int maxWeight = (int)(800 * strength);
        hero.WriteStats();

        Assert.Equal($"Name: {hero.CharacterName}\nHP: {maxHealth} / {maxHealth}\nInventory weight: 0 / {maxWeight}\nEndurance multiplayer: {endurance}x\nStrength multiplayer: {strength}x\n\nNo weapon equiped\nNo shield equiped\nNo armor equiped\n\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void FullInv() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Stackable("genericItem", 15, 10));
        hero.AddToInv(new Stackable("genericItem2", 10, 5));
        hero.AddToInv(new Stackable("genericItem", 15, 10));
        hero.AddToInv(new Stackable("genericItem2", 10, 5));
        hero.AddToInv(new Item("genericItem3", 10));

        Random rnd = new Random(Program.rndSeed);
        double endurance = (double)rnd.Next(75, 151) / 100;
        double strength = (double)rnd.Next(75, 151) / 100;
        int maxHealth = (int)(1000 * endurance);
        int maxWeight = (int)(800 * strength);
        hero.WriteStats();

        Assert.Equal($"Name: {hero.CharacterName}\nHP: {maxHealth} / {maxHealth}\nInventory weight: 410 / {maxWeight}\nEndurance multiplayer: {endurance}x\nStrength multiplayer: {strength}x\n\nNo weapon equiped\nNo shield equiped\nNo armor equiped\n\n", stringWriter.ToString());
    }
}