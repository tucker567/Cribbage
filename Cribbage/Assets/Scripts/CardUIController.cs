using UnityEngine;
using UnityEngine.UI;

public class CardUIController : MonoBehaviour
{
    public Image cardImage;

    public void SetCard(Sprite artwork)
    {
        cardImage.sprite = artwork;
    }
}