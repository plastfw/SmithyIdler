using System.Collections.Generic;
using UnityEngine;

public class SwordStorage : MonoBehaviour
{
  [SerializeField] private List<GameObject> _swords;

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.TryGetComponent(out Player player))
    {
      GiveSwords(player);
    }
  }

  public void TakeSwords(GameObject sword)
  {
    _swords.Add(sword);
  }

  private void GiveSwords(Player player)
  {
    foreach (GameObject sword in _swords)
    {
      sword.transform.SetParent(player.SwordHandPoint);
      player.TakeSword(sword);
    }

    _swords.Clear();
  }
}