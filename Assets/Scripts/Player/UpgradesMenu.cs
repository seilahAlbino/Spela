using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    public GameObject[] itens;
    public GameObject sprite;
    public Transform playerPosition;
    public GameObject pet;
    public GameObject upgradeMenu;
    public Text textUpgrade;
    public Text btnTextUpgrade;
    Animator anim;
    GameObject itemAtual;
    bool canAnim = true;
    // Start is called before the first frame update
    void Start()
    {
        //upgradeMenu.sortingLayer = 100;
        upgradeMenu.SetActive(false);
        anim = upgradeMenu.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            GameObject item = itens[i];
            if (Vector2.Distance(item.transform.position, /*new Vector2(transform.position.x + 1.85f, transform.position.y + 0.76f)*/transform.position) < 1)
            {
                itemAtual = item;
                ItemInformation itemInfo = item.GetComponent<ItemInformation>();
                SpriteRenderer spriteRend = item.GetComponent<SpriteRenderer>();

                sprite.GetComponent<Image>().sprite = spriteRend.sprite;
                sprite.GetComponent<RectTransform>().sizeDelta = new Vector2(itemInfo.width, itemInfo.height);

                textUpgrade.text = itemInfo.currentUpgrade < 5 ? itemInfo.currentUpgrade+"/5\n"+ itemInfo.description.Replace("{upgrade}",itemInfo.upgrade.ToString()) : itemInfo.currentUpgrade + "/5";
                btnTextUpgrade.text = itemInfo.currentUpgrade < 5 ? "UPGRADE\n" + itemInfo.price : "MAX";
                upgradeMenu.gameObject.SetActive(true);

                if (canAnim)
                {
                    anim.Play("EnterUpgradeMenu");
                    canAnim = false;
                }

                break;
            }
            else
            {
                if (itemAtual == item)
                {
                    canAnim = true;
                    upgradeMenu.SetActive(false);
                }
            }
            
        }
    }

    public void Upgrade()
    {
        for (int i = 0; i < itens.Length; i++)
        {
            if (itemAtual == itens[i])
            {
                ItemInformation itemInfo = itens[i].GetComponent<ItemInformation>();
                if (playerPosition.gameObject.GetComponent<PlayerController>().money >= itemInfo.price)
                {
                    if (itemInfo.currentUpgrade < 5)
                    {
                        itemInfo.currentUpgrade++;
                        itemInfo.price += itemInfo.newPrice;
                        itemInfo.upgrade += itemInfo.newUpgrade;

                        if (itemInfo.name.Equals("LifeUp")){
                            playerPosition.gameObject.GetComponent<PlayerController>().maxLife += itemInfo.newUpgrade;
                            playerPosition.gameObject.GetComponent<PlayerController>().life += itemInfo.newUpgrade;
                        }
                        else if (itemInfo.name.Equals("PetAttackUp")){
                            pet.GetComponent<PetController>().attackMin[pet.GetComponent<PetController>().PetN] += itemInfo.newUpgrade;
                            pet.GetComponent<PetController>().attackMax[pet.GetComponent<PetController>().PetN] += itemInfo.newUpgrade;
                        }
                        playerPosition.gameObject.GetComponent<PlayerController>().money -= itemInfo.price;
                    }
                }
                else
                {
                    Debug.Log("n podes comprar");
                }
                break;
            }
        }
    }
}
