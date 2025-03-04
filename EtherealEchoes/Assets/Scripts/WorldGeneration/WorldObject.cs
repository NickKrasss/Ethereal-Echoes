using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldObject : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private LandscapeGenerator landGen;
    [SerializeField] private PlaceGenerator placeGen;

    [SerializeField] private WorldDraw worldDraw;

    [SerializeField] private NavMeshSurface[] navSurfaces;

    public World world;

    private void Start()
    {
        landGen = GetComponent<LandscapeGenerator>();
        placeGen = GetComponent<PlaceGenerator>();
        world = new World(width, height, landGen, placeGen);
        world.GenerateWorld();

        worldDraw.world = world;
        worldDraw.DrawEverything();

        foreach (var sur in navSurfaces)
            sur.BuildNavMeshAsync();
    }
}
