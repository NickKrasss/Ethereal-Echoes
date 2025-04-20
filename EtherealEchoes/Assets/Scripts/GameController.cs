using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] worlds;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private MinimapScr minimapScr;

    private int currentWorldInd = 0;


    private void Start()
    {
        StartCoroutine(GameCycle());
    }

    private IEnumerator GameCycle()
    {
        NextWorld();
        yield return new WaitForEndOfFrame();
        minimapScr.GenerateTexture();
        Instantiate(playerPrefab, new Vector2(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2), Quaternion.Euler(-25, 0, 0));
        Camera.main.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2 - 5, Camera.main.transform.position.z);

        while (true) 
        {
            while (G.Instance.currentTime > 0)
            {
                yield return new WaitForSeconds(1);
                G.Instance.currentTime -= 1;
            }
            if (NextWorld() == null)
                break;
        }
        

    }

    private GameObject NextWorld()
    {
        if (G.Instance.currentWorldObj != null)
        {
            Destroy(G.Instance.currentWorldObj);
            G.Instance.currentWorldObj = null;
            currentWorldInd++;
        }

        if (worlds.Length <= currentWorldInd)
            return null;

        G.Instance.currentTime = worlds[currentWorldInd].GetComponent<WorldObject>().worldTime;
        return Instantiate(worlds[currentWorldInd]);
    }
}
