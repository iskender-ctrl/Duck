using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDraggable", menuName = "Custom/Create Draggable")]
public class Draggable : ScriptableObject
{
    public static Sprite dragImageToCompare;
    public GameObject dragObjectPrefabToCompare;
    [System.Serializable]
    public struct DraggableCollection
    {
        public string name;
        public GameObject dragObjectPrefab;
        public Skill.SkillType skillType;
        public Sprite dragImage;
    }

    public DraggableCollection[] draggables;
    private int currentObjectIndex = 0;

    public DraggableCollection GetCurrentDraggable()
    {
        if (draggables != null && draggables.Length > 0)
        {
            return draggables[currentObjectIndex];
        }
        return new DraggableCollection();
    }
    public void MoveToNextObject()
    {
        if (draggables != null && draggables.Length > 0)
        {
            // İndeksi bir arttır (sıradaki objeye geç)
            currentObjectIndex = (currentObjectIndex + 1) % draggables.Length;
        }
        else
        {
            Debug.LogError("No draggable objects defined.");
        }
    }
    public void OnDragged(GameObject buttonObject)
    {
        DraggableCollection currentDraggable = GetCurrentDraggable();

        // Null kontrolü
        if (currentDraggable.dragObjectPrefab != null)
        {
            DraggableUI draggableUI = FindObjectOfType<DraggableUI>();

            // Eğer bulunursa ve SetUI fonksiyonu varsa, çağır
            if (draggableUI != null)
            {
                draggableUI.SetUI(currentDraggable.dragImage);
            }
            else
            {
                Debug.LogError("DraggableUI not found.");
            }

            Image spawnedImage = buttonObject.GetComponent<Image>();
            if (spawnedImage != null)
            {
                spawnedImage.sprite = currentDraggable.dragImage;
            }
            else
            {
                Debug.LogError("Image component not found on spawned object.");
            }

            MoveToNextObject();
        }
        else
        {
            Debug.LogError("Draggable prefab is null.");
        }
    }

}