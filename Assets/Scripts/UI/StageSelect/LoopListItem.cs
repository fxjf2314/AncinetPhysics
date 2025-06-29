using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoopListItem : MonoBehaviour {

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject lockMask;

    [SerializeField]
    private Image lockIcon;

    [SerializeField]
    private TMP_Text labelText;
    
    private Data currentData;

    public Data CurrentData => currentData;

    public void SetData(Data data) {
        currentData = data;
        backgroundImage.sprite = data.image;
        nameText.text = data.name;
        color = data.color;

        bool isUnlocked = LockManager.Instance.IsLevelUnlocked(data);
        lockMask.SetActive(!isUnlocked);

        if(!isUnlocked)
        {
            labelText.text = $"需要进度为 {data.requiredStars} 时解锁";
        }
    }

    public int index;

    public void ShiftRight(float offset, float duration = 0.2f) {
        StartCoroutine(ShiftCoroutine(1, duration, offset));
    }
    
    public void ShiftLeft(float offset, float duration = 0.2f) {
        StartCoroutine(ShiftCoroutine(-1, duration, offset));
    }

    private IEnumerator ShiftCoroutine(int direction, float duration, float offset) {
        direction /= Mathf.Abs(direction);

        index += direction;

        Vector3 newPos = new(index * offset, 0, 0);

        float currentTime = 0;
        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, currentTime / duration);
            yield return null;
        }
        transform.localPosition = newPos;
    }

}
