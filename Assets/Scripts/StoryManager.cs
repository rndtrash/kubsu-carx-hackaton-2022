using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public Image StoryImage;
    public TextMeshProUGUI Speaker;
    public TextMeshProUGUI Message;

    public Sprite[] Backgrounds;

    public GameObject Officer;

    public float Delay = 5.0f;

    public GameObject watch;
    
    public struct StoryPart
    {
        public string Speaker;
        public string Message;
        public int TextureIndex;
        
        public delegate void ShownDelegate();

        public ShownDelegate OnShow;
    }

    private StoryPart[] _story;
    private int _storyProgress = 0;
    
    private DateTime _nextFrameDelay = DateTime.UtcNow;
    
    // Start is called before the first frame update
    void Start()
    {
        _story = new StoryPart[]
        {
            new()
            {
                Speaker = "Борис",
                Message = "Полиция, откройте!",
                TextureIndex = -1, // TODO!!!!!!!! FIXME!!!!!!!!!
                OnShow = Knocking
            },
            
            new()
            {
                Speaker = "",
                Message = "Он пытается открыть дверь. Дверь была не заперта. Борис вошёл в дом как можно скорее.",
                TextureIndex = -1,
                OnShow = null
            },
            
            new()
            {
                Speaker = "Борис",
                Message = "Здесь кто-нибудь есть?",
                TextureIndex = 0,
                OnShow = MovePlayer
            },
            
            new()
            {
                Speaker = "",
                Message = "Ответа не последовало.",
                TextureIndex = -1,
                OnShow = null
            },
            
            new()
            {
                Speaker = "",
                Message = "Он обратил внимание на приоткрытую дверь слева. Из неё тянуло холодом.",
                TextureIndex = -1,
                OnShow = TurnPlayer
            },
            
            new()
            {
                Speaker = "",
                Message = "Борис аккуратно открыл дверь. Она раскрылась с громким скрипом.",
                TextureIndex = -1,
                OnShow = null
            },
            
            new()
            {
                Speaker = "",
                Message = "Войдя в комнату, трудно было не заметить выбитое окно.",
                TextureIndex = 1,
                OnShow = Squeak
            },
            new()
            {
                Speaker = "",
                Message = "Подробно изучив комнату, Борис решается осмотреть окно.",
                TextureIndex = -1,
                OnShow = MovePlayerWindow
            },
            new()
            {
                Speaker = "",
                Message = "На полу лежали часы. Подозрительно знакомые Борису часы.",
                TextureIndex = -1,
                OnShow = SpawnWatch
            }
        };

        NextStoryFrame();
    }

    public void OnClickNextFrame()
    {
        if ((DateTime.UtcNow - _nextFrameDelay).TotalSeconds < Delay)
            return;
        
        NextStoryFrame();
    }
    
    private void NextStoryFrame()
    {
        if (_storyProgress >= _story.Length)
        {
            SceneManager.LoadScene("Corkboard");
            return;
        }

        var storyFrame = _story[_storyProgress++];

        Speaker.text = storyFrame.Speaker;

        Message.text = storyFrame.Speaker.Length == 0 ? storyFrame.Message : $"\"{storyFrame.Message}\"";

        if (storyFrame.TextureIndex != -1)
            StoryImage.sprite = Backgrounds[storyFrame.TextureIndex];

        storyFrame.OnShow?.Invoke();
    }

    private void Knocking()
    {
        GameObject.Find("knocking").GetComponent<AudioSource>().Play();
    }
    
    private void MovePlayer()
    {
        Officer.SetActive(true);
    }

    private void TurnPlayer()
    {
        Officer.GetComponent<ConstantMove>().TargetZ = -45;
    }

    private void Squeak()
    {
        GameObject.Find("door_squeak").GetComponent<AudioSource>().Play();
        var c = Officer.GetComponent<ConstantMove>();
        c.TargetZ = 0;
        c.Follow = null;
        Officer.GetComponent<RectTransform>().anchoredPosition =
            GameObject.Find("officerTargetRoom").GetComponent<RectTransform>().anchoredPosition;
    }

    private void MovePlayerWindow()
    { 
        Officer.GetComponent<ConstantMove>().SetFollow(GameObject.Find("officerTargetWindow"));
    }

    private void SpawnWatch()
    {
        PoliceManager.FoundWristwatch = true;
        watch.SetActive(true);
    }
}
