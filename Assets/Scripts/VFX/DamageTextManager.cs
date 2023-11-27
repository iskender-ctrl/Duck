using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageTextManager : MonoBehaviour
{
    public float destroyTimer;
    public TextMeshProUGUI prefab;

    public float maxPositionRange = 5;
    
    public float minRotation = -15;
    public float maxRotation = 15;

    public Transform canvas;

    public void SpawnText(Bounds hitObject,float damage)
    {
        var spawnPoint = hitObject.center + Random.value * new Vector3(Random.value*maxPositionRange,Random.value*maxPositionRange);
        var rotation = Quaternion.AngleAxis(Random.Range(minRotation,maxRotation), Vector3.forward);

        var instance = Instantiate(prefab, spawnPoint,rotation,canvas);
        
        
        instance.text = damage.ToString();
        instance.color = Color.Lerp(Color.white, Color.red, damage / 32f);
        Destroy(instance.gameObject,destroyTimer);
    }
}
