using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class CannonSmokeEffect : MonoBehaviour
{
    [SerializeField] private Image imageHolder;
    [SerializeField] private float animDuration = .8f; 
    
    private List<Sprite> _sprites;


    // Start is called before the first frame update
    void Start()
    {
        _sprites = GameData.Instance.GameAssets.cannonMuzzleSmokeEffect;
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        var delay = animDuration / _sprites.Count;
            
        //destroy animation
        foreach (var img in _sprites)
        {
            imageHolder.sprite = img;
            yield return new WaitForSeconds(delay);
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
