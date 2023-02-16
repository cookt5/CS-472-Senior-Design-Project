using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staircase : PlacedObject
{
    private bool downward = true;
    private Tile originTile, destinationTile;
    private Layer originLayer, destiniationLayer;

    [SerializeField] private Sprite upwardSprite = null;
    [SerializeField] private Sprite downwardSprite = null;

    [SerializeField] private Tile testTile = null;

    private new BaseStats myStats = new BaseStats();

    [ContextMenu("TestInitialization")]
    public void InitializeTile()
    {
        PlaceObject(testTile);
    }

    public Tile GetDestinationTile()
    {
        return destinationTile;
    }

    public bool PlaceObject(Tile location, bool downward = true)
    {
        // If the staircase can't be reached
        if(!location.GetIsNavigable())
        {
            return false;
        }

        // If the above or below tile doesn't exist
        if((downward && location.GetBelowNeighbor() == null) || (!downward && location.GetAboveNeighbor() == null))
        {
            return false;
        }

        // If the above or below tile doesn't have a navigable neighbor (where the NPC would be teleported to)
        if((downward && location.GetBelowNeighbor().GetNavigableNeighbor() == null) || (!downward && location.GetAboveNeighbor().GetNavigableNeighbor() == null))
        {
            return false;    
        }

        base.PlaceObject(location);
        this.downward = downward;
        this.originTile = location;
        this.originLayer = location.GetLayer();
        if(downward)
        {
            GetComponent<SpriteRenderer>().sprite = downwardSprite;
            this.destinationTile = location.GetBelowNeighbor();
            this.destiniationLayer = destinationTile.GetLayer();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = upwardSprite;
            this.destinationTile = location.GetAboveNeighbor();
            this.destiniationLayer = destinationTile.GetLayer();
        }

        myStats.x = location.GetXYLocation().Item1;
        myStats.y = location.GetXYLocation().Item2;
        myStats.z = location.GetLayer().GetLayerNumber();
        myStats.isDownward = downward;
        myStats.guid = Id;

        // Signal successful placement
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            return;
        }
        Tile teleportLocation = destinationTile.GetNavigableNeighbor();
        if(teleportLocation != null)
        {
            other.transform.position = teleportLocation.transform.position;
        }
    }

    public override void SaveMyData()
    {
        SaveData<BaseStats>(myStats);
    }

    public override void LoadMyData()
    {
        myStats = LoadData<BaseStats>();
        Map myMap = FindObjectOfType<Map>();
        PlaceObject(myMap.GetTile(myStats.x, myStats.y, myStats.z), myStats.isDownward);
    }
}
