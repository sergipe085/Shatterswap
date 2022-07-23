using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : Singleton<SceneTransition>
{
    [SerializeField] private Transform upImage = null;
    [SerializeField] private Transform downImage = null;

    private float initialYUp = 0.0f;
    private float initialYDown = 0.0f;

    protected override void Awake() {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        DOTween.Init();
    }

    private void Start() {
        initialYUp = upImage.position.y;
        initialYDown = downImage.position.y;

        Open();
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(LoadSceneEnumerator(sceneName));
    }

    private IEnumerator LoadSceneEnumerator(string sceneName) {
        Close();

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.5f);

        Open();
    }

    private void Open() {
        upImage.DOLocalMoveY(initialYUp + 5f, 1.0f).SetEase(Ease.OutCirc);
        downImage.DOLocalMoveY(initialYDown - 5f, 1.0f).SetEase(Ease.OutCirc);
    }

    private void Close() {
        upImage.DOLocalMoveY(0, 1.0f).SetEase(Ease.OutCirc);
        downImage.DOLocalMoveY(0, 1.0f).SetEase(Ease.OutCirc);
    }
}
