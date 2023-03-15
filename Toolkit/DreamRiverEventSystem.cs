using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SKODE
{
    public static class DreamRiverEventSystem
    {
        /// <summary>
        /// 获得当前鼠标悬浮处UI
        /// </summary>
        public static Transform? GetOverUI()
        {
            Transform obj = null;

            EventSystem uiEventSystem = EventSystem.current;
            if (uiEventSystem != null)
            {
                PointerEventData uiPointerEventData = new PointerEventData(uiEventSystem);
                uiPointerEventData.position = Input.mousePosition;

                List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

                uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);
                if (uiRaycastResultCache.Count > 0)
                {
                    obj = uiRaycastResultCache[0].gameObject.transform;
                }
            }

            return obj;
        }

        /// <summary>
        /// 获得当前鼠标悬浮处所有UI
        /// </summary>
        public static List<Transform> GetAllOverUI(this Transform trans)
        {
            List<Transform> objs = new List<Transform>();

            EventSystem uiEventSystem = EventSystem.current;
            if (uiEventSystem != null)
            {
                PointerEventData uiPointerEventData = new PointerEventData(uiEventSystem);
                uiPointerEventData.position = Input.mousePosition;

                List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

                uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);
                for (int i = 0; i < uiRaycastResultCache.Count; i++)
                {
                    objs.Add(uiRaycastResultCache[i].gameObject.transform);
                }
            }

            return objs;
        }

        /// <summary>
        /// 获得当前鼠标点击的UI
        /// </summary>
        public static Transform? GetClickUI(this Transform trans)
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;

            return obj == null ? null : obj.transform;
        }
    }
}