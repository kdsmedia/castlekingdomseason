using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select_Upgrade : MonoBehaviour
{

    public int index;
    public bool Archer, Barrack, Mage, Cannon, RainFire, Solider;
    private new string name;
    Select_Upgrade[] upgrades;
   
    public Text _text;
    private int number = 0;
    public int cost;
    public GameObject Selected;
    void Start()
    {
        upgrades = FindObjectsOfType<Select_Upgrade>();
        if (Archer)
        {        
            name = "Archer";
            if (index == 1)
            {
                _text.text = "MARKSMEN TOWERS RETURN 90% OF VALUE WHEN SOLD";
                Selected.transform.position = transform.position;
            }
        }
        else if (Barrack)
        {
            name = "Barrack";
        }
        else if (Mage)
        {
            name = "Mage";
        }
        else if (Cannon)
        {
            name = "Cannon";
        }
        else if (RainFire)
        {
            name = "Rain";
        }
        else
        {
            name = "Solider";
        }
       // PlayerPrefs.SetInt(name, 1);
        Check();
    }
    public void Check()
    {
        if (index == 1)
        {
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = Color.white;
        }
        else
        {
            if (PlayerPrefs.GetInt(name) >= index)
            {
                GetComponent<Button>().interactable = true;
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponent<Image>().color = Color.grey;
            }
        }
    }
    public void Upgrade()
    {
        Selected.transform.position = transform.position;
        number++;
        foreach(Select_Upgrade upgrade in upgrades)
        {
            if (upgrade != this)
            {
                upgrade.number = 0;
            }
        }
        ShowText();
        if (number == 2 && cost<=ShowNumberStar.currentStar)
        {
            PlayerPrefs.SetInt("Cost", PlayerPrefs.GetInt("Cost") + cost);
            ShowNumberStar._instance.ShowCurrentStar();
            TruelyUpgrade();
        }
    }
    private void ShowText()
    {
        if (Archer)
        {
            if (index == 1)
            {
                _text.text = "MARKSMEN TOWERS RETURN 90% OF VALUE WHEN SOLD";
            }
            else if (index == 2)
            {
                _text.text = "INCREASES MARKSMEN ATTACK RANGE";
            }
            else if (index == 3)
            {
                _text.text = "PIERCING SHOTS IGNORE A PORTION OF THE ENEMY PHYSICAL ARMOR";
            }
            else if (index == 4)
            {
                _text.text = "INCREASES MARKSMEN ATTACK RANGE";
            }
            else if (index == 5)
            {
                _text.text = "MARKSMEN ATTACK HAVE A CHANCE OF DEALING DOUBLE DAMAGE";
            }
        }
        else if (Barrack)
        {
            if (index == 1)
            {
                _text.text = "BARRACK TRAIN SOLDIERS WITH MORE HEALTH";
            }
            else if (index == 2)
            {
                _text.text = "BARRACK TRAIN SOLDIERS WITH IMPROVED ARMOR";
            }
            else if (index == 3)
            {
                _text.text = "INCREASES RALLY POINT RANGE AND REDUCES SOLDIER TRAINING TIME";
            }
            else if (index == 4)
            {
                _text.text = "BARRACK TRAIN SOLDIERS WITH MORE HEALTH";
            }
            else if (index == 5)
            {
                _text.text = "WHEN ATTACKED, SOLDIER'S SPIKED ARMOR DEALS DAMAGE TO THE ATTACKER";
            }
        }
        else if (Mage)
        {
            if (index == 1)
            {
                _text.text = "INCREASE WIZARD'S ATTACK RANGE";
            }
            else if (index == 2)
            {
                _text.text = "MAGIC ATTACKS DESTROY A PORTION OF ENEMY PHYSICAL ARMOR ON EVERY HIT";
            }
            else if (index == 3)
            {
                _text.text = "MAGE TOWERS CONSTRUCTION AND UPGRADING COST ARE REDUCED BY 10%";
            }
            else if (index == 4)
            {
                _text.text = "INCREASES WIZARD'S ATTACK DAMAGE";
            }
            else if (index == 5)
            {
                _text.text = "MAGIC ATTACKS SLOW ENEMIES BY HALF THEIR SPEED FOR A MOMENT";
            }
        }
        else if (Cannon)
        {
            if (index == 1)
            {
                _text.text = "INCREASES ARTILLERY ATTACK DAMAGE";
            }
            else if (index == 2)
            {
                _text.text = "INCREASES ARTILLERY ATTACK RANGE";
            }
            else if (index == 3)
            {
                _text.text = "ARTILLERY CONSTRUCTION AND UPGRADING COST ARE REDUCED BY 10%";
            }
            else if (index == 4)
            {
                _text.text = "ARTILLERY SPECIAL ABILITIES COST ARE REDUCED BY 25%";
            }
            else if (index == 5)
            {
                _text.text = "ARTILLERY SUFFERS NO REDUCTION OF SPLASH AND CHAIN LIGHTNING DAMAGE";
            }
            
        }
        else if (RainFire)
        {
            if (index == 1)
            {
                _text.text = "ADDS 2 ADDITIONAL METEORS AND INCREASES METEOR DAMAGE";
            }
            else if (index == 2)
            {
                _text.text = "METEORS SET THE GROUND ON FIRE BURNING ENEMIES OVER IT FOR 5 SECONDS";
            }
            else if (index == 3)
            {
                _text.text = "INCREASES METEOR DAMAGE, EXPLOSION REDIUS BY 25% AND REDUCES COOLDOWN BY 10 SECONDS";
            }
            else if (index == 4)
            {
                _text.text = "DOUBLES DAMAGE AND DURATION OF SCORCHED EARTH, AND REDUCES COOLDOWN BY 10 SECONDS";
            }
            else if (index == 5)
            {
                _text.text = "INCREASES METEOR DAMAGE AND RAIN ADDITIONAL METEORS AT RANDOM LACATION ALL OVER THE BATTLEFIED";
            }
        }
        else
        {
            if (index == 1)
            {
                _text.text = "WELL FED FARMERS HAVE ADDITIONAL HEALTH AND DEAL A LITTLE MORE DAMAGE";
            }
            else if (index == 2)
            {
                _text.text = "CONSCRIPTS HAVE MORE HEALTH AND ARE BETTER EQUIPPED";
            }
            else if (index == 3)
            {
                _text.text = "WARRIORS HAVE EVEN MORE HEALTH AND ARE EXELLENTLY EQUIPPED";
            }
            else if (index == 4)
            {
                _text.text = "LEGIONAIRES HAVE THE MOST HEALTH AND WEAR THE BEST EQUIPMENT";
            }
            else if (index == 5)
            {
                _text.text = "GIVE LEGIONAIRES A SPEAR THROW ATTACK THAT CAN TARGET GROUND AND FLYING ENEMIES";
            }
        }
    }
    public void TruelyUpgrade()
    {
        if (index == 1)
        {
            PlayerPrefs.SetInt(name, 2); // đặt giá trị cho key ==2;
            
            foreach (Select_Upgrade upgrade in upgrades)
            {
                upgrade.Check();
            }

        }
        else
        {
            if (PlayerPrefs.GetInt(name) >= index)
            {
                PlayerPrefs.SetInt(name, index + 1);
                //upgrades = FindObjectsOfType<Select_Upgrade>();
                foreach (Select_Upgrade upgrade in upgrades)
                {
                    upgrade.Check();
                }
            }
        }
        PlayerPrefs.SetInt(name + "upgrade"+index, 1);
    }
}
