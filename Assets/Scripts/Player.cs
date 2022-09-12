using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    [SerializeField]
    private Rigidbody rigidbody;

    private Vector3 moveDir;
    private float moveSpeed = 80.0f;
    public int Damage = 10;
    public int HP { get; private set; } = 100;

    public void Update()
    {
        if(false == photonView.IsMine)
        {
            return;
        }
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    public void FixedUpdate()
    {
        if (false == photonView.IsMine)
        {
            return;
        }
        rigidbody.MovePosition(transform.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Attack()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("CheckCollision", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void CheckCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider collider in colliders)
        {
            Player enemy = collider.GetComponent<Player>();
            if (enemy == null || enemy == this)
            {
                continue;
            }
            enemy.photonView.RPC("OnDamage", RpcTarget.All, Damage);

        }
            
        // 여기서 검사를 하고, OnDamage를 여기에서 호출
        // 충돌을 했는지 안했는지를 여기에서 검사
        // 왜? 여기는 호스트만 검사하고, 우리의 정책은 호스트가 충돌 검사를 판정하는걸로 정했으니까!
        // 검사가 끝나고 충돌을 한 애들의 결과는 여기에서 뿌려줘야 한다.
    }

    // 피해를 입은 시점에 호출되는 함수
    [PunRPC]
    public virtual void OnDamage(int damage)
    {       
        HP -= damage;
        rigidbody.velocity += Vector3.up * 5;
        //Vector3 velocity = enemy.rigidbody.velocity;
        //velocity.y = 1;
        //enemy.rigidbody.velocity = velocity;
    }
}
