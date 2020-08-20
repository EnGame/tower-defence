using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyProfile _enemyProfile;
    private int waypointIndex;

    [SerializeField] private int _health;
    private int _attack;
    private int _gold;
    private float _moveSpeed;
    private void Update()
    {
        Move();
    }

    public void InitEnemy(int wave)
    {
        _health = _enemyProfile.health + wave;
        _attack = _enemyProfile.attack;
        _gold = _enemyProfile.gold;
        _moveSpeed = _enemyProfile.moveSpeed + wave * 0.05f;
    }
    private void Move()
    {
        var targetPosition = GameManager.GetInstance().wayPoints[waypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

        var direction = targetPosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (waypointIndex < GameManager.GetInstance().wayPoints.Length - 1)
            {
                waypointIndex++;
            }
            else
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        GameManager.GetInstance().ReceiveDamage(_attack);
        GameManager.GetInstance().enemyList.Remove(this);
        Destroy(gameObject);
    }
    public void ReceiveDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Dead();
        }
    }
    private void Dead()
    {
        GameManager.GetInstance().enemyList.Remove(this);
        GameManager.GetInstance().AddGold(_gold);
        GameManager.GetInstance().enemyDestroyed++;
        Destroy(gameObject);
    }
}
