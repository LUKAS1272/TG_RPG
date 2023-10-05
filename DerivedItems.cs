namespace TGproject;

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

public class Letter : Item {
    string letterText;

    public Letter(string _name, int _weight, string _letterText) : base(_name, _weight) {
        letterText = _letterText;
        weight = 0;
    }

    public override void Read() {
        Console.WriteLine($"\n{letterText}");
    }
}

public class Food : Stackable {
    int regeneration;

    public Food(string _name, int _weight, int _itemCount, int _regeneration) : base(_name, _weight, _itemCount) {
        regeneration = _regeneration;
    }

    public override int Eat(Character hero) {
        itemCount--;
        Console.WriteLine($"\n{hero.CharacterName} just got healed {regeneration} HP by eating {name}");

        if (itemCount == 0) { hero.RemoveFromInv(this); }
        return regeneration;
    }
}
