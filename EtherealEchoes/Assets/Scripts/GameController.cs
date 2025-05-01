using NavMeshPlus.Components;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] worlds;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private MinimapScr minimapScr;

    [SerializeField] private NavMeshSurface[] surfaces;

    private int currentWorldInd = 0;


    private void Start()
    {
        StartCoroutine(GameCycle());
    }

    private IEnumerator GameCycle()
    {
        G.Instance.isWorldLoading = true;
        GameObject world = null;
        
        world = NextWorld();
        yield return new WaitForEndOfFrame();
        GenerateNavmesh();
        minimapScr.GenerateTexture();
        Instantiate(playerPrefab, new Vector2(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2), Quaternion.Euler(-25, 0, 0));
        Camera.main.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2 - 5, Camera.main.transform.position.z);

        G.Instance.isWorldLoading = false;
        yield return new WaitForSeconds(1f);
        TransitionOverlayController.Instance.FadeOut(0.5f, 0f);
        yield return new WaitForSeconds(1f);
        while (true) 
        {
            while (G.Instance.currentTime > 0)
            {
                yield return new WaitForSeconds(1);
                if (!G.Instance.playerDead) G.Instance.currentTime -= 1;
            }
            yield return new WaitForSeconds(1f);
            TransitionOverlayController.Instance.FadeIn(0.5f, 0f);
            yield return new WaitForSeconds(1f);
            G.Instance.isWorldLoading = true;
            

            if (NextWorld() == null)
            {
                break;
            }
            G.Instance.playerObj.GetComponent<Stats>().CurrentHealth = G.Instance.playerObj.GetComponent<Stats>().MaxHealth;
            yield return new WaitForEndOfFrame();
            GenerateNavmesh();

            G.Instance.playerObj.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2, G.Instance.playerObj.transform.position.z);

            Camera.main.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2 - 5, Camera.main.transform.position.z);
            minimapScr.GenerateTexture();
            AudioManager.Instance.EndAllSounds();
            G.Instance.isWorldLoading = false;
            yield return new WaitForSeconds(1f);
            TransitionOverlayController.Instance.FadeOut(0.5f, 0f);
            yield return new WaitForSeconds(1f);
        }
        

    }

    private void GenerateNavmesh()
    {
        foreach (var sur in surfaces)
        {
            sur.BuildNavMeshAsync();
        }
    }

    private GameObject NextWorld()
    {

        if (G.Instance.currentWorldObj != null)
        {
            Destroy(G.Instance.currentWorldObj);
            G.Instance.currentWorldObj = null;
            currentWorldInd++;
            G.Instance.currentLevel++;
        }
        GameObject world = _NextWorld();

        return world;

        GameObject _NextWorld()
        {
            if (worlds.Length <= currentWorldInd)
                return null;

            G.Instance.currentTime = worlds[currentWorldInd].GetComponent<WorldObject>().worldTime;

            GameObject world = Instantiate(worlds[currentWorldInd]);
            UIController.Instance.ShowWorldName(worlds[currentWorldInd].GetComponent<WorldObject>().worldName);

            return world;
        }
    }
}
