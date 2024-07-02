using UnityEngine;

public class ZombieSpawnPoint : MonoBehaviour
{
    [SerializeField] bool m_IsSpawning;
    [SerializeField] bool m_IsAccessible;
    /// <summary>
    /// Returns wether the spawn point is spawning zombies or not
    /// </summary>
    public bool IsSpawning => m_IsSpawning;
    /// <summary>
    /// Returns wether or not the spawn point can be accessed 
    /// </summary>
    public bool IsAccessible => m_IsAccessible;

    public void SetIsSpawning(bool isSpawning)
    {
        m_IsSpawning = isSpawning;
    }

    public void SetIsAccessible(bool isAccessible)
    {
        m_IsAccessible = isAccessible;
    }
}
