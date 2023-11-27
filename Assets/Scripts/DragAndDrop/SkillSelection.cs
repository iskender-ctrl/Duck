using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillSelection : MonoBehaviour
{
    private bool isButtonClicked = false;
    public Transform spawnAreaTransform;
    private Button lastClickedButton;
    private bool isDragging = false;
    private Vector3 initialMousePos;
    private GameObject spawnedSkill;
    private bool isLongPress = false;
    private float pressStartTime;
    public float longPressDuration = 1.0f;
    int mouseUpCount;
    [System.Serializable]
    public struct TypeSpawnPoint
    {
        public Skill.SkillType selectedSkillType;
        public Transform[] spawnTransform;
    }
    public TypeSpawnPoint[] SpawnPointWithType;
    GameObject prefab;
    private Dictionary<Skill.SkillType, Transform[]> spawnPointsDictionary = new Dictionary<Skill.SkillType, Transform[]>();
    Draggable.DraggableCollection draggableCollection;
    private void Awake()
    {
        foreach (var typeSpawnPoint in SpawnPointWithType)
            spawnPointsDictionary[typeSpawnPoint.selectedSkillType] = typeSpawnPoint.spawnTransform;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnMouseClick();
        else if (Input.GetMouseButton(0)) OnMouseHold();
        else if (Input.GetMouseButtonUp(0)) OnMouseUp();

        if (isDragging) OnDrag();

        if (mouseUpCount >= 2) NullTheChooseObject();
    }
    private void OnMouseClick()
    {
        isButtonClicked = true;
        pressStartTime = Time.time;
        isLongPress = false;
        initialMousePos = Input.mousePosition;
    }
    private void OnMouseHold()
    {
        if (isButtonClicked && !isDragging && Time.time - pressStartTime >= longPressDuration) { isLongPress = true; }
        if (isButtonClicked && !isLongPress && Vector3.Distance(initialMousePos, Input.mousePosition) > 10f) { isDragging = true; OnDragStart(); }
    }
    private void OnMouseUp()
    {
        isButtonClicked = false;
        mouseUpCount++;

        if (isDragging) OnDragEnd();
        else if (Time.time - pressStartTime < longPressDuration) OnClick();

        isDragging = false;
        isLongPress = false;
    }
    private void OnDragStart()
    {
        if (!isLongPress && spawnedSkill == null)
        {
            if (spawnedSkill != null) Destroy(spawnedSkill);
            spawnedSkill = Instantiate(prefab, Input.mousePosition, Quaternion.identity);
        }
    }
    private void OnDrag()
    {
        if (isDragging)
        {
            Debug.Log("Dragging");
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = spawnAreaTransform.position.z - Camera.main.transform.position.z;
            spawnedSkill.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
    private void OnDragEnd()
    {
        if (IsMouseOverSpawnArea())
        {
            if (spawnedSkill != null)
            {
                Destroy(spawnedSkill);
                SpawnSkillPrefab(draggableCollection.skillType);
            }
        }
        else
        {
            Destroy(spawnedSkill);
            NullTheChooseObject();
        }
    }
    private void OnClick()
    {
        if (IsMouseOverSpawnArea()) SpawnSkillPrefab(draggableCollection.skillType);
    }
    void NullTheChooseObject()
    {
        prefab = null;
        spawnedSkill = null;
    }
    public void OnButtonClick(GameObject obj)
    {
        Image image = obj.GetComponent<Image>();
        if (image != null)
        {
            mouseUpCount = 0;
            Sprite sprite = image.sprite;
            isButtonClicked = true;
            Draggable draggable = Resources.Load<Draggable>("Skills");

            if (draggable == null)
            {
                return;
            }

            draggableCollection = Array.Find(draggable.draggables, x => x.name == sprite.name);
            prefab = draggableCollection.dragObjectPrefab;

            if (!draggableCollection.Equals(default(Draggable.DraggableCollection)))
                lastClickedButton = obj.GetComponent<Button>();
            else
            {
                return;
            }
        }
    }
    private bool IsMouseOverSpawnArea()
    {
        RectTransform spawnAreaRectTransform = spawnAreaTransform.GetComponent<RectTransform>();
        if (spawnAreaRectTransform != null)
        {
            Vector2 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnAreaRectTransform, mousePosition, null, out Vector2 localPoint);
            return spawnAreaRectTransform.rect.Contains(localPoint);
        }
        return false;
    }
    private void SpawnSkillPrefab(Skill.SkillType skillType)
    {
        TypeSpawnPoint spawnPoint = FindSpawnPointBySkillType(skillType);
        if (spawnPoint.spawnTransform != null && spawnPoint.spawnTransform.Length > 0 && prefab != null)
        {
            foreach (Transform spawnTransform in spawnPoint.spawnTransform)
            {
                GameObject spawnedObject = Instantiate(prefab, spawnTransform.position, Quaternion.identity);
                Image clickedButtonImage = lastClickedButton.GetComponent<Image>();
                if (clickedButtonImage != null)
                {
                    Draggable.DraggableCollection randomCollection = GetRandomDraggableCollection();
                    clickedButtonImage.sprite = randomCollection.dragImage;
                }
            }
            NullTheChooseObject();
        }
    }
    private Draggable.DraggableCollection GetRandomDraggableCollection()
    {
        Draggable draggable = Resources.Load<Draggable>("Skills");

        if (draggable != null && draggable.draggables != null && draggable.draggables.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, draggable.draggables.Length);
            return draggable.draggables[randomIndex];
        }
        return default(Draggable.DraggableCollection);
    }
    private TypeSpawnPoint FindSpawnPointBySkillType(Skill.SkillType skillType)
    {
        TypeSpawnPoint spawnPoint = Array.Find(SpawnPointWithType, x => x.selectedSkillType == skillType);
        return spawnPoint;
    }
}