using System;
using UnityEngine;


[Serializable]
public struct Line
{
    public Vector2 start;
    public Vector2 end;
    public bool isTransparent;
    public Color color;

    public Line(Vector2 start, Vector2 end, Color color, bool isTransparent = false)
    {
        this.start = start;
        this.end = end;
        if (this.end.x == this.start.x) this.end.x += 0.01f;
        this.isTransparent = isTransparent;
        this.color = color;
    }

    public bool IsOnLine(Vector2 point)
    {
        return (point.x >= start.x || point.x >= end.x) && (point.x <= end.x || point.x <= start.x);
    }

    public Color GetColor()
    {
        if (isTransparent) return new Color(color.r / 2, color.g / 2, color.b / 2, color.a / 2);
        else return color;
    }
}

[Serializable]
public struct Function
{
    public float a;
    public float b;

    public Function(float a, float b)
    {
        this.a = a;
        this.b = b;
    }

    public Function(Vector2 pointA, Vector2 pointB)
    {
        this.a = (pointB.y - pointA.y) / (pointB.x - pointA.x);
        this.b = pointA.y - this.a * pointA.x;
    }

    public float GetY(float x)
    {
        return a * x + b;
    }

    public Vector2 GetIntersection(Function func)
    {
        // 1. Nach X auflösen
        // 1.1 steigungen zusammenzählen
        float newA = this.a - func.a;
        // 1.2 Y-Schnitte zusammenzählen
        float newB = func.b - this.b;
        // 1.3 X ausrechnen : x = yabschnitt / steigung
        float newX = newB / newA;
        // 2. X einsetzten um das Y zu bekommen
        float newY = GetY(newX);
        return new Vector2(newX, newY);
    }


}

public struct MathRay
{

    public Vector2 position;
    public float angle;
    public Function function;
    public Vector2 point;

    public MathRay(Vector2 position, float angle)
    {
        this.position = position;
        this.angle = angle;
        float rad = Mathf.Deg2Rad * angle;
        point = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        function = new Function(position, position + point);
    }

    public bool IsInFront(Vector2 point)
    {
        if (angle > 270 || angle < 90)
        {
            if (point.x > position.x) return true;
            else return false;
        }
        else
        {
            if (point.x < position.x) return true;
            else return false;
        }
    }


}

public struct HitPoint
{
    public Vector2 position;
    public float distance;
    public bool isTransparent;


    public HitPoint(Vector2 position, float distance, bool transparent)
    {
        this.position = position;
        this.distance = distance;
        isTransparent = transparent;
    }
}

