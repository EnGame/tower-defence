using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerProfile towerProfile;
    public int level;
    
    [SerializeField] private Transform _turretHead;

    private float _fireTimer;
    private Enemy _target;
    private ParticleSystem shootEffect;

    #region Unity Methods
    private void Start()
    {
        if(towerProfile == null)
        {
            throw new Exception("Tower profile is empty");
        }
        _fireTimer = 0;
        shootEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        _fireTimer -= Time.deltaTime;

        if (_target == null)
            GetEnemy();
        else if (Vector3.Distance(transform.position, _target.transform.position) > towerProfile.levels[level].attackDistance)
            _target = null;

        if (_target == null) return;

        var direction = _target.transform.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _turretHead.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_fireTimer <= 0)
        {
            _fireTimer = towerProfile.levels[level].fireSpeed;
            if (_target != null) Shoot();
        }
    }

    //Click on turret
    private void OnMouseDown()
    {
        GameManager.GetInstance().Select(this);
        GameManager.GetInstance().gameUi.ShowUpgradePanel();
    }

    //Draw shoot area (only editor)
    private void OnDrawGizmosSelected()
    {
        if (towerProfile == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, towerProfile.levels[level].attackDistance);
    }

    #endregion
    
    //Get Nearest Enemy
    private void GetEnemy()
    {
        var minDistance = towerProfile.levels[level].attackDistance;
        foreach (var enemy in GameManager.GetInstance().enemyList)
        {
            var distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance < towerProfile.levels[level].attackDistance)
            {
                _target = enemy;
                minDistance = distance;
            }
        }
    }

    private void Shoot()
    {
        _target.ReceiveDamage(towerProfile.levels[level].attack);
        shootEffect.Play();
    }

    public bool CanUpgrade()
    {
        var levelsCount = towerProfile.levels.Count - 1;
        return level < levelsCount && towerProfile.levels[level + 1].price <= GameManager.GetInstance().GetGoldCount();
    }
}
