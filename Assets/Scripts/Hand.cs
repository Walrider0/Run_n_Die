using UnityEngine;

public enum Holding
{
    Empty,
    Something
}

public class Hand : MonoBehaviour
{
    public Holding holding;
    void Start()
    {
       holding = Holding.Empty; 
    }

    void Update()
    {
        
    }
}
