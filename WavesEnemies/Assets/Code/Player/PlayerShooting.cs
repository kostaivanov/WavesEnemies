using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

internal class PlayerShooting : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform firePoint, targetPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    private PhotonView photonView;

    private float bulletForce = 20f;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        //Vector3 targetV3 = targetPoint.transform.position;
        //Vector2 v2 = new Vector2();
        //v2 = targetV3;
        //Vector2 lookDirection = v2 - rb.position;
        //float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
        Destroy(bullet, 2f);
    }
}
