namespace TGproject;

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

    // Usable
    public virtual void Read() {}
    public virtual int Eat(Character hero) { return -1; }
}

public class Stackable : Item {
    public override int ItemCountSet { get { return itemCount; } set { itemCount = value; } }

    public Stackable(string _name, int _weight, int _itemCount) : base(_name, _weight) {
        itemCount = _itemCount;
        isStackable = true;
    }
}
