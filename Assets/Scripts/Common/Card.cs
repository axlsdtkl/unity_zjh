using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    //权值
    public int Weight { get; set; }
    //颜色
    public int Color { get; set; }

    public Card(int weight,int color)
    {
        this.Weight = weight;
        this.Color = color;
    }
}
