using UnityEngine;

public enum ColorEnum
{
    Red,
    Blue,
    Yellow
};

public enum GeometricShapesEnum
{
    Square,
    Circle,
    Triangle
}

public enum NumberEnum
{
    One,
    Two,
    Three
};

public enum ModelEnum
{
    Empty,
    Full,
    Stripes
};


[CreateAssetMenu(fileName = "Card",menuName = "New Card")]
public class Card : ScriptableObject
{
    public Sprite sprite;
    public ColorEnum color;
    public GeometricShapesEnum geometricShapes;
    public NumberEnum number;
    public ModelEnum model;
}
