using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GridObjectVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemImage = null;

    public Item item = null;
    public GridObject gridObject = null;
    private GridSystem gridSystem = null;

    private void Start() {
        DOTween.Init();
    }

    public void Initialize(GridObject _gridObject) {
        this.gridObject = _gridObject;
        this.gridSystem = gridObject.GetGridSystem();

        gridObject.OnGridObjectChange += ClearImage;
        gridObject.OnGridStopChange += UpdateVisual;
        gridObject.OnDestroyed += GridObject_OnDestroyed;
        gridObject.OnSelectEvent += GridObject_OnSelect;

        UpdateVisual();
    }

    private void UpdateVisual() {
        item = gridObject.GetItem();

        if (item) itemImage.sprite = item.image;
        else itemImage.sprite = null;
    }

    private void ClearImage() {
        itemImage.sprite = null;
    }

    public void ChangeImage(Item _item) {
        if (_item) itemImage.sprite = _item.image;
        else itemImage.sprite = null;
    }

    private void GridObject_OnDestroyed() {
        SpawnExplosion();
    }

    private void GridObject_OnSelect() {
        transform.DOPunchScale(new Vector3(0.2f, 0.2f), 1.0f);
    }

    private void SpawnExplosion() {
        Instantiate(item.explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    public void MoveTo(Vector2 position, Action OnComplete) {
        transform.DOMove(position, 1.0f).SetEase(Ease.OutCubic).OnComplete(() => OnComplete?.Invoke());
    }
}
