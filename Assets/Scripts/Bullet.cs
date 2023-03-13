using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float Damage;
    public string key;
    
    public void StartTimer()
    {
        Debug.Log(key);
        Pooler.instance.DelayedDepop(2, key, this.gameObject);
    }
    
    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pooler.instance.Depop(key, gameObject);
    }
}
