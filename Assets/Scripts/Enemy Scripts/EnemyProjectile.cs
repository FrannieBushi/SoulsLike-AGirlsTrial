using System;
using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject projectile;
    Animator anim;
    public float timeToShoot;
    public float shootCooldown;
    
    public bool freqShooter;
    public bool watcher;

    public bool hasNegativeScale;
    public bool hasShootAnimation;
    public GameObject startShootPosition;
    public EnemyMovement enemyMovement;
    void Start()
    {
        shootCooldown = timeToShoot;
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        if(shootCooldown < 0)
        {
            shootCooldown = timeToShoot;
        }

        if(freqShooter && !hasShootAnimation) 
        {
            shootCooldown -= Time.deltaTime;

            if(shootCooldown < 0)
            {
                Shoot();
            }
        }

        if(freqShooter && hasShootAnimation)
        {
            shootCooldown -= Time.deltaTime;
            if(shootCooldown < 0)
            {
                anim.SetBool("attackshoot", true);
            }

        }
              
    }

    public void Shoot()
    {
        GameObject projectileAux = Instantiate(projectile, transform.position, Quaternion.identity);

        if(transform.localScale.x < 0)
        {
            if(!hasNegativeScale)
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f, 0f), ForceMode2D.Force);
            }
            else
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 0f), ForceMode2D.Force);    
            }     
        }
        else
        {
            if(!hasNegativeScale)
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 0f), ForceMode2D.Force);
            }
            else
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f, 0f), ForceMode2D.Force);    
            }
        }
    }

    public void ShootWithAnimation()
    {
        GameObject projectileAux = Instantiate(projectile, startShootPosition.transform.position, Quaternion.identity);

        if(transform.localScale.x < 0)
        {
            if(!hasNegativeScale)
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f, 0f), ForceMode2D.Force);
            }
            else
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 0f), ForceMode2D.Force);    
            }     
        }
        else
        {
            if(!hasNegativeScale)
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500f, 0f), ForceMode2D.Force);
            }
            else
            {
                projectileAux.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f, 0f), ForceMode2D.Force);    
            }
        }
    }

    public void DisableMovement()
    {
        enemyMovement.enabled = false;
    }

    public void AbleMovement()
    {
        enemyMovement.enabled = true;
        anim.SetBool("attackshoot", false);    
    }
}
