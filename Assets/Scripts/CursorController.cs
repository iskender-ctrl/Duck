using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    //
    //
    //TODO this one is not well implemented make it better
    //
    //
    [SerializeField] private Sprite target;
    [SerializeField] private Sprite wrong;
    [SerializeField] private Image img;

    public void SetWrong()
    {
        img.sprite = wrong;
    }

    public void SetTrue()
    {
        img.sprite = target;
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        img.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DOTween.Complete(this);
            img.transform.DOScale(.6f, .5f);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            DOTween.Complete(this);
            img.transform.DOScale(1f, .5f);
        }
    }
}
