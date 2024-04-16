using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Plant{
    public int x;
    public int y;
    public float r;
    public float g;
    public float b;
    public bool still;

    public Plant(int plantX, int plantY, float plantR, float plantG, float plantB){
        x = plantX;
        y = plantY;
        r = plantR;
        g = plantG;
        b = plantB;
        still = false;
    }

    float rand(){
        return 2f * UnityEngine.Random.value - 1f;
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

    private int[] direction(int x, int y, bool[,] covered){
        List<int[]> directions = new List<int[]>(4);
        int width = covered.GetLength(0);
        
        int[] direction = new int[]{mod(x + 1, width), y};
        if(!covered[direction[0], direction[1]]){
            directions.Add(new int[]{direction[0], direction[1]});
        }

        direction = new int[]{mod(x - 1, width), y};
        if(!covered[direction[0], direction[1]]){
            directions.Add(new int[]{direction[0], direction[1]});
        }
        
        direction = new int[]{x, mod(y + 1, width)};
        if(!covered[direction[0], direction[1]]){
            directions.Add(new int[]{direction[0], direction[1]});
        }

        direction = new int[]{x, mod(y - 1, width)};
        if(!covered[direction[0], direction[1]]){
            directions.Add(new int[]{direction[0], direction[1]});
        }

        if(directions.Count > 0) {
            var random = new System.Random();
            int index = random.Next(directions.Count);
            return directions[index];
        } else {
            return new int[]{-1, 0};        
        }
    }

    private float colorize(float num){
        if(num < 0 || num > 1){
            return Mathf.Abs(Mathf.Abs(num - 1) - 1);
        }
        return num;
    }
    public void draw(Color[] colors, int width){
        colors[y * width + x] = new Color(colorize(r), colorize(g), colorize(b), 1.0f);
    }

    public void duplicate(List<Plant> plants, float recolor, bool[,] covered){
        if(plants.Count == plants.Capacity){
            return;
        }
        int[] pos = direction(x, y, covered);
        if(pos[0] != -1){
            plants.Add(new Plant(pos[0], pos[1], r + recolor * rand(), g + recolor * rand(), b + recolor * rand()));
            covered[pos[0], pos[1]] = true;
        } else {
            still = true;
        }
    }      
}
