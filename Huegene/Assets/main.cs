using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Main : MonoBehaviour{
    private int width = 512;
    private int numPlants = 10000;
    static float recolor = 0.01f;
    public Material mat;
    private Texture2D texture;
    private List<Plant> plants;
    private int length;
    private Plant plant;
    private long drawTime;
    private long breedTime;
    private long ageTime;
    Stopwatch stopWatch = new Stopwatch();
    Color[] colors;

    float rand(){
        return Random.value;
    }

    void Start(){
        //Intialize texture
        texture = new Texture2D(width, width);
        plants = new List<Plant>(numPlants);
        colors = new Color[width * width];

        //fill texture
        for(int x = 0; x < width; x++){
            for(int y = 0; y < width; y++){
                texture.SetPixel(x, y, Color.black);
            }
        }
        texture.Apply(); //not sure if this is needed
        mat.SetTexture("_MainTex", texture); //sends texture
        for(int i = 0; i < 1; i++){
            plants.Add(new Plant(width / 2f, width / 2f, rand(), rand(), rand()));
        }
        stopWatch.Start();
    }


    void Update(){
        length = plants.Count;
        drawTime = 0;
        breedTime = 0;
        ageTime = 0;

        if(plants.Count < numPlants){
            for(int i = 0; i < length; i++){
                plant = plants[i];
                drawTime -= stopWatch.ElapsedMilliseconds;
                plant.draw(colors, width);
                drawTime += stopWatch.ElapsedMilliseconds;
                breedTime -= stopWatch.ElapsedMilliseconds;
                plant.duplicate(plants, recolor);
                breedTime += stopWatch.ElapsedMilliseconds;
                ageTime -= stopWatch.ElapsedMilliseconds;
                plant.age(recolor);
                ageTime += stopWatch.ElapsedMilliseconds;
            }
        } else {
            foreach(Plant plant in plants){
                drawTime -= stopWatch.ElapsedMilliseconds;
                plant.draw(colors, width);
                drawTime += stopWatch.ElapsedMilliseconds;
                ageTime -= stopWatch.ElapsedMilliseconds;
                plant.age(recolor);
                ageTime += stopWatch.ElapsedMilliseconds;
            }
        }
        UnityEngine.Debug.Log("drawTime: " + drawTime + ", breedTime: " + breedTime + ", ageTime: " + ageTime);
        texture.SetPixels(colors);
        texture.Apply();
    }
}
