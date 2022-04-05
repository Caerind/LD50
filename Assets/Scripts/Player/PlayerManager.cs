using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> players = new List<GameObject>();
    private Vector3[] spawnPoints;

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 pos = Vector3.zero;
        if (players.Count > 0)
        {
            int count = 0;
            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    pos += player.transform.position;
                    count++;
                }
            }

            GameObject[] bags = GameObject.FindGameObjectsWithTag("Bag");
            foreach (GameObject bag in bags)
            {
                if (bag != null && bag.activeSelf)
                {
                    pos += bag.transform.position;
                    count++;
                }
            }

            if (count > 0)
            {
                pos /= count;
            }
        }
        return pos;
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);

        for (int i = 0; i < players.Count; ++i)
        {
            GameObject p = players[i];
            PlayerController pc = p.GetComponent<PlayerController>();
            if (!pc.spawned)
            {
                if (spawnPoints != null && i < spawnPoints.Length)
                {
                    p.transform.position = spawnPoints[i];
                    pc.spawned = true;
                }
                else
                {
                    p.transform.position = Vector3.zero;
                }
            }
        }

        Camera.main.transform.position = GetPlayerPosition();
    }

    public void SetSpawnPoints(Vector3[] spawnPoints)
    {
        this.spawnPoints = spawnPoints;

        for (int i = 0; i < players.Count; ++i)
        {
            GameObject p = players[i];
            PlayerController pc = p.GetComponent<PlayerController>();
            if (!pc.spawned && i < spawnPoints.Length)
            {
                p.transform.position = spawnPoints[i];
                pc.spawned = true;
                pc.isInZone = false;
            }
        }
    }

    public void ResetPlayersSpawned()
    {
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                player.GetComponent<PlayerController>().spawned = false;
                player.GetComponent<PlayerController>().isInZone = false;
                player.GetComponent<PlayerController>().carryBag = false;
                player.GetComponent<PlayerController>().hasBag = false;
                player.GetComponentInChildren<Animator>().SetBool("HasBag", false);
            }
        }
    }

    public bool DetectPlayer(Vector2 pos, Vector2 dir, float fieldRadius, float fieldAngle, float tooClose)
    {
        float dp = Mathf.Cos(fieldAngle * 0.5f * Mathf.Deg2Rad);
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                Vector2 playerPos = player.transform.position;
                Vector2 targetPos = dir * fieldRadius + pos;

                Vector2 playerVector = playerPos - pos;
                if (playerVector.sqrMagnitude > fieldRadius * fieldRadius) // Player is too far
                {
                    continue;
                }

                Vector2 playerDir = playerVector.normalized;
                Vector2 targetDir = (targetPos - pos).normalized;
                if (playerVector.sqrMagnitude < tooClose * tooClose) // Player is too close
                {

                }
                else
                {
                    if (Vector2.Dot(playerDir, targetDir) < dp) // Player is not in the direction
                    {
                        continue;
                    }
                }

                RaycastHit2D hit = Physics2D.Raycast(pos + playerDir * 0.5f, playerDir, fieldRadius, LayerMask.GetMask("Default", "Player"));
                if (hit.collider != null && hit.collider.gameObject.transform.parent.GetComponent<PlayerController>() != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public GameObject GetNearestPlayer(Vector3 pos)
    {
        GameObject nearestPlayer = null;
        float nearestSqrDistance = float.MaxValue;
        foreach (GameObject player in players)
        {
            if (nearestPlayer == null)
            {
                nearestPlayer = player;
                nearestSqrDistance = (player.transform.position - pos).sqrMagnitude;
            }
            else
            {
                float sqrD = (player.transform.position - pos).sqrMagnitude;
                if (sqrD < nearestSqrDistance)
                {
                    nearestSqrDistance = sqrD;
                    nearestPlayer = player;
                }
            }
        }
        return nearestPlayer;
    }
}