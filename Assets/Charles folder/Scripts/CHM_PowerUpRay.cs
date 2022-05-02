using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CHM_PowerUpRay : MonoBehaviour
{
    public int level;
    
    //The base diameter of the ray.
    [SerializeField] private float basicDiameter;
    //The modifier ratio for the ray size, according to the level of the ray.
    [SerializeField] private float sizeRatiolvl1, sizeRatiolvl2, sizeRatiolvl3;
    //The final diameter of the ray.
    private float diameter;

    //The base reload of the ray.
    [SerializeField] private float basicReload;
    //The modifier ratio for the reload time, according to the level of the ray.
    [SerializeField] private float reloadRatiolvl1, reloadRatiolvl2, reloadRatiolvl3;
    //The final reload time of the ray.
    private float reload;
    public bool reloading;

    //The basic duration of the ray.
    [SerializeField] private float basicDuration;
    //The duration modifying ratio for the ray.
    [SerializeField] private float durationRatiolvl1, durationRatiolvl2, durationRatiolvl3;
    //The final duration of the ray.
    private float duration;

    //The remaining duration of the ray.
    private float timeRemaining;

    //the basic damage per tic of the ray.
    [SerializeField] private float basicDamages;
    //The damage modifier for the ray according to its level.
    [SerializeField] private float damageRatiolvl1, damageRatiolvl2, damageRatiolvl3;
    //The final damage per tic of the ray.
    private int damages;

    //Delay between tics (its on tics that the ray inflicts damages).
    [SerializeField] private float ticRate;
    //Time remaining before the next damage dealt.
    private float damageTiming;

    //Bool to check if the ray deals physical or magical damages.
    [SerializeField] private bool magicalDamages;
    private DamageType damageType;

    //Ray speed.
    [SerializeField] private float speed;

    //List of enemies in range of the ray.
    private List<GameObject> enemies;

    //Is the button for the spell clicked.
    public bool spellCalled;

    public bool spellOnGoing;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private CHM_SpellsManager spellManager;

    [SerializeField] private Sprite raySprite;

    private Touch touch;

    public Enemy enemyComponent;

    public CircleCollider2D circleCollider;

    //public bool pauseCount;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        CheckLevel();
        damageTiming = ticRate;
        if (magicalDamages == true)
        {
            damageType = DamageType.Magical;
        }
        else damageType = DamageType.Physical;

        GetComponent<CircleCollider2D>().radius = diameter;
    }

    public void Update()
    {
        if(spellOnGoing == true)
        {
            MoveRay();
            TimeRay();
        }
        else if(reloading == true)
        {
            ReloadRay();
        }
    }

    //private void OnEnable()
    //{
    //    timeRemaining = duration;
    //}
    
    public void CheckLevel()
    {
        if(level == 1)
        {
            diameter = basicDiameter * sizeRatiolvl1;
            duration = basicDuration * durationRatiolvl1;
            damages = (int)Mathf.Round(basicDamages * damageRatiolvl1);
            reload = basicReload * reloadRatiolvl1;
        }
        
        if(level == 2)
        {
            diameter = basicDiameter * sizeRatiolvl2;
            duration = basicDuration * durationRatiolvl2;
            damages = (int)Mathf.Round(basicDamages * damageRatiolvl2);
            reload = basicReload * reloadRatiolvl2;
        }

        if(level ==3)
        {
            diameter = basicDiameter * sizeRatiolvl3;
            duration = basicDuration * durationRatiolvl3;
            damages = (int)Mathf.Round(basicDamages * damageRatiolvl3);
            reload = basicReload * reloadRatiolvl3;
        }

    }


    public void CastRay(Vector3 position)
    {
        spellOnGoing = true;
        transform.position = position;
        timeRemaining = duration;
        damageTiming = ticRate;
        //gameObject.SetActive(true);
        spriteRenderer.sprite = raySprite;
    }
    
    private void MoveRay()
    {
        Vector2 mousePos = new Vector2();

        if (transform.position != Input.mousePosition.)
        {
            transform.position = Vector3.MoveTowards(transform.position, Input.mousePosition, speed);
        }
    }
    private void ReloadRay()
    {
        spellManager.reloadRemaining -= Time.deltaTime;
        if (spellManager.reloadRemaining <= 0)
        {
            reloading = false;
            spellManager.canCastRay = true;
        }
    }
    private void TimeRay()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            
            if (damageTiming <= 0)
            {
                RayDamage();
                damageTiming = ticRate;
            }
            else damageTiming -= Time.deltaTime;
        }
        else EndRay();

    }

    private void RayDamage()
    {
        
        //if (enemies.Count != 0)
        //{

        //    for (int i = 0; i > enemies.Count; i++)
        //    {
        //        enemyComponent = enemies[i].GetComponent<Enemy>();
        //        enemyComponent.TakeDamage(damageType, damages);
        //    }
        //}
    }

    private void EndRay()
    {
        //gameObject.SetActive(false);
        spriteRenderer.sprite = null;
        spellOnGoing = false;
        reloading = true;
        spellManager.reloadRemaining = reload;
        circleCollider.enabled = false;
        HideRay();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<Enemy>() != null)
        {
            enemies.Add(go);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (enemies.Contains(go))
        {
            enemies.Remove(go);
        }
    }

    private void HideRay()
    {

    }
}
