using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private Camera _camera;
    private Collider2D _collider;
    private SpriteRenderer[] _spriteRenderers;
    public string nextTutorial = "";

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<Collider2D>();
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
    }

    private bool isMouseOnCollider()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        return _collider.OverlapPoint(mousePosition);

    }
    // Update is called once per frame
    void Update()
    {
        if (isMouseOnCollider())
        {
            foreach (var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = Color.white;
            }

            if (Input.GetMouseButtonUp(0))
            {
                TransitionController.Instance.GoToScene(nextTutorial);
            }
        }
        else
        {
            foreach (var spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = Color.white *.8f;
            }  
        }
    }
}
