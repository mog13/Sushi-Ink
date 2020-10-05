using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    private Camera _camera;
    private Collider2D _collider;
    public int levelNumber = 0;
    private LvlInfo _lvlInfo;

    private SpriteRenderer _sr;
    private Vector2 hoverPos,origPos;
    private float hover = 0;
    private AudioSource _audio;
    public bool isTutorial = false;
    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<Collider2D>();
        _lvlInfo = LevelDictionary.Instance.levels[levelNumber];
        _sr = GetComponentInChildren<SpriteRenderer>();
        origPos = _sr.transform.position;
        hoverPos = origPos - Vector2.down / 2f;
        _audio = GetComponent<AudioSource>();
       
        if (!_lvlInfo.unlocked)
        {
            _sr.color *= .6f;
            foreach (var text in  GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.color = Color.black *.6f;
            }

           
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (_lvlInfo.unlocked && _collider.OverlapPoint(mousePosition))
        {
            hover += .03f;
            if (Input.GetMouseButtonUp(0))
            {
                _audio.Play();
                if(!isTutorial) TransitionController.Instance.GoToLvl(_lvlInfo);
                else TransitionController.Instance.GoToScene("Tutorial1");
            }
        }
        else
        {
            hover -= .05f;
        }

        hover = Mathf.Clamp(hover, 0, 1);
        _sr.transform.position = Vector2.Lerp(origPos, hoverPos, hover);

    }
}
