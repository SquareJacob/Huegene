using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Main : MonoBehaviour{
    private int width = 512;
    private int numPlants = 1000;
    static float recolor = 0.05f;
    public Material mat;
    private Texture2D texture;
    private List<Plant> plants;
    private int length;
    private Plant plant;
    private long drawTime;
    private long breedTime;
    private long ageTime;
    private Stopwatch stopWatch = new Stopwatch();
    private Color[] colors;
    private bool[,] covered;

    float rand(){
        return Random.value;
    }

    void Start(){
        //Intialize texture
        texture = new Texture2D(width, width);
        plants = new List<Plant>(width * width);
        colors = new Color[width * width];
        covered = new bool[width, width];

        //fill texture and covered
        for(int x = 0; x < width; x++){
            for(int y = 0; y < width; y++){
                texture.SetPixel(x, y, Color.black);
                covered[x, y] = false;
            }
        }
        texture.Apply(); //not sure if this is needed
        mat.SetTexture("_MainTex", texture); //sends texture
        for(int i = 0; i < 1; i++){
            plants.Add(new Plant(width / 2, width / 2, rand(), rand(), rand()));
            covered[width / 2, width / 2] = true;
        }
        stopWatch.Start();
    }


    void Update(){
        length = plants.Count;
        drawTime = 0;
        breedTime = 0;
        ageTime = 0;

        if(plants.Count < width * width){
            for(int i = 0; i < length; i++){
                plant = plants[i];
                if(plant.still){
                    continue;
                }
                breedTime -= stopWatch.ElapsedMilliseconds;
                plant.duplicate(plants, recolor, covered);
                breedTime += stopWatch.ElapsedMilliseconds;
                drawTime -= stopWatch.ElapsedMilliseconds;
                plant.draw(colors, width);
                drawTime += stopWatch.ElapsedMilliseconds;
            }
        }
        UnityEngine.Debug.Log("drawTime: " + drawTime + ", breedTime: " + breedTime + ", ageTime: " + ageTime);
        texture.SetPixels(colors);
        texture.Apply();
    }
}
