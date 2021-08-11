using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GeneralMonsterScript : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _trans;
    private Animator _animator;
    private AudioSource _audio;
    private GameObject player;
    private float currentHealth;
    private float currentAttackDelay;
    private bool roared = false;

    [SerializeField] private float detectionDistance;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float maxHealth;
    [SerializeField] static private bool fightStarted = true;
    [SerializeField] private AudioClip[] roar = new AudioClip[3];
    [SerializeField] private AudioClip hit;
    [SerializeField] private GameObject supply;

    public Animator GetAnimator() { return _animator; }

    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _trans = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        currentHealth = maxHealth;
        currentAttackDelay = attackDelay;
    }
    public virtual void Update()
    {
        if ((Vector3.Distance(_trans.position, player.transform.position) < detectionDistance || fightStarted))
        {
            fightStarted = true;
            _agent.destination = player.transform.position;
            _animator.SetBool("isMoving", true);
        
        }
        else
        {
            _agent.destination = _trans.position;
            _animator.SetBool("isMoving", false);
        }

        if (currentAttackDelay > -1)
            currentAttackDelay -= Time.deltaTime;

        if(Vector3.Distance(_trans.position, player.transform.position) < 2.5 && currentAttackDelay < 0)
        {           
            HitPlayer();           
        }

        if (!roared)
            StartCoroutine(Roar());

    }

    public void GetHit(float damage)
    {
        fightStarted = true;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            for (int i = 0; i < Random.Range(0, 4); i++)
            {
                Instantiate(supply, new Vector3(_trans.position.x, 0, _trans.position.z), _trans.rotation);
            }
            Destroy(this.gameObject);
        }
    }
    public virtual void HitPlayer()
    {
        player.GetComponent<PlayerScript>().GetHit(damage);
        currentAttackDelay = attackDelay;
        _audio.PlayOneShot(hit);
    }

    IEnumerator Roar()
    {
        roared = true;
        _audio.PlayOneShot(roar[Random.Range(0, 2)]);
        yield return new WaitForSeconds(2);
        roared = false;
    }
}
