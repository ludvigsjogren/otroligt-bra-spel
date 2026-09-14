using UnityEngine;
using TMPro;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private int goldcoinsToCollect = 10;
    [SerializeField] private TMP_Text goldcoinText;
    [SerializeField] private AudioClip pickupSoundEffect;
    private int goldcoins = 0;
    private AudioSource audioSource;

    private void Start()
    {
        goldcoinText.text = "" + goldcoins;
        audioSource = GetComponent<AudioSource>();
    }

    public void AddGoldcoin()
    {
        goldcoins++;
        goldcoinText.text = "" + goldcoins;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(pickupSoundEffect);
    }

    public int GetGoldcoins() { return goldcoins;}
    public int GetGoldcoinsToCollect() { return goldcoinsToCollect;}
}
