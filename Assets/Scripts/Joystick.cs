using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _interactiveRect;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;

    private Camera _camera;
    private Vector2 _input = Vector2.zero;
    public Vector2 Direction => new Vector2(_input.x, _input.y);

    private void Start()
    {
        _camera = null;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            _camera = _canvas.worldCamera;

        var center = new Vector2(0.5f, 0.5f);
        _background.pivot = center;
        _handle.anchorMin = center;
        _handle.anchorMax = center;
        _handle.pivot = center;
        _handle.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.gameObject.SetActive(true);
        _background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _background.gameObject.SetActive(false);
        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var joystickPosition = RectTransformUtility.WorldToScreenPoint(_camera, _background.position);
        var joystickRadius = _background.sizeDelta / 2;
        _input = (eventData.position - joystickPosition) / (joystickRadius * _canvas.scaleFactor);
        HandleInput(_input.magnitude, _input.normalized, joystickRadius);
    }

    private void HandleInput(float magnitude, Vector2 normalised, Vector2 radius)
    {
        if (magnitude > 0)
        {
            if (magnitude > 1)
                _input = normalised;
        }
        else
        {
            _input = Vector2.zero;
        }
       

        _handle.anchoredPosition = _input * radius;
    }


    private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
    {
        var localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_interactiveRect, screenPosition, _camera,
            out localPoint))
        {
            var pivotOffset = _interactiveRect.pivot * _interactiveRect.sizeDelta;
            return localPoint - (_background.anchorMax * _interactiveRect.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }
}