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
    
    static int healthPotion = 15;
    static int healthUpgrade = 10;
    static int swordUpgrade = 10;
    static int armorLevel = 0;
    static int maxArmorPers = 65;

    static bool isAlive = true;
    static bool isMonsterDebuffed = false;

    static void Main()
    {
        

        Console.WriteLine("=== Idle Dungeon Clicker ===");
        Console.WriteLine("Your hero fights monsters automatically.");
        Console.WriteLine("Earn gold, upgrade your hero, and survive as long as possible!\n");

        Thread battleThread = new Thread(AutoBattle);
        battleThread.Start();

        while (true)
        {
            DisplayMenu(); // Always show menu
            string choice = Console.ReadLine();
            HandleMenuChoice(choice);
        }
    }

    //-------------------------------------------------------------------------------- MENU --------------------------------------------------------------------------

    static void DisplayMenu()
    {
        
        
        Console.WriteLine("=== Idle Dungeon Clicker ===");

        ShowStats(); // Display current stats

        Console.WriteLine("\nOptions:");
        Console.WriteLine("[1] Upgrade Sword (10 Gold) (+15)");
        Console.WriteLine("[2] Buy Potion (5 Gold) (+15)");
        Console.WriteLine("[3] Buy Armor (20 Gold) (+5%)");
        Console.WriteLine("[4] Upgrade Max Hleath (20 Gold) (+10)");
        Console.WriteLine("[5] Show Stats");
        Console.WriteLine("[6] Exit");
        Console.Write("\nChoose an option: ");

       
    }

    static void HandleMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                UpgradeSword();
                break;
            case "2":
                BuyPotion();
                break;
            case "3":
                UpgradeArmor();
                break;
            case "4":
                UpgradeHeroHealth();
                break;
            case "5":
                ShowStats();
                break;
            case "6":
                Environment.Exit(0); // Exits the game
                break;
            default:
                Console.WriteLine("Invalid option! Try again.");
                Thread.Sleep(1000); // Short delay before refreshing menu
                break;
        }
        
    }

    static void PrintBattleLog(string message)
    {
        Console.SetCursorPosition(0, 10); // Move cursor below menu (adjust as needed)
        Console.WriteLine(new string(' ', Console.WindowWidth)); // Clear previous log
        Console.SetCursorPosition(0, 10);
        Console.WriteLine(message);
    }


    //-------------------------------------------------------------------------------- MENU --------------------------------------------------------------------------

    //-------------------------------------------------------------------------------- BATTLE --------------------------------------------------------------------------
    static void AutoBattle()
    {
        int monsterDeathCounter = 0;
        while (true)
        {
            if (isAlive)
            {
                monsterHealth -= heroDamage;
                Console.WriteLine("\nHero attacks for " + heroDamage + $" damage! (HP: {monsterHealth}))");

                if (monsterHealth <= 0)
                {
                    monsterDeathCounter++;
                    monstersDefeated++;
                    gold += goldPerKill;
                    Console.WriteLine("Monster defeated! You earned " + goldPerKill + " gold.");
                    monsterHealth = 50 + (monstersDefeated * 5);
                    monsterDamage += 1;
                    if(monsterDeathCounter >= 2)
                    {
                        RefreshConsole();
                    }
                }
                else
                {
                    // calculcate monste damage 
                    int actualMonsterDamage = isMonsterDebuffed ? monsterDamage / 3 : monsterDamage;

                    // Apply armor reduction
                    int reducedDamage = armorLevel > 0 
                        ? (int)(actualMonsterDamage * (1 - (armorLevel / 100.0))) 
                        : actualMonsterDamage; // If no armor, take full damage
                    heroHealth -= reducedDamage;
                    Console.WriteLine($"Monster attacks! You take {reducedDamage} damage. (HP: {heroHealth}/{maxHeroHealth})");

                    if (heroHealth <= 0)
                    {
                        RefreshConsole();
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

    static void UpgradeArmor()
    {
        if (gold >= 10)
        {
            if (armorLevel < maxArmorPers)
            {
                gold -= 10;
                armorLevel += 5; // Increases defense by 5%
                Console.WriteLine($"Armor upgraded! Damage reduction is now {armorLevel}%.");
            }
            else
            {
                Console.WriteLine("Armor is already at max level (80% damage reduction)!");
            }
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
        Console.WriteLine("Monster is weakened for 10 seconds!");

        // Run the debuff removal in a background task
        Task.Run(async () =>
        {
            await Task.Delay(10000); // Wait for 10 seconds
            isMonsterDebuffed = false;
            Console.WriteLine("Monster has regained full strength!");
        });
    }

    // ------------------------------------------------------------------------------- DEBUFFs ------------------------------------------------------------------------

    static void RefreshConsole()
    {
        Console.Clear(); // Clear the screen
        Console.SetCursorPosition(0, 0); // Reset cursor to top
        DisplayMenu(); // Reprint the menu
    }

    static void ShowStats()
    {
        Console.WriteLine("\n=== Hero Stats ===");
        Console.WriteLine($"Gold: {gold}");
        Console.WriteLine($"Damage: {heroDamage}");
        Console.WriteLine($"Health: {heroHealth}/{maxHeroHealth}");
        Console.WriteLine($"Monsters Defeated: {monstersDefeated}");
        Console.WriteLine($"Monster Damage: {monsterDamage}");
        Console.WriteLine($"Armor Defense: {armorLevel}%");
        Console.WriteLine($"Monster Debuff Active: {(isMonsterDebuffed ? "Yes (50% damage)" : "No")}");
    }
}
