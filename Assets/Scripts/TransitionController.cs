using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{

    public struct LevelOutcome
    {
        public float score;
        public List<Recipe> successfullSales;
        public int unhappyCustomers;

        public LevelOutcome(float _score, int _unhappyCustomers, List<Recipe> _successfullSales)
        {
            unhappyCustomers = _unhappyCustomers;
            successfullSales = _successfullSales;
            score = _score;
        }
    }
    public class TransitionController : MonoBehaviour
    {
        public static TransitionController Instance;
        private Animator _animator;
        private bool _animating = false;
        private AsyncOperation _loadingOperation;
        public LvlInfo selectedLevel;
        public LevelOutcome lastLevelOutcome;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            _animator = GetComponent<Animator>();

        }
        
        IEnumerator WaitThenDoThings(float time, string sceneName)
        {
            yield return new WaitForSeconds(time);
            _loadingOperation = SceneManager.LoadSceneAsync(sceneName);
            // while (_loadingOperation.progress < 0.8f)
            // {
            //     
            // }

            StartCoroutine(WaitAndUnload());
        }

        IEnumerator WaitAndUnload()
        {
            yield return new WaitForSeconds(.2f);
            _animator.Play("leaveAnim");
            _animating = false;
        }

        public void GoToLvl(LvlInfo _lvlInfo)
        {
            if (!_animating)
            {
                selectedLevel = _lvlInfo;
                _animating = true;
                _animator.Play("enterAnim");
                StartCoroutine(WaitThenDoThings(2f, "PreLevel"));
            }
        }

        public void LoadLevel()
        {
            if (!_animating)
            {
                _animating = true;
                _animator.Play("enterAnim");
                StartCoroutine(WaitThenDoThings(2f, selectedLevel.sceneName));
            }
        }

        public void GoToScene(string sceneName)
        {
            if (!_animating)
            {
                _animating = true;
                _animator.Play("enterAnim");
                StartCoroutine(WaitThenDoThings(2f, sceneName));
            }
        }

        public void EndOfLevel(LevelOutcome levelOutcome)
        {
            lastLevelOutcome = levelOutcome;
            if (levelOutcome.score >= selectedLevel.goals[0])
            {
                // this will increment as its display no  so +1 than array index
                LevelDictionary.Instance.Unlock(selectedLevel.levelNo);
            }
            GoToScene("PostLevel");
        }
    }
}