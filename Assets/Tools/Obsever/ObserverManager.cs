using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern.Obsever
{
    public class ObserverManager<T> : MonoBehaviour where T:Enum
    {
        private static ObserverManager<T> _instance;

        public static ObserverManager<T> Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<ObserverManager<T>>(true);
                    if (!_instance)
                    {
                        GameObject newGameObject = new GameObject("EventDispatcher_ "+typeof(T).Name);
                        _instance = newGameObject.AddComponent<ObserverManager<T>>();
                    }
                }

                return _instance;
            }
        }

        private Dictionary<T, Action<object>> _events = new Dictionary<T, Action<object>>();

        //Singleton
        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }
    
        //Đăng kí lắng nghe sự kiện
        public void RegisterEvent(T eventID,Action<object> callback)
        {
            if (callback == null)
            {
                Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
                return;
            }

            if (eventID == null)
            {
                Debug.LogWarning($"EventID is NULL. Cannot register event.");
                return;
            }
            if (!_events.TryAdd(eventID, callback))
            {
                _events[eventID] += callback;
            }
        }
        // không đăng kí lắng nghe nữa
        public void RemoveEvent(T eventID,Action<object> callback)
        {
            if (_events.ContainsKey(eventID))
            {
                _events[eventID] -= callback;
                if (_events[eventID] == null)
                {
                    _events.Remove(eventID);
                }
            }
            else
            {
                Debug.LogWarning($"Event '{eventID}' not found in EventDispatcher_ "+typeof(T).Name);
            }
        }

        public void RemoveAllEvent()
        {
            _events.Clear();
        }
        // Đăng sự kiện lên 
        public void PostEvent(T eventID, object param = null)
        {
            if (!_events.ContainsKey(eventID))
            {
                Debug.LogWarning($"Event:{eventID.GetType().Name} has no Listener EventDispatcher_ "+typeof(T).Name);
                return;
            }

            if (_events[eventID] == null)
            {
                Debug.LogWarning($"Callback for event {eventID.GetType().Name} is NULL.");
                _events.Remove(eventID);
                return;
            }
            _events[eventID]?.Invoke(param);
        }
    }
}
