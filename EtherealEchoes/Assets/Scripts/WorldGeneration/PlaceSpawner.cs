using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class PlaceSpawner : MonoBehaviour, PlaceGenerator
{
    [SerializeField]
    private PlaceType[] placeTypes;

    [SerializeField]
    private Place playerPlace;

    private Place[] places;

    private int[,] landscape;

    private List<(int, int)> clearPoints;

    private List<GameObject> placeObjects = new List<GameObject>();

    public (bool, int[,], Place[]) GeneratePlaces(int[,] map, List<(int, int)> _clearPoints = null)
    {
        landscape = map;
        places = new Place[] { };
        clearPoints = _clearPoints;
        int x = landscape.GetLength(0) / 2;
        int y = landscape.GetLength(1) / 2;
        int x1 = x - playerPlace.width / 2; int y1 = y - playerPlace.height / 2;
        int x2 = x1 + playerPlace.width; int y2 = y1 + playerPlace.height;
        Sector sector = new Sector(x1, y1, x2, y2);
        SpawnPlace(playerPlace, sector);

        if (!SpawnPlaces()) return (false, null, null);
        return (true, landscape, places);
    }

    public void ClearPlaces()
    { 
        while (placeObjects.Count != 0)
        {
            Destroy(placeObjects[0]);
            placeObjects.RemoveAt(0);
        }
    }

    private void SpawnPlace(Place place, Sector sector)
    {
        GameObject obj = Instantiate(
                        place.obj,
                        new Vector3(sector.center[0] + place.offset_x + Random.Range(-place.random_offset, place.random_offset), sector.center[1] + place.offset_y + Random.Range(-place.random_offset, place.random_offset), place.offset_z),
                        Quaternion.Euler(place.isGroupObject ? 0 : place.rotationAngle, 0, 0)
                        );

        obj.transform.SetParent(transform);
        place.hadArleadySpawned = true;
        places.Append(place);
        if (place.isGroupObject)
        {
            foreach (var child in obj.GetComponentsInChildren<Transform>()[1..])
            {
                if (child.parent == obj.transform)
                {
                    if (place.rotationAngle != 0) child.transform.rotation = Quaternion.Euler(place.rotationAngle, 0, 0);
                    child.transform.SetParent(transform);
                    placeObjects.Add(child.gameObject);
                }
            }
            obj.transform.DetachChildren();
            Destroy(obj);
        }
        else
            placeObjects.Add(obj);
        World.Fill(landscape, sector, 2);
    }

    public bool SpawnPlaces()
    {
        foreach (PlaceType placeType in placeTypes)
        {
            int count = Random.Range(placeType.lowerCount, placeType.upperCount);

            for (int i = 0; i < count; i++)
            {
                Place place = GetRandomPlace(placeType);
                Sector sector = findSectorFilledWith(place.width, place.height, 0);
                if (sector == null)
                {
                    return false;
                }
                SpawnPlace(place, sector);
            }
        }
        return true;
    }

    private Sector findSectorFilledWith(int width, int height, int digit)
    {
        
        Sector sector = null;
        int attempts = 0;
        while (!World.isSectorFilledWith(landscape, sector, digit))
        {
            attempts++;
            if (attempts >= 400000)
            {
                Debug.Log("Error, Regenerating world.");
                return null; 
            }

            (int x, int y) = clearPoints[Random.Range(0, clearPoints.Count)];
            int xxx = 0;
            while (landscape[x, y] != digit)
            {
                xxx++;
                if (xxx > 10000000)
                {
                    Debug.Log("Error, Regenerating world.");
                    return null;
                }
                (x, y) = clearPoints[Random.Range(0, clearPoints.Count)];
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

    [SerializeField]
    public bool isGroupObject = false;

}