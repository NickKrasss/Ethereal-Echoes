using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
public class GearContainer : MonoBehaviour
{
    public int current_gears;
    public int max_gears;

    TMP_Text textMeshPro;  
    public void AddGears(int gears)
    {
        if (current_gears  < max_gears)
        {
            current_gears += gears;
            if (current_gears > max_gears)
            {
                current_gears = max_gears;
            }
        }
    }

    private void Update()
    {
        if (!textMeshPro)
        {
            textMeshPro = GameObject.FindGameObjectWithTag("Gears").GetComponent<TMP_Text>();
            return;
        }
        textMeshPro.text = $"{current_gears} / {max_gears}";
    }


}
