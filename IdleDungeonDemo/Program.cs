using System;
using System.Diagnostics;
using System.Threading;

class IdleDungeonClicker
{
    static int gold = 0;
    static int heroDamage = 10;
    static int monsterHealth = 50;
    static int monstersDefeated = 0;
    static int goldPerKill = 5;
    static int heroHealth = 100;
    static int maxHeroHealth = 100;
    static int monsterDamage = 5;
    static int healthPotion = 5;

    static int healthUpgrade = 10;
    static int swordUpgrade = 10;

    static bool isAlive = true;
    static bool isMonsterDebuffed = true;

    static void Main()
    {
        Console.WriteLine("=== Idle Dungeon Clicker ===");
        Console.WriteLine("Your hero fights monsters automatically.");
        Console.WriteLine("Earn gold, upgrade your hero, and survive as long as possible!\n");

        Thread battleThread = new Thread(AutoBattle);
        battleThread.Start();

        while (true)
        {
            Console.WriteLine("\nOptions: [1] Upgrade Sword (10 Gold) | [2] Upgrade Hero's Health (20 Gold) | [3] buy Potion | [4] Show Stats | [5] Exit  ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UpgradeSword();
                    break;
                case "2":
                    UpgradeHeroHealth();
                    break;
                case "3":
                    BuyPotion();
                    break;
                case "4":
                    ShowStats();
                    break;
                default:
                    break;
            }
        }
    }

    //-------------------------------------------------------------------------------- BATTLE --------------------------------------------------------------------------
    static void AutoBattle()
    {
        while (true)
        {
            if (isAlive)
            {
                Console.WriteLine("\nHero attacks for " + heroDamage + " damage!");
                monsterHealth -= heroDamage;

                if (monsterHealth <= 0)
                {
                    monstersDefeated++;
                    gold += goldPerKill;
                    Console.WriteLine("Monster defeated! You earned " + goldPerKill + " gold.");
                    monsterHealth = 50 + (monstersDefeated * 5);
                    monsterDamage += 1;
                }
                else
                {
                    int actualMonsterDamage = isMonsterDebuffed ? monsterDamage / 2 : monsterDamage;
                    heroHealth -= actualMonsterDamage;

                    Console.WriteLine($"Monster attacks! You take {actualMonsterDamage} damage. (HP: {heroHealth}/{maxHeroHealth})");

                    if (heroHealth <= 0)
                    {
                        Console.WriteLine("You have been defeated! Respawning in 5 seconds...");
                        isAlive = false;
                        Thread.Sleep(5000);

                        // Revive with 50% HP
                        heroHealth = maxHeroHealth / 2;
                        Console.WriteLine($"You have respawned with {heroHealth}/{maxHeroHealth} HP!");

                        // Apply debuff
                        DebuffMonster();
                        isAlive = true;
                    }
                }
            }
            Thread.Sleep(5000);
        }
    }
    //-------------------------------------------------------------------------------- BATTLE --------------------------------------------------------------------------


    // ------------------------------------------------------------------------------- UPGRADES ------------------------------------------------------------------------


    static void BuyPotion()
    {
        if (gold >= 5)
        {
            if(heroHealth < maxHeroHealth)
            {
                gold -= 5;
                heroHealth += healthPotion;
                Console.WriteLine($"Hero health has been restored! +{healthPotion}. ");
            }
            else
            {
                Console.WriteLine("Your health is already full!");
            }
        }
        else
        {
            Console.WriteLine("Not enough gold!");
        }
    }

    static void UpgradeHeroHealth()
    {
        if(gold >= 20)
        {
            gold -= 20;
            maxHeroHealth += healthUpgrade;
            heroHealth += healthUpgrade;
            Console.WriteLine($"Hero max health upgraded! {maxHeroHealth} | {healthUpgrade} ");
        }
        else
        {
            Console.WriteLine("Not enough gold!");
        }
    }
    static void UpgradeSword()
    {
        if (gold >= 10)
        {
            gold -= 10;
            heroDamage += 5;
            Console.WriteLine("Sword upgraded! Your damage is now " + heroDamage);
        }
        else
        {
            Console.WriteLine("Not enough gold!");
        }
    }


    // ------------------------------------------------------------------------------- UPGRADES ------------------------------------------------------------------------

    // ------------------------------------------------------------------------------- DEBUFFs ------------------------------------------------------------------------
    static void DebuffMonster()
    {
        isMonsterDebuffed = true;
        Console.WriteLine("Monster is weakened for 5 seconds!");

        // Create a separate thread to remove the debuff after 5 seconds
        new Thread(() =>
        {
            Thread.Sleep(5000);
            isMonsterDebuffed = false;
            Console.WriteLine("Monster has regained full strength!");
        }).Start();
    }

    // ------------------------------------------------------------------------------- DEBUFFs ------------------------------------------------------------------------
  
    static void ShowStats()
    {
        Console.WriteLine("\n=== Hero Stats ===");
        Console.WriteLine("Gold: " + gold);
        Console.WriteLine("Damage: " + heroDamage);
        Console.WriteLine($"Health: {maxHeroHealth}\\{heroHealth}");
        Console.WriteLine("Monsters Defeated: " + monstersDefeated);
        Console.WriteLine("Monster Damage: " + monsterDamage);
    }
}
