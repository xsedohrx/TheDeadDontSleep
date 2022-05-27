public class AutoDestroyPoolableObject : PoolableObject
{
    public float autoDestroyTime = 5f;
    private const string disableMethodName = "Disable";

    public virtual void OnEnable() {
        CancelInvoke(disableMethodName);
        Invoke(disableMethodName, autoDestroyTime);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}
