using UnityEngine;
using UnityEngine.EventSystems;

// 상속해서 사용하시면 됩니다.
// 마우스를 사용해 클릭, 드래그, 위에서 땔 때, 놓았을 때 실행
// eventData의 경우 메소드를 들여다보시면 어떤 오브젝트인지 알 수 있을 겁니당.
// 반드시 public이어야 사용가능 할 거에요.
// Raycast Target으로 사용하는 원리이기 때문에 위에 다른 거로 막혀있으면 사용 불가합니다.

public class MouseDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDropHandler
{
    public virtual void OnPointerDown(PointerEventData eventData) { return; }
    public virtual void OnDrag(PointerEventData eventData) { return; }
    public virtual void OnPointerUp(PointerEventData eventData) { return; }
    public virtual void OnDrop(PointerEventData eventData) { return; }
}