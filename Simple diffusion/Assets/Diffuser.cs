using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDCell {
    public float a = 1.0f;
    public float b = 0f;
}



public class Diffuser : MonoBehaviour{
    public Material mat;
    private Texture2D texture;
    private RDCell[,] grid, nextGrid;
    private int width = 256;
    private int height = 256;
    public float dA = 1f; //how much nearby A chemicals dip
    public float dB = 0.5f; //how much nearby B chemicals dip
    public float f = 0.055f; //feed rate for chemical A
    public float k = 0.062f; //feed rate for chemical B

    int mod(int x, int m) {
        int n = x % m;
        return n < 0 ? n + m : n;
    }

    //Laplace functions find weighted difference between a cell and its neighbor cells
    float LaplaceA(int x, int y){
        float sum = grid[x, y].a * -1f;

        sum += grid[mod(x + 1, width), mod(y, height)].a * 0.2f;
        sum += grid[mod(x - 1, width), mod(y, height)].a * 0.2f;
        sum += grid[mod(x, width), mod(y + 1, height)].a * 0.2f;
        sum += grid[mod(x, width), mod(y - 1, height)].a * 0.2f;

        sum += grid[mod(x + 1, width), mod(y + 1, height)].a * 0.05f;
        sum += grid[mod(x - 1, width), mod(y - 1, height)].a * 0.05f;
        sum += grid[mod(x - 1, width), mod(y + 1, height)].a * 0.05f;
        sum += grid[mod(x + 1, width), mod(y - 1, height)].a * 0.05f;
        return sum;
    }

    float LaplaceB(int x, int y){
        float sum = grid[x, y].b * -1f;

        sum += grid[mod(x + 1, width), mod(y, height)].b * 0.2f;
        sum += grid[mod(x - 1, width), mod(y, height)].b * 0.2f;
        sum += grid[mod(x, width), mod(y + 1, height)].b * 0.2f;
        sum += grid[mod(x, width), mod(y - 1, height)].b * 0.2f;

        sum += grid[mod(x + 1, width), mod(y + 1, height)].b * 0.05f;
        sum += grid[mod(x - 1, width), mod(y - 1, height)].b * 0.05f;
        sum += grid[mod(x - 1, width), mod(y + 1, height)].b * 0.05f;
        sum += grid[mod(x + 1, width), mod(y - 1, height)].b * 0.05f;
        return sum;
    }


    void Start(){
        //Intialize grid and texture
        texture = new Texture2D(width, height);
        grid = new RDCell[width, height];
        //fill grid and texture
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                texture.SetPixel(x, y, Color.red); //Make it a different color than used so that if something goes wrong here its obvious
                grid[x, y] = new RDCell();
            }
        }
        texture.Apply(); //not sure if this is needed
        mat.SetTexture("_MainTex", texture); //sends texture
        //make clump of B in center area to initialize spread
        int ax = width / 2;
        int ay = height / 2;
        grid[ax, ay].b = 1f;
        grid[ax, ay + 1].b = 1f;
        grid[ax + 1, ay].b = 1f;
        grid[ax + 1, ay + 1].b = 1f;
    }


    void Update(){
        for(int i = 0; i < 10; i++){ //Perform calculations 10 times before drawing to allow quicker simulations (less time spent drawing)
            nextGrid = new RDCell[width, height];
            for(int x = 0; x < width; x++){
                for(int y = 0; y < height; y++){
                    //Generate next Grid using current grid
                    nextGrid[x, y] = new RDCell();
                    float a = grid[x, y].a;
                    float b = grid[x, y].b;
                    nextGrid[x, y].a = a + (dA * LaplaceA(x, y) - (a * b * b) + (f * (1 - a)));
                    nextGrid[x, y].b = b + (dB * LaplaceB(x, y) + (a * b * b) - ((k + f) * b));
                }
            }
            grid = nextGrid;
        }
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                //apply next Grid to texture
                texture.SetPixel(x, y, Color.Lerp(Color.green, Color.blue, nextGrid[x, y].b));
            }
        }
        texture.Apply();
    }
}
