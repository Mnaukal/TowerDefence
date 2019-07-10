using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Base abstract class for all towers
/// </summary>
public abstract class Tower : MonoBehaviour
{
    /// <summary>
    /// Seconds between shots
    /// </summary>
    [SerializeField] 
    protected float ReloadTime = 1f;
    /// <summary>
    /// Maximum distance to aim for an enemy
    /// </summary>
    [SerializeField] 
    protected float Range = 3f;
    /// <summary>
    /// Damage given by each projectile
    /// </summary>
    [SerializeField] 
    protected int Damage = 1;
    /// <summary>
    /// Projectile Prefab to be shot
    /// </summary>
    [SerializeField] 
    protected Projectile Projectile;
    /// <summary>
    /// Controls the speed of projectile
    /// </summary>
    [SerializeField]
    protected float fireForce = 0.05f;
    /// <summary>
    /// GameObject to display the range of Tower to player
    /// </summary>
    [SerializeField]
    private GameObject TowerRange;

    private bool selected = false;
    private float timer = 0f;

    #region Events
    /// <summary>
    /// Called when tower shoots
    /// </summary>
    public event TowerShotEventHandler TowerShot;

    protected void RaiseTowerShot(Projectile projectile)
    {
        if (TowerShot != null)
            TowerShot(this, new TowerShotEventArgs(this, projectile));
    }
    #endregion

    private void Start()
    {
        TowerRange.transform.localScale = new Vector3(2 * Range, 2 * Range, 1);
        ResetTimer();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && EnemyInRange())
        {
            Shoot();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = ReloadTime;
    }

    /// <summary>
    /// Checks whether there is an enemy in range of tower
    /// </summary>
    /// <returns>true if tower can shoot on an enemy in range</returns>
    protected virtual bool EnemyInRange()
    {
        var col = Physics2D.OverlapCircle(transform.position, Range, LayerMask.GetMask("Enemy"));
        return col != null;
    }

    /// <summary>
    /// Finds the first (the one with highest progress along the path) enemy in range 
    /// </summary>
    /// <returns>Enemy to shoot at; null if no enemy is in range</returns>
    protected virtual Enemy FirstEnemyInRange()
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, Range, LayerMask.GetMask("Enemy")).Select(x => x.GetComponent<Enemy>()).ToArray();
        if (enemies == null || enemies.Length == 0)
            return null;
        return enemies.Maximum(new EnemyTotalProgressComparer());
    }

    /// <summary>
    /// Shooting to be impelemented by derived classes
    /// </summary>
    protected abstract void Shoot();

    private void OnMouseDown()
    {
        Select();
    }

    /// <summary>
    /// Display the range in game and TODO open menu for upgrades of this enemy
    /// </summary>
    private void Select()
    {
        DeselectAllTowers();
        selected = true;
        TowerRange.SetActive(true);
        // deselect when clicking elsewhere
        GameControllerS.I.EventManager.PointerDown += EventManager_PointerDown;
    }

    private void DeselectAllTowers()
    {
        foreach (Tower t in GameControllerS.I.TowersParent.GetComponentsInChildren<Tower>())
            t.Deselect();
    }

    private void EventManager_PointerDown(object sender, PointerEventArgs args)
    {
        Deselect();
    }

    public void Deselect()
    {
        selected = false;
        TowerRange.SetActive(false);
        GameControllerS.I.EventManager.PointerDown -= EventManager_PointerDown;
    }

    public float GetReloadTime() => ReloadTime;
    public float GetRange() => Range;
    public int GetDamage() => Damage;
}
