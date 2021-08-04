using UnityEngine;
using UnityEngine.AI;

public abstract class GeneralMonsterScript : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _trans;
    private GameObject player;
    private float currentHealth;
    private float currentAttackDelay;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float maxHealth;
    /*[SerializeField] private float attackDistance;*/
    //[SerializeField] private float scalar;
    [SerializeField] static private bool fightStarted;
    /*[SerializeField] private float speed;*/
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        /*_agent.speed = speed;*/
        _trans = GetComponent<Transform>();
        player = GameObject.Find("Player");
        currentHealth = maxHealth;
        currentAttackDelay = attackDelay;
    }
    public virtual void Update()
    {
        if ((Vector3.Distance(_trans.position, player.transform.position) < detectionDistance || fightStarted) /*&& Vector3.Distance(_trans.position, player.transform.position) > attackDistance*/)
        {
            fightStarted = true;
            _agent.destination = player.transform.position;
        }
        else
            _agent.destination = _trans.position;

        if (currentAttackDelay > -1)
            currentAttackDelay -= Time.deltaTime;

        if(Vector3.Distance(_trans.position, player.transform.position) < 2 && currentAttackDelay < 0)
        {
            HitPlayer();
            currentAttackDelay = attackDelay;
        }

        /*scalar = Vector3.Dot(_trans.forward.normalized, Vector3.Normalize(player.transform.position - _trans.position));

        Ray ray = new Ray(_trans.position, player.transform.position - _trans.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject);
        }

        Debug.DrawRay(_trans.position, player.transform.position - _trans.position, Color.red);*/
    }

    public void GetHit(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void HitPlayer()
    {
        player.GetComponent<PlayerScript>().GetHit(damage);
    }
}