using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GeneralMonsterScript : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _trans;
    private Animator _animator;
    //private AudioSource _audio;
    private GameObject player;
    private float currentHealth;
    private float currentAttackDelay;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float maxHealth;
    [SerializeField] static private bool fightStarted;
    //[SerializeField] private AudioClip steps;

    /*private bool running = false;
    private bool hit = false;*/
    public Animator GetAnimator() { return _animator; }

    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _trans = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
        //_audio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        currentHealth = maxHealth;
        currentAttackDelay = attackDelay;
    }
    public virtual void Update()
    {
        if ((Vector3.Distance(_trans.position, player.transform.position) < detectionDistance || fightStarted))
        {
            /*if(!fightStarted)
                running = true;*/
            fightStarted = true;
            _agent.destination = player.transform.position;
            _animator.SetBool("isMoving", true);
        
        }
        else
        {
            _agent.destination = _trans.position;
            _animator.SetBool("isMoving", false);
            //running = false;
        }

        if (currentAttackDelay > -1)
            currentAttackDelay -= Time.deltaTime;

        if(Vector3.Distance(_trans.position, player.transform.position) < 2 && currentAttackDelay < 0)
        {           
            HitPlayer();           
        }

        /*if (hit)
        {
            StartCoroutine(HitCoroutine());
        }

        if (running)
        {
            print("dfhdhdf");
            running = false;
            _audio.clip = steps;
            _audio.time = Random.Range(0, 7);
            _audio.loop = true;
            _audio.Play();
        }*/
    }

    public void GetHit(float damage)
    {
        fightStarted = true;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public virtual void HitPlayer()
    {
        player.GetComponent<PlayerScript>().GetHit(damage);
        /*running = false;
        hit = true;*/
        currentAttackDelay = attackDelay;
    }

    /*IEnumerator HitCoroutine()
    {
        hit = false;

        //_audio.clip = param-param;
        //_audio.loop = false;
        //_audio.Play();

        yield return new WaitForSeconds(1f);

        running = true;
    }*/
}
