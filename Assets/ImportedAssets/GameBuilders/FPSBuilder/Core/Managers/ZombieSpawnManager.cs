using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawnManager : MonoBehaviour
{
    [SerializeField]
    int remaningAmountToSpawn, totalAmountToSpawn, aliveZombies, zombiesKilledThisRound;
    bool canSpawnZombie, canSpawn;

    [SerializeField] List<ZombieSpawnPoint> activeSpawnPoints = new List<ZombieSpawnPoint>();
    [SerializeField] GameObject walkingZombie, runningZombie, sprintingZombie;
    [SerializeField] Transform zombieParent;
    [SerializeField] int maxNumberOfAliveZombies, startingAmountToSpawn;
    [SerializeField] float zombieSpawnCooldown;

    public bool canSpawnZombies = true;

    public static event Action onAllZombiesKilled;

    int currentRound;

    private void OnEnable()
    {
        ZombieHealthController.onDeath += KillZombie;
        //ZombieSpawnPoint.onSpawnPointActivateStatusUpdated += UpdateSpawnPoint;
        RoundManager.onNewRoundStarted += StartSpawning;
        //PlayerHealth.onDeath += StopSpawning;
    }
    private void OnDisable()
    {
        ZombieHealthController.onDeath -= KillZombie;
        //ZombieSpawnPoint.onSpawnPointActivateStatusUpdated -= UpdateSpawnPoint;
        RoundManager.onNewRoundStarted -= StartSpawning;
        //PlayerHealth.onDeath -= StopSpawning;
    }

    void Start()
    {
        totalAmountToSpawn = startingAmountToSpawn;
        remaningAmountToSpawn = totalAmountToSpawn;
        canSpawnZombie = true;

    }

    void Update()
    {
        if (canSpawn)
        {
            if (remaningAmountToSpawn == 0)
            {
                StopSpawning();
            }

            if (aliveZombies < maxNumberOfAliveZombies)
            {
                TrySpawnZombie();
            }
        }
    }

    void StartSpawning(int _currentRound)
    {
        currentRound = _currentRound;

        if (canSpawnZombies)
        {
            totalAmountToSpawn = startingAmountToSpawn * _currentRound;
            remaningAmountToSpawn = totalAmountToSpawn;
            zombiesKilledThisRound = 0;
            canSpawn = true;
        }
    }

    void StopSpawning()
    {
        canSpawn = false;
    }

    void TrySpawnZombie()
    {
        if (canSpawnZombie)
        {
            canSpawnZombie = false;
            remaningAmountToSpawn--;

            aliveZombies++;
            GameObject zombieToSpawn = walkingZombie;

            if (currentRound >= 8)
            {
                zombieToSpawn = sprintingZombie;
            }
            else if (currentRound >= 3 && currentRound < 8)
            {
                zombieToSpawn = runningZombie;
            }

            int rand = UnityEngine.Random.Range(0, activeSpawnPoints.Count);

            //if (zombieParent)
            //{
                GameObject clone = Instantiate(zombieToSpawn, activeSpawnPoints[rand].transform.position, Quaternion.identity/*, zombieParent*/);
                if (clone.TryGetComponent<ZombieHealthController>(out ZombieHealthController zombieHealth))
                {
                    zombieHealth.SetHealth(90 + (currentRound * 5));
                }
            //}

            StartCoroutine(SpawnCooldown());
        }
    }

    void UpdateSpawnPoint(ZombieSpawnPoint spawnPointToUpdate, bool _isSpawnPointActive)
    {
        if (_isSpawnPointActive && !activeSpawnPoints.Contains(spawnPointToUpdate))
            activeSpawnPoints.Add(spawnPointToUpdate);
        //else
        //    activeSpawnPoints.Remove(spawnPointToUpdate);
    }

    IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(zombieSpawnCooldown);
        canSpawnZombie = true;
    }

    void KillZombie(bool wasHeadshot)
    {
        aliveZombies--;
        zombiesKilledThisRound++;
        if (zombiesKilledThisRound == totalAmountToSpawn)
        {
            onAllZombiesKilled?.Invoke();
        }
    }
}
