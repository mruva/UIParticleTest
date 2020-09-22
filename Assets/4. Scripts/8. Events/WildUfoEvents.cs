using UnityEngine;
using UnityEngine.Events;

public static class WildUfoEvents 
{

    public static HitEvent enemyHit = new HitEvent(); // non effettivamente utilizzato al momento
    public static ShootEvent enemyShoot = new ShootEvent();
    public static DestructionEvent destruction = new DestructionEvent();
    public static EntityEvent entityHit = new EntityEvent();
    public static MalusEvent malusHit = new MalusEvent(); //sostituito in toto dall'entityevent
    public static BoolEvent boolHit = new BoolEvent();
    public static PlayerStatsEvent playerStats = new PlayerStatsEvent(); //non piu utilizzato
    public static AudioEvent audioEvent = new AudioEvent();
    public static ParticleFX particleFXEvent = new ParticleFX(); //al momento non usato

}

//non utilizzato al momento
#region EnemyHit
public class HitEvent : UnityEvent<HitEventData> { }

public class HitEventData
{
    public int enemyID;
    public GameObject victim;
    //public GameObject bullet; // per ora lo commento perchè lo uso solamente con le trappole a contatto
    public float damage;
    public HitEventData (int enemyID, float damage)
    {
        //this.shooter = shooter;
        //this.victim = victim;
        //this.bullet = bullet;
        this.enemyID = enemyID;
        this.damage = damage;
    }
}

#endregion

#region ShootEvent

public class ShootEvent : UnityEvent<ShootEventData> { }

public class ShootEventData
{
    public Vector3 position;
    public Quaternion rotation;

    public ShootEventData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }


}

#endregion

#region DestructionEvent

public class DestructionEvent : UnityEvent<DestructionEventData> { }

public class DestructionEventData
{
    public int id;

    public DestructionEventData (int id)
    {
        this.id = id;
    }


}

#endregion

#region Entity
public class EntityEvent : UnityEvent<EntityEventData> { }

public class EntityEventData
{
    public int points;
    public int credits;
    public int lives;
    public float health;
    public EntityType currentEntityType;
    public enum EntityType
    {
        bonus,
        malus
    }
    
    public EntityEventData (int points, int credits, int lives, float health, EntityType entityType)
    {
        this.points = points;
        this.credits = credits;
        this.lives = lives;
        this.health = health;
        this.currentEntityType = entityType;
    }
}
#endregion 

#region Malus

public class MalusEvent : UnityEvent<MalusEventData> { }
public class MalusEventData 
{
    public int points;
    public int credits;
    public int lives;
    public float health;

    public MalusEventData(int points, int credits, int lives, float health)
    {
        this.points = points;
        this.credits = credits;
        this.lives = lives;
        this.health = health;
    }
}


#endregion

#region BoolEvent

public class BoolEvent : UnityEvent<BoolEventData> { }

public class BoolEventData
{
    public bool boolEvent;
    public enum BoolEvent
    {
        endLevel,
        levelWon,
        ambienteMusic,
        enemyZone,
        isColliding,
        playerDead,
        menuMusic,
        gameIsPaused,
        gameIsOver,
        debris,
        shieldActivated,
        playerShoot,
        playerRocket,
        enemyDestroyed
    }

    public BoolEvent currentEvent;

    public BoolEventData(bool boolEvent, BoolEvent currentEvent)
    {
        this.boolEvent = boolEvent;
        this.currentEvent = currentEvent;
    }

}
#endregion

#region PlayerStats
public class PlayerStatsEvent : UnityEvent<PlayerStatsEventData> { }

public class PlayerStatsEventData
{
    public int intStats;
    public float floatStats;

    public enum StatsDataType
    {
        credits,
        totalLives,
        currentHealth,
        currentShield,
        damageReceived
    }

    public StatsDataType currentStatsDataType;
    public PlayerStatsEventData(int intStats, float floatStats, StatsDataType currentStatsDataType)
    {
        this.intStats = intStats;
        this.floatStats = floatStats;
        this.currentStatsDataType = currentStatsDataType;
    }

}
#endregion

#region AudioEvent
public class AudioEvent : UnityEvent<AudioEventData> { }

public class AudioEventData
{
    public Vector3 position;
    public string name;
    public bool loop;
    public bool playAtTheSameTime;

    public enum GameObjectSource
    {
        Bomb,
        MagneticTrap,
        RedTrap,
        SegaTrap,
        BulletShot,
        CoinCollected,
        PlayerEngine, //questo forse è da loopare quindi da togliere da qua
        LevelCompleted,
        GameOver,
        ButtonMenu,
        Music,
        Clock,
        PlayerDeath,
        ShieldActivated,
        PlayerShoot,
        EnemyShoot,
        PlayerRocket,
        LaserHit,
        EnemyDeath
    }

    public GameObjectSource currentGameObjectSource;

    public AudioEventData(Vector3 position, string name, GameObjectSource currentGameObjectSource, bool loop, bool playAtTheSameTime)
    {
        this.position = position;
        this.name = name;
        this.currentGameObjectSource = currentGameObjectSource;
        this.loop = loop;
        this.playAtTheSameTime = playAtTheSameTime;
    }
}


#endregion

// non piu usato per ora
#region ParticleFX

public class ParticleFX : UnityEvent<ParticleFXEventData> { }

public class ParticleFXEventData
{
    public string name;
    public Transform transform;

    public ParticleFXEventData(string name, Transform transform)
    {
        this.name = name;
        this.transform = transform;
    }
}

#endregion
