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
        hero.AddToInv(new Item("genericItem", 1000, 1, false)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void MaxWeightMultipleItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 100, 10, true)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void MaxWeightMultipleRecords() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 100, 5, true));
        hero.AddToInv(new Item("genericItem", 100, 5, true)); // Error

        Assert.Equal("Error: The hero can't hold this item(s), it is too heavy!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void NegativeWeight() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", -1, 1, true)); // Error

        Assert.Equal("Error: You can't add an item with negative weight!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void NegativeCount() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 100, -1, true)); // Error

        Assert.Equal("Error: You can't add negative / zero ammount of items!\n", stringWriter.ToString());
    }

    [Fact] // Error
    public void UnstackableItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 1, 2, false)); // Error

        Assert.Equal("Error: The genericItem is not stackable, you can't add multiple of them at one time!\n", stringWriter.ToString());
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
        hero.AddToInv(new Item("genericItem", 0, 10, true));
        hero.AddToInv(new Item("genericItem2", 0, 5, true));
        hero.AddToInv(new Item("genericItem", 0, 10, true));
        hero.AddToInv(new Item("genericItem2", 0, 5, true));
        hero.WriteInv();

        Assert.Equal("These are the items in your inventory\n- genericItem (20)\n- genericItem2 (10)\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void UnstackableInvItems() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Item("genericItem", 0, 1, false));
        hero.AddToInv(new Item("genericItem", 0, 1, false));
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
        hero.AddToInv(new Coin("gold", 100, 3, true, CoinType.Gold));
        hero.AllMoney();

        Assert.Equal("You have a total of 300 copper coins\n- 0 x copper\n- 0 x silver\n- 3 x gold\n", stringWriter.ToString());
    }

    [Fact] // Pass
    public void AnyCoins() {
        StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Character hero = new Character("hero");
        hero.AddToInv(new Coin("gold", 100, 3, true, CoinType.Gold));
        hero.AddToInv(new Coin("silver", 10, 10, true, CoinType.Silver));
        hero.AddToInv(new Coin("copper", 1, 20, true, CoinType.Copper));
        hero.AllMoney();

        Assert.Equal("You have a total of 420 copper coins\n- 20 x copper\n- 10 x silver\n- 3 x gold\n", stringWriter.ToString());
    }
}