using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Tower : MonoBehaviour
{
    public float ReloadTime = 1f;
    public float Range = 3f;
    public Projectile Projectile;
    public GameObject TowerRange;

    private bool selected = false;
    private float timer = 0f;

    private void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && EnemyInRange())
        {
            Shoot();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = ReloadTime;
    }

    protected virtual bool EnemyInRange()
    {
        var col = Physics2D.OverlapCircle(transform.position, Range, LayerMask.GetMask("Enemy"));
        return col != null;
    }

    protected virtual Enemy FirstEnemyInRange()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("Enemy")).Select(x => x.GetComponent<Enemy>()).ToArray();
        if (enemies == null || enemies.Length == 0)
            return null;
        return enemies.Maximum(new EnemyTotalProgressComparer());
    }

    protected abstract void Shoot();

    private void OnMouseDown()
    {
        Select();
    }

    void Select()
    {
        selected = true;
        TowerRange.SetActive(true);
        GameControllerS.I.EventManager.PointerDown += EventManager_PointerDown;
    }

    private void EventManager_PointerDown(object sender, PointerEventArgs args)
    {
        Deselect();
    }

    void Deselect()
    {
        selected = false;
        TowerRange.SetActive(false);
        GameControllerS.I.EventManager.PointerDown -= EventManager_PointerDown;
    }
}
