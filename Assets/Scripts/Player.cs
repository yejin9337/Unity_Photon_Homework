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
            
        // ���⼭ �˻縦 �ϰ�, OnDamage�� ���⿡�� ȣ��
        // �浹�� �ߴ��� ���ߴ����� ���⿡�� �˻�
        // ��? ����� ȣ��Ʈ�� �˻��ϰ�, �츮�� ��å�� ȣ��Ʈ�� �浹 �˻縦 �����ϴ°ɷ� �������ϱ�!
        // �˻簡 ������ �浹�� �� �ֵ��� ����� ���⿡�� �ѷ���� �Ѵ�.
    }

    // ���ظ� ���� ������ ȣ��Ǵ� �Լ�
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
