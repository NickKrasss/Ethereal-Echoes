using NavMeshPlus.Components;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] worlds;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private MinimapScr minimapScr;

    [SerializeField] private NavMeshSurface[] surfaces;

    [SerializeField] private Image skullImage;

    private float targetAlpha = 0;

    private int currentWorldInd = 0;

    private bool hunt = false;


    private void Start()
    {
        StartCoroutine(GameCycle());
        StartCoroutine(HuntCycle());
    }

    private void Update()
    {
        skullImage.color = new Color(skullImage.color.r, skullImage.color.g, skullImage.color.b, Mathf.Lerp(skullImage.color.a, targetAlpha, Time.deltaTime*3));
        if (G.Instance.currentTime < 30)
        {
            targetAlpha = G.Instance.currentTime%2;
            hunt = true;
        }
        else
        {
            targetAlpha = 0;
            hunt = false;
        }
    }

    private IEnumerator HuntCycle()
    {
        if (hunt)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 7)
            {
                int ind = Random.Range(0, enemies.Length - 7);
                for (int i = 0; i < 4; i++)
                    enemies[ind+i].GetComponent<EnemyAI>().Spot();
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(HuntCycle());
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
            G.Instance.playerObj.GetComponent<Stats>().CurrentEnergy = G.Instance.playerObj.GetComponent<Stats>().MaxEnergy;
            yield return new WaitForEndOfFrame();
            GenerateNavmesh();

            G.Instance.playerObj.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2, G.Instance.playerObj.transform.position.z);
            G.Instance.playerObj.GetComponent<ShowLevel>().MakeText();

            Camera.main.transform.position = new Vector3(G.Instance.currentWorld.Width / 2, G.Instance.currentWorld.Height / 2 - 5, Camera.main.transform.position.z);
            minimapScr.GenerateTexture();
            AudioManager.Instance.EndAllSounds();
            
            yield return new WaitForSeconds(1f);
            G.Instance.isWorldLoading = false;
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
