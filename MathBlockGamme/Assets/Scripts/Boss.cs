using UnityEngine;

public class Boss : TurnTakerBase
{
   public override bool TakeTurn()
   {
      Debug.Log("Boss Turn");
      return true;
   }
}
