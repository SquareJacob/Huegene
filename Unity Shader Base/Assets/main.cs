using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Diffuser : MonoBehaviour{
    public Material mat;
    private Texture2D texture;
    private int width = 255;
    private int height = 255;


    void Start(){
        //Intialize texture
        texture = new Texture2D(width, height);
        //fill texture
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                texture.SetPixel(x, y, Color.red);
            }
        }
        texture.Apply(); //not sure if this is needed
        mat.SetTexture("_MainTex", texture); //sends texture
    }


    void Update(){

    }
}
