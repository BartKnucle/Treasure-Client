using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  public abstract class LayerSO : ScriptableObject
  {
    public bool activated = true;
    public virtual Layer CreateManager(GameObject go, Manager world)
    {
      Layer layerComponent = go.AddComponent<Layer>();
      layerComponent.layerSO = this;
      layerComponent.world = world;
      return layerComponent;
    }
    public virtual Tile AddTile(Manager world, Layer layer) {
      return new Tile(world, layer);
    }
  }
}
