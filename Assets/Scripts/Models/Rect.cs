using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rect
{

    int _x1;
    int _x2;
    int _y1;
    int _y2;

    public int X1 { get => _x1; set => _x1 = value; }
    public int X2 { get => _x2; set => _x2 = value; }
    public int Y1 { get => _y1; set => _y1 = value; }
    public int Y2 { get => _y2; set => _y2 = value; }

    public Rect(int x1, int y1, int width, int height) {
        X1 = x1;
        X2 = x1 + width;
        Y1 = y1;
        Y2 = y1+ height;
        
    }

    public int Area() {
        return Width() * Height();
    }

    public int Width(){
        return Mathf.Abs(X2 - X1);
    }
    
    public int Height()
    {
        return Mathf.Abs(Y2 - Y1);
    }

    public (int x, int y) Center()
    {
        int center_x = (int)((X1 + X2) / 2);
        int center_y = (int)((Y1 + Y2) / 2);
        return (center_x, center_y);
    }

    public bool Intersects(Rect other) {
        return (X1 <= other.X2 && X2 >= other.X1 && Y1 <= other.Y2 && Y2 >= other.Y1);
    }

    public override string ToString() {
        return "Rect: (" + X1 + ", " + Y1 +") ("+ X2 + ", " + Y2 + ")";
    }

}
