using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBigBoss : MonoBehaviour
{ 
    private bool _bigBoss = true;
    enum StepFight { First, Second, Three, Fourth }

    private StepFight _stepFight;
    private NavMeshAgent _agent;
    

    private Vector3 _originPosition;
    private bool _isPatrol = true;

    [SerializeField] private PlayerGame _playerGame;
    private float _playerDistance;

    public GameObject BulletPrefab;
    public Transform _spawnBullet;
    private float _health;
    private float _maxHealth;

    public float ShotPower;
    public float SpeedMove;
   
    
    private void Start()
    {
        _health = _maxHealth = 1000f;
        _originPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        
        transform.LookAt(Vector3.back);
        _stepFight = StepFight.First;

        StartCoroutine(PatrolEnemy());
        StartCoroutine(Shot());
       
    }
    private void Update()
    {
        if (_playerGame != null)
        {
            _playerDistance = Vector3.Distance(_playerGame.transform.position, transform.position);
            Debug.Log(_playerDistance);
            MoveEnemy();
        }      
        SelectStep();
        SelectStepFight();
    }

    #region MoveEnemyPatrol
    private void MoveEnemy()
    {
        if (_playerDistance < 25f && _playerGame != null) _isPatrol = false;
        else _isPatrol = true;
      
    }
    private IEnumerator PatrolEnemy()
    {
        while (true)
        {
            if (_isPatrol && _bigBoss)
            {
                var random = Random.insideUnitCircle * 10f;
                var targetpatrol = _originPosition + new Vector3(random.x, 0f, random.y);
                _agent.SetDestination(targetpatrol);
                yield return new WaitForSeconds(3f);
            }
            else if(!_isPatrol)
            {
                
                _agent.SetDestination(_playerGame.transform.position);
                _agent.transform.LookAt(_playerGame.transform.position);
            }
            yield return null;
        }
    }
    #endregion

    #region SpawnEnemyMinBoss
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("45ACP"))
        {
            _playerGame = FindObjectOfType<PlayerGame>();           
            StartCoroutine(SpawnEnemyMinBoss());          
        }
    }
    private IEnumerator SpawnEnemyMinBoss()
    {
        if (_bigBoss == false) yield break;

        yield return new WaitForSeconds(2f);

        var enemyMin1 = Instantiate(gameObject, _originPosition + new Vector3(-6f, -1f, -6f), Quaternion.identity);
        enemyMin1.transform.localScale = gameObject.transform.localScale / 2;
        enemyMin1.GetComponent<EnemyBigBoss>()._bigBoss = false;
        enemyMin1.GetComponent<EnemyBigBoss>()._isPatrol = false;
        enemyMin1.GetComponent<EnemyBigBoss>()._health = _maxHealth / 2;
        
        
        var enemyMin2 = Instantiate(gameObject, _originPosition + new Vector3(6f, -1f, -6f), Quaternion.identity);
        enemyMin2.transform.localScale = gameObject.transform.localScale / 2;
        enemyMin2.GetComponent<EnemyBigBoss>()._bigBoss = false;
        enemyMin2.GetComponent<EnemyBigBoss>()._isPatrol = false;
        enemyMin2.GetComponent<EnemyBigBoss>()._health = _maxHealth / 2;

        Destroy(enemyMin1, 4f);
        Destroy(enemyMin2, 4f);
        
    }


    #endregion

    #region StepFight

    public void TakeDamage(float damage)
    {
        if (_health <= 0f) Destroy(gameObject);
        _health -= damage;
    }
    private void SelectStep()
    {
        if (_health < (_maxHealth / 100 * 75) && _health > (_maxHealth / 100 * 50)) _stepFight = StepFight.Second;
        if (_health < (_maxHealth / 100 * 50) && _health > (_maxHealth / 100 * 25)) _stepFight = StepFight.Three;
        if (_health < (_maxHealth / 100 * 25)) _stepFight = StepFight.Fourth;        
    }

    private void SelectStepFight()
    {
        
        switch (_stepFight)
        {
            case StepFight.First:
                if (_bigBoss) gameObject.GetComponent<MeshRenderer>().material.color = Color.green;               
                break;
            case StepFight.Second:
                if (_bigBoss) gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;               
                break;
            case StepFight.Three:
                if (_bigBoss) gameObject.GetComponent<MeshRenderer>().material.color = Color.red;              
                break;
            case StepFight.Fourth:
                if (_bigBoss) gameObject.GetComponent<MeshRenderer>().material.color = Color.black;                
                break;
        }
    }
    #endregion

    #region ShotGun
    private void ShotGun()
    {
        var bullet = Instantiate(BulletPrefab, _spawnBullet.position, _spawnBullet.rotation).GetComponent<Rigidbody>();
        bullet.AddForce(_spawnBullet.forward * ShotPower,ForceMode.Impulse);
    }

    private IEnumerator Shot()
    {
        while(true)
        {
            if(_playerDistance < 10f && _playerGame != null)
            {
                yield return new WaitForSeconds(1f);
                ShotGun();
            }
            yield return null;
        }
    }
    #endregion

}
