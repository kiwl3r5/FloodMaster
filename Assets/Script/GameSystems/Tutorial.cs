using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int page = 1;
    [SerializeField] private int maxPage = 5;
    [SerializeField] private GameObject tutotoalUi;
    [SerializeField] private Text Header;
    [SerializeField] private Text Descrip;
    [SerializeField] private GameObject[] picture;
    
    void Update()
    {
        PageContent(page);
    }

    public void CloseTutorial()
    {
        tutotoalUi.SetActive(false);
    }
    public void OpenTutorial()
    {
        tutotoalUi.SetActive(true);
    }
    public void TutorialNextPage()
    {
        if (page == maxPage)
        {
            return;
        }
        page++;
    }
    public void TutorialPreviousPage()
    {
        if (page == 1)
        {
            return;
        }
        page--;
    }
    
    private void PageContent(int contentPage)
    {
        if (contentPage == 1)
        {
            Header.text = "How to use Skill";
            Descrip.text = "You can use the skill if you have enough -Good Deed Points- or -Skill Points- . You can get Skill Point from collecting garbage that floats along the water or Clear the sewer.";
            PictureSetup(1);
        }
        if (contentPage == 2)
        {
            Header.text = "Skill Type";
            Descrip.text = "In this game, you have 3 skills to use. The first skill is the boots," +
                           " which will make the player run faster in the water, the second skill is to intimidate," +
                           " scare enemies, and not attack the player, and the third skill is a particular skill that can be collected from clearing the sewer and from supply drop that let you spawn your own boat and use skill points as a fuel.";
            PictureSetup(2);
        }
        if (contentPage == 3)
        {
            Header.text = "How To Reduce Flood rate and the flood level";
            Descrip.text = "You can reduce Flood rate by clearing garbage that clogs the StormDrain and from destroy fat-lump in the sewer";
            PictureSetup(3);
        }
        if (contentPage == 4)
        {
            Header.text = "Enemy Type";
            Descrip.text = "In this game, there are two types of enemies. The first type is the rat. will come out to harass" +
                           " the player and will occur at a difficulty level of more than 2 or more, the second type is the most" +
                           " dangerous, the crocodile will have a chance to occur In difficulty levels greater than 3 and can kill the player. make the game over.";
            PictureSetup(4);
        }
    }

    private void PictureSetup(int page)
    {
        if (page == 1)
        {
            picture[0].SetActive(true);
            picture[1].SetActive(false);
            picture[2].SetActive(false);
            picture[3].SetActive(false);
        }
        if (page == 2)
        {
            picture[0].SetActive(false);
            picture[1].SetActive(true);
            picture[2].SetActive(false);
            picture[3].SetActive(false);
        }
        if (page == 3)
        {
            picture[0].SetActive(false);
            picture[1].SetActive(false);
            picture[2].SetActive(true);
            picture[3].SetActive(false);
        }
        if (page == 4)
        {
            picture[0].SetActive(false);
            picture[1].SetActive(false);
            picture[2].SetActive(false);
            picture[3].SetActive(true);
        }
    }
}
