using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldObject : MonoBehaviour
{
    [SerializeField] public string worldName;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private LandscapeGenerator landGen;
    [SerializeField] private PlaceGenerator placeGen;

    [SerializeField] private WorldDraw worldDraw;

    [SerializeField] private GameObject[] bosses;

    [SerializeField] public int worldTime;

    [SerializeField] private Color lightColor;
    [SerializeField] private float lightIntensity;
    [SerializeField] private float shadowStrength;
    [SerializeField] private Quaternion lightRotation;

    public World world;


    private void Start()
    {
        G.Instance.currentWorldObj = gameObject;
        landGen = GetComponent<LandscapeGenerator>();
        placeGen = GetComponent<PlaceGenerator>();
        world = new World(width, height, landGen, placeGen);
        world.GenerateWorld();

        worldDraw.world = world;
        worldDraw.DrawEverything();

        G.Instance.currentWorld = world;

        if (G.Instance.gameLight == null) return; 

        G.Instance.gameLight.color = lightColor;
        G.Instance.gameLight.intensity = lightIntensity;
        G.Instance.gameLight.transform.rotation = lightRotation;
        G.Instance.gameLight.shadowStrength = shadowStrength;
    }
}
