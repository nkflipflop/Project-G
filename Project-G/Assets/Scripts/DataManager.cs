public static class DataManager 
{
    private static int _dungeonLevel = 0, _medkits = 3, _shields = 0;
    private static float _health = 100;
    private static WeaponBase _weapon;

    public static int DungeonLevel  {
        get { return _dungeonLevel; }
        set { _dungeonLevel = value; }
    }

    public static float Health {
        get { return _health; }
        set { _health = value; }
    }

    public static WeaponBase Weapon {
        get { return _weapon; }
        set { _weapon = value; }
    }

    public static int Medkits  {
        get { return _medkits; }
        set { _medkits = value; }
    }

    public static int Shields  {
        get {  return _shields; }
        set { _shields = value; }
    }
}