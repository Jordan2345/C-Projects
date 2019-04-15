using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
/*
* TODO List
* 
* -Add a factory for cookie generation
* -Organize code
* -Fix layout
* -Add settings menu
* -Add prestige multiplier(Doubtful)
* -Add music/sound that can be toggled in settings menu
* 
* 
* 
*/


public class CookieClick : MonoBehaviour
{
    /******************Text variables******************/
    [Header("Text Field")]
    [SerializeField] TextMeshProUGUI CookieScoreText,PointerUpgradeText, AutoClickerUpgradeText,MultiplierText,InformationText,StatsText,BuyingPowerText;
    /******************Float variables*****************/
    [Header("Number Field")]
    public static float numCookiesClicked,pointerLevel,autoClickerLevel,pointerCost,autoClickerCost,multiplierCost,
                           pointerIncrement,autoClickerIncrement,multiplierCoefficient,multiplierLevel;
    /******************String variables*****************/
    [Header("String Field")]
    private string pointerUpgradeText, autoUpgradeText, multiplierUpgradeText,name;
    /******************Boolean variables***************/
    [Header("Boolean Field")]
    [SerializeField] bool isMaxPower;
    /******************Other Fields********************/
    public static CookieClick gameData;
    public bool hasReset;
    private IEnumerator autoClickerCoroutine;
    private void Awake()
    {
        name = RetrieveName.save_Names.userName;
    }
    void Start ()
    {
        //initialize the game variables
        numCookiesClicked = 0f;
        pointerCost = 10;
        pointerLevel = 0;
        multiplierCost = 1000;
        multiplierCoefficient = 1;
        multiplierLevel = 0;
        autoClickerLevel = 0;
        autoClickerCost = 50;
        pointerIncrement = 1;
        autoClickerIncrement = 0;
        pointerUpgradeText = "Upgrade Clicker to Level 1:  (Cost 10)";
        autoUpgradeText = "Upgrade Auto Clicker to Level 1:  (Cost 50)";
        multiplierUpgradeText = "Multiplier x2\n(Cost 1000)";
        isMaxPower = false;
        hasReset = false;
        /**********************************/
        CookieScoreText.text = "Total Cookies:  0";
        PointerUpgradeText.text = pointerUpgradeText;
        AutoClickerUpgradeText.text = autoUpgradeText;
        StatsText.text = "\tSTATS\nCookies per click:  " + pointerIncrement + "\nCookies automatically farmed/second:  " + autoClickerIncrement + "\nCurrent Multiplier:  x" + multiplierCoefficient;
        MultiplierText.text = multiplierUpgradeText;
        InformationText.text = "";
        BuyingPowerText.text = "1x Buying";
        /*********************************/
        //call the auto clicker coroutine in start function because if it's called in the update function
        //it will run 60 times or so per second (depends on your framrate)
        AutoClicker();

	}
    void Update()
    {
        name = RetrieveName.save_Names.userName;
        //called each frame so it's neccessary to update text fields
        if(hasReset)
        {
            Reset();
            hasReset = false;
        }
        CookieScoreText.text = "Total Cookies:  " + CookiesWithAbbreviation(numCookiesClicked);
        if(isMaxPower)
        {
            float num_Cookies = numCookiesClicked;
            float level = pointerLevel;
            float costOfNext = pointerCost;
            float totCost = 0f;
            float maxAmount = 0f;
            while(num_Cookies>=costOfNext)
            {
                totCost += costOfNext;
                maxAmount++;
                num_Cookies -= costOfNext;
                level++;
                costOfNext = 10f * Mathf.Pow(1.1f, level);
            }
            if(totCost==0f)
                pointerUpgradeText = "Upgrade Clicker to Level " + level + ":  (Cost " + CookiesWithAbbreviation(pointerCost) + ")";
            else
                pointerUpgradeText = "Upgrade Clicker to Level " + level + "(+"+(level-pointerLevel)+"):  (Cost " + CookiesWithAbbreviation(totCost) + ")";
            //reset values to check auto clicker upgrade
            num_Cookies = numCookiesClicked;
            level = autoClickerLevel;
            costOfNext = autoClickerCost;
            totCost = 0f;
            maxAmount = 0f;
            while (num_Cookies >= costOfNext)
            {
                totCost += costOfNext;
                maxAmount++;
                num_Cookies -= costOfNext;
                level++;
                costOfNext = 50f * Mathf.Pow(1.1f, level);
            }
            if(totCost==0f)
                autoUpgradeText = "Upgrade Clicker to Level " + level + "\n(Cost " + CookiesWithAbbreviation(autoClickerCost) + ")";
            else
                autoUpgradeText = "Upgrade Clicker to Level " + level + "(+"+ (level-autoClickerLevel) + ")\n(Cost " + CookiesWithAbbreviation(totCost) + ")";
            //reset values to check multiplier upgrade
            num_Cookies = numCookiesClicked;
            level = multiplierLevel;
            costOfNext = multiplierCost;
            totCost = 0f;
            maxAmount = 0f;
            while (num_Cookies >= costOfNext)
            {
                totCost += costOfNext;
                maxAmount++;
                num_Cookies -= costOfNext;
                level++;
                costOfNext = 1000f * Mathf.Pow(2f, level);
            }
            if(totCost==0f)
                multiplierUpgradeText= "Multiplier x2\n(Cost " + CookiesWithAbbreviation(multiplierCost) + ")";
            else
                multiplierUpgradeText = "Multiplier x2(+"+ (level-multiplierLevel) + ")\n(Cost " + CookiesWithAbbreviation(totCost) + ")";
            PointerUpgradeText.text = pointerUpgradeText;
            AutoClickerUpgradeText.text = autoUpgradeText;
            MultiplierText.text = multiplierUpgradeText;
        }
        else
        {
            PointerUpgradeText.text = "Upgrade Clicker to Level " + (pointerLevel + 1) + ":  (Cost " + CookiesWithAbbreviation(pointerCost) + ")";
            AutoClickerUpgradeText.text = "Upgrade Clicker to Level " + (autoClickerLevel + 1) + "\n(Cost " + CookiesWithAbbreviation(autoClickerCost) + ")";
            MultiplierText.text = "Multiplier x2\n(Cost " + CookiesWithAbbreviation(multiplierCost) + ")";
        }
        float pointerRepresent = pointerIncrement * multiplierCoefficient;
        float autoRepresent = autoClickerIncrement * multiplierCoefficient;
        StatsText.text = "\tSTATS\nCookies per click:  " + CookiesWithAbbreviation(pointerRepresent) + "\nAuto Cookies/Second:  " + CookiesWithAbbreviation(autoRepresent) + "\nCurrent Multiplier:  x" + multiplierCoefficient;

        //For testing reasons
        if (Input.GetKeyDown(KeyCode.L))
        {
            numCookiesClicked += Mathf.Pow(10f, 12f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
            numCookiesClicked = 0;
    }
    public void OnCookieClick()
    {
        numCookiesClicked += pointerIncrement*multiplierCoefficient;
        //reset the text at the bottom if needed
        InformationText.text = "";
       
    }
    public void MultiplierUpgrade()
    {
        //check if the player has enough cookies to buy multiplier upgrade
        if (numCookiesClicked < multiplierCost)
        {
            //display error message at the bottom
            InformationText.text= "You don't have enough Cookies to buy that";
        }
        else
        {
            if (isMaxPower)
            {
                while (numCookiesClicked >= multiplierCost)
                {
                    numCookiesClicked -= multiplierCost;
                    multiplierLevel++;
                    if (multiplierLevel == 1)
                    {
                        //multiplierCoefficient for first level will be 2x
                        multiplierCoefficient = 2;
                    }
                    else
                    {
                        multiplierCoefficient += 2;
                    }
                    //use kongregate's formula for cost upgrade
                    multiplierCost = 1000f * Mathf.Pow(2f, multiplierLevel);
                }
            }
            else
            {
                //buy multiplier upgrade
                numCookiesClicked -= multiplierCost;
                multiplierLevel++;
                if (multiplierLevel == 1)
                {
                    //multiplierCoefficient for first level will be 2x
                    multiplierCoefficient = 2;
                }
                else
                {
                    multiplierCoefficient += 2;
                }
                //use kongregate's formula for cost upgrade
                multiplierCost = 1000f * Mathf.Pow(2f, multiplierLevel);

            }
        }

    }
    public void UpgradePointer()
    {
        //check if the player has enough cookies to buy upgrade first
        if(numCookiesClicked<pointerCost)
        {
            //display the error message at the bottom
            InformationText.text = "You don't have enough Cookies to buy that";
        }
        else
        {
            if(isMaxPower)
            {
                while(numCookiesClicked>=pointerCost)
                {
                    numCookiesClicked -= pointerCost;
                    pointerLevel++;
                    pointerCost = 10f * Mathf.Pow(1.1f, pointerLevel);
                }
                pointerIncrement = (1 * pointerLevel) * multiplierCoefficient;
                InformationText.text = "Pointer is now at level " + pointerLevel;
            }
            else
            {
                //buy the upgrade for the player
                numCookiesClicked -= pointerCost;
                pointerLevel++;
                //calculate the cost of the next level using kongregate's formula
                //although the rate is not a float, we will assume it is so that the formula doesn't multiply by a constant 1 each level
                //for the pointer, we assume the rate of growth is 1.1 cookies/click
                //cost(next)=cost(base) x (rate of growth)^amount owned
                pointerCost = 10f * Mathf.Pow(1.1f, pointerLevel);
                //now for production the formula is as follows
                //production(total)=(base production x amount owned) x multipliers
                pointerIncrement = (1 * pointerLevel) * multiplierCoefficient;
                InformationText.text = "Pointer is now at level " + pointerLevel;
            }

        }
    }
    public void UpgradeAutoClicker()
    {
        //check if the player has enough cookies to buy upgrade first
        if (numCookiesClicked < autoClickerCost)
        {
            //display the error message at the bottom
            InformationText.text = "You don't have enough Cookies to buy that";
        }
        else
        { 
            if(isMaxPower)
            {
                while(numCookiesClicked>=autoClickerCost)
                {
                    numCookiesClicked -= autoClickerCost;
                    autoClickerLevel++;
                    autoClickerCost = 50f * Mathf.Pow(1.1f, autoClickerLevel);
                }
                autoClickerIncrement = (1 * autoClickerLevel) * multiplierCoefficient;
                InformationText.text = "Auto Clicker is now at level " + autoClickerLevel;
            }
            else
            {
                //buy the upgrade for the player
                numCookiesClicked -= autoClickerCost;
                autoClickerLevel++;
                //calculate the cost of the next level using kongregate's formula
                //for the auto clicker, we assume the rate of growth is 1.1 cookies/click
                //cost(next)=cost(base) x (rate of growth)^amount owned
                autoClickerCost = 50f * Mathf.Pow(1.1f, autoClickerLevel);
                //now for production the formula is as follows
                //production(total)=(base production x amount owned) x multipliers
                autoClickerIncrement = (1 * autoClickerLevel) * multiplierCoefficient;
                InformationText.text = "Auto Clicker is now at level " + autoClickerLevel;
            }

        }
    }
    public void ChangeBuyingPower()
    {
        if (isMaxPower)
        {
            isMaxPower = false;
            BuyingPowerText.text = "1x Buying";

        }
        else
        {
            isMaxPower = true;
            BuyingPowerText.text = "Max Buying";
        }
    }
    public void AutoClicker()
    {
        //run each second
        autoClickerCoroutine = RunAutoClicker(1f);
        StartCoroutine(autoClickerCoroutine);
    }
    public void Reset()
    {
        hasReset = true;

        File.Delete(Application.persistentDataPath + "/GameInfo/GameInfoEarth"+name+".dat");
        File.Delete(Application.persistentDataPath + "/GameInfo/GameInfoMoon" + name + ".dat");

        Debug.Log("Deleted /GameInfo");
        numCookiesClicked = 0f;
        pointerCost = 10;
        pointerLevel = 0;
        multiplierCost = 1000;
        multiplierCoefficient = 1;
        multiplierLevel = 0;
        autoClickerLevel = 0;
        autoClickerCost = 50;
        autoClickerIncrement = 0f;
        pointerIncrement = 1;
        isMaxPower = false;
        hasReset = false;
    }
    /*
     * For Autoclicker as well as possible factory upgrade, use coroutine
    */
    private string CookiesWithAbbreviation(float amount)
    {
        string answer = "";

        if(amount > 0 && amount<1000)
        {
                answer = amount.ToString("F0");
        }
        else if(amount >= Mathf.Pow(10f, 3f) && amount < Mathf.Pow(10f,6f))
        {
            //add k suffix to end of number
            float abbreviatedCookies = amount / Mathf.Pow(10f,3f);
            //check if number is evenly divisible for decimal formatting
            if (abbreviatedCookies % 2 == 0)
                answer = abbreviatedCookies.ToString("F0") + "k";
            else
                answer = abbreviatedCookies.ToString("F2") + "k";
        }
        else if(amount >= Mathf.Pow(10f,6f) && amount < Mathf.Pow(10f,9f))
        {
            //add m suffix to end of number
            float abbreviatedCookies = amount / Mathf.Pow(10f,6f);
            //check if number is evenly divisible for decimal formatting
            if (abbreviatedCookies % 2 == 0)
                answer = abbreviatedCookies.ToString("F0") + " Million";
            else
                answer = abbreviatedCookies.ToString("F2") + " Million";
        }
        else if(amount >= Mathf.Pow(10f,9f) && amount < Mathf.Pow(10f,12f))
        {
            //add b suffix to end of number
            float abbreviatedCookies = amount / Mathf.Pow(10f,9f);
            //check if number is evenly divisible for decimal formatting
            if (abbreviatedCookies % 2 == 0)
                answer = abbreviatedCookies.ToString("F0") + " Billion";
            else
                answer = abbreviatedCookies.ToString("F2") + " Billion";
        }
        else if(amount>=Mathf.Pow(10f,12f) && amount < Mathf.Pow(10f,15f))
        {
            //add t(uadrillion) suffix
            float abbreviatedCookies = amount / Mathf.Pow(10f, 12f);
            //check if number is evenly divisible for decimal formatting
            if (abbreviatedCookies % 2 == 0)
                answer = abbreviatedCookies.ToString("F0") + " Trillion";
            else
                answer = abbreviatedCookies.ToString("F2") + " Trillion";
        }
        return answer;
    }
    private IEnumerator RunAutoClicker(float waitTime)
    {
        //calculate autoclicker actions
        while (true)
        {
            numCookiesClicked += autoClickerIncrement*multiplierCoefficient;
            yield return new WaitForSeconds(waitTime);
        }
    }
}