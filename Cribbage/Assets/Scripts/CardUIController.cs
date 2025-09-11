using UnityEngine;
using UnityEngine.UI;

public class CardUIController : MonoBehaviour
{
    public Image cardImage;
    public string number;
    public string suitNumber;

    public void SetCardArt(Sprite artwork)
    {
        cardImage.sprite = artwork;
    }
    public void SetCardNumberAndSuit( string suitNumber, string cardNumber)
    {
        this.number = cardNumber;
        this.suitNumber = suitNumber;
    }
}