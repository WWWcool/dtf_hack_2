using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventBool : UnityEvent<bool>
{
}
[System.Serializable]
public class UnityEventInt : UnityEvent<int>
{
}

[System.Serializable]
public class UnityEventFloat : UnityEvent<float>
{
}

[System.Serializable]
public class UnityEventVector2 : UnityEvent<Vector2>
{
}

[System.Serializable]
public class UnityEventGameObject : UnityEvent<GameObject>
{
}

[System.Serializable]
public class UnityEventGameObjectVector2 : UnityEvent<GameObject, Vector2>
{
}
