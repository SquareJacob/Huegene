using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant{
    public float x;
    public float y;
    public float r;
    public float g;
    public float b;

    public Plant(float plantX, float plantY, float plantR, float plantG, float plantB){
        x = plantX;
        y = plantY;
        r = plantR;
        g = plantG;
        b = plantB;
    }

    float rand(){
        return 2f * Random.value - 1f;
    }

    int round(float num){
        return (int) Mathf.Round(num);
    }

    int mod(int n, int m){
        int r = n % m;
        while(r < 0){
            r += m;
        }
        return r;
    }

    private float colorize(float num){
        if(num < 0 || num > 1){
            return Mathf.Abs(Mathf.Abs(num - 1) - 1);
        }
        return num;
    }
    public void draw(Color[] colors, int width){
        colors[mod(round(y), width) * width + mod(round(x), width)] = new Color(colorize(r), colorize(g), colorize(b), 1.0f);
    }

    public void duplicate(List<Plant> plants, float recolor){
        if(plants.Count == plants.Capacity){
            return;
        }
        float angle = Mathf.PI * rand();
        plants.Add(new Plant(x +  Mathf.Cos(angle), y + Mathf.Sin(angle), r + recolor * rand(), g + recolor * rand(), b + recolor * rand()));
    }

    public void age(float recolor){
        float angle = Mathf.PI * rand();
        x += Mathf.Cos(angle);
        y += Mathf.Sin(angle);
        r += recolor * rand();
        g += recolor * rand();
        b += recolor * rand();
    }       
}
