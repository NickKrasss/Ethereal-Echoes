using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldFill))]
public class PlaceSpawner : MonoBehaviour
{
    [SerializeField]
    private PlaceType[] placeTypes;

    [SerializeField]
    private Place playerPlace;

    private WorldFill worldScr;

    private void Awake()
    {
        worldScr = GetComponent<WorldFill>();
    }

    private void Update()
    {
        
        if (!worldScr.placesSpawned)
        {
            if (worldScr.generated)
            {
                int x = worldScr.width / 2;
                int y = worldScr.height / 2;
                int x1 = x - playerPlace.width / 2; int y1 = y - playerPlace.height / 2;
                int x2 = x1 + playerPlace.width; int y2 = y1 + playerPlace.height;
                Sector sector = new Sector(x1, y1, x2, y2);
                SpawnPlace(playerPlace, sector);

                SpawnPlaces();
                worldScr.placesSpawned = true;
            }
        }
    }

    private void SpawnPlace(Place place, Sector sector)
    {
        GameObject obj = Instantiate(
                        place.obj,
                        new Vector3(sector.center[0] + place.offset_x + Random.Range(-place.random_offset, place.random_offset), sector.center[1] + place.offset_y + Random.Range(-place.random_offset, place.random_offset), place.offset_z),
                        Quaternion.Euler(place.rotationAngle, 0, 0)
                        );
        obj.transform.SetParent(worldScr.transform);
        place.hadArleadySpawned = true;
        sector.Fill(worldScr, 0);
    }

    public void SpawnPlaces()
    {
        foreach (PlaceType placeType in placeTypes)
        {
            int count = Random.Range(placeType.lowerCount, placeType.upperCount);

            for (int i = 0; i < count; i++)
            {
                Place place = GetRandomPlace(placeType);
                Sector sector = findSectorFilledWith(place.width, place.height, 1);
                SpawnPlace(place, sector);
            }
        }
    }

    private Sector findSectorFilledWith(int width, int height, int digit)
    {
        
        Sector sector = null;
        int attempts = 0;
        while (!worldScr.isSectorEmpty(sector, digit))
        {
            attempts++;
            if (attempts >= 40000)
            {
                Debug.Log("Не нашлось");
                return null; 
            }

            int x = Random.Range(0, worldScr.width);
            int y = Random.Range(0, worldScr.height);
            while (worldScr.world[x, y] != digit)
            {
                x = Random.Range(0, worldScr.width);
                y = Random.Range(0, worldScr.height);
            }
            if (width == 0 || height == 0)
                return new Sector(x, y, x, y);
            int x1 = x - width / 2; int y1 = y - height / 2;
            int x2 = x1 + width; int y2 = y1 + height;
            sector = new Sector(x1, y1, x2, y2);

        };
        return sector;
    }

    private Place GetRandomPlace(PlaceType placeType)
    {
        int x = Random.Range(0, placeType.FrequencySum);
        foreach (Place place in placeType.places)
        {
            if (x < place.frequency)
            {
                if (!place.canRepeat && place.hadArleadySpawned) return GetRandomPlace(placeType);
                return place;
            }
            x -= place.frequency;
        }
        return null;
    }
}

[System.Serializable]
public class PlaceType
{
    [SerializeField]
    public string name;

    [SerializeField]
    public int lowerCount;

    [SerializeField]
    public int upperCount;

    [SerializeField]
    public Place[] places;

    public int FrequencySum
    {
        get
        {
            int result = 0;
            foreach (Place place in places)
            {
                result += place.frequency;
            }
            return result;
        }
    }
}

[System.Serializable]
public class Place
{

    [SerializeField]
    public bool canRepeat = true;

    
    public bool hadArleadySpawned = false;

    [SerializeField]
    public int frequency = 100;

    [SerializeField]
    public int width;

    [SerializeField]
    public int height;

    [SerializeField]
    public float offset_x;

    [SerializeField]
    public float offset_y;

    [SerializeField]
    public float offset_z = 0;

    [SerializeField]
    public float random_offset;

    [SerializeField]
    public GameObject obj;

    [SerializeField]
    public float rotationAngle = -25;

}