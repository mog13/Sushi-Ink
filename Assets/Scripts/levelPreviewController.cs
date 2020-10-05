
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class levelPreviewController : MonoBehaviour
{
    private LvlInfo _lvlInfo;
    public GameObject recipePreviewPrefab;
    void Start()
    {
        _lvlInfo = TransitionController.Instance.selectedLevel;
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = _lvlInfo.levelName;
        texts[1].text = _lvlInfo.timeToComplete.ToString() + " - seconds";

        HorizontalLayoutGroup container = GetComponentInChildren<HorizontalLayoutGroup>();
        foreach (Recipe recipe in _lvlInfo.Recipes)
        {
           var image = Instantiate(recipePreviewPrefab, container.transform);
           image.GetComponent<Image>().sprite = recipe.outPut;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TransitionController.Instance.LoadLevel();
        }
    }
}
